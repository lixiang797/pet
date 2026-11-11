using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject animalPanel;
    [SerializeField] private GameObject projectPanel;

    [Header("Main buttons")]
    [SerializeField] private Button animalSelectButton;
    [SerializeField] private Button projectSelectButton;
    [SerializeField] private Button startRaceButton;
    [SerializeField] private Button quitButton;

    [Header("Animal buttons")]
    [SerializeField] private Button rabbitButton;
    [SerializeField] private Button turtleButton;
    [SerializeField] private Button monkeyButton;
    [SerializeField] private TMP_Text selectedAnimalText;

    [Header("Project buttons")]
    [SerializeField] private Button sprintButton;
    [SerializeField] private Button obstacleButton;
    [SerializeField] private TMP_Text selectedProjectText;

    [Header("Back buttons")]
    [SerializeField] private Button animalBackButton;
    [SerializeField] private Button projectBackButton;

    private void Start()
    {
        if (GameFlowManager.Instance == null)
        {
            Debug.LogError("MainMenu 场景需要一个 GameFlowManager 对象。");
            return;
        }

        animalSelectButton?.onClick.AddListener(() => ShowOnly(animalPanel));
        projectSelectButton?.onClick.AddListener(() => ShowOnly(projectPanel));
        startRaceButton?.onClick.AddListener(GameFlowManager.Instance.LoadRaceScene);
        quitButton?.onClick.AddListener(GameFlowManager.Instance.QuitGame);

        rabbitButton?.onClick.AddListener(() => SelectAnimal("Rabbit"));
        turtleButton?.onClick.AddListener(() => SelectAnimal("Turtle"));
        monkeyButton?.onClick.AddListener(() => SelectAnimal("Monkey"));

        sprintButton?.onClick.AddListener(() => SelectProject(RaceProjectType.Sprint));
        obstacleButton?.onClick.AddListener(() => SelectProject(RaceProjectType.Obstacle));

        animalBackButton?.onClick.AddListener(ShowMain);
        projectBackButton?.onClick.AddListener(ShowMain);

        ShowMain();
    }

    private void SelectAnimal(string animalName)
    {
        GameFlowManager.Instance.SelectAnimal(animalName);
        ShowMain();
    }

    private void SelectProject(RaceProjectType project)
    {
        GameFlowManager.Instance.SelectProject(project);
        ShowMain();
    }

    private void ShowMain()
    {
        ShowOnly(mainPanel);
        RefreshLabels();
    }

    private void ShowOnly(GameObject visiblePanel)
    {
        if (mainPanel != null) mainPanel.SetActive(mainPanel == visiblePanel);
        if (animalPanel != null) animalPanel.SetActive(animalPanel == visiblePanel);
        if (projectPanel != null) projectPanel.SetActive(projectPanel == visiblePanel);
    }

    private void RefreshLabels()
    {
        if (GameFlowManager.Instance == null)
            return;

        if (selectedAnimalText != null)
            selectedAnimalText.text = $"当前动物：{GameFlowManager.Instance.SelectedAnimalName}";

        if (selectedProjectText != null)
            selectedProjectText.text = $"当前项目：{GameFlowManager.Instance.SelectedProject}";
    }
}
