using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
    // �ټ� ������ ������ �����ϴ� ������ ��ü
    public enum FieldType
    {
        Forest,
        Hell,
        Winter,
        Candy,
        Desert
    }

    public GameObject[] forest;
    // 0 - ����
    // 1 - ����
    // 2 - ��
    // 3 - ����

    public GameObject[] hell;
    // 0 - ����
    // 1 - ������ũ
    // 2 - ����
    // 3 - ��

    public GameObject[] winter;
    // 0 - ����
    // 1 - �����
    // 2 - ����
    // 3 - ����

    public GameObject[] candy;
    // 0 - ����
    // 1 - ĵ��1
    // 2 - ĵ��2
    // 3 - ��

    public GameObject[] desert;
    // 0 - ����
    // 1 - ������
    // 2 - ����
    // 3 - ����        

    FieldType currentField; // ���� �� �ϳ��� ����
    private int randomInt;  // �ڿ� ���� Ƚ�� 
    float spawnAdj = 30.0f;     // �ʵ� �� �ڿ� ���� ��ġ ���� ����
    GameObject field;
    Vector3 []fieldArr = new Vector3[7]; // ó�� ���� ���� ��ġ �迭

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

        // �ʵ� ������Ʈ ���� �� ��ġ ����
        for (int i = 0; i < 7; i++)
        {
            CreateField(fieldArr[i]);
        }

        GenerateNewField = (spawn_vec) => CreateField(spawn_vec);
    }

    // ���� ȯ�濡 ���� ��ȯ
    public GameObject[] GetField(FieldType choose)
    {
        // ���� ���õ� ȯ�濡 ���� �迭 ��ȯ
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

    // �������� �ϳ��� FieldType�� ��ü �ε��� ���
    public FieldType GetRandomFieldType()
    {
        int enumLength = System.Enum.GetValues(typeof(FieldType)).Length;
        int randomIndex = UnityEngine.Random.Range(0, enumLength);
        return (FieldType)randomIndex;
    }    

    public void CreateField(Vector3 spwanVec)
    {
        // �ʵ� ������Ʈ ����
        currentField = GetRandomFieldType();
        GameObject field = Instantiate(GetField(currentField)[0]);

        // �ʵ� ��ġ ����
        field.transform.position = spwanVec;
        field.transform.localScale = new Vector3(100, 1, 100);

        // ���� �迭 ũ�� ����
        randomInt = (int)UnityEngine.Random.Range(4, 8);

        // �ڿ� ���� ����
        for (int j = 0; j < randomInt; j++)
        {
            int randomIndex = (int)UnityEngine.Random.Range(1, GetField(currentField).Length);   // ��� �ڿ��� �������� ���� (1 ~ 4)
            float randomRot = UnityEngine.Random.Range(0f, 180f);       // y�� ȸ�� ũ�� ����
            Vector3 newScale = new Vector3(0.15f, 10, 0.15f); // ���ο� ������ ���� ����

            // �ڿ� ����
            GameObject resource = Instantiate(GetField(currentField)[randomIndex]);
            // ������ �ڽ� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
            resource.transform.SetParent(field.transform);

            Vector3 randVec = new Vector3
                (UnityEngine.Random.Range(-spawnAdj, spawnAdj),
                -1.5f,
                UnityEngine.Random.Range(-spawnAdj, spawnAdj));
            resource.transform.position = field.transform.position + randVec;   // �ڽ� ������Ʈ�� ��ġ�� ���� ����
            resource.transform.Rotate(Vector3.up, randomRot);   // y�� �������� 0 ~ 180�� ����
            resource.transform.localScale = newScale;       // �ڽ� ������Ʈ�� ũ�⸦ ���� ����            
        }
    }
}
