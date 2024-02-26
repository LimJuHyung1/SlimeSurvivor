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

        player = GameObject.Find("Slime_01");       // 플레이어(슬라임)을 참조할 수 있도록 설정
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
            transform.LookAt(player.gameObject.transform);  // 몬스터의 방향이 플레이어(슬라임)을 바라봄

            // 공격 애니메이션이 실행되지 않는 상태라면 플레이어를 찾아 이동
            if (isStop == false && this.hp > 0)
            {
                // Vector3.MoveTowards(자신의 위치, 플레이어의 위치, 이동 속도)
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
            Invoke("Dead", 1.25f);      // 1.25초 후 삭제됨
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
