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
    public VariableJoystick joy;        // joystick �� ���� �������� �����̵��� ��   
    public GameObject[] buttons;
    private Vector3 moveVec;    // rigid.MovePosition�� ���� �����̵��� ����
    private bool isMoving = true;
    public bool isSkillAttack = false;
    public static bool isDead = false;

    public static System.Action HitPlayer;  // ���Ͱ� �÷��̾ �������� �� ����
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
        // ������ ��� �̵� �Ұ�
        if(faceMode != 2 && isMoving)
        {
            // joystick�� ���� x ���� z ���� ���޹���
            float x = joy.Horizontal;
            float z = joy.Vertical;

            // Move
            moveVec = new Vector3(x, 0, z) * walkSpeed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveVec);

            if (moveVec.sqrMagnitude == 0)
                return; // no input = no rotation

            Quaternion dirQuat = Quaternion.LookRotation(moveVec);  // ȸ���Ϸ��� ����

            // �ε巴�� �̵��� �� �ֵ��� ����
            Quaternion moveQuat = Quaternion.Slerp(rigid.rotation, dirQuat, 0.3f);
            rigid.MoveRotation(moveQuat);
        }
    }
   

    public override void Attack()    // ���� ���� ����
    {
        faceMode = 2;
        if (!isSkillAttack)
        {
            anim.SetTrigger("isAttacking");
            // ���� ��ư ��Ȱ��ȭ
            manager.GetComponent<SlimeSurvivorManager>().
                UninteractableAttackButton(this.atkDelay);
        }
        isSkillAttack = false;
        Invoke("WaitIdle", 1f);
    }

    public override void Hit()   // ��ư ��Ȱ��ȭ �����ϱ�
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

    // Ư�� ��Ȳ�� ���� ǥ�� �����ϴ� �Լ�
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
