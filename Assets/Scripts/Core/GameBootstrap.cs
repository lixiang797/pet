using UnityEngine;
using UnityEngine.UI;

/// <summary>Creates a complete primitive-art demo at runtime. Generate the two scenes with Tools/Pet Game/Create Demo Scenes.</summary>
public sealed class GameBootstrap : MonoBehaviour
{
    public bool raceScene;
    private void Awake()
    {
        EnsureManagers();
        if (raceScene) BuildRace(); else BuildMenu();
    }
    private static void EnsureManagers()
    {
        if (GameFlowManager.Instance == null) new GameObject("GameFlowManager").AddComponent<GameFlowManager>();
        if (PlayerProgress.Instance == null) new GameObject("PlayerProgress").AddComponent<PlayerProgress>();
    }
    private void BuildMenu()
    {
        Canvas canvas = CreateCanvas();
        Text title = MakeText(canvas.transform, "PET SPORTS DAY", 40, new Vector2(0, 250));
        MakeText(canvas.transform, "Choose an animal and event, then race!", 20, new Vector2(0, 205));
        Text status = MakeText(canvas.transform, "", 18, new Vector2(0, -260));
        Button animalButton = MakeButton(canvas.transform, "Animal: Rabbit", new Vector2(0, 140));
        Button eventButton = MakeButton(canvas.transform, "Event: Sprint", new Vector2(0, 80));
        Button trainButton = MakeButton(canvas.transform, "Train selected animal (-50 coins)", new Vector2(0, 20));
        Button shopButton = MakeButton(canvas.transform, "Unlock next animal", new Vector2(0, -40));
        Button startButton = MakeButton(canvas.transform, "START RACE", new Vector2(0, -110));
        Button quitButton = MakeButton(canvas.transform, "QUIT", new Vector2(0, -170));
        string[] animals = { "Rabbit", "Turtle", "Monkey", "Elephant", "Fox", "Penguin" };
        int animalIndex = 0;
        animalButton.onClick.AddListener(() =>
        {
            for (int n = 0; n < animals.Length; n++)
            {
                animalIndex = (animalIndex + 1) % animals.Length;
                if (PlayerProgress.Instance.IsUnlocked(animals[animalIndex])) break;
            }
            GameFlowManager.Instance.SelectAnimal(animals[animalIndex]);
            animalButton.GetComponentInChildren<Text>().text = "Animal: " + animals[animalIndex];
        });
        eventButton.onClick.AddListener(() =>
        {
            RaceProjectType next = GameFlowManager.Instance.SelectedProject == RaceProjectType.Sprint ? RaceProjectType.Obstacle : RaceProjectType.Sprint;
            GameFlowManager.Instance.SelectProject(next);
            eventButton.GetComponentInChildren<Text>().text = "Event: " + next;
        });
        trainButton.onClick.AddListener(() =>
        {
            bool trained = PlayerProgress.Instance.Train(GameFlowManager.Instance.SelectedAnimalName, true);
            status.text = trained ? "Speed training complete!" : "Need 50 coins to train.";
        });
        string[] shopAnimals = { "Elephant", "Fox", "Penguin" };
        int[] prices = { 250, 300, 280 };
        shopButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < shopAnimals.Length; i++)
            {
                if (!PlayerProgress.Instance.IsUnlocked(shopAnimals[i]))
                {
                    if (PlayerProgress.Instance.SpendCoins(prices[i]))
                    {
                        PlayerProgress.Instance.UnlockAnimal(shopAnimals[i]);
                        status.text = shopAnimals[i] + " unlocked!";
                    }
                    else status.text = "Need " + prices[i] + " coins.";
                    return;
                }
            }
            status.text = "All shop animals unlocked!";
        });
        startButton.onClick.AddListener(() => GameFlowManager.Instance.LoadRaceScene());
        quitButton.onClick.AddListener(() => GameFlowManager.Instance.QuitGame());
    }
    private void BuildRace()
    {
        CreateWorld();
        RaceManager race = new GameObject("RaceManager").AddComponent<RaceManager>();
        var racers = new System.Collections.Generic.List<AnimalController>();
        string[] names = { "Rabbit", "Turtle", "Monkey" };
        float[] speeds = { 7.3f, 6.0f, 6.7f };
        float[] jumps = { 6.5f, 5.2f, 8.0f };
        float[] powers = { 2.2f, 3.2f, 2.5f };
        float[] lanes = { -2.5f, 0f, 2.5f };
        for (int i = 0; i < names.Length; i++)
        {
            AnimalData data = ScriptableObject.CreateInstance<AnimalData>();
            AnimalSkill skill = ScriptableObject.CreateInstance<AnimalSkill>();
            AnimalSkillType kind = i == 0 ? AnimalSkillType.Sprint : i == 1 ? AnimalSkillType.Recover : AnimalSkillType.Jump;
            skill.Configure(kind, i == 2 ? 1.7f : 1.55f, 2f);
            data.Configure(names[i], speeds[i] + PlayerProgress.Instance.GetSpeedTraining(names[i]) * 0.25f,
                jumps[i] + PlayerProgress.Instance.GetJumpTraining(names[i]) * 0.25f, 100f, powers[i], skill);
            GameObject animal = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            animal.name = names[i]; animal.transform.position = new Vector3(lanes[i], 1f, 0f);
            animal.GetComponent<Renderer>().material.color = i == 0 ? new Color(1f, .55f, .4f) : i == 1 ? new Color(.4f, .75f, .45f) : new Color(.55f, .55f, 1f);
            Rigidbody body = animal.AddComponent<Rigidbody>(); body.mass = 1f;
            AnimalController controller = animal.AddComponent<AnimalController>(); controller.SetAnimalData(data);
            racers.Add(controller);
        }
        GameObject[] obstacles = new GameObject[4];
        for (int i = 0; i < obstacles.Length; i++)
        {
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = "Obstacle " + (i + 1);
            obstacle.transform.position = new Vector3((i % 2 == 0 ? -1.2f : 1.2f), .8f, 12f + i * 11f);
            obstacle.transform.localScale = new Vector3(1.3f, 1.6f, .8f);
            obstacle.GetComponent<Collider>().isTrigger = true;
            obstacle.AddComponent<Obstacle>(); obstacles[i] = obstacle;
        }
        race.Configure(racers, obstacles);
        BuildRaceUI();
        CreateCamera();
    }
    private static void CreateWorld()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground"; ground.transform.position = new Vector3(0, -.5f, 30f); ground.transform.localScale = new Vector3(12f, 1f, 75f);
        ground.tag = "Ground";
        for (int lane = -1; lane <= 1; lane++)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.name = "Lane"; line.transform.position = new Vector3(lane * 2.5f, .02f, 30f); line.transform.localScale = new Vector3(.04f, .03f, 70f);
            Collider col = line.GetComponent<Collider>(); if (col != null) Destroy(col);
            line.GetComponent<Renderer>().material.color = Color.white;
        }
        GameObject finish = GameObject.CreatePrimitive(PrimitiveType.Cube);
        finish.name = "Finish Line"; finish.transform.position = new Vector3(0, .05f, 61f); finish.transform.localScale = new Vector3(9f, .08f, .25f);
        Collider finishCollider = finish.GetComponent<Collider>(); if (finishCollider != null) Destroy(finishCollider);
        finish.GetComponent<Renderer>().material.color = Color.yellow;
    }
    private void BuildRaceUI()
    {
        Canvas canvas = CreateCanvas();
        Text status = MakeText(canvas.transform, "READY", 22, new Vector2(0, 300));
        Text ranking = MakeText(canvas.transform, "", 18, new Vector2(-490, 180)); ranking.alignment = TextAnchor.UpperLeft;
        Text skill = MakeText(canvas.transform, "", 18, new Vector2(420, 300));
        Button start = MakeButton(canvas.transform, "START", new Vector2(0, -285));
        Button back = MakeButton(canvas.transform, "MENU", new Vector2(-500, -300));
        GameObject panel = new GameObject("ResultPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);
        RectTransform rect = panel.GetComponent<RectTransform>(); rect.sizeDelta = new Vector2(500, 400); rect.anchoredPosition = Vector2.zero;
        panel.GetComponent<Image>().color = new Color(0, 0, 0, .88f);
        Text result = MakeText(panel.transform, "RESULT", 25, new Vector2(0, 80));
        Button replay = MakeButton(panel.transform, "RACE AGAIN", new Vector2(0, -100));
        Button menu = MakeButton(panel.transform, "MAIN MENU", new Vector2(0, -160));
        RaceSceneUIController ui = canvas.gameObject.AddComponent<RaceSceneUIController>();
        ui.statusText = status; ui.rankText = ranking; ui.skillText = skill; ui.startButton = start;
        ui.resultPanel = panel; ui.resultText = result; ui.replayButton = replay; ui.menuButton = menu;
        back.onClick.AddListener(() => GameFlowManager.Instance.LoadMenuScene());
    }
    private static Canvas CreateCanvas()
    {
        GameObject go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        return canvas;
    }
    private static Text MakeText(Transform parent, string value, int size, Vector2 position)
    {
        GameObject go = new GameObject(value + " Text", typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>(); rect.sizeDelta = new Vector2(700, 60); rect.anchoredPosition = position;
        Text text = go.GetComponent<Text>(); text.text = value; text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); text.fontSize = size; text.alignment = TextAnchor.MiddleCenter; text.color = Color.white; text.horizontalOverflow = HorizontalWrapMode.Overflow;
        return text;
    }
    private static Button MakeButton(Transform parent, string label, Vector2 position)
    {
        GameObject go = new GameObject(label + " Button", typeof(RectTransform), typeof(Image), typeof(Button)); go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>(); rect.sizeDelta = new Vector2(320, 48); rect.anchoredPosition = position;
        Image image = go.GetComponent<Image>(); image.color = new Color(.12f, .38f, .55f, .95f);
        Button button = go.GetComponent<Button>(); button.targetGraphic = image;
        Text text = MakeText(go.transform, label, 18, Vector2.zero); RectTransform textRect = text.GetComponent<RectTransform>(); textRect.sizeDelta = rect.sizeDelta;
        return button;
    }
    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera"; cameraObject.transform.position = new Vector3(0, 10, -13); cameraObject.transform.rotation = Quaternion.Euler(25, 0, 0);
        Camera cam = cameraObject.GetComponent<Camera>(); cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.45f, .7f, .9f);
    }
}
