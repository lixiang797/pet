using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PetGameSceneCreator
{
    [MenuItem("Tools/Pet Game/Create Demo Scenes")]
    public static void CreateDemoScenes()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        if (!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets", "Scenes");
        CreateScene("MainMenu", false);
        CreateScene("AnimalRaceScene", true);
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
            new EditorBuildSettingsScene("Assets/Scenes/AnimalRaceScene.unity", true)
        };
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Pet Game", "Demo scenes created. Open MainMenu and press Play.", "OK");
    }

    private static void CreateScene(string name, bool raceScene)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject bootstrap = new GameObject("GameBootstrap");
        GameBootstrap component = bootstrap.AddComponent<GameBootstrap>();
        component.raceScene = raceScene;
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/" + name + ".unity");
    }
}
