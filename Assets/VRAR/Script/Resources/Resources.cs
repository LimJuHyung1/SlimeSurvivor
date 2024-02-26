using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    // �����ӿ��� �ڿ� �ο�
    // ��ӵǴ� �ڿ� ������ ���� �ٸ� ���� �ο�

    private ParticleSystem particle;    // ���ݴ����� �� �߻��Ǵ� ��ƼŬ
    private AudioSource audio;          // ���ݴ����� �� �鸮�� �Ҹ�
    public AudioClip removeSound;       // �ı��Ǿ��� �� �鸮�� �Ҹ�
    public GameObject cube;             // �ı��Ǿ��� �� ����� �ڿ�

    public int lifeCount;       // ���?
    private MeshRenderer meshRenderer;

    public enum ResourceType        // + �ı��� ������ ť�� ����
    {
        // ���� �ڿ�
        Tree,       // 2
        Flower,     // 1
        Stone,      // 2
        
        // Hell
        Castle,     // 5
        Spike,      // 3

        // Winter
        Snowman,    // 3

        // Candy
        Candy,      // 3
        Cake,       // 4

        // Desert
        Cactus,     // 3
        Hill        // 4
    }

    public ResourceType resourceType;
    int cubeNum = 0;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();        

        if (particle == null)
        {
            Debug.LogError("Particle System ������Ʈ�� ã�� �� �����ϴ�.");
        }

        SetCubeNum();
    }

    public void ActivateParticleSystem()    // ��ƼŬ ����
    {
        if (particle != null)
        {
            // Particle System ����
            particle.Play();
            this.lifeCount -= 1;

            if(this.lifeCount == 0)
            {
                audio.clip = removeSound; // ����� AudioClip ����
                audio.Play();

                InstantiateCube();
                Invoke("Removed", 1f);
            }

            audio.Play();            
        }
    }

    void InstantiateCube()      // ť�� ����
    {
        // ť�� ����
        for(int i = 0; i < cubeNum; i++)  
        {
            GameObject resourceCube = Instantiate(cube);

            resourceCube.transform.position = this.transform.position 
                + new Vector3((int)UnityEngine.Random.Range(0,2)
                , 5f
                , (int)UnityEngine.Random.Range(0, 2));
        }
    }

    void SetCubeNum()   // ������ ť�� ���� ����
    {
        switch (this.resourceType)
        {
            case ResourceType.Tree:
                cubeNum = 3;
                break;
            case ResourceType.Flower:
                cubeNum = 2;
                break;
            case ResourceType.Stone:
                cubeNum = 3;
                break;

            case ResourceType.Castle:
                cubeNum = 6;
                break;
            case ResourceType.Spike:
                cubeNum = 4;
                break;

            case ResourceType.Snowman:
                cubeNum = 4;
                break;

            case ResourceType.Candy:
                cubeNum = 4;
                break;
            case ResourceType.Cake:
                cubeNum = 5;
                break;

            case ResourceType.Cactus:
                cubeNum = 4;
                break;
            case ResourceType.Hill:
                cubeNum = 5;
                break;

            default:
                Debug.Log("ResourceType Error");
                cubeNum = 0;
                break;
        }
    }

    void Removed()
    {
        this.gameObject.SetActive(false);
    }
}
