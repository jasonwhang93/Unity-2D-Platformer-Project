using UnityEngine;

public class Map2CameraFollow : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public float xLeftLimit = -10f;  // ���� ����
    public float xRightLimit = 10f;  // ������ ����
    public float yTopLimit = 10f;    // ���� ����
    public float yBottomLimit = -10f; // �Ʒ��� ����
    private bool isCameraLocked = false;

    private void LateUpdate()  // LateUpdate�� ����Ͽ� �÷��̾��� �������� ��� ó���� �� ī�޶� �����Դϴ�.
    {
        if (player)
        {
            Vector3 newCameraPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            // �������� ������ ��
            if (player.position.x < transform.position.x && !isCameraLocked)
            {
                isCameraLocked = true;
            }

            // ���������� �����̷��� �� ��
            if (player.position.x > transform.position.x && isCameraLocked)
            {
                isCameraLocked = false;
            }

            if (isCameraLocked)
            {
                newCameraPosition.x = transform.position.x;
            }

            // ī�޶� �̵� ����
            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, xLeftLimit, xRightLimit);
            newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, yBottomLimit, yTopLimit);

            transform.position = newCameraPosition;
        }
    }
}
