using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NeedleSetting))]
public class NeedleSetting_Edit : Editor
{
    NeedleSetting NS;

    private void OnEnable()
    {
        NS = target as NeedleSetting;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("SetNeedle_Ascending"))
        {
            NS.SetNeedle_Ascending();
        }

        if (GUILayout.Button("SetNeedle_Descending"))
        {
            NS.SetNeedle_Decending();
        }

        if (GUILayout.Button("SetMuzzle_Ascending"))
        {
            NS.SetMuzzle_Ascending();
        }

        if (GUILayout.Button("SetMuzzle_Descending"))
        {
            NS.SetMuzzle_Decending();
        }
    }
}
