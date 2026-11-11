using System.Collections.Generic;
using UnityEngine;

public sealed class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }

    [SerializeField] private List<AnimalController> racers = new List<AnimalController>();
    [SerializeField, Min(1f)] private float finishDistance = 60f;

    public RaceProjectType SelectedProject { get; private set; } = RaceProjectType.Sprint;
    public bool RaceStarted { get; private set; }
    public bool RaceFinished { get; private set; }
    public IReadOnlyList<AnimalController> FinishOrder => finishOrder;

    private readonly List<AnimalController> finishOrder = new List<AnimalController>();
    private AnimalController playerRacer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameFlowManager.Instance != null)
        {
            SelectedProject = GameFlowManager.Instance.SelectedProject;
            string selectedName = GameFlowManager.Instance.SelectedAnimalName;
            playerRacer = racers.Find(racer => racer != null && racer.AnimalName == selectedName);
        }

        if (playerRacer == null)
        {
            playerRacer = racers.Find(racer => racer != null);
            Debug.LogWarning("未找到菜单选择的动物，已选择 racers 列表中的第一个动物作为玩家角色。");
        }

        foreach (AnimalController racer in racers)
        {
            if (racer != null)
                racer.PrepareForRace(racer == playerRacer, finishDistance);
        }
    }

    public void StartRace()
    {
        if (RaceStarted || RaceFinished)
            return;

        if (racers.Count == 0 || racers.TrueForAll(racer => racer == null))
        {
            Debug.LogError("RaceManager 的 racers 列表为空，请在 Inspector 中添加动物。");
            return;
        }

        finishOrder.Clear();
        RaceStarted = true;
        foreach (AnimalController racer in racers)
        {
            if (racer != null)
                racer.SetRaceActive(true);
        }
    }

    public void RegisterFinish(AnimalController racer)
    {
        if (racer == null || finishOrder.Contains(racer))
            return;

        finishOrder.Add(racer);
        int activeRacerCount = racers.FindAll(item => item != null).Count;
        if (finishOrder.Count >= activeRacerCount)
            EndRace();
    }

    private void EndRace()
    {
        RaceFinished = true;
        RaceStarted = false;

        RaceSceneUIController ui = FindObjectOfType<RaceSceneUIController>();
        if (ui != null)
            ui.ShowResults();
    }
}
