using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Creature : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float atk;
    public float walkSpeed;
    public float atkDelay;
    private Vector3 playerPos;

    public GameObject particlePrefab; // 파티클 프리팹을 여기에 할당하세요.
    private GameObject spawnedParticle; // 생성된 파티클 오브젝트를 저장하기 위한 변수

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        if (this.gameObject.CompareTag("Monster"))
        {
            Debug.Log("플레이어에게 데미지를 주었습니다!");
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            Debug.Log("몬스터에게 데미지를 주었습니다!");
        }
    }

    public virtual void Hit()
    {

    }

    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }

    public void PlayParticle(Vector3 position)
    {
        // 원하는 위치에 파티클 생성
        spawnedParticle = Instantiate(particlePrefab, playerPos, Quaternion.identity);

        spawnedParticle.GetComponent<ParticleSystem>().Play();
    }

    void PlayParticle(GameObject tmp)
    {
        // 원하는 위치에 파티클 생성
        spawnedParticle = Instantiate(particlePrefab, playerPos, Quaternion.identity);

        spawnedParticle.GetComponent<ParticleSystem>().Play();
    }

    public void StopParticle()
    {
        // 파티클 비활성화
        if (spawnedParticle != null)
        {
            var particleSystem = spawnedParticle.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
            else
            {
                Debug.LogError("파티클 시스템을 찾을 수 없습니다.");
            }
        }
    }
}
