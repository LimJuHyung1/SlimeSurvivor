using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public float radius = 100.25f; // ������
    public int numberOfPoints = 6; // ������ ����Ʈ ���� (60�� �����̹Ƿ� 360 / 60)
    Vector3[] aroundVecs;          // ���� ������ ��ǥ��
    bool isGenerated = false;      // ������ ��� ������ �����Ǿ� �ִ���

    Ray []rays;
    RaycastHit hit;    

    int fieldIndex; // �ش� ���� ������Ʈ�� � �������� �ľ��ϴ� ����
    float spawnTime = 0;
    float spawnMaxTime = 30f;   // ���� ��ȯ �ð�

    void Start()
    {
        aroundVecs = new Vector3[numberOfPoints];
        rays = new Ray[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            // 360���� numberOfPoints�� ����Ͽ� ��ǥ�� ���
            float angle = i * 360f / numberOfPoints;
            float radians = angle * Mathf.Deg2Rad;

            // x�� z ��ǥ�� ����Ͽ� �� ������ ��ǥ�� ����
            float x = this.gameObject.transform.position.x + radius * Mathf.Cos(radians);
            float z = this.gameObject.transform.position.z + radius * Mathf.Sin(radians);

            // 60�� �������� Ray�� �����Ͽ� ������ �����ϴ��� Ȯ���� �� �ֵ��� ����
            aroundVecs[i] = new Vector3(x, this.gameObject.transform.position.y, z) 
                + new Vector3(0, 5, 0);
            rays[i] = new Ray(aroundVecs[i], transform.up * -15);            
        }

        // ������ ���� �ε��� �з�
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

    void Update()       // ���͸� ��ȯ
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
        // ���� ���� ������ �ٸ� ������ 6�� �����ϴ��� Ȯ��
        if (isGenerated == false && other.CompareTag("Player"))
        {
            // ���� 6���� ������ ������ �������� ���� ������ ���� ����
            for (int i = 0; i < numberOfPoints; i++)
            {                
                if (!Physics.Raycast(rays[i], out hit))
                {
                    Generator.GenerateNewField(aroundVecs[i] - new Vector3(0, 5, 0));
                    // Debug.Log(this.gameObject.name + " - " + i.ToString() + "���� ����");                    
                }                
            }
            isGenerated = !isGenerated;
        }
    }    
}
