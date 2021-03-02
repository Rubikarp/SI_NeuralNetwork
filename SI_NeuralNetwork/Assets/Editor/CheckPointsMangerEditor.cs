using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CheckPointManager))]
public class CheckPointsMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheckPointManager selected = (CheckPointManager)target;
        if (GUILayout.Button("InitChecpoints"))
        {
            selected.Initialisation();
        }
    }
}
