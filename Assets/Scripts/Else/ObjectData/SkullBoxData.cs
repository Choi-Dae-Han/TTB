using System.Collections.Generic;

[System.Serializable]
public class SkullBoxData
{
    public List<FireMuzzleData> MuzzleList;
    public bool IsRot;
    public float RotSpeed;

    public SkullBoxData(List<FireMuzzleData> muzzleList, float rotSpeed, bool isRot = false)
    {
        MuzzleList = muzzleList;
        RotSpeed = rotSpeed;
        IsRot = isRot;
    }
}
