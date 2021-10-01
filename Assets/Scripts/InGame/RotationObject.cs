using UnityEngine;

public class RotationObject : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, ROTATE
    }
    public STATE State = STATE.CREATE;

    public float fRotateSpeed = 0f;

    void Start()
    {
        ChangeState(STATE.ROTATE);
    }


    void FixedUpdate()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch(State)
        {
            case STATE.ROTATE:
                Rotating();
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (State == s) return;
        State = s;
        switch(s)
        {
            case STATE.IDLE:
                break;
            case STATE.ROTATE:
                break;
        }
    }

    private void Rotating()
    {
        transform.Rotate(new Vector3(0f, 0f, fRotateSpeed * Time.fixedDeltaTime));
    }
}
