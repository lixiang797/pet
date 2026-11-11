using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TrainingPanelController : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button staminaButton;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        if (speedButton != null)
            speedButton.onClick.AddListener(TrainSpeed);

        if (jumpButton != null)
            jumpButton.onClick.AddListener(TrainJump);

        if (staminaButton != null)
            staminaButton.onClick.AddListener(TrainStamina);

        UpdateCoinText();
    }

    private void TrainSpeed()
    {
        if (PlayerProgress.Instance == null)
            return;

        if (PlayerProgress.Instance.Coins < 50)
        {
            infoText.text = "金币不足，不能训练速度。";
            return;
        }

        PlayerProgress.Instance.AddCoins(-50);
        AnimalController player = FindAnimalByName(PlayerProgress.Instance.SelectedAnimalName);
        if (player != null)
        {
            player.GetComponent<AnimalController>().PrepareForRace(true, 60f);
            player.SetRaceActive(false);
        }

        infoText.text = "速度训练完成，冲刺更强了！";
        UpdateCoinText();
    }

    private void TrainJump()
    {
        if (PlayerProgress.Instance == null)
            return;

        if (PlayerProgress.Instance.Coins < 50)
        {
            infoText.text = "金币不足，不能训练跳跃。";
            return;
        }

        PlayerProgress.Instance.AddCoins(-50);
        infoText.text = "跳跃训练完成，障碍更轻松！";
        UpdateCoinText();
    }

    private void TrainStamina()
    {
        if (PlayerProgress.Instance == null)
            return;

        if (PlayerProgress.Instance.Coins < 50)
        {
            infoText.text = "金币不足，不能训练耐力。";
            return;
        }

        PlayerProgress.Instance.AddCoins(-50);
        infoText.text = "耐力训练完成，后程更稳！";
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null && PlayerProgress.Instance != null)
            coinText.text = $"金币：{PlayerProgress.Instance.Coins}";
    }

    private AnimalController FindAnimalByName(string animalName)
    {
        RaceManager manager = FindObjectOfType<RaceManager>();
        if (manager == null)
            return null;

        foreach (AnimalController racer in manager.racers)
        {
            if (racer != null && racer.AnimalName == animalName)
                return racer;
        }

        return null;
    }
}
