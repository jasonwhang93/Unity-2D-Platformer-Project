using UnityEngine;

public enum QuestStatus
{
    NotAccepted,
    Accepted,
    InProgress,
    Completed
}

[System.Serializable]
public class Quest
{
    // Quest�� �⺻ ����
    public int questID;
    public string questTitle;

    // ����Ʈ�� ����
    public int rewardMoney;

    // ����Ʈ ����
    public QuestStatus status
    {
        get
        {
            return PlayerData.GetQuestStatus(questID);
        }
        set
        {
            PlayerData.SetQuestStatus(questID, value);
        }
    }

    public void NotAcceptQuest()
    {
        if (status == QuestStatus.NotAccepted)
        {
            status = QuestStatus.NotAccepted;

            // ���� ���� �� ����
            PlayerData.SaveQuest(this);
        }
    }

    // ����Ʈ ���� ������ ���� �׼� �޼����
    public void AcceptQuest()
    {
        if (status == QuestStatus.NotAccepted)
        {
            status = QuestStatus.Accepted;

            // ���� ���� �� ����
            PlayerData.SaveQuest(this);
        }
    }

    public void StartQuest()
    {
        if (status == QuestStatus.Accepted)
        {
            status = QuestStatus.InProgress;

            // ���� ���� �� ����
            PlayerData.SaveQuest(this);
        }
    }

    public void CompleteQuest()
    {
        if (status == QuestStatus.InProgress)
        {
            status = QuestStatus.Completed;

            // ���� ���� �� ����
            PlayerData.SaveQuest(this);
        }
    }

    public bool IsQuestCompleted()
    {
        return status == QuestStatus.Completed;
    }
}
