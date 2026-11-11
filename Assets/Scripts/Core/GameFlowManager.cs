using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }
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

    public void SelectAnimal(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) SelectedAnimalName = name;
    }

    public void SelectProject(RaceProjectType project) => SelectedProject = project;
    public void LoadRaceScene() => SceneManager.LoadScene("AnimalRaceScene");
    public void LoadMenuScene() => SceneManager.LoadScene("MainMenu");
    public void ReplayRace() => SceneManager.LoadScene("AnimalRaceScene");
    public void QuitGame() => Application.Quit();
}
