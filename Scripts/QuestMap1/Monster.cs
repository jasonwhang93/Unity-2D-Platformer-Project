using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���Ϳ� �÷��̾� ���� �浹 ó��
        if (collision.gameObject.CompareTag("Player"))
        {
            // MonsterSpawner�� ReducePlayerLife �޼��带 ȣ��
            MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
            if (spawner != null)
            {
                spawner.ReducePlayerLife();
            }

            // ���͸� �ı��մϴ�.
            Destroy(gameObject);
        }
    }
}
