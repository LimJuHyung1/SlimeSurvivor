using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    private Dictionary<string, int> cubeToStatus 
        = new Dictionary<string, int>();
    private List<string> acivatedList = new List<string>();

    public GameObject skillManager;
    public static Action<GameObject> getCubeAction;

    void Start()
    {
        getCubeAction = (cube) => GetCube(cube);
    }

    public void GetCube(GameObject getCube) // ť�� ���
    {
        string cubeName = getCube.name.ToString();

        // ť�� ������ �°� �������ͽ� ���� �� ����
        if (!cubeToStatus.ContainsKey(cubeName))
        {
            cubeToStatus.Add(cubeName, 1);            
        }
        else
        {
            cubeToStatus[cubeName]++;
            if (cubeToStatus[cubeName] % 8 == 0)
            {
                // ť�� 10���� ��ų ������
                skillManager.GetComponent<SkillManager>().LevelUp(cubeName);                
            }
        }
        /*
        foreach (KeyValuePair<string, int> pair in cubeToStatus)
        {
            Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
        }*/
    }
}
