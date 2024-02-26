using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // ���� ��� ������Ʈ�� Transform
    private float disY = 16;    // ��� ������Ʈ�κ��� Y�Ÿ�
    private float disZ = -20;   // ��� ������Ʈ�κ��� Z�Ÿ�
    Vector3 dis;

    void Start()
    {
        dis = new Vector3(0, disY, disZ);   
    }

    void Update()
    {
        if (target != null)
        {
            // ī�޶� ��ġ ����
            this.transform.position = target.position + dis;

            // ī�޶� �׻� ��� ������Ʈ�� �ٶ󺸰Բ� ȸ�� ����
            transform.LookAt(target);
        }
    }
}
