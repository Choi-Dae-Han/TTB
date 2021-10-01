using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public enum CAMERASTATE
    {
        CREATE, IDLE, FOLLOWING
    }
    public CAMERASTATE cameraState = CAMERASTATE.CREATE;

    public enum VSTATE
    {
        VIDLE, UP, DOWN
    }
    public VSTATE vState = VSTATE.VIDLE;

    public enum HSTATE
    {
        HIDLE, RIGHT, LEFT
    }
    public HSTATE hState = HSTATE.HIDLE;

    [SerializeField] private float Speed = 2f;
    [SerializeField] private float StayRangeX = 125f;
    [SerializeField] private float StayRangeY = 70f;
    [SerializeField] private float HighestY = 0f;
    [SerializeField] private float LowestY = 0f;
    [SerializeField] private float RightestX = 0f;
    [SerializeField] private float LeftestX = 0f;
    public Vector3 LastBallPos = Vector3.zero;
    public Vector3 GoingTarget = Vector3.zero;
    public Transform BallTr;

    void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch (cameraState)
        {
            case CAMERASTATE.CREATE:
                ResetCamera();
                ChangeState(CAMERASTATE.IDLE);
                break;
            case CAMERASTATE.FOLLOWING:
                FollowObject();
                break;
        }
    }

    public void ChangeState(CAMERASTATE s)
    {
        if(cameraState == s) return;
        cameraState = s;

        switch (s)
        {
            case CAMERASTATE.IDLE:
                GoingTarget = transform.position;
                ChangeVState(VSTATE.VIDLE);
                ChangeHState(HSTATE.HIDLE);
                break;
            case CAMERASTATE.FOLLOWING:
                break;
        }
    }

    public void ChangeVState(VSTATE s)
    {
        if (vState == s) return;
        vState = s;

        switch (s)
        {
            case VSTATE.UP:
                LowestY = 0f;
                HighestY = transform.position.y + StayRangeY;
                break;
            case VSTATE.DOWN:
                HighestY = 0f;
                LowestY = transform.position.y - StayRangeY;
                break;
        }
    }

    public void ChangeHState(HSTATE s)
    {
        if (hState == s) return;
        hState = s;

        switch(s)
        {
            case HSTATE.LEFT:
                RightestX = 0f;
                LeftestX = transform.position.x - StayRangeX;
                break;
            case HSTATE.RIGHT:
                LeftestX = 0f;
                RightestX = transform.position.x + StayRangeX;
                break;
        }
    }

    private void FollowObject()
    {
        if (BallTr != null)
            LastBallPos = BallTr.position;

        if (LastBallPos.y > transform.position.y + StayRangeY)
            ChangeVState(VSTATE.UP);
        else if (LastBallPos.y < transform.position.y - StayRangeY)
            ChangeVState(VSTATE.DOWN);

        if (LastBallPos.x > transform.position.x + StayRangeX)
            ChangeHState(HSTATE.RIGHT);
        else if (LastBallPos.x < transform.position.x - StayRangeX)
            ChangeHState(HSTATE.LEFT);

        switch (vState)
        {
            case VSTATE.UP:
                if (LastBallPos.y > HighestY) HighestY = LastBallPos.y;
                GoingTarget.y = HighestY - StayRangeY;
                break;
            case VSTATE.DOWN:
                if (LastBallPos.y < LowestY) LowestY = LastBallPos.y;
                GoingTarget.y = LowestY + StayRangeY;
                break;
        }

        switch(hState)
        {
            case HSTATE.RIGHT:
                if (LastBallPos.x > RightestX) RightestX = LastBallPos.x;
                GoingTarget.x = RightestX - StayRangeX;
                break;
            case HSTATE.LEFT:
                if (LastBallPos.x < LeftestX) LeftestX = LastBallPos.x;
                GoingTarget.x = LeftestX + StayRangeX;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, GoingTarget, Speed * Time.smoothDeltaTime);
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(0f, 0f, -100f);
        GoingTarget = transform.position;
    }
}
