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

    public void GetCube(GameObject getCube) // 큐브 흡수
    {
        string cubeName = getCube.name.ToString();

        // 큐브 종류에 맞게 스테이터스 생성 및 증가
        if (!cubeToStatus.ContainsKey(cubeName))
        {
            cubeToStatus.Add(cubeName, 1);            
        }
        else
        {
            cubeToStatus[cubeName]++;
            if (cubeToStatus[cubeName] % 8 == 0)
            {
                // 큐브 10개당 스킬 레벨업
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
