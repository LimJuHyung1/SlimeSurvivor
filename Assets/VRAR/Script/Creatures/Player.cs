using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : Creature
{
    Rigidbody rigid;
    Animator anim;

    public GameObject slimeBody;
    public Material idleFace;
    public Material attackFace;
    public Material hitFace;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private SphereCollider sphereCollider;

    public static int faceMode;

    public SlimeSurvivorManager manager;
    public VariableJoystick joy;        // joystick 을 통해 슬라임을 움직이도록 함   
    public GameObject[] buttons;
    private Vector3 moveVec;    // rigid.MovePosition을 통해 움직이도록 설정
    private bool isMoving = true;
    public bool isSkillAttack = false;
    public static bool isDead = false;

    public static System.Action HitPlayer;  // 몬스터가 플레이어를 공격했을 때 실행
    public static System.Action IdlePlayer;

    protected override void Start()
    {
        faceMode = 0;
        isDead = false;

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        skinnedMeshRenderer = slimeBody.GetComponent<SkinnedMeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();

        HitPlayer = () => Hit();
        IdlePlayer = () => WaitIdle();
    }

    void Update()
    {
        if(this.gameObject.transform.position.y < -5f)
        {
            this.transform.position = new Vector3(0, 5, 0);
        }

        if (this.hp > this.maxHp) hp = maxHp;            

        switch (faceMode)
        {
            case 0:
                ChangeFaceToIdle();
                break;
            case 1:
                ChangeFaceToIdle();
                break;
            case 2:
                ChangeFaceToAttack();
                break;
            case 3:
                ChangeFaceToHit();
                break;

            default:
                ChangeFaceToIdle();
                Debug.Log("FaceMode Error");
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 공격할 경우 이동 불가
        if(faceMode != 2 && isMoving)
        {
            // joystick을 통해 x 값과 z 값을 전달받음
            float x = joy.Horizontal;
            float z = joy.Vertical;

            // Move
            moveVec = new Vector3(x, 0, z) * walkSpeed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveVec);

            if (moveVec.sqrMagnitude == 0)
                return; // no input = no rotation

            Quaternion dirQuat = Quaternion.LookRotation(moveVec);  // 회전하려는 방향

            // 부드럽게 이동할 수 있도록 설정
            Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, dirQuat, 0.3f);
            rigid.MoveRotation(moveQuat);
        }
    }
   

    public override void Attack()    // 공격 상태 정의
    {
        faceMode = 2;
        if (!isSkillAttack)
        {
            anim.SetTrigger("isAttacking");
            // 공격 버튼 비활성화
            manager.GetComponent<SlimeSurvivorManager>().
                UninteractableAttackButton(this.atkDelay);
        }
        isSkillAttack = false;
        Invoke("WaitIdle", 1f);
    }

    public override void Hit()   // 버튼 비활성화 설정하기
    {
        faceMode = 3;
        if (this.hp > 0)
            anim.SetTrigger("isHitting");
        else 
        {
            if (!isDead)
            {
                isDead = true;
                anim.SetTrigger("isDead");
                joy.gameObject.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
                isMoving = false;

                SlimeSurvivorManager.endScreen(0.7f);
                Invoke("WaitUnactive", 1f);                
            }
        }
    }    

    void WaitUnactive()
    {        
        this.gameObject.SetActive(false);
    }

    // 특정 상황에 따라 표정 변경하는 함수
    void ChangeFaceToIdle()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[1] = idleFace;

        skinnedMeshRenderer.materials = materials;
    }

    void ChangeFaceToAttack()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[1] = attackFace;

        skinnedMeshRenderer.materials = materials;
    }

    void ChangeFaceToHit()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[1] = hitFace;

        skinnedMeshRenderer.materials = materials;
    }    

    public void WaitIdle()
    {
        faceMode = 0;
    }
}
