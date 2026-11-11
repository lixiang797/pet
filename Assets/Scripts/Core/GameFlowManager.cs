using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Persists menu selections while switching between scenes.</summary>
public sealed class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private string raceSceneName = "AnimalRaceScene";

    public string SelectedAnimalName { get; private set; } = "Rabbit";
    public RaceProjectType SelectedProject { get; private set; } = RaceProjectType.Sprint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectAnimal(string animalName)
    {
        if (!string.IsNullOrWhiteSpace(animalName))
            SelectedAnimalName = animalName;
    }

    public void SelectProject(RaceProjectType project)
    {
        SelectedProject = project;
    }

    public void LoadRaceScene()
    {
        SceneManager.LoadScene(raceSceneName);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void ReplayRace()
    {
        SceneManager.LoadScene(raceSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
