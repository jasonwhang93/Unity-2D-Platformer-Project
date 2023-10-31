using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GoalTileController : MonoBehaviour
{
    public Text coinText; // ���� ȹ���� ������ ������ ǥ���ϴ� Text ������Ʈ

    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾��� �Ӹ� �� ��ġ�� �������� cellPosition�� ����մϴ�.
            Vector3 headPosition = collision.bounds.center + new Vector3(0, collision.bounds.extents.y, 0);
            Vector3Int cellPosition = tilemap.WorldToCell(headPosition);

            // �÷��̾��� �� �Ʒ� ��ġ�� �������� cellPosition�� ����մϴ�.
            Vector3 footosition = collision.bounds.center - new Vector3(0, collision.bounds.extents.y, 0);
            Vector3Int cellPosition1 = tilemap.WorldToCell(footosition);

            // �÷��̾��� �����ʸ��� ������ ��ġ�� �������� cellPosition�� ����մϴ�.
            Vector3 rightSideosition = collision.bounds.center + new Vector3(collision.bounds.extents.x, 0, 0);
            Vector3Int cellPosition2 = tilemap.WorldToCell(rightSideosition);

            // �÷��̾��� ���ʸ��� ���� ��ġ�� �������� cellPosition�� ����մϴ�.
            Vector3 leftSideosition = collision.bounds.center - new Vector3(collision.bounds.extents.x, 0, 0);
            Vector3Int cellPosition3 = tilemap.WorldToCell(leftSideosition);

            if (tilemap.GetTile(cellPosition) == null && tilemap.GetTile(cellPosition1) == null && 
                tilemap.GetTile(cellPosition2) == null && tilemap.GetTile(cellPosition3) == null)
            {

            }
            else
            {
                // PlayerData�� ���� ȹ���� ���� ������ �����մϴ�.
                PlayerData.playerEarnCoin = int.Parse(coinText.text);

                // �� Ŭ���� ��ȣ�� �ִ� �ڵ� (��: ���� ������ ��ȯ)
                // TODO: ���⿡ �� Ŭ���� ��ȣ�� �ִ� �ڵ带 �߰��մϴ�.
                PlayerData.isMap2Cleared = true;
                SceneManager.LoadScene("MainVillage");
            }
        }
    }
}
