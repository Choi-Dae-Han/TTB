using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlBoxData
{
    public List<string> PrefabPathOfBoxesToChange;
    public List<MoveData> BoxToChange;
    public List<ObjectData> BoxesPos;
    public List<BrokenBoxData> BD;
    public List<PortalData> PD;
    public List<ObjectMakerData> OD;
    public List<NeedleBoxData> ND;
    public List<SkullBoxData> SD;

    public ControlBoxData(List<string> prefabPathOfBoxesToChange, List<MoveData> boxToChange, List<ObjectData> poses, List<BrokenBoxData> bd = null,
                          List<PortalData> pd = null, List<ObjectMakerData> od = null, List<NeedleBoxData> nd = null, List<SkullBoxData> sd = null)
    {
        PrefabPathOfBoxesToChange = prefabPathOfBoxesToChange;
        BoxToChange = boxToChange;
        BoxesPos = poses;
        BD = bd;
        PD = pd;
        OD = od;
        ND = nd;
        SD = sd;
    }
}
