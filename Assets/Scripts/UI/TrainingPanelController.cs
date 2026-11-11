using UnityEngine;
using UnityEngine.UI;

public sealed class TrainingPanelController : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Text messageText;
    [SerializeField] private Text coinsText;
    private void Start()
    {
        if (speedButton != null) speedButton.onClick.AddListener(() => Train(true));
        if (jumpButton != null) jumpButton.onClick.AddListener(() => Train(false));
        Refresh();
    }
    private void Train(bool speed)
    {
        if (PlayerProgress.Instance == null) return;
        bool done = PlayerProgress.Instance.Train(PlayerProgress.Instance.SelectedAnimalName, speed);
        if (messageText != null) messageText.text = done ? "Training complete!" : "Not enough coins.";
        Refresh();
    }
    private void Refresh()
    {
        if (coinsText != null && PlayerProgress.Instance != null) coinsText.text = "Coins: " + PlayerProgress.Instance.Coins;
    }
}
