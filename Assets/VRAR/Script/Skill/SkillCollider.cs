using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    BoxCollider box;

    public GameObject player;
    public int skillLevel = 0;    // 배포시 0으로 세팅하기!
    public static bool isDrain = false;
    public bool amIBuff = true;
    float walkSpeedOrigin;

    void Start()
    {
        player = GameObject.Find("Slime_01");

        if (this.name != "Debuff" && this.name != "Healing")
        {
            box = GetComponent<BoxCollider>();
            amIBuff = false;
        }            
    }

    void OnTriggerEnter(Collider other)
    {
        player.GetComponent<Player>().isSkillAttack = true;
        player.GetComponent<Player>().Attack();
        if (other.CompareTag("Monster"))
        {
            switch (this.name)
            {
                case "Meteors AOE": //메테오 효과
                    other.GetComponent<Monsters>().DamagedHp(10, skillLevel);
                    break;

                case "Snow AOE":    // 아이스 스피어 효과
                    other.GetComponent<Monsters>().DamagedHp(6, skillLevel);
                    other.GetComponent<Monsters>().walkSpeed /= 2;
                    break;

                case "Holy hit":    // 라이트닝 스트라이크 효과
                    other.GetComponent<Monsters>().DamagedHp(8, skillLevel);
                    break;

                case "Green hit":   // 슬래쉬 효과
                    if (ZeroOrOne() == 0)
                    {
                        other.GetComponent<Monsters>().DamagedHp(5, skillLevel);
                    }
                    else if (ZeroOrOne() == 1)
                    {
                        other.GetComponent<Monsters>().DamagedHp(5, skillLevel * 2);
                    }
                    break;

                case "Explosion":   // 해머 크래쉬 효과
                    walkSpeedOrigin = other.GetComponent<Monsters>().walkSpeed;
                    other.GetComponent<Monsters>().DamagedHp(8, skillLevel);

                    if(other != null)
                    {
                        other.GetComponent<Monsters>().walkSpeed = 0;
                        Invoke("StunOff", 1 * skillLevel + 1);
                    }                        
                    break;
            }
            if (isDrain)
            {
                GameObject player = GameObject.Find("Slime_01");
                player.GetComponent<Player>().hp += 2 * skillLevel;
            }
        }
        else if (other.CompareTag("Dragon"))
        {
            switch (this.name)
            {
                case "Meteors AOE": //메테오 효과
                    other.GetComponent<Dragons>().DamagedHp(10, skillLevel);
                    break;

                case "Snow AOE":    // 아이스 스피어 효과
                    other.GetComponent<Dragons>().DamagedHp(6, skillLevel);
                    other.GetComponent<Dragons>().walkSpeed /= 2;
                    break;

                case "Holy hit":    // 라이트닝 스트라이크 효과
                    other.GetComponent<Dragons>().DamagedHp(8, skillLevel);
                    break;

                case "Green hit":   // 슬래쉬 효과
                    if (ZeroOrOne() == 0)
                    {
                        other.GetComponent<Dragons>().DamagedHp(5, skillLevel);
                    }
                    else if (ZeroOrOne() == 1)
                    {
                        other.GetComponent<Dragons>().DamagedHp(5, skillLevel * 2);
                    }
                    break;

                case "Explosion":   // 해머 크래쉬 효과
                    walkSpeedOrigin = other.GetComponent<Dragons>().walkSpeed;
                    other.GetComponent<Dragons>().DamagedHp(8, skillLevel);
                    if (other != null)
                    {
                        other.GetComponent<Dragons>().walkSpeed = 0;
                        Invoke("StunOff", 1 + skillLevel);
                    }
                    break;
            }
            if (isDrain)
            {
                GameObject player = GameObject.Find("Slime_01");
                player.GetComponent<Player>().hp += 2 * skillLevel;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
            other.GetComponent<Monsters>().Idle();
    }

    int ZeroOrOne()
    {
        System.Random random = new System.Random();

        return random.Next(0, 2); // 0과 1 중 랜덤값 반환
    }

    void StunOff(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            if (other.GetComponent<Monsters>().hp > 0)
                other.GetComponent<Monsters>().walkSpeed = walkSpeedOrigin;
        }
        else if(other.gameObject.CompareTag("Dragon"))
        {
            if (other.GetComponent<Dragons>().hp > 0)
                other.GetComponent<Dragons>().walkSpeed = walkSpeedOrigin;
        }        
    }
}
