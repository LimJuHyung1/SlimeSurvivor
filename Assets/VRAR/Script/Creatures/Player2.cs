using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    private BoxCollider box;
    GameObject slime;
    Transform parent;

    void Start()
    {
        box = GetComponent<BoxCollider>();

        parent = transform.parent;

        if (parent != null)
        {
            slime = parent.gameObject;
            box = GetComponent<BoxCollider>();
        }
        else
        {
            Debug.Log("�θ� ������Ʈ�� �������� �ʽ��ϴ�.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.faceMode == 2)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }             
    }

    // �÷��̾ �������� ��� ����
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Resources"))
        {
            other.GetComponent<Resources>().ActivateParticleSystem();
        }
    }    
}
