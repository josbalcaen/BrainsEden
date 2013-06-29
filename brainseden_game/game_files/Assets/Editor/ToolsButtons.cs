using UnityEditor;
using System.Collections;
using UnityEngine;

public class ToolsButtons: EditorWindow
{
	private GameObject _prefab;
	private GameObject _anchor_point;

    [MenuItem("Window/Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ToolsButtons),false,"Tools");
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Create anchor points from selected"))
        {
            _prefab = GameObject.FindGameObjectWithTag("Manager").GetComponent<AnchorGenerator>().prefab;
			GameObject[] _selection= Selection.gameObjects;
			for (int i = 0; i < _selection.Length; i++)
			{
				_anchor_point = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;
				//_anchor_point = Instantiate(_prefab, _selection[i].transform.position, Quaternion.identity) as GameObject;
				_anchor_point.transform.position = _selection[i].transform.position;
				_anchor_point.transform.parent = _selection[i].transform;
			}

        }
		/*
        if (GUILayout.Button("Conditions"))
        {
            ConditionEditor.ShowWindow();
        }
		if (GUILayout.Button("Level Order"))
        {
            LevelOrderEditor.ShowWindow();
        }*/
//        if (GUILayout.Button("Localisation"))
//        {
//            LocalisationEditor.ShowWindow();
//        }
//        if (GUILayout.Button("Event Order"))
//        {
//            EventOrderEditor.ShowWindow();
//        }
//        if (GUILayout.Button("Cleanup Assets"))
//        {
//            CleanupProject.ShowWindow();
//        }


        GUILayout.EndVertical();
    }
}
