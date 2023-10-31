using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestContent : MonoBehaviour
{
    // �Է¹��� Quest ����
    [Header("Quest Info")]
    [SerializeField] private int questID;
    [SerializeField] private string questTitle;
    [SerializeField] private int rewardMoney;

    public Button questButton;

    private Quest quest; // ���� QuestContent�� ����� Quest ��ü

    public delegate void OnQuestClicked(QuestContent clickedQuestContent);
    public OnQuestClicked onQuestClickedCallback;

    private void Awake()
    {
        CreateQuest();  // Quest ��ü�� �ʱ�ȭ�մϴ�.
        questButton.onClick.AddListener(OnQuestButtonClickedInternal);
    }

    // Quest ��ü�� �����ϰ� �ʱ�ȭ�ϴ� �޼���
    private void CreateQuest()
    {
        quest = new Quest
        {
            questID = this.questID,
            questTitle = this.questTitle,
            rewardMoney = this.rewardMoney
        };
    }


    public void InitializeQuestContent(Quest newQuest)
    {
        quest = newQuest;
        questButton.onClick.AddListener(OnQuestButtonClicked);
    }

    private void OnQuestButtonClickedInternal()
    {
        OnQuestButtonClicked();  // ������ ������ ȣ��

        onQuestClickedCallback?.Invoke(this);  // �ݹ� ȣ��
    }

    public void Setup(Quest newQuest)
    {
        quest = newQuest;

        // ����Ʈ �����͸� UI ��ҿ� ���ε�
        // ��: titleText.text = quest.questTitle;
        // ������, ���� � ���⿡�� ������ �� �ֽ��ϴ�.
    }

    public Quest GetQuest()
    {
        if (quest == null)
        {
            CreateQuest();
        }
        return quest;
    }

    // �߰�������, Quest�� �����ϴ� ������ ��ư�� ������ �� �ֽ��ϴ�.
    public void OnQuestButtonClicked()
    {
        if (quest.status == QuestStatus.NotAccepted)
        {

        }
        // �߰����� ���� (��: ����Ʈ ���� �˸� ǥ��)
    }
}
