using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData
{
    public MovingBox.KINDOFMOVE kindOfMove;
    public float MoveSpeed;
    public float fRadian;
    public float fRadius;
    public MovingBox.STATE StartState;
    public float AwakeTargetPosX;
    public float AwakeTargetPosY;
    public List<Vector3> TargetPos = new List<Vector3>();

    public MoveData(MovingBox.KINDOFMOVE kindMove, float moveSpeed, float radian = 0f, float radius = 0f, MovingBox.STATE startState = MovingBox.STATE.MOVE,
        float awakeTargetPosX = 0f, float awakeTargetPosY = 0f, List<Vector3> targetPos = null)
    {
        kindOfMove = kindMove;
        MoveSpeed = moveSpeed;
        fRadian = radian;
        fRadius = radius;
        StartState = startState;
        AwakeTargetPosX = awakeTargetPosX;
        AwakeTargetPosY = awakeTargetPosY;
        TargetPos = targetPos;
    }
}
