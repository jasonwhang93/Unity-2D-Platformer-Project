using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public AudioClip backgroundMusicClip; // ��� ���� Ŭ��
    public bool isGameEnd;
    public string sceneName;
    public int questID;

    // Start is called before the first frame update
    void Start()
    {
        // AudioManager �ν��Ͻ��� �ִ��� Ȯ��
        if (AudioManager.instance != null)
        {
            // ��� ���� ���
            AudioManager.instance.PlayMusic(backgroundMusicClip);
        }
        else
        {
            Debug.LogWarning("AudioManager not found.");
        }
    }

    private void Update()
    {
        // ������ ������ ���� ó��
        if (isGameEnd)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // ����Ʈ�� ���¸� Ŭ����� ����
        PlayerData.isMap1Cleared = true;

        isGameEnd = false;

        // MainVillage �� �ε�
        SceneManager.LoadScene("MainVillage");
    }
}
