using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SetLevelTab : EditorWindow
{
    private int levelVal = 1;

    [MenuItem("Tools/PokeMaster/SetLevel")]
    private static void ShowWindow()
    {
        var window = GetWindow<SetLevelTab>();
        window.titleContent = new GUIContent("Set Level");
        window.Show();
    }

    private void OnEnable() => levelVal = PlayerPrefs.GetInt("next_level");

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Level", "The current level number."),
            GUILayout.Width(50));
        levelVal = EditorGUILayout.IntField(levelVal, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Set progress", GUILayout.Width(120), GUILayout.Height(30)))
        {
            PlayerPrefs.SetInt("next_level", levelVal);
            int levelMod = levelVal % 20;
            if (levelMod == 0) levelMod = 20;
            PlayerPrefs.SetInt("next_levelSc", levelMod);
        }

        GUILayout.Space(15);

        EditorGUILayout.LabelField("PlayerPrefs", EditorStyles.boldLabel);
        if (GUILayout.Button("Delete PlayerPrefs", GUILayout.Width(120), GUILayout.Height(30)))
            PlayerPrefs.DeleteAll();

        GUILayout.Space(15);

        EditorGUILayout.LabelField("EditorPrefs", EditorStyles.boldLabel);
        if (GUILayout.Button("Delete EditorPrefs", GUILayout.Width(120), GUILayout.Height(30)))
            EditorPrefs.DeleteAll();
    }
}
#endif