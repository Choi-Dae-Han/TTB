using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, AWAKEMOVE, MOVE
    }
    public STATE state = STATE.CREATE;

    public enum KINDOFMOVE
    {
        ROUNDTRIP, STOPAFTERMOVE, CIRCLE
    }
    public KINDOFMOVE KofM = KINDOFMOVE.ROUNDTRIP;

    private delegate void MoveFunc();
    [SerializeField] private MoveFunc MF = null;
    public float fXVelocity = 0f;
    public float fYVelocity = 0f;
    [SerializeField] public float fMoveSpeed = 0f;
    private int TargetNum = 0;
    private Vector3 CurTargetPos = Vector3.zero;
    [SerializeField] private Vector3 OriginPos = Vector3.zero;
    public Vector3 AwakeTargetPos = Vector3.zero;
    public List<Vector3> TargetPos = new List<Vector3>();
    public STATE StartState = STATE.MOVE;
    public STATE BeforeState = STATE.CREATE;
    public ControlBox CB;

    // 원 이동 관여
    public float fRadian = 0f;
    public float fRadius = 1f;

    void Start()
    {
        SetMove();
    }

    private void FixedUpdate()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch (state)
        {
            case STATE.CREATE:
                ChangeState(STATE.IDLE);
                break;
            case STATE.IDLE:
                break;
            case STATE.AWAKEMOVE:
                MovoToAwakeTarget();
                break;
            case STATE.MOVE:
                MF();
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (state == s) return;
        BeforeState = state;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                fXVelocity = 0f;
                fYVelocity = 0f;
                if (CB != null)
                {
                    if (CB.SR.color != CB.OriginColor)
                        CB.SR.color = CB.OriginColor;
                    CB.NumberOfStopedBoxes += 1;
                }
                break;
            case STATE.AWAKEMOVE:
                if (CB != null)
                {
                    if (CB.SR.color != CB.ColorToChange)
                        CB.SR.color = CB.ColorToChange;
                    CB.NumberOfStopedBoxes -= 1;
                }
                ReadyMove();
                break;
            case STATE.MOVE:
                if (CB != null && BeforeState != STATE.AWAKEMOVE)
                {
                    if (CB.SR.color != CB.ColorToChange)
                        CB.SR.color = CB.ColorToChange;
                    CB.NumberOfStopedBoxes -= 1;
                }
                ReadyMove();
                break;
        }
    }

    public void ReadyMove()
    {
        float DistX = Vector2.Distance(transform.position, new Vector2(CurTargetPos.x, transform.position.y));
        float DistY = Vector2.Distance(transform.position, new Vector2(transform.position.x, CurTargetPos.y));

        if (transform.position.x > CurTargetPos.x) fXVelocity = -fMoveSpeed;
        else fXVelocity = fMoveSpeed;
        if (transform.position.y > CurTargetPos.y) fYVelocity = -fMoveSpeed;
        else fYVelocity = fMoveSpeed;

        if (DistY > DistX) fXVelocity *= DistX / DistY;
        if (DistY < DistX) fYVelocity *= DistY / DistX;
        else
        {
            fYVelocity *= 0.71f;
            fXVelocity *= 0.71f;
        }
    }

    private void SetMove()
    {
        fMoveSpeed *= Time.fixedDeltaTime;

        switch (KofM)
        {
            case KINDOFMOVE.ROUNDTRIP:
                MF = RoundTrip;
                if (AwakeTargetPos != Vector3.zero)
                {
                    StartState = STATE.AWAKEMOVE;
                    OriginPos = transform.position + AwakeTargetPos;
                }
                else
                    OriginPos = transform.position;
                TargetPos[0] += OriginPos;
                CurTargetPos = TargetPos[0];
                for (int i = 0; i < TargetPos.Count - 1; ++i)
                    TargetPos[i + 1] += TargetPos[i];
                if (CB != null)
                {
                    if(StartState == STATE.MOVE || StartState == STATE.AWAKEMOVE)
                        CB.NumberOfStopedBoxes += 1;
                }
                break;
            case KINDOFMOVE.STOPAFTERMOVE:
                MF = MoveAndStop;
                if (AwakeTargetPos != Vector3.zero)
                {
                    StartState = STATE.AWAKEMOVE;
                    OriginPos = transform.position + AwakeTargetPos;
                }
                else
                    OriginPos = transform.position;
                TargetPos[0] += OriginPos;
                CurTargetPos = TargetPos[0];
                for (int i = 0; i < TargetPos.Count - 1; ++i)
                    TargetPos[i + 1] += TargetPos[i];
                break;
            case KINDOFMOVE.CIRCLE:
                MF = CircleMove;
                if (CB != null)
                {
                    if (StartState == STATE.MOVE || StartState == STATE.AWAKEMOVE)
                        CB.NumberOfStopedBoxes += 1;
                }
                break;
        }
        ChangeState(StartState);
    }

    private void MovoToAwakeTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, OriginPos, fMoveSpeed);
        if (transform.position == OriginPos)
        {
            ChangeState(STATE.MOVE);
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, CurTargetPos, fMoveSpeed);
    }

    private void CircleMove()
    {
        if (fRadian > 360f)
            fRadian = fRadian - 360f;
        fRadian += Time.fixedDeltaTime * fRadius;

        float deRad = fRadian * Mathf.Deg2Rad;
        fXVelocity = Mathf.Cos(deRad) * fMoveSpeed;
        fYVelocity = Mathf.Sin(deRad) * fMoveSpeed;
        CurTargetPos = transform.position + new Vector3(fXVelocity, fYVelocity, 0f);
        MoveToTarget();
    }

    private void RoundTrip()
    {
        if (transform.position == CurTargetPos)
        {
            if (TargetNum < TargetPos.Count - 1)
            {
                TargetNum++;
                CurTargetPos = TargetPos[TargetNum];
            }
            else
            {
                TargetNum = -1;
                CurTargetPos = OriginPos;
            }

            ReadyMove();
        }
        MoveToTarget();
    }

    void MoveAndStop()
    {
        if (transform.position != CurTargetPos)
        {
            MoveToTarget();
        }
        else
        {
            ChangeState(STATE.IDLE);
            if (TargetNum < TargetPos.Count - 1)
            {
                TargetNum++;
                CurTargetPos = TargetPos[TargetNum];
            }
            else
            {
                TargetNum = -1;
                CurTargetPos = OriginPos;
            }
        }
    }
}
