using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoNamer))]
public class AutoNamer_Edit : Editor
{
    AutoNamer AN;

    private void OnEnable()
    {
        AN = target as AutoNamer;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("ChangeName"))
        {
            AN.AutoNaming();
        }
        if (GUILayout.Button("SetNeedle_DownToUp"))
        {
            AN.SetNeedle_DownToUp();
        }
        if (GUILayout.Button("SetNeedle_UpToDown"))
        {
            AN.SetNeedle_UpToDown();
        }     
        if (GUILayout.Button("SetInfo"))
        {
            AN.SetInfo();
        }      
        //if (GUILayout.Button("SaveToPrefab"))
        //{
        //    SaveTOPrefab();
        //}
    }

    //public void SaveTOPrefab()
    //{
    //    for (int i = 0; i < AN.ObjectToChange.Length; ++i)
    //    {
    //        PrefabUtility.SaveAsPrefabAssetAndConnect(AN.ObjectToChange[i], AN.path + AN.PrefabName + " " + (i + 1) + ".prefab", InteractionMode.UserAction);
    //    }
    //}
}
