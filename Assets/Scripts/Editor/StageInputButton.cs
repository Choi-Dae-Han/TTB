using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveAllData))]
public class StageInputButton : Editor
{
    SaveAllData SD;

    void OnEnable()
    {
        SD = target as SaveAllData;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("Reset Data"))
        {
            SD.ResetData();
        }


        if (GUILayout.Button("Reset Data(OpenStage)"))
        {
            SD.OpenAllStage();
        }
    }
}
