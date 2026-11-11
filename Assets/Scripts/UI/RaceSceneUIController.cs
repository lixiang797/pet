using System.Text;
using UnityEngine;
using UnityEngine.UI;

public sealed class RaceSceneUIController : MonoBehaviour
{
    public Text statusText;
    public Text rankText;
    public Text skillText;
    public Button startButton;
    public GameObject resultPanel;
    public Text resultText;
    public Button replayButton;
    public Button menuButton;

    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(() => RaceManager.Instance?.StartRace());
        if (replayButton != null) replayButton.onClick.AddListener(() => GameFlowManager.Instance?.ReplayRace());
        if (menuButton != null) menuButton.onClick.AddListener(() => GameFlowManager.Instance?.LoadMenuScene());
        if (resultPanel != null) resultPanel.SetActive(false);
    }
    private void Update()
    {
        RaceManager race = RaceManager.Instance;
        if (race == null) return;
        if (statusText != null) statusText.text = race.RaceStarted ? "GO!  A/D move · Space jump · Left Shift skill" : "Ready: " + race.SelectedProject;
        if (race.PlayerRacer != null && skillText != null)
        {
            float cd = race.PlayerRacer.SkillCooldown;
            skillText.text = race.PlayerRacer.SkillName + (cd <= 0 ? " READY [Left Shift]" : " cooldown " + cd.ToString("0.0") + "s");
        }
        RefreshRank(race);
    }
    private void RefreshRank(RaceManager race)
    {
        if (rankText == null) return;
        StringBuilder text = new StringBuilder("Finished:\n");
        for (int i = 0; i < race.FinishOrder.Count; i++)
            if (race.FinishOrder[i] != null) text.AppendLine((i + 1) + ". " + race.FinishOrder[i].AnimalName);
        rankText.text = text.ToString();
    }
    public void ShowResults()
    {
        if (resultPanel != null) resultPanel.SetActive(true);
        if (resultText == null || RaceManager.Instance == null) return;
        StringBuilder text = new StringBuilder("RACE RESULTS\n");
        for (int i = 0; i < RaceManager.Instance.FinishOrder.Count; i++)
            if (RaceManager.Instance.FinishOrder[i] != null) text.AppendLine($"{i + 1}. {RaceManager.Instance.FinishOrder[i].AnimalName}");
        int rank = RaceManager.Instance.PlayerRacer != null ? RaceManager.Instance.FinishOrder.IndexOf((AnimalController)RaceManager.Instance.PlayerRacer) + 1 : 0;
        if (rank > 0) text.AppendLine("\nReward: " + (rank == 1 ? 120 : rank == 2 ? 80 : 40) + " coins");
        resultText.text = text.ToString();
    }
}
