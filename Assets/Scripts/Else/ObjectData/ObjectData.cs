using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public Vector3 ObjPos;
    public Quaternion ObjRot;
    public Vector3 ObjScale;
    public bool IsMoving;

    public ObjectData(Vector3 pos, Quaternion rot, Vector3 scale, bool move = false)
    {
        ObjPos = pos;
        ObjRot = rot;
        ObjScale = scale;
        IsMoving = move;
    }
}
