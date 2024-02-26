using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    // 슬라임에게 자원 부여
    // 상속되는 자원 종류에 따라 다른 스탯 부여

    private ParticleSystem particle;    // 공격당했을 때 발생되는 파티클
    private AudioSource audio;          // 공격당했을 때 들리는 소리
    public AudioClip removeSound;       // 파괴되었을 때 들리는 소리
    public GameObject cube;             // 파괴되었을 때 생기는 자원

    public int lifeCount;       // 목숨?
    private MeshRenderer meshRenderer;

    public enum ResourceType        // + 파괴시 생성할 큐브 개수
    {
        // 공통 자원
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
            Debug.LogError("Particle System 컴포넌트를 찾을 수 없습니다.");
        }

        SetCubeNum();
    }

    public void ActivateParticleSystem()    // 파티클 실행
    {
        if (particle != null)
        {
            // Particle System 실행
            particle.Play();
            this.lifeCount -= 1;

            if(this.lifeCount == 0)
            {
                audio.clip = removeSound; // 재생할 AudioClip 설정
                audio.Play();

                InstantiateCube();
                Invoke("Removed", 1f);
            }

            audio.Play();            
        }
    }

    void InstantiateCube()      // 큐브 생성
    {
        // 큐브 생성
        for(int i = 0; i < cubeNum; i++)  
        {
            GameObject resourceCube = Instantiate(cube);

            resourceCube.transform.position = this.transform.position 
                + new Vector3((int)UnityEngine.Random.Range(0,2)
                , 5f
                , (int)UnityEngine.Random.Range(0, 2));
        }
    }

    void SetCubeNum()   // 생성할 큐브 개수 설정
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
