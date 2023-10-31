using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public string connectSceneName; // ��ȯ�� ���� �̸�
    public int requiredQuestID; // �� ��ȯ�� �ʿ��� ����Ʈ ID

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (PlayerData.GetQuestStatus(requiredQuestID) == QuestStatus.InProgress)
                {
                    SceneManager.LoadScene(connectSceneName);
                }
                else
                {
                    Debug.Log("����Ű�� ���� ����Ʈ�� ���� ���� �����Դϴ�.");
                }
            }
        }
    }
}
