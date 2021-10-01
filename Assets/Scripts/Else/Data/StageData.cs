using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int GotCoins;
    public bool Opened;
    public List<Vector3> GroundPoses;
    public List<MoveData> MD;
    public List<ObjectData> ObjTrInfo;
    public List<string> ObjectToCreatePath;
    public List<PortalData> PD;
    public List<BrokenBoxData> BD;
    public List<ObjectMakerData> OD;
    public List<ControlBoxData> CD;
    public List<NeedleBoxData> ND;
    public List<SkullBoxData> SD;

    public StageData(bool opened, int gotCoin = 0, List<Vector3> groundPoses = null, List<string> objToCreate = null, List<ObjectData> objData = null,
                     List<MoveData> md = null, List<PortalData> pd = null, List<BrokenBoxData> bd = null, List<ObjectMakerData> od = null, List<ControlBoxData> cd = null,
                     List<NeedleBoxData> nd = null, List<SkullBoxData> sd = null)
    {
        GotCoins = gotCoin;
        Opened = opened;
        if (objData != null) ObjTrInfo = objData;
        if (md != null) MD = md;
        if (objToCreate != null) ObjectToCreatePath = objToCreate;
        if (groundPoses != null) GroundPoses = groundPoses;
        if (pd != null) PD = pd;
        if (bd != null) BD = bd;
        if (od != null) OD = od;
        if (cd != null) CD = cd;
        if (nd != null) ND = nd;
        if (sd != null) SD = sd;
    }
}
