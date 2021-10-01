using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeedleBoxData
{
    public List<float> NeedleCycle;
    public List<float> NeedleFirstAppearTime;
    public List<Quaternion> NeedleRotation;
    public List<Vector2> NeedleTarget;
    public string NeedleObjPath;

    public NeedleBoxData(List<float> needleCycle, List<float> needleFirstAppearTime, List<Vector2> targetPos, List<Quaternion> rotation, string needleObjPath)
    {
        NeedleCycle = needleCycle;
        NeedleFirstAppearTime = needleFirstAppearTime;
        NeedleTarget = targetPos;
        NeedleRotation = rotation;
        NeedleObjPath = needleObjPath;
    }
}
