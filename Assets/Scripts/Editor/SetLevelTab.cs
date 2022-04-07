using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SetLevelTab : EditorWindow
{
    private int _levelVal = 1;
    private int _maxLevelVal;

    [MenuItem("Tools/CanTemplate/SetLevel")]
    private static void ShowWindow()
    {
        var window = GetWindow<SetLevelTab>();
        window.titleContent = new GUIContent("Set Level");
        window.Show();
    }

    private void OnEnable()
    {
        _levelVal = PlayerPrefs.GetInt("next_level", 1);
        _maxLevelVal = PlayerPrefs.GetInt("max_level", 1);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Level", "The current level number."),
            GUILayout.Width(50));
        _levelVal = EditorGUILayout.IntField(_levelVal, GUILayout.Width(50));

        GUILayout.Space(20);

        EditorGUILayout.LabelField(new GUIContent("Max Level", "The max level number."),
            GUILayout.Width(62.5f));
        _maxLevelVal = EditorGUILayout.IntField(_maxLevelVal, GUILayout.Width(50));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set current level", GUILayout.Width(120), GUILayout.Height(30)))
        {
            PlayerPrefs.SetInt("next_level", _levelVal);
            int levelMod = _levelVal % _maxLevelVal;
            if (levelMod == 0) levelMod = _maxLevelVal;
            PlayerPrefs.SetInt("next_levelSc", levelMod);
        }

        if (GUILayout.Button("Set max level", GUILayout.Width(120), GUILayout.Height(30)))
            PlayerPrefs.SetInt("max_level", _maxLevelVal);

        GUILayout.EndHorizontal();

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