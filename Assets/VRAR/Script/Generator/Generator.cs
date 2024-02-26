using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
    // 다섯 가지의 지형을 관리하는 열거형 객체
    public enum FieldType
    {
        Forest,
        Hell,
        Winter,
        Candy,
        Desert
    }

    public GameObject[] forest;
    // 0 - 지형
    // 1 - 나무
    // 2 - 꽃
    // 3 - 바위

    public GameObject[] hell;
    // 0 - 지형
    // 1 - 스파이크
    // 2 - 나무
    // 3 - 성

    public GameObject[] winter;
    // 0 - 지형
    // 1 - 눈사람
    // 2 - 나무
    // 3 - 바위

    public GameObject[] candy;
    // 0 - 지형
    // 1 - 캔디1
    // 2 - 캔디2
    // 3 - 꽃

    public GameObject[] desert;
    // 0 - 지형
    // 1 - 선인장
    // 2 - 바위
    // 3 - 나무        

    FieldType currentField; // 지형 중 하나를 선택
    private int randomInt;  // 자원 생성 횟수 
    float spawnAdj = 30.0f;     // 필드 위 자원 생성 위치 조절 변수
    GameObject field;
    Vector3 []fieldArr = new Vector3[7]; // 처음 생성 지형 위치 배열

    public static Action<Vector3> GenerateNewField;

    void Start()
    {
        fieldArr[0] = new Vector3(-50.25f, 0, 86.75f);
        fieldArr[1] = new Vector3(50.25f, 0, 86.75f);
        fieldArr[2] = new Vector3(-100.5f, 0, 0);
        fieldArr[3] = new Vector3(0, 0, 0);
        fieldArr[4] = new Vector3(100.5f, 0, 0);
        fieldArr[5] = new Vector3(-50.25f, 0, -86.75f);
        fieldArr[6] = new Vector3(50.25f, 0, -86.75f);

        // 필드 오브젝트 생성 및 위치 지정
        for (int i = 0; i < 7; i++)
        {
            CreateField(fieldArr[i]);
        }

        GenerateNewField = (spawn_vec) => CreateField(spawn_vec);
    }

    // 현재 환경에 대해 반환
    public GameObject[] GetField(FieldType choose)
    {
        // 현재 선택된 환경에 대한 배열 반환
        switch (choose)
        {
            case FieldType.Forest:
                return forest;
            case FieldType.Hell:
                return hell;
            case FieldType.Winter:
                return winter;
            case FieldType.Candy:
                return candy;
            case FieldType.Desert:
                return desert;
            default:
                return null;
        }
    }

    // 무작위로 하나의 FieldType의 객체 인덱스 출력
    public FieldType GetRandomFieldType()
    {
        int enumLength = System.Enum.GetValues(typeof(FieldType)).Length;
        int randomIndex = UnityEngine.Random.Range(0, enumLength);
        return (FieldType)randomIndex;
    }    

    public void CreateField(Vector3 spwanVec)
    {
        // 필드 오브젝트 생성
        currentField = GetRandomFieldType();
        GameObject field = Instantiate(GetField(currentField)[0]);

        // 필드 위치 지정
        field.transform.position = spwanVec;
        field.transform.localScale = new Vector3(100, 1, 100);

        // 스폰 배열 크기 설정
        randomInt = (int)UnityEngine.Random.Range(4, 8);

        // 자원 생성 과정
        for (int j = 0; j < randomInt; j++)
        {
            int randomIndex = (int)UnityEngine.Random.Range(1, GetField(currentField).Length);   // 어느 자원을 생성할지 지정 (1 ~ 4)
            float randomRot = UnityEngine.Random.Range(0f, 180f);       // y축 회전 크기 설정
            Vector3 newScale = new Vector3(0.15f, 10, 0.15f); // 새로운 스케일 값을 설정

            // 자원 생성
            GameObject resource = Instantiate(GetField(currentField)[randomIndex]);
            // 생성한 자식 오브젝트를 부모 오브젝트의 자식으로 설정
            resource.transform.SetParent(field.transform);

            Vector3 randVec = new Vector3
                (UnityEngine.Random.Range(-spawnAdj, spawnAdj),
                -1.5f,
                UnityEngine.Random.Range(-spawnAdj, spawnAdj));
            resource.transform.position = field.transform.position + randVec;   // 자식 오브젝트의 위치를 랜덤 지정
            resource.transform.Rotate(Vector3.up, randomRot);   // y축 기준으로 0 ~ 180도 변경
            resource.transform.localScale = newScale;       // 자식 오브젝트의 크기를 랜덤 지정            
        }
    }
}
