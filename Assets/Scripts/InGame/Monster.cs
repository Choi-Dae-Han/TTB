using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, MOVE, SAMEYWITHTARGET, DEAD
    }
    public STATE state = STATE.CREATE;

    public float fMoveSpeed = 50.0f;
    public float fTime = 0.0f;
    private int StepOfAngry = 0;
    [SerializeField] private float AngryTime = 4.0f;

    [SerializeField] private RectTransform Left = null;
    [SerializeField] private RectTransform Right = null;
    [SerializeField] private RectTransform Top = null;

    [SerializeField] private Transform TargetObj = null;

    [SerializeField] private AudioClip DeadSound = null;
    [SerializeField] private GameObject DeadEffect = null;

    private AudioSource AM;
    private GameManager GM;
    private Animator Ani;
    private SpriteRenderer SR;
    private Rigidbody2D RB;
    [SerializeField] private float ColSize;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        Ani = GetComponent<Animator>();
        ColSize = GetComponent<CapsuleCollider2D>().size.y;
    }
    private void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch(state)
        {
            case STATE.CREATE:
                ChangeState(STATE.IDLE);
                break;
            case STATE.IDLE:
                break;
            case STATE.MOVE:
                Angry();               
                if (Mathf.Abs(RB.velocity.y) <= 0.1f) MoveToTarget();
                break;
            case STATE.SAMEYWITHTARGET:
                Angry();
                if (TargetObj.position.x > transform.position.x + 30f ||
                    TargetObj.position.x < transform.position.x - 30f)
                    ChangeState(STATE.MOVE);
                break;
            case STATE.DEAD:
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                TargetObj = null;
                Ani.SetFloat("MoveSpeed", 0.0f);
                break;
            case STATE.MOVE:
                Ani.SetFloat("MoveSpeed", fMoveSpeed * 0.02f);
                break;
            case STATE.SAMEYWITHTARGET:
                Ani.SetFloat("MoveSpeed", 0.0f);
                break;
            case STATE.DEAD:
                Dead();
                break;
        }
    }

    private void MoveToTarget()
    {
        if (TargetObj.position.x > transform.position.x + 30f)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            RB.velocity = new Vector2(fMoveSpeed, 0f);
        }
        else if (TargetObj.position.x < transform.position.x - 30f)
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            RB.velocity = new Vector2(-fMoveSpeed, 0f);
        }
        else
        {
            ChangeState(STATE.SAMEYWITHTARGET);
        }
    }

    private void Angry()
    {
        if (StepOfAngry < 3)
        {
            fTime += Time.smoothDeltaTime;
            if (fTime >= AngryTime)
            {
                StepOfAngry++;
                fTime = 0f;
                SR.color -= new Color(0f, 0.25f, 0.25f, 0f);
                fMoveSpeed *= 1.3f;
                if(state == STATE.MOVE)
                    Ani.SetFloat("MoveSpeed", fMoveSpeed * 0.02f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Vector3 Contact = CrashObj.contacts[0].point;
            float fDistLeft = Vector2.Distance(Contact, Left.position);
            float fDistRight = Vector2.Distance(Contact, Right.position);
            float fDistTop = Vector2.Distance(Contact, Top.position);
            Ball ball = CrashObj.gameObject.GetComponent<Ball>();

            if (fDistTop < fDistLeft && fDistTop < fDistRight) // 위 충돌
            {
                ball.gameObject.transform.position = new Vector3(ball.gameObject.transform.position.x,
                             transform.position.y + ColSize * 0.5f + ball.fRadius, 0f);

                if (ball.fReverseGravity > 0)
                    ball.fYVelocity = ball.fBouncePower;
                else
                    ball.fYVelocity = ball.fHitPower;
                ChangeState(STATE.DEAD);
            }
            else
                ball.ChangeState(Ball.STATE.DEAD);
        }
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            TargetObj = CrashObj.gameObject.transform;
            ChangeState(STATE.MOVE);
        }
    }

    private void OnTriggerExit2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            ChangeState(STATE.IDLE);
        }
    }

    public void Dead()
    {
        if (AM != null)
            AM.PlayOneShot(DeadSound);
        if (DeadEffect != null)
        {
            GameObject obj = Instantiate(DeadEffect);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
        Destroy(gameObject);
    }
}
