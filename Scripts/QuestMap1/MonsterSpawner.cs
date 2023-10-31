using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject spawnArea; // ���� ���� ���� (MonsterSpawnBox ������Ʈ)
    private Transform parentSpawnTransform;
    private Camera mainCamera;

    public float spawnRate = 2f; // ���� ���� ����
    public float nextSpawnTime = 1f; // ���� ���� ���� �ð�

    private List<GameObject> spawnedMonsters = new List<GameObject>(); // ������ ���� ���

    public Text currentScoreText; // ���� �ؽ�Ʈ UI
    public Text maxScoreText;
    public Image heart1, heart2, heart3; // ���� ����� ��Ÿ���� ��Ʈ �̹��� UI

    private bool isGameEnded = false;

    public MapController mapController;

    private void Start()
    {
        parentSpawnTransform = spawnArea.GetComponent<Transform>();
        mainCamera = Camera.main;

        Debug.Log("�ְ� ����: " + PlayerData.playerMaxScore);

        InitGame();

        // �ʱ� UI ����
        UpdateUI();
    }

    private void Update()
    {
        if (!isGameEnded && Time.time >= nextSpawnTime)
        {
            int numberOfMonstersToSpawn = Random.Range(1, 3);

            for (int i = 0; i < numberOfMonstersToSpawn; i++)
            {
                SpawnMonster();
            }

            nextSpawnTime = Time.time + 1f / spawnRate;
        }

        // �÷��̾��� ���� ��� Ȯ��
        if (!isGameEnded)
        {
            CheckPlayerRemainingHearts();
        }

        // UI�� ������Ʈ�ϴ� �Լ� ȣ��
        UpdateUI();
    }

    private void InitGame()
    {
        isGameEnded = false;
        PlayerData.playerCurrentScore = 0;
        PlayerData.playerRemainHeart = 3;
    }


    private void SpawnMonster()
    {
        GameObject[] monsterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Monster");
        int randomIndex = Random.Range(0, monsterPrefabs.Length);
        GameObject monsterPrefab = monsterPrefabs[randomIndex];

        Vector3 spawnPosition = GetRandomSpawnPosition();

        if (spawnPosition != Vector3.zero)
        {
            GameObject newObject = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            newObject.transform.SetParent(parentSpawnTransform);

            spawnedMonsters.Add(newObject);

            // ���͸� ī�޶� �Ʒ� ������ ����� �ı��ϵ��� ����
            StartCoroutine(DestroyWhenOutOfCameraBounds(newObject));
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnArea.GetComponent<Collider2D>().bounds.min.x, spawnArea.GetComponent<Collider2D>().bounds.max.x),
                spawnArea.GetComponent<Collider2D>().bounds.max.y,
                0f
            );

            bool overlap = false;

            // ������ ���͵�� �浹 �˻�
            foreach (GameObject monster in spawnedMonsters)
            {
                // Null üũ�� �߰��մϴ�.
                if (monster != null)
                {
                    Collider2D monsterCollider = monster.GetComponent<Collider2D>();

                    if (monsterCollider != null)
                    {
                        if (monsterCollider.bounds.Contains(spawnPosition))
                        {
                            overlap = true;
                            break;
                        }
                    }
                }
            }

            if (!overlap)
            {
                return spawnPosition;
            }

            attempts++;
        }

        return Vector3.zero;
    }

    private IEnumerator DestroyWhenOutOfCameraBounds(GameObject obj)
    {
        while (true)
        {
            // obj�� null�� �ƴϰ� ���� �ı����� �ʾ��� ��
            if (obj != null && obj.transform.position.y < mainCamera.transform.position.y - mainCamera.orthographicSize)
            {
                spawnedMonsters.Remove(obj);
                Destroy(obj);
                PlayerData.playerCurrentScore++;
                yield break;
            }
            yield return null;
        }
    }

    public void ReducePlayerLife()
    {
        PlayerData.playerRemainHeart--;
        UpdateUI();
        CheckPlayerRemainingHearts();
    }


    private void CheckPlayerRemainingHearts()
    {
        if (PlayerData.playerRemainHeart <= 0)
        {
            isGameEnded = true;
            EndGame();
        }
    }


    private void EndGame()
    {
        // ���� ���� ó��
        // ������ ���͵��� ��� �ı�
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null)
            {
                Destroy(monster);
            }
        }

        // ������ ���� ��� �ʱ�ȭ
        spawnedMonsters.Clear();

        if (PlayerData.playerMaxScore <= PlayerData.playerCurrentScore)
        {
            PlayerData.playerMaxScore = PlayerData.playerCurrentScore;
        }

        // AudioManager �ν��Ͻ��� �ִ��� Ȯ�� �� ���� ����
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.StopMusic();
        }

        mapController.isGameEnd = true;
    }

    // UI ������Ʈ �Լ�
    private void UpdateUI()
    {
        // ���� ǥ��
        currentScoreText.text = PlayerData.playerCurrentScore.ToString();
        maxScoreText.text = PlayerData.playerMaxScore.ToString();

        // ���� ��� ǥ�� (��Ʈ �̹��� Ȱ��ȭ/��Ȱ��ȭ)
        heart1.enabled = PlayerData.playerRemainHeart >= 1;
        heart2.enabled = PlayerData.playerRemainHeart >= 2;
        heart3.enabled = PlayerData.playerRemainHeart >= 3;
    }
}
