using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    public Transform target; // 카메라가 주위를 돌 고정 대상
    public float rotationSpeed = 2.0f;
    public float angleToLook = 30.0f; // 바라볼 각도
    public float distance = 3.0f; // 카메라와의 거리

    void Start()
    {
        // 특정 각도에서 특정 오브젝트를 바라보게 설정합니다.
        transform.rotation = Quaternion.Euler(angleToLook, 0, 0);
        transform.position = target.position - transform.forward * distance; // 카메라 위치 조정
    }

    void Update()
    {
        // 카메라가 주위를 돌 고정 대상이 있다면
        if (target != null)
        {
            // 오브젝트를 중심으로 주위를 회전
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
