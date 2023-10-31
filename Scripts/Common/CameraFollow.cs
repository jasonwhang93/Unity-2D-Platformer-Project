using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // ��� (�÷��̾�)

    [Header("Follow Settings")]
    public float smoothSpeed = 3f;        // ī�޶� �̵��� �ε巯��
    public Vector2 offset = new Vector2(0, 2);     // ī�޶�� ��� ������ �Ÿ�
    public bool adaptiveFollowSpeed = true;       // ����� �ӵ��� ���� ī�޶� ���� �ӵ� ���� ����
    public float followSpeedMultiplier = 2f;       // ����� �ӵ��� ���� ī�޶� ���� �ӵ� ����

    [Header("Bounds Settings")]
    public bool enableBounds = true;  // �Ѱ� ���� ��� ����
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;

    private float cameraHalfWidth, cameraHalfHeight;

    private void Start()
    {
        adaptiveFollowSpeed = true;
        enableBounds = true;

        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;

        // ���� ���� �� ī�޶� ��ġ�� Ÿ�� ��ġ�� ����
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10);
        if (enableBounds)
        {
            desiredPosition = new Vector3(
                Mathf.Clamp(desiredPosition.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),
                Mathf.Clamp(desiredPosition.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight),
                -10);
        }
        transform.position = desiredPosition;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null) return;

        float currentFollowSpeed = smoothSpeed;

        if (adaptiveFollowSpeed)
        {
            float targetSpeed = target.GetComponent<Rigidbody2D>().velocity.magnitude;
            currentFollowSpeed += targetSpeed * followSpeedMultiplier;
        }

        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10);

        if (enableBounds)
        {
            desiredPosition = new Vector3(
                Mathf.Clamp(desiredPosition.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),
                Mathf.Clamp(desiredPosition.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight),
                -10);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * currentFollowSpeed);

        // �Ѱ迡 �������� �� �÷��̾��� ��ġ�� �����մϴ�.
        TargetClamp();
    }

    void TargetClamp()
    {
        // �÷��̾��� �ݶ��̴� ũ�⸦ ���մϴ� (����: BoxCollider2D�� ����Ѵٰ� ����)
        BoxCollider2D playerCollider = target.GetComponent<BoxCollider2D>();
        float playerWidth = playerCollider.size.x * target.localScale.x / 2; // �÷��̾� �ʺ��� ����
        float playerHeight = playerCollider.size.y * target.localScale.y / 2; // �÷��̾� ������ ����

        float targetMaxX = transform.position.x + cameraHalfWidth + playerWidth;
        float targetMinX = transform.position.x - cameraHalfWidth - playerWidth;
        float targetMaxY = transform.position.y + cameraHalfHeight + playerHeight;
        float targetMinY = transform.position.y - cameraHalfHeight - playerHeight;

        target.position = new Vector3(
            Mathf.Clamp(target.position.x, targetMinX, targetMaxX),
            Mathf.Clamp(target.position.y, targetMinY, targetMaxY),
            target.position.z);
    }
}