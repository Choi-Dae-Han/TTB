using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[System.Obsolete]
[System.Serializable]
[CustomEditor(typeof(Stage))]
public class StageCreater : Editor
{
    Stage S;
    StageData SD;
    public List<Vector3> groundPoses = new List<Vector3>();
    public List<MoveData> md = new List<MoveData>();
    public List<PortalData> pData = new List<PortalData>();
    public List<ObjectMakerData> oData = new List<ObjectMakerData>();
    public List<BrokenBoxData> bData = new List<BrokenBoxData>();
    public List<ControlBoxData> cData = new List<ControlBoxData>();
    public List<NeedleBoxData> nData = new List<NeedleBoxData>();
    public List<SkullBoxData> sData = new List<SkullBoxData>();
    public List<ObjectData> objTRInfo = new List<ObjectData>();
    public List<string> objectToCreatePath = new List<string>();

    private void OnEnable()
    {
        S = target as Stage;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("theorem"))
        {
            Set();
        }

        if (GUILayout.Button("SetMap"))
        {
            LoadData();
            ResetData();
            SetGrounds();
            SetDeadLine();
            SetObjects();
            SaveData();
        }

        //if (GUILayout.Button("DeleteC"))
        //{
        //    DeleteChilds();
        //}
    }

    public void SetGrounds()
    {
        for (int i = 0; i < S.GroundTR.Count; ++i)
        {
            groundPoses.Add(S.GroundTR[i].position);
        }
    }

    public void SetObjects()
    {
        for (int i = 0; i < S.Objects.Count; ++i)
        {          
            GameObject prefabObj = PrefabUtility.GetPrefabParent(S.Objects[i]) as GameObject;
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabObj);

            string newPath = path.Replace("Assets/resources/", "");
            string newPath1 = newPath.Replace(".prefab", "");

            ObjectData ObjTrInfo = new ObjectData(S.Objects[i].transform.localPosition,
                                                  S.Objects[i].transform.localRotation,
                                                  S.Objects[i].transform.localScale);
            MovingBox MB = S.Objects[i].GetComponent<MovingBox>();
            Portal P = S.Objects[i].GetComponent<Portal>();
            BrokenBox B = S.Objects[i].GetComponent<BrokenBox>();
            ObjectMaker O = S.Objects[i].GetComponent<ObjectMaker>();
            ControlBox C = S.Objects[i].GetComponent<ControlBox>();
            Box_Needle N = S.Objects[i].GetComponent<Box_Needle>();
            SkullBox SB = S.Objects[i].GetComponent<SkullBox>();

            if (MB != null)
            {
                if (MB.CB == null)
                {
                    MoveData MD = new MoveData(
                        MB.KofM, MB.fMoveSpeed, MB.fRadian, MB.fRadius, MB.StartState,
                        MB.AwakeTargetPos.x, MB.AwakeTargetPos.y, MB.TargetPos);
                    ObjTrInfo.IsMoving = true;
                    md.Add(MD);
                }
                else
                    continue;
            }

            objectToCreatePath.Add(newPath1);
            objTRInfo.Add(ObjTrInfo);

            if (P != null)
            {
                Transform exitPortalTr = P.TargetPos;
                MovingBox exitPortalMB = exitPortalTr.gameObject.GetComponent<MovingBox>();
                PortalData pd = new PortalData();

                ObjectData exitPortalTrInfo = new ObjectData(exitPortalTr.localPosition,
                                             exitPortalTr.localRotation,
                                             exitPortalTr.localScale);
                pd.ExitPOD = exitPortalTrInfo;
                if (exitPortalMB != null)
                {
                    MoveData exitPortalMD = new MoveData(
                        exitPortalMB.KofM, exitPortalMB.fMoveSpeed, exitPortalMB.fRadian, exitPortalMB.fRadius, exitPortalMB.StartState,
                        exitPortalMB.AwakeTargetPos.x, exitPortalMB.AwakeTargetPos.y, exitPortalMB.TargetPos);

                    exitPortalTrInfo.IsMoving = true;
                    pd.ExitPMD = exitPortalMD;
                }
                pData.Add(pd);
            }
            else if (B != null)
            {
                BrokenBoxData bd = new BrokenBoxData(B.nDestroyCount);
                bData.Add(bd);
            }
            else if (N != null)
            {
                GameObject needleObj = PrefabUtility.GetPrefabParent(N.SavingNeedle[0].gameObject) as GameObject;
                string nPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(needleObj);
                string newNPath = nPath.Replace("Assets/resources/", "");
                string newNPath1 = newNPath.Replace(".prefab", "");

                List<Quaternion> tempRot = new List<Quaternion>();
                List<Vector2> tempTarget = new List<Vector2>();
                List<float> tempFirstAppaerTime = new List<float>();
                List<float> tempCycle = new List<float>();
                for(int j = 0; j < N.SavingNeedle.Count; ++j)
                {
                    tempFirstAppaerTime.Add(N.SavingNeedle[j].fFistAppearTime);
                    tempCycle.Add(N.SavingNeedle[j].fCycle);
                    tempRot.Add(N.SavingNeedle[j].transform.localRotation);
                    tempTarget.Add(N.SavingNeedle[j].TargetPos);
                }
                NeedleBoxData nd = new NeedleBoxData(tempCycle, tempFirstAppaerTime, tempTarget, tempRot, newNPath1);
                nData.Add(nd);
            }
            else if (SB != null)
            {
                List<FireMuzzleData> fmd = new List<FireMuzzleData>();
                RotationObject ro = SB.gameObject.GetComponent<RotationObject>();
                bool tempIsRot = false;
                float tempRotSpeed = 0f;
                if (ro != null)
                {
                    tempIsRot = true;
                    tempRotSpeed = ro.fRotateSpeed;
                }
                GameObject muzzleObj = PrefabUtility.GetPrefabParent(SB.UsingMuzzles[0].gameObject) as GameObject;
                string mPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(muzzleObj);
                string newMPath = mPath.Replace("Assets/resources/", "");
                string newMPath1 = newMPath.Replace(".prefab", "");

                for (int j = 0; j < SB.UsingMuzzles.Count; ++j)
                {
                    FireMuzzle tempF = SB.UsingMuzzles[j];
                    FireMuzzleData tempFm = new FireMuzzleData(newMPath1, tempF.transform.localPosition, tempF.transform.localRotation, tempF.fFirstFireTime,
                        tempF.fDeleteTime, tempF.fFireDelay, tempF.ProjSpeed, tempF.ProjScale);
                    fmd.Add(tempFm);
                }

                SkullBoxData sd = new SkullBoxData(fmd, tempRotSpeed, tempIsRot);
                sData.Add(sd);
            }
            else if (O != null)
            {
                string OPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(O.ObjectToCreate);
                string OPath1 = OPath.Replace("Assets/resources/", "");
                string OPath2 = OPath1.Replace(".prefab", "");
                ObjectMakerData od = new ObjectMakerData(OPath2, O.fCycle, O.fTime, O.Velo);
                oData.Add(od);
            }
            else if (C != null)
            {
                ControlBoxData cd;
                List<string> tempPrefabObjPath = new List<string>();
                List<MoveData> tempMD = new List<MoveData>();
                List<ObjectData> tempPoses = new List<ObjectData>();

                List<BrokenBoxData> cBd_List = new List<BrokenBoxData>();
                List<PortalData> cPd_List = new List<PortalData>();
                List<NeedleBoxData> cNd_List = new List<NeedleBoxData>();
                List<ObjectMakerData> cOd_List = new List<ObjectMakerData>();
                List<SkullBoxData> cSd_List = new List<SkullBoxData>();

                for (int ci = 0; ci < C.BoxToChange.Count; ++ci)
                {
                    BrokenBox tempB = C.BoxToChange[ci].GetComponent<BrokenBox>();
                    Portal tempP = C.BoxToChange[ci].GetComponent<Portal>();
                    ObjectMaker tempO = C.BoxToChange[ci].GetComponent<ObjectMaker>();
                    Box_Needle tempN = C.BoxToChange[ci].GetComponent<Box_Needle>();
                    SkullBox tempS = C.BoxToChange[ci].GetComponent<SkullBox>();

                    ObjectData targetBoxInfo = new ObjectData(C.BoxToChange[ci].transform.localPosition,
                                      C.BoxToChange[ci].transform.localRotation,
                                      C.BoxToChange[ci].transform.localScale);
                    GameObject cPrefabObj = PrefabUtility.GetPrefabParent(C.BoxToChange[ci].gameObject) as GameObject;
                    string cPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(cPrefabObj);
                    string newCPath = cPath.Replace("Assets/resources/", "");
                    string newCPath1 = newCPath.Replace(".prefab", "");

                    MovingBox targetBox = C.BoxToChange[ci];
                    MoveData MD = new MoveData(
                        targetBox.KofM, targetBox.fMoveSpeed, targetBox.fRadian, targetBox.fRadius, targetBox.StartState,
                        targetBox.AwakeTargetPos.x, targetBox.AwakeTargetPos.y, targetBox.TargetPos);
                    tempPrefabObjPath.Add(newCPath1);
                    tempMD.Add(MD);
                    tempPoses.Add(targetBoxInfo);

                    if (tempB != null)
                    {
                        BrokenBoxData cbd = new BrokenBoxData(tempB.nDestroyCount);
                        cBd_List.Add(cbd);
                    }
                    else if (tempN != null)
                    {
                        List<Quaternion> tempRot = new List<Quaternion>();
                        List<Vector2> tempTarget = new List<Vector2>();
                        List<float> tempFirstAppaerTime = new List<float>();
                        List<float> tempCycle = new List<float>();
                        for (int j = 0; j < tempN.SavingNeedle.Count; ++j)
                        {
                            tempFirstAppaerTime.Add(N.SavingNeedle[j].fFistAppearTime);
                            tempCycle.Add(N.SavingNeedle[j].fCycle);
                            tempRot.Add(tempN.SavingNeedle[j].transform.localRotation);
                            tempTarget.Add(tempN.SavingNeedle[j].TargetPos);
                        }
                        GameObject needleObj = PrefabUtility.GetPrefabParent(tempN.SavingNeedle[0].gameObject) as GameObject;
                        string nPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(needleObj);
                        string newNPath = nPath.Replace("Assets/resources/", "");
                        string newNPath1 = newNPath.Replace(".prefab", "");

                        NeedleBoxData cnd = new NeedleBoxData(tempCycle, tempFirstAppaerTime, tempTarget, tempRot, newNPath1);
                        cNd_List.Add(cnd);
                    }
                    else if (tempP != null) // tempP == CB의 자식포탈, cTr == CB의 자식포탈의 타겟포탈, cPd_List == CD에있는 포탈데이터리스트
                    {
                        Transform cTr = tempP.TargetPos;
                        MovingBox cTrMB = cTr.GetComponent<MovingBox>();
                        ObjectData cOd = new ObjectData(cTr.localPosition, cTr.localRotation, cTr.localScale); // Exit포탈 데이터
                        MoveData cMd = null;
                        if (cTrMB != null)
                        {
                            cMd = new MoveData(cTrMB.KofM, cTrMB.fMoveSpeed, cTrMB.fRadian, cTrMB.fRadius, cTrMB.StartState,
                                                        cTrMB.AwakeTargetPos.x, cTrMB.AwakeTargetPos.y, cTrMB.TargetPos); // Exit포탈 Move데이터
                            cOd.IsMoving = true;
                        }
                        PortalData cpd = new PortalData(cOd, cMd);
                        cPd_List.Add(cpd);
                    }
                    else if (tempS != null)
                    {
                        List<FireMuzzleData> tempFmd = new List<FireMuzzleData>();
                        RotationObject ro = tempS.gameObject.GetComponent<RotationObject>();
                        bool tempIsRot = false;
                        float tempRotSpeed = 0f;
                        if (ro != null)
                        {
                            tempIsRot = true;
                            tempRotSpeed = ro.fRotateSpeed;
                        }

                        GameObject muzzleObj = PrefabUtility.GetPrefabParent(tempS.UsingMuzzles[0].gameObject) as GameObject;
                        string mPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(muzzleObj);
                        string newMPath = mPath.Replace("Assets/resources/", "");
                        string newMPath1 = newMPath.Replace(".prefab", "");

                        for (int j = 0; j < tempS.UsingMuzzles.Count; ++j)
                        {
                            FireMuzzle tempF = tempS.UsingMuzzles[j];
                            FireMuzzleData tempFm = new FireMuzzleData(newMPath1, tempF.transform.localPosition, tempF.transform.localRotation, tempF.fFirstFireTime,
                                tempF.fDeleteTime, tempF.fFireDelay, tempF.ProjSpeed, tempF.ProjScale);
                            tempFmd.Add(tempFm);
                        }

                        SkullBoxData csd = new SkullBoxData(tempFmd, tempRotSpeed, tempIsRot);
                        cSd_List.Add(csd);
                    }
                    else if (tempO != null)
                    {
                        string OPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(tempO.ObjectToCreate);
                        string OPath1 = OPath.Replace("Assets/resources/", "");
                        string OPath2 = OPath1.Replace(".prefab", "");
                        ObjectMakerData cod = new ObjectMakerData(OPath2, O.fCycle, O.fTime, O.Velo);
                        cOd_List.Add(cod);
                    }
                }
                cd = new ControlBoxData(tempPrefabObjPath, tempMD, tempPoses, cBd_List, cPd_List, cOd_List, cNd_List, cSd_List);
                cData.Add(cd);
            }
        }
    }

    public void SetDeadLine()
    {
        S.DeadLinePos = S.DeadLineTR.localPosition;
        S.DeadLineSize = S.DeadLineTR.gameObject.GetComponent<BoxCollider2D>().size;
    }

    void SaveData()
    {
        StageData newSD = new StageData(SD.Opened, SD.GotCoins, groundPoses, objectToCreatePath, objTRInfo, md, pData, bData, oData, cData, nData, sData);
        string jsonData = JsonUtility.ToJson(newSD);
        File.WriteAllText(Application.persistentDataPath + DataManager.DataPath + S.gameObject.name + ".json", jsonData.ToString()); // 세이브
    }

    void LoadData()
    {
        if (SD == null)
        {
            string jsonString = File.ReadAllText(Application.persistentDataPath + DataManager.DataPath + S.gameObject.name + ".json");
            SD = JsonUtility.FromJson<StageData>(jsonString); // 로드
        }
    }

    void ResetData()
    {
        if (md != null) md.Clear();
        if (groundPoses != null) groundPoses.Clear();
        if (objectToCreatePath != null) objectToCreatePath.Clear();
        if (objTRInfo != null) objTRInfo.Clear();
    }

    /// <main>
    /// ///////////////////////////////////////////////////////////////////////////
    /// <temp>

    void DeleteChilds()
    {
        for (int i = 0; i < S.ObjectsTr.childCount; ++i)
        {
            TextObject to = S.ObjectsTr.GetChild(i).GetComponent<TextObject>();
            if (to != null) to.transform.SetParent(S.transform);
        }
        DestroyImmediate(S.CommonGroundTr.gameObject);
        DestroyImmediate(S.ObjectsTr.gameObject);
        DestroyImmediate(S.SpecialGroundTr.gameObject);

        for(int i = 0; i < S.transform.childCount; ++i)
        {
            if (S.transform.GetChild(i).name == "DeadLine" || S.transform.GetChild(i).name == "BackGround")
            {
                DestroyImmediate(S.transform.GetChild(i).gameObject);
                --i;
            }
        }
    }

    void Set()
    {
        for (int i = 0; i < S.gameObject.transform.childCount; ++i)
        {
            if (S.gameObject.transform.GetChild(i).name == "Common Grounds")
            {
                S.CommonGroundTr = S.gameObject.transform.GetChild(i);
            }
            else if (S.gameObject.transform.GetChild(i).name == "Special Grounds")
            {
                S.SpecialGroundTr = S.gameObject.transform.GetChild(i);
            }
            else if (S.gameObject.transform.GetChild(i).name == "Objects")
            {
                S.ObjectsTr = S.gameObject.transform.GetChild(i);
            }
        }
        for (int i = 0; i < S.transform.childCount; ++i)
        {
            if (S.transform.GetChild(i).name == "DeadLine")
            {
                S.DeadLineTR = S.transform.GetChild(i);
                break;
            }
        }

        //찾기
        //////////////////////////////////////////////////////////////////
        //실행

        S.GroundTR.Clear();
        S.Objects.Clear();
        for (int i = 0; i < S.CommonGroundTr.childCount; ++i)
        {
            S.GroundTR.Add(S.CommonGroundTr.GetChild(i));
        }

        for (int i = 0; i < S.SpecialGroundTr.childCount; ++i)
        {
            MovingBox temp = S.SpecialGroundTr.GetChild(i).GetComponent<MovingBox>();
            if (temp == null)
            {
                if (S.SpecialGroundTr.GetChild(i).GetComponent<Portal_Exit>() == null)
                    S.Objects.Add(S.SpecialGroundTr.GetChild(i).gameObject);
            }
            else
            {
                if(temp.CB == null)
                {
                    if (S.SpecialGroundTr.GetChild(i).GetComponent<Portal_Exit>() == null)
                        S.Objects.Add(S.SpecialGroundTr.GetChild(i).gameObject);
                }
            }    
        }

        for (int i = 0; i < S.ObjectsTr.childCount; ++i)
        {
            MovingBox temp = S.ObjectsTr.GetChild(i).GetComponent<MovingBox>();
            if (temp == null)
            {
                if (S.ObjectsTr.GetChild(i).GetComponent<Portal_Exit>() == null)
                    S.Objects.Add(S.ObjectsTr.GetChild(i).gameObject);
            }
            else
            {
                if (temp.CB == null)
                {
                    if (S.ObjectsTr.GetChild(i).GetComponent<Portal_Exit>() == null)
                        S.Objects.Add(S.ObjectsTr.GetChild(i).gameObject);
                }
            }
        }

        for (int i = 0; i < S.GroundTR.Count; ++i)
        {
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(S.GroundToCreate);
            obj.transform.SetParent(S.CommonGroundTr);
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = S.GroundTR[i].localPosition;
            obj.name = S.GroundToCreate.name;
            DestroyImmediate(S.GroundTR[i].gameObject);
            S.GroundTR[i] = obj.transform;
        }
    }
}
