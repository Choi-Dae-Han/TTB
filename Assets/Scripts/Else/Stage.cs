using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage : MonoBehaviour
{
    public float fLimitTime = 30f;
    public bool Opened = false;
    public int GotCoins = 0;
    public GameObject NextStage;

    // 기본 바닥
    public GameObject GroundToCreate;

    // 배경
    public GameObject BackGroundObj;
    public Sprite BackGroundSprite;

    // 데드라인
    public GameObject DeadLine;
    public Vector2 DeadLinePos;
    public Vector2 DeadLineSize;

    public StageData Data;

#if UNITY_EDITOR
    public Transform CommonGroundTr;
    public Transform ObjectsTr;
    public Transform SpecialGroundTr;

    public List<Transform> GroundTR;
    public Transform DeadLineTR;
    public List<GameObject> Objects;
#endif

    public void CreateBackGround()
    {
        GameObject backGround = Instantiate(BackGroundObj);
        backGround.GetComponent<SpriteRenderer>().sprite = BackGroundSprite;
        backGround.transform.localRotation = Quaternion.identity;
        backGround.transform.localScale = Vector3.one;
    }

    public void CreateDeadLine()
    {
        GameObject dl = Instantiate(DeadLine);
        dl.transform.SetParent(transform);
        dl.transform.localPosition = DeadLinePos;
        dl.transform.localRotation = Quaternion.identity;
        dl.transform.localScale = Vector3.one;
        dl.GetComponent<BoxCollider2D>().size = DeadLineSize;
    }

    public void CreateGrounds()
    {
        for (int i = 0; i < Data.GroundPoses.Count; ++i)
        {
            GameObject ground = Instantiate(GroundToCreate);
            ground.transform.SetParent(transform);
            ground.transform.localPosition = Data.GroundPoses[i];
            ground.transform.localRotation = Quaternion.identity;
            ground.transform.localScale = Vector3.one;
        }
    }

    public void CreateObj()
    {
        int m = 0;
        int b = 0;
        int p = 0;
        int o = 0;
        int c = 0;
        int n = 0;
        int s = 0;

        for (int i = 0; i < Data.ObjectToCreatePath.Count; ++i)
        {
            GameObject obj = Instantiate(Resources.Load(Data.ObjectToCreatePath[i]) as GameObject);
            obj.transform.SetParent(transform);
            ObjDataToTr(Data.ObjTrInfo[i], obj.transform);

            BrokenBox BB = obj.GetComponent<BrokenBox>();
            Portal P = obj.GetComponent<Portal>();
            ObjectMaker OM = obj.GetComponent<ObjectMaker>();
            ControlBox CB = obj.GetComponent<ControlBox>();
            Box_Needle BN = obj.GetComponent<Box_Needle>();
            SkullBox SB = obj.GetComponent<SkullBox>();

            if (Data.ObjTrInfo[i].IsMoving)
            {
                MoveData tempMD = Data.MD[m];
                MovingBox mb = obj.AddComponent<MovingBox>();
                obj.GetComponent<InGameObj>().MB = mb;
                MoveDataToMB(tempMD, mb);
                ++m;
            }

            if (BB != null)
            {
                BB.nDestroyCount = Data.BD[b].nDestroyCount;
                ++b;
            }
            else if (BN != null)
            {
                NeedleBoxData tempNd = Data.ND[n];
                BN.gameObject.GetComponent<SpriteRenderer>().sprite = GroundToCreate.GetComponent<SpriteRenderer>().sprite;
                NDataToN(tempNd, BN);
                ++n;
            }
            else if (P != null)
            {
                GameObject targetPortal = Instantiate(P.ExitPortal);
                targetPortal.transform.SetParent(transform);
                ObjDataToTr(Data.PD[p].ExitPOD, targetPortal.transform);
                if (Data.PD[p].ExitPMD.MoveSpeed != 0f)
                {
                    MovingBox pmb = targetPortal.AddComponent<MovingBox>();
                    MoveDataToMB(Data.PD[p].ExitPMD, pmb);
                }
                P.TargetPos = targetPortal.transform;
                ++p;
            }
            else if (SB != null)
            {
                SBDataToSB(Data.SD[s], SB);
                ++s;
            }
            else if (OM != null)
            {
                OMDataToOM(Data.OD[o], OM);
                ++o;
            }
            else if (CB != null)
            {
                int max = Data.CD[c].BoxToChange.Count;
                int cp = 0;
                int cb = 0;
                int co = 0;
                int cn = 0;
                int cs = 0;

                for (int Ci = 0; Ci < max; ++Ci)
                {
                    MoveData tempMD = Data.CD[c].BoxToChange[Ci];
                    MovingBox boxToChangeObj = Instantiate(Resources.Load(Data.CD[c].PrefabPathOfBoxesToChange[Ci]) as GameObject).AddComponent<MovingBox>();
                    boxToChangeObj.gameObject.GetComponent<InGameObj>().MB = boxToChangeObj;
                    boxToChangeObj.CB = CB;
                    boxToChangeObj.transform.SetParent(transform);
                    CB.BoxToChange.Add(boxToChangeObj);
                    MoveDataToMB(tempMD, boxToChangeObj);
                    ObjDataToTr(Data.CD[c].BoxesPos[Ci], boxToChangeObj.transform);

                    Portal cP = boxToChangeObj.gameObject.GetComponent<Portal>();
                    BrokenBox cB = boxToChangeObj.gameObject.GetComponent<BrokenBox>();
                    ObjectMaker cO = boxToChangeObj.gameObject.GetComponent<ObjectMaker>();
                    Box_Needle cN = boxToChangeObj.gameObject.GetComponent<Box_Needle>();
                    SkullBox cS = boxToChangeObj.gameObject.GetComponent<SkullBox>();

                    if (cP != null)
                    {
                        GameObject TargetPortal = Instantiate(cP.ExitPortal);
                        cP.TargetPos = TargetPortal.transform;
                        TargetPortal.transform.SetParent(transform);
                        ObjDataToTr(Data.CD[c].PD[cp].ExitPOD, cP.TargetPos);
                        ++cp;
                    }
                    else if (cB != null)
                    {
                        cB.nDestroyCount = Data.CD[c].BD[cb].nDestroyCount;
                        ++cb;
                    }
                    else if (cN != null)
                    {
                        cN.gameObject.GetComponent<SpriteRenderer>().sprite = GroundToCreate.GetComponent<SpriteRenderer>().sprite;
                        NDataToN(Data.CD[c].ND[cn], cN);
                        ++cn;
                    }
                    else if (cS != null)
                    {
                        SBDataToSB(Data.CD[c].SD[cs], cS);
                        ++cs;
                    }
                    else if (cO != null)
                    {
                        OMDataToOM(Data.CD[c].OD[co], cO);
                        ++co;
                    }
                }
                ++c;
            }
        }
    }

    private void MoveDataToMB(MoveData md, MovingBox mb)
    {
        mb.KofM = md.kindOfMove;
        mb.fMoveSpeed = md.MoveSpeed;
        mb.fRadian = md.fRadian;
        mb.fRadius = md.fRadius;
        mb.StartState = md.StartState;
        mb.AwakeTargetPos = new Vector3(md.AwakeTargetPosX,
                                                    md.AwakeTargetPosY,
                                                    0f);
        mb.TargetPos = md.TargetPos;
    }

    private void OMDataToOM(ObjectMakerData omd, ObjectMaker om)
    {
        om.ObjectToCreate = Resources.Load(omd.ObjectToCreatePath) as GameObject;
        om.fCycle = omd.fCycle;
        om.fTime = omd.fTime;
        om.Velo = omd.Velo;
    }

    private void NDataToN(NeedleBoxData nd, Box_Needle bn)
    {
        for (int j = 0; j < nd.NeedleCycle.Count; ++j)
        {
            Needle needle = Instantiate(Resources.Load(nd.NeedleObjPath) as GameObject).GetComponent<Needle>();
            needle.transform.SetParent(bn.transform);
            needle.transform.localPosition = Vector3.zero;
            needle.transform.localScale = new Vector3(1f, 1.2f, 1f);
            needle.transform.localRotation = nd.NeedleRotation[j];
            needle.fCycle = nd.NeedleCycle[j];
            needle.fFistAppearTime = nd.NeedleFirstAppearTime[j];
            needle.TargetPos = nd.NeedleTarget[j];
            needle.gameObject.SetActive(true);
        }
    }

    private void SBDataToSB(SkullBoxData sd, SkullBox s)
    {
        if(sd.IsRot)
        {
            RotationObject ro = s.gameObject.AddComponent<RotationObject>();
            ro.fRotateSpeed = sd.RotSpeed;
        }
        for (int i = 0; i < sd.MuzzleList.Count; ++i)
        {
            FireMuzzle fm = Instantiate(Resources.Load(sd.MuzzleList[i].MuzzleObjPath) as GameObject).GetComponent<FireMuzzle>();
            fm.transform.SetParent(s.transform);
            fm.transform.localPosition = sd.MuzzleList[i].Pos;
            fm.transform.localRotation = sd.MuzzleList[i].Rot;
            fm.fFireDelay = sd.MuzzleList[i].FireDelay;
            fm.fFirstFireTime = sd.MuzzleList[i].FirstFireTime;
            fm.fDeleteTime = sd.MuzzleList[i].DeleteTime;
            fm.ProjSpeed = sd.MuzzleList[i].ProjSpeed;
            fm.ProjScale = sd.MuzzleList[i].ProjScale;
            fm.gameObject.SetActive(true);
        }
    }

    private void ObjDataToTr(ObjectData od, Transform tr)
    {
        tr.localPosition = od.ObjPos;
        tr.localScale = od.ObjScale;
        tr.localRotation = od.ObjRot;
    }
}
