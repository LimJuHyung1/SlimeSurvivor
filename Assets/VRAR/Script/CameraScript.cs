using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // 따라갈 대상 오브젝트의 Transform
    private float disY = 16;    // 대상 오브젝트로부터 Y거리
    private float disZ = -20;   // 대상 오브젝트로부터 Z거리
    Vector3 dis;

    void Start()
    {
        dis = new Vector3(0, disY, disZ);   
    }

    void Update()
    {
        if (target != null)
        {
            // 카메라 위치 설정
            this.transform.position = target.position + dis;

            // 카메라가 항상 대상 오브젝트를 바라보게끔 회전 설정
            transform.LookAt(target);
        }
    }
}
