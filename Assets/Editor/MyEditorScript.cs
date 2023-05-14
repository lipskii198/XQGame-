using UnityEditor;
class MyEditorScript
{
     static void PerformBuild ()
     {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "build/Windows/Game.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.scenes = new[] { "Assets/_game/Scenes/MainMenu.unity", "Assets/_game/Scenes/AntsLevel.unity", "Assets/_game/Scenes/BossTest.unity", "Assets/_game/Scenes/Intro.unity"};
        BuildPipeline.BuildPlayer(buildPlayerOptions);
     }
}