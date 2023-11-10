using UnityEngine;
using UnityEditor;

public class CopyComponentsToTargetPrefab : EditorWindow
{
    private GameObject sourcePrefab;
    private GameObject targetPrefab;
    private string sourceTag;
    private int sourceLayer;

    [MenuItem("Shepherd0619/Copy Components to Prefab")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CopyComponentsToTargetPrefab));
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy Components to Prefab", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Source Prefab", GUILayout.Width(100));
        sourcePrefab = (GameObject)EditorGUILayout.ObjectField(sourcePrefab, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Target Prefab", GUILayout.Width(100));
        targetPrefab = (GameObject)EditorGUILayout.ObjectField(targetPrefab, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Copy Components"))
        {
            if (sourcePrefab == null || targetPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select source and target prefabs.", "OK");
                return;
            }

            CopyScripts();
        }
    }

    private void CopyScripts()
    {
        Component[] sourceComponents = sourcePrefab.GetComponents<Component>();

        foreach (Component component in sourceComponents)
        {
            if (component.GetType() == typeof(Transform))
                continue;

            UnityEditorInternal.ComponentUtility.CopyComponent(component);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetPrefab);
        }

        targetPrefab.tag = sourceTag;
        targetPrefab.layer = sourceLayer;

        PrefabUtility.SavePrefabAsset(targetPrefab);
        EditorUtility.DisplayDialog("Success", "Scripts, tag, and layer copied and saved to target prefab.", "OK");
    }
}