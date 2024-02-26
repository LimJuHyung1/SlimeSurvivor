using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public float radius = 100.25f; // 반지름
    public int numberOfPoints = 6; // 생성할 포인트 개수 (60도 간격이므로 360 / 60)
    Vector3[] aroundVecs;          // 주위 지형의 좌표값
    bool isGenerated = false;      // 주위에 모든 지형이 생성되어 있는지

    Ray []rays;
    RaycastHit hit;    

    int fieldIndex; // 해당 지형 오브젝트가 어떤 지형인지 파악하는 변수
    float spawnTime = 0;
    float spawnMaxTime = 30f;   // 몬스터 소환 시간

    void Start()
    {
        aroundVecs = new Vector3[numberOfPoints];
        rays = new Ray[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            // 360도를 numberOfPoints로 등분하여 좌표를 계산
            float angle = i * 360f / numberOfPoints;
            float radians = angle * Mathf.Deg2Rad;

            // x와 z 좌표를 계산하여 원 주위의 좌표를 얻음
            float x = this.gameObject.transform.position.x + radius * Mathf.Cos(radians);
            float z = this.gameObject.transform.position.z + radius * Mathf.Sin(radians);

            // 60도 간격으로 Ray를 생성하여 지형이 존재하는지 확인할 수 있도록 설정
            aroundVecs[i] = new Vector3(x, this.gameObject.transform.position.y, z) 
                + new Vector3(0, 5, 0);
            rays[i] = new Ray(aroundVecs[i], transform.up * -15);            
        }

        // 지형에 따라 인덱스 분류
        switch (this.gameObject.name)
        {
            case "Hex10(Clone)":
                fieldIndex = 0;
                return;
            case "Hex28(Clone)":
                fieldIndex = 1;
                return;
            case "Hex22(Clone)":
                fieldIndex = 2;
                return;
            case "Hex30(Clone)":
                fieldIndex = 3;
                return;
            case "Hex6(Clone)":
                fieldIndex = 4;
                return;

            default:
                fieldIndex = -1;
                return;            
        }
    }

    void Update()       // 몬스터를 소환
    {
        if(SlimeSurvivorManager.totalMonsterCount <= 200)
        {
            spawnTime += Time.deltaTime;

            if (spawnTime > spawnMaxTime && spawnMaxTime > 5)
            {
                SlimeSurvivorManager.spawnMonster(fieldIndex, this.transform.position);
                spawnTime = 0;
                spawnMaxTime -= 2;
            }
        }        
    }
    
    void OnTriggerEnter(Collider other)
    {
        // 현재 지형 주위에 다른 지형이 6개 존재하는지 확인
        if (isGenerated == false && other.CompareTag("Player"))
        {
            // 주위 6개의 지점중 지형이 존재하지 않은 지점에 지형 생성
            for (int i = 0; i < numberOfPoints; i++)
            {                
                if (!Physics.Raycast(rays[i], out hit))
                {
                    Generator.GenerateNewField(aroundVecs[i] - new Vector3(0, 5, 0));
                    // Debug.Log(this.gameObject.name + " - " + i.ToString() + "지형 생성");                    
                }                
            }
            isGenerated = !isGenerated;
        }
    }    
}
