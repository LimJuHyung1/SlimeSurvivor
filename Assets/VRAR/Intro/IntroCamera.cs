using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    public Transform target; // ī�޶� ������ �� ���� ���
    public float rotationSpeed = 2.0f;
    public float angleToLook = 30.0f; // �ٶ� ����
    public float distance = 3.0f; // ī�޶���� �Ÿ�

    void Start()
    {
        // Ư�� �������� Ư�� ������Ʈ�� �ٶ󺸰� �����մϴ�.
        transform.rotation = Quaternion.Euler(angleToLook, 0, 0);
        transform.position = target.position - transform.forward * distance; // ī�޶� ��ġ ����
    }

    void Update()
    {
        // ī�޶� ������ �� ���� ����� �ִٸ�
        if (target != null)
        {
            // ������Ʈ�� �߽����� ������ ȸ��
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
