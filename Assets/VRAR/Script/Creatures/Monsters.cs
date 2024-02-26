using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : Creature
{
    GameObject player;
    Animator anim;  // Animator 컴포넌트 참조
    BoxCollider box;
    AudioSource audio;

    public AudioClip deathSound;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
        audio = GetComponent<AudioSource>();
        anim.SetBool("isWalking", true);

        player = GameObject.Find("Slime_01");       // 플레이어(슬라임)을 참조할 수 있도록 설정
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {                        
            transform.LookAt(player.gameObject.transform);  // 몬스터의 방향이 플레이어(슬라임)을 바라봄

            // 공격 애니메이션이 실행되지 않는 상태라면 플레이어를 찾아 이동
            if (!this.anim.GetBool("isAttacking") && this.hp > 0)
            {
                // Vector3.MoveTowards(자신의 위치, 플레이어의 위치, 이동 속도)
                this.transform.position = Vector3.MoveTowards
                (this.transform.position, player.transform.position, 0.01f * this.walkSpeed);
            }            
        }
    }

    public override void Hit()
    {
        anim.SetTrigger("isHitting");
    }

    public override void Dead()
    {
        int randSpawnIndex = (int)UnityEngine.Random.Range(1, 2);
        SlimeSurvivorManager.returnCube(this.transform, randSpawnIndex);

        base.Dead();
    }

    public void Idle()
    {
        anim.SetBool("isWalking", true);        
    }

    public void DamagedHp(int originDamage, int skillLevel)
    {
        this.hp -= originDamage * skillLevel;
        if (this.hp > 0) Hit();
        else 
        {
            anim.SetTrigger("isDead");
            audio.clip = deathSound;
            audio.Play();
            Invoke("Dead", 1.25f);      // 1.25초 후 삭제됨
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isAttacking");
            this.box.enabled = false;
            audio.Play();

            Invoke("AttackTiming", this.atkDelay / 2);
            Invoke("LaterAttack", this.atkDelay);
        }
    }

    void AttackTiming()
    {
        player.GetComponent<Player>().hp -= this.atk;
        SlimeSurvivorManager.IsDamaged();
        Player.HitPlayer();

        GameObject skill = Instantiate(particlePrefab, player.transform);
        skill.GetComponent<AudioSource>().Play();
    }

    void LaterAttack()
    {
        Player.IdlePlayer();
        this.box.enabled = true;
    }
}
