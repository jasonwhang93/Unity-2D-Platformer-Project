using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestUIController questUIController;  // QuestUIController ����

    public QuestContent levelOneQuestContentPrefab;  // LevelOne QuestContent ������
    public QuestContent levelTwoQuestContentPrefab;  // LevelTwo QuestContent ������

    private int selectedQuestID = 0;

    private void Start()
    {
        // ����� ����Ʈ �����͸� �����ɴϴ�.
        Quest savedQuest1 = PlayerData.GetSavedQuest(1);
        Quest savedQuest2 = PlayerData.GetSavedQuest(2);

        if (savedQuest1 == null)
            CreateQuestInstance(levelOneQuestContentPrefab.GetQuest());
        else
            CreateQuestInstance(savedQuest1); // ������ �κ�

        if (savedQuest2 == null)
            CreateQuestInstance(levelTwoQuestContentPrefab.GetQuest());
        else
            CreateQuestInstance(savedQuest2); // ������ �κ�

        EventManager.instance.AddEvent("Quest :: UIToggle", (p) =>
        {
            ToggleQuestUI();
        });

        // TODO: �� QuestContent�� �ν��Ͻ��� �ݹ��� �����ϵ��� ����
        // levelOneQuestContentPrefab.onQuestClickedCallback = OnQuestContentClicked;
        // levelTwoQuestContentPrefab.onQuestClickedCallback = OnQuestContentClicked;
    }

    private void Update()
    {
        CheckQuestIsCompleted();
    }

    private void OnQuestContentClicked(QuestContent clickedContent)
    {
        Debug.Log($"Clicked on quest: {clickedContent.GetQuest().questID}");

        // �ʿ��� �߰� ó���� ���⿡ �ֽ��ϴ�.
        selectedQuestID = clickedContent.GetQuest().questID;
    }

    // ��� â�� �ݴ� �Լ��� ȣ���ϴ� �Լ�
    public void OnCancelDialogButtonClicked()
    {
        questUIController.CloseAllWindows();
        selectedQuestID = 0;
    }

    public void OnNextButtonClicked()
    {
        if (selectedQuestID == 1)
        {
            questUIController.ShowLevel0neQuestDescriptDialogUI();
        }
        else if (selectedQuestID == 2)
        {
            questUIController.ShowLevelTwoQuestDescriptDialogUI();
        }
        else
        {
            // �ƹ��� ������ ���� �ʴ´�.
        }
    }

    public void OnAccecptButtonClicked()
    {
        // ����Ʈ�� �������� ���� ���� ����
        Quest selectedQuest = null;

        // ���õ� ����ƮID�� ���� �ش� ����Ʈ ��ü�� �����ɴϴ�.
        if (selectedQuestID == 1)
        {
            selectedQuest = levelOneQuestContentPrefab.GetQuest();
        }
        else if (selectedQuestID == 2)
        {
            selectedQuest = levelTwoQuestContentPrefab.GetQuest();
        }

        // ���õ� ����Ʈ�� ���� ���, �� ����Ʈ�� ���¸� '����'���� �����մϴ�.
        if (selectedQuest != null)
        {
            selectedQuest.AcceptQuest();
            OnCancelDialogButtonClicked();

            // ����Ʈ ����Ʈ UI�� ������Ʈ�մϴ�.
            questUIController.UpdateQuestList();
        }
    }

    public void OnPrevButtonClicked()
    {
        if (levelOneQuestContentPrefab.GetQuest().status == QuestStatus.NotAccepted ||
            levelTwoQuestContentPrefab.GetQuest().status == QuestStatus.NotAccepted)
        {
            questUIController.ShowAcceptableQuestsUI();
        }
    }

    public void OnCompleteButtonClick()
    {
        // ��� ����Ʈ�� ��ȸ
        Quest[] allQuests = { levelOneQuestContentPrefab.GetQuest(), levelTwoQuestContentPrefab.GetQuest() };
        foreach (Quest quest in allQuests)
        {
            // ����Ʈ ���°� Completed�̸�
            if (quest.status == QuestStatus.Completed)
            {
                // �÷��̾�� ���� ����
                PlayerData.playerMoney += quest.rewardMoney;

                // ����Ʈ ���¸� NotAccepted�� ����
                quest.status = QuestStatus.NotAccepted;
            }
        }

        // UI ����
        questUIController.UpdateQuestList();
        OnCancelDialogButtonClicked();
    }

    public void OnRejectButtonClick()
    {
        // ����Ʈ�� �������� ���� ���� ����
        Quest selectedQuest = null;

        // ���õ� ����ƮID�� ���� �ش� ����Ʈ ��ü�� �����ɴϴ�.
        if (selectedQuestID == 1)
        {
            selectedQuest = levelOneQuestContentPrefab.GetQuest();
        }
        else if (selectedQuestID == 2)
        {
            selectedQuest = levelTwoQuestContentPrefab.GetQuest();
        }

        // ���õ� ����Ʈ�� ���� ���, �� ����Ʈ�� ���¸� '����'���� �����մϴ�.
        if (selectedQuest != null)
        {
            selectedQuest.NotAcceptQuest();

            OnCancelDialogButtonClicked();

            // ����Ʈ ����Ʈ UI�� ������Ʈ�մϴ�.
            questUIController.UpdateQuestList();
        }
    }

    public void CheckQuestIsCompleted()
    {
        if (PlayerData.isMap1Cleared && levelOneQuestContentPrefab.GetQuest().status == QuestStatus.InProgress)
        {
            Debug.Log("1");
            levelOneQuestContentPrefab.GetQuest().CompleteQuest();
            questUIController.UpdateQuestList();
            PlayerData.isMap1Cleared = false; // ��1 Ŭ���� ���� �ʱ�ȭ
        }

        if (PlayerData.isMap2Cleared && levelTwoQuestContentPrefab.GetQuest().status == QuestStatus.InProgress)
        {
            Debug.Log("2");
            levelTwoQuestContentPrefab.GetQuest().CompleteQuest();
            questUIController.UpdateQuestList();
            PlayerData.isMap2Cleared = false; // ��2 Ŭ���� ���� �ʱ�ȭ
        }
    }

    public void CreateQuestInstance(Quest quest)
    {
        QuestContent questContentPrefab;

        // ����Ʈ ID�� ���� ������ QuestContent ������ ����
        if (quest.questID == 1)
        {
            questContentPrefab = levelOneQuestContentPrefab;
        }
        else
        {
            questContentPrefab = levelTwoQuestContentPrefab;
        }

        QuestContent questContentInstance = Instantiate(questContentPrefab);

        // �ν��Ͻ��� �ݹ� ����
        questContentInstance.onQuestClickedCallback = OnQuestContentClicked;

        switch (quest.status)
        {
            case QuestStatus.NotAccepted:
                questUIController.AddQuestToAcceptableList(questContentInstance);
                break;
            case QuestStatus.Accepted:
            case QuestStatus.InProgress:
                questUIController.AddQuestToInProgressList(questContentInstance);
                break;
            case QuestStatus.Completed:
                questUIController.AddQuestToCompleteList(questContentInstance);
                break;
        }

        PlayerData.SaveQuest(quest);
    }

    // QuestUIController�� â���� ����ϴ� �Լ�
    public void ToggleQuestUI(object param = null)
    {
        bool hasNotAcceptedQuests = false;
        bool hasAcceptedOrInProgressQuests = false;

        Quest[] allQuests = { levelOneQuestContentPrefab.GetQuest(), levelTwoQuestContentPrefab.GetQuest() };

        // Check the status of all quests
        foreach (Quest quest in allQuests)
        {
            if (quest.status == QuestStatus.NotAccepted)
            {
                hasNotAcceptedQuests = true;
            }
            else if (quest.status == QuestStatus.Accepted || quest.status == QuestStatus.InProgress || quest.status == QuestStatus.Completed)
            {
                hasAcceptedOrInProgressQuests = true;
            }
        }

        // Use QuestUIController methods based on quest status
        if (hasAcceptedOrInProgressQuests)
        {
            questUIController.ShowInProgressOrCompleteQuestsUI();
        }
        else if (hasNotAcceptedQuests)
        {
            questUIController.ShowAcceptableQuestsUI();
        }
        else
        {
            // Optionally, handle the case where all quests are completed or no quests are available
        }
    }
}
