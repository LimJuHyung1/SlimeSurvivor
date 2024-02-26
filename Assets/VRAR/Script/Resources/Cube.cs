using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    SphereCollider sphere;
    Rigidbody rigid;
    AudioSource audio;

    GameObject player;
    bool isEnd = false;

    float rotationSpeed = 30.0f; // 회전 속도

    void Start()
    {
        sphere = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        player = GameObject.Find("Slime_01");
    }

    void Update()
    {
        RotateCube();
    }

    void RotateCube()
    {
        // 회전
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어 y축 고정
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            player.GetComponent<Rigidbody>().constraints = 
                RigidbodyConstraints.FreezeRotationX | 
                RigidbodyConstraints.FreezeRotationY | 
                RigidbodyConstraints.FreezeRotationZ;
            this.transform.position = Vector3.MoveTowards
                (this.transform.position, player.transform.position, 0.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isEnd == false)
        {
            isEnd = true;
            audio.Play();
            ResourceHandler.getCubeAction(returnCube());
            Invoke("Removed", 0.5f);
        }
    }

    GameObject returnCube()
    {
        return this.gameObject;
    }

    void Removed()
    {
        player.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
        player.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;
        this.gameObject.SetActive(false);
    }
}
