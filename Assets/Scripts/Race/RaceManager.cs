using System.Collections.Generic;
using UnityEngine;

public sealed class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }
    [SerializeField] private List<AnimalController> racers = new List<AnimalController>();
    [SerializeField, Min(10f)] private float finishDistance = 60f;
    public RaceProjectType SelectedProject { get; private set; }
    public bool RaceStarted { get; private set; }
    public bool RaceFinished { get; private set; }
    public IReadOnlyList<AnimalController> FinishOrder => finishOrder;
    public AnimalController PlayerRacer { get; private set; }

    private readonly List<AnimalController> finishOrder = new List<AnimalController>();
    private GameObject[] obstacles;
    private bool rewardGranted;

    private void Awake() { Instance = this; }

    public void Configure(List<AnimalController> entries, GameObject[] trackObstacles)
    {
        racers = entries ?? new List<AnimalController>();
        obstacles = trackObstacles;
        SelectedProject = GameFlowManager.Instance != null ? GameFlowManager.Instance.SelectedProject : RaceProjectType.Sprint;
        string selected = GameFlowManager.Instance != null ? GameFlowManager.Instance.SelectedAnimalName : "Rabbit";
        PlayerRacer = racers.Find(r => r != null && r.AnimalName == selected);
        if (PlayerRacer == null) PlayerRacer = racers.Find(r => r != null);
        foreach (AnimalController racer in racers)
            if (racer != null) racer.PrepareForRace(racer == PlayerRacer, finishDistance);
        SetObstacles(SelectedProject == RaceProjectType.Obstacle);
    }

    public void StartRace()
    {
        if (RaceStarted || RaceFinished || racers.Count == 0) return;
        finishOrder.Clear();
        RaceStarted = true;
        foreach (AnimalController racer in racers) if (racer != null) racer.SetRaceActive(true);
    }

    public void RegisterFinish(AnimalController racer)
    {
        if (racer == null || finishOrder.Contains(racer)) return;
        finishOrder.Add(racer);
        int count = racers.FindAll(r => r != null).Count;
        if (finishOrder.Count >= count) EndRace();
    }

    private void EndRace()
    {
        RaceFinished = true; RaceStarted = false;
        if (!rewardGranted && PlayerProgress.Instance != null && PlayerRacer != null)
        {
            int rank = finishOrder.IndexOf(PlayerRacer) + 1;
            if (rank > 0) PlayerProgress.Instance.ApplyPlayerReward(rank);
            rewardGranted = true;
        }
        FindObjectOfType<RaceSceneUIController>()?.ShowResults();
    }

    private void SetObstacles(bool active)
    {
        if (obstacles == null) return;
        foreach (GameObject obstacle in obstacles) if (obstacle != null) obstacle.SetActive(active);
    }
}
