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

    public GameObject particlePrefab; // ��ƼŬ �������� ���⿡ �Ҵ��ϼ���.
    private GameObject spawnedParticle; // ������ ��ƼŬ ������Ʈ�� �����ϱ� ���� ����

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        if (this.gameObject.CompareTag("Monster"))
        {
            Debug.Log("�÷��̾�� �������� �־����ϴ�!");
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            Debug.Log("���Ϳ��� �������� �־����ϴ�!");
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
        // ���ϴ� ��ġ�� ��ƼŬ ����
        spawnedParticle = Instantiate(particlePrefab, playerPos, Quaternion.identity);

        spawnedParticle.GetComponent<ParticleSystem>().Play();
    }

    void PlayParticle(GameObject tmp)
    {
        // ���ϴ� ��ġ�� ��ƼŬ ����
        spawnedParticle = Instantiate(particlePrefab, playerPos, Quaternion.identity);

        spawnedParticle.GetComponent<ParticleSystem>().Play();
    }

    public void StopParticle()
    {
        // ��ƼŬ ��Ȱ��ȭ
        if (spawnedParticle != null)
        {
            var particleSystem = spawnedParticle.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
            else
            {
                Debug.LogError("��ƼŬ �ý����� ã�� �� �����ϴ�.");
            }
        }
    }
}
