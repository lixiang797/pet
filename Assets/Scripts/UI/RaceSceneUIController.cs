using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class RaceSceneUIController : MonoBehaviour
{
    [Header("Race UI")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button backToMenuButton;

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        startButton?.onClick.AddListener(StartRace);
        backToMenuButton?.onClick.AddListener(LoadMenu);
        replayButton?.onClick.AddListener(Replay);
        menuButton?.onClick.AddListener(LoadMenu);

        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (statusText != null)
        {
            RaceProjectType project = RaceManager.Instance != null
                ? RaceManager.Instance.SelectedProject
                : RaceProjectType.Sprint;
            statusText.text = $"准备比赛：{project}";
        }

        RefreshRanking();
    }

    private void Update()
    {
        if (RaceManager.Instance == null)
            return;

        if (RaceManager.Instance.RaceStarted && statusText != null)
            statusText.text = "比赛进行中！WASD 移动，空格跳跃";

        RefreshRanking();
    }

    private void StartRace()
    {
        if (RaceManager.Instance == null)
        {
            Debug.LogError("AnimalRaceScene 场景中找不到 RaceManager。");
            return;
        }

        if (startButton != null)
            startButton.gameObject.SetActive(false);

        RaceManager.Instance.StartRace();
    }

    private void RefreshRanking()
    {
        if (rankText == null || RaceManager.Instance == null)
            return;

        var order = RaceManager.Instance.FinishOrder;
        if (order.Count == 0)
        {
            rankText.text = "完赛名次将在这里显示";
            return;
        }

        StringBuilder text = new StringBuilder("已完赛：\n");
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] != null)
                text.AppendLine($"{i + 1}. {order[i].AnimalName}");
        }
        rankText.text = text.ToString();
    }

    public void ShowResults()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (statusText != null)
            statusText.text = "比赛结束！";

        if (resultText == null || RaceManager.Instance == null)
            return;

        StringBuilder text = new StringBuilder("比赛结果\n");
        var order = RaceManager.Instance.FinishOrder;
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] != null)
                text.AppendLine($"第 {i + 1} 名：{order[i].AnimalName}");
        }
        resultText.text = text.ToString();
    }

    private void Replay()
    {
        GameFlowManager.Instance?.ReplayRace();
    }

    private void LoadMenu()
    {
        GameFlowManager.Instance?.LoadMenuScene();
    }
}
