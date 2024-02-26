using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Dragons : Creature
{
    Animator anim;
    GameObject player;
    BoxCollider[] box;
    AudioSource audio;

    bool isStop = false;

    public AudioClip deathSound;
    public AudioClip attackSound;

    void Start()
    {
        anim = GetComponent<Animator>();
        box = gameObject.GetComponents<BoxCollider>();
        audio = GetComponent<AudioSource>();

        player = GameObject.Find("Slime_01");       // �÷��̾�(������)�� ������ �� �ֵ��� ����
    }

    public override void Hit()
    {
        anim.SetBool("isHit", true);
        Invoke("ChangeToWalk", 2f);
    }

    void ChangeToWalk()
    {
        anim.SetBool("isHit", false);
    }

    public override void Dead()
    {
        int randSpawnIndex = (int)UnityEngine.Random.Range(5, 8);
        SlimeSurvivorManager.returnCube(this.transform, randSpawnIndex);

        base.Dead();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player.gameObject.transform);  // ������ ������ �÷��̾�(������)�� �ٶ�

            // ���� �ִϸ��̼��� ������� �ʴ� ���¶�� �÷��̾ ã�� �̵�
            if (isStop == false && this.hp > 0)
            {
                // Vector3.MoveTowards(�ڽ��� ��ġ, �÷��̾��� ��ġ, �̵� �ӵ�)
                this.transform.position = Vector3.MoveTowards
                (this.transform.position, player.transform.position, 0.01f * this.walkSpeed);
            }
        }
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
            this.box[0].enabled = false;
            isStop = true;
            audio.clip = attackSound;
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
        SpecialEffect();
        GameObject skill = Instantiate(particlePrefab, player.transform);
        skill.GetComponent<AudioSource>().Play();
    }

    void SpecialEffect()
    {
        switch (this.particlePrefab.name)
        {
            case "Stone slash":
                this.atk += 1;
                break;
            case "Snow AOE":
                player.GetComponent<Player>().walkSpeed /= 2;
                Invoke("ReturnOriginPlayerSpeed", 5f);
                break;
            case "Meteors AOE":
                if(player.GetComponent<Player>().maxHp > 3)
                    player.GetComponent<Player>().maxHp -= 3;
                break;
            case "Explosion":
                if(this.atkDelay > 1)
                    this.atkDelay -= 0.25f;
                break;
        }
    }

    void ReturnOriginPlayerSpeed()
    {
        player.GetComponent<Player>().walkSpeed *= 2;
    }

    void LaterAttack()
    {
        Player.IdlePlayer();
        isStop = false;
        this.box[0].enabled = true;
    }
}
