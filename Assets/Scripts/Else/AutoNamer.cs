using UnityEngine;

public class AutoNamer : MonoBehaviour
{
    public GameObject[] ObjectToChange;
    public Box_Needle[] N;
    public float Cycle = 0f;
    public float Diff = 0f;
    public float FirstAppear = 0f;
    public Vector2 TargetPos = new Vector2(0f, 40f);
    public Vector3 NeedleRot = Vector3.zero;

    public string NameToChange = "";
    public Stage[] StageOriginal;
    public Stage[] StageNew;
    //public string path = "";
    //public string PrefabName = "";
    public void AutoNaming()
    {
        for (int i = 0; i < ObjectToChange.Length; ++i)
        {
            ObjectToChange[i].name = NameToChange + " " + (i + 1);
        }
    }

    public void SetInfo()
    {
        for(int i = 0; i < StageOriginal.Length; ++i)
        {
            StageNew[i].DeadLinePos = StageOriginal[i].DeadLinePos;
            StageNew[i].DeadLineSize = StageOriginal[i].DeadLineSize;
        }
    }

    //public void SetNextStage()
    //{
    //    for(int i = 0; i < StageNSet.Length - 1; ++i)
    //    {
    //        StageNSet[i].NextStage = StageNSet[i + 1].gameObject;
    //    }
    //}

    //public void SetStage()
    //{
    //    for(int i = 0; i < Stages.Length; ++i)
    //    {
    //        Stages[i].stage = StageQQ[i].GetComponent<Stage>();
    //    }
    //}

    public void SetNeedle_DownToUp()
    {
        for (int i = 0; i < N.Length; ++i)
        {
            for (int j = 0; j < N[i].SavingNeedle.Count; ++j)
            {
                N[i].SavingNeedle[j].fCycle = Cycle;
                N[i].SavingNeedle[j].fFistAppearTime = FirstAppear + Diff * i;
                N[i].SavingNeedle[j].transform.Rotate(NeedleRot);
                N[i].SavingNeedle[j].TargetPos = TargetPos;
            }
        }
    }

    public void SetNeedle_UpToDown()
    {
        for (int i = 0; i < N.Length; ++i)
        {
            for (int j = 0; j < N[i].SavingNeedle.Count; ++j)
            {
                N[i].SavingNeedle[j].fCycle = Cycle;
                N[i].SavingNeedle[j].fFistAppearTime = FirstAppear - Diff * i;
                N[i].SavingNeedle[j].transform.Rotate(NeedleRot);
                N[i].SavingNeedle[j].TargetPos = TargetPos;
            }
        }
    }
}
