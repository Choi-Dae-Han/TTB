using UnityEngine;

public class Needle : MonoBehaviour
{
    public float fAppearSpeed = 0f;
    public float fDisAppearSpeed = 0f;
    public float fTime = 0f;
    public float fCycle = 0f;
    public float fFistAppearTime = 0f;
    public Vector3 OriginPos = Vector3.zero;
    public Vector3 TargetPos = Vector3.zero;
    public AudioClip AppearSound;
    public AudioSource AM;
    public GameManager GM;

    public enum STATE
    {
        CREATE, FIRSTAPPEAR, IDLE, APPEAR, DISAPPEAR
    }
    public STATE state = STATE.IDLE;

    void Start()
    {
        OriginPos = transform.localPosition;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(AM == null)
            AM = GetComponent<AudioSource>();
        ChangeState(STATE.FIRSTAPPEAR);
    }

    void FixedUpdate()
    {
        StateProcess();
    }

    public void StateProcess()
    {
        switch(state)
        {
            case STATE.CREATE:
                break;
            case STATE.FIRSTAPPEAR:
                if (fTime >= fCycle)
                    ChangeState(STATE.APPEAR);
                fTime += Time.fixedDeltaTime;
                break;
            case STATE.IDLE:
                fTime += Time.fixedDeltaTime;
                if (fTime >= fCycle)
                {
                    ChangeState(STATE.APPEAR);
                }
                break;
            case STATE.APPEAR:
                Appear();
                break;
            case STATE.DISAPPEAR:
                DisAppear();
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch(s)
        {
            case STATE.CREATE:
                break;
            case STATE.FIRSTAPPEAR:
                fTime = fFistAppearTime;
                break;
            case STATE.IDLE:
                fTime = 0f;
                break;
            case STATE.APPEAR:
                fTime = 0f;
                AM.clip = AppearSound;
                AM.Play();
                break;
            case STATE.DISAPPEAR:
                break;
        }
    }

    public void Appear()
    {
        if (transform.localPosition != TargetPos)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPos, fAppearSpeed);
        else
        {
            fTime += Time.fixedDeltaTime;
            if (fTime >= 1f)
            {
                fTime -= 1f;
                ChangeState(STATE.DISAPPEAR);
            }
        }
    }

    public void DisAppear()
    {
        if (transform.localPosition != OriginPos)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, OriginPos, fDisAppearSpeed);
        else
            ChangeState(STATE.IDLE);
    }

    private void OnTriggerStay2D(Collider2D CrashObj)
    {
        if(CrashObj.CompareTag("Player") && state != STATE.IDLE)
        {
            CrashObj.GetComponent<Ball>().ChangeState(Ball.STATE.DEAD);
        }
    }
}
