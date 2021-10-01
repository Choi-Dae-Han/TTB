using UnityEngine;

[System.Serializable]
public class FireMuzzleData
{
    public string MuzzleObjPath;
    public Vector3 Pos;
    public Quaternion Rot; 
    public float FirstFireTime;
    public float DeleteTime;
    public float FireDelay;
    public float ProjSpeed;
    public float ProjScale;

    public FireMuzzleData(string muzzleObjPath, Vector3 pos, Quaternion rot, float firstFireTime, float deleteTime, float fireDelay, float projSpeed, float projScale)
    {
        MuzzleObjPath = muzzleObjPath;
        Pos = pos;
        Rot = rot;
        FirstFireTime = firstFireTime;
        DeleteTime = deleteTime;
        FireDelay = fireDelay;
        ProjSpeed = projSpeed;
        ProjScale = projScale;
    }
}
