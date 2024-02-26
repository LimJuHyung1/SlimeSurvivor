using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : Creature
{
    GameObject player;
    Animator anim;  // Animator ������Ʈ ����
    BoxCollider box;
    AudioSource audio;

    public AudioClip deathSound;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
        audio = GetComponent<AudioSource>();
        anim.SetBool("isWalking", true);

        player = GameObject.Find("Slime_01");       // �÷��̾�(������)�� ������ �� �ֵ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {                        
            transform.LookAt(player.gameObject.transform);  // ������ ������ �÷��̾�(������)�� �ٶ�

            // ���� �ִϸ��̼��� ������� �ʴ� ���¶�� �÷��̾ ã�� �̵�
            if (!this.anim.GetBool("isAttacking") && this.hp > 0)
            {
                // Vector3.MoveTowards(�ڽ��� ��ġ, �÷��̾��� ��ġ, �̵� �ӵ�)
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
            Invoke("Dead", 1.25f);      // 1.25�� �� ������
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
