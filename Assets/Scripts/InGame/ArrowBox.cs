using UnityEngine;

public class ArrowBox : InGameObj
{
    [SerializeField] private float fWeightlessSpeed = 0f;
    [SerializeField] private Vector3 Dir = Vector3.zero;
    [SerializeField] private Transform MuzzleTr = null;
    [SerializeField] private GameObject Effect = null;
    [SerializeField] private RectTransform[] EventTr = null;
    [SerializeField] private AudioClip SE_Wall = null;
    [SerializeField] private AudioClip SE_Ground = null;
    private float CrashingTime = 0f;
    private GameManager GM;
    private AudioSource AM;

    new void Start()
    {
        base.Start();
        fWeightlessSpeed *= Time.fixedDeltaTime;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.gameObject.GetComponent<AudioSource>();
        if (GetComponent<MovingBox>() != null)
            MB = GetComponent<MovingBox>();
    }

    private void BallHit(ref float ballVelo, float hitPower)
    {
        ballVelo = hitPower;
        AM.PlayOneShot(SE_Wall);
    }

    public void CreateEffect(Vector3 pos)
    {
        if (Effect != null)
        {
            GameObject obj = Instantiate(Effect);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.localScale = transform.localScale;
            obj.transform.position = pos;
        }
    }

    private void SendToDir(Ball ball, Vector3 effectPos)
    {
        AM.PlayOneShot(SE_Ground);
        ball.gameObject.transform.position = MuzzleTr.position + Dir * ball.fRadius;
        ball.ChangeState(Ball.STATE.WEIGHTLESS, fWeightlessSpeed * Dir.x, fWeightlessSpeed * Dir.y);
        if (Effect != null) CreateEffect(effectPos);
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Ball ball = CrashObj.gameObject.GetComponent<Ball>();
            Vector3 Contact = CrashObj.bounds.ClosestPoint(transform.position);
            float fDistLeft = Vector2.Distance(Contact, EventTr[0].position);
            float fDistRight = Vector2.Distance(Contact, EventTr[1].position);
            float fDistTop = Vector2.Distance(Contact, EventTr[2].position);
            float fDistBottom = Vector2.Distance(Contact, EventTr[3].position);

            if (ball.state == Ball.STATE.WEIGHTLESS) ball.ChangeState(Ball.STATE.LIVE);

            if (fDistTop < fDistLeft && fDistTop < fDistRight) // 위 충돌
            {
                if (ball.fReverseGravity > 0)
                    SendToDir(ball, Contact);
                else
                {
                    if (ball.TopCrashed) ball.ChangeState(Ball.STATE.DEAD);
                    ball.BottomCrashed = true;
                    if (MB == null) BallHit(ref ball.fYVelocity, ball.fHitPower);
                    else BallHit(ref ball.fYVelocity, ball.fHitPower + MB.fYVelocity);
                }
            }
            else if (fDistBottom <= fDistLeft && fDistBottom <= fDistRight)
            {
                if (ball.fReverseGravity > 0)
                {
                    if (ball.BottomCrashed) ball.ChangeState(Ball.STATE.DEAD);
                    ball.TopCrashed = true;
                    if (MB == null) BallHit(ref ball.fYVelocity, -ball.fHitPower);
                    else BallHit(ref ball.fYVelocity, -ball.fHitPower + MB.fYVelocity);
                }
                else
                    SendToDir(ball, Contact);
            }
            else if (fDistLeft <= fDistTop && fDistLeft <= fDistBottom)
            {
                if (ball.LeftCrashed) ball.ChangeState(Ball.STATE.DEAD);
                ball.RightCrashed = true;
                if (MB == null) BallHit(ref ball.fXVelocity, -ball.fHitPower);
                else BallHit(ref ball.fXVelocity, -ball.fHitPower + MB.fXVelocity);
            }
            else
            {
                if (ball.RightCrashed) ball.ChangeState(Ball.STATE.DEAD);
                ball.LeftCrashed = true;
                if (MB == null) BallHit(ref ball.fXVelocity, ball.fHitPower);
                else BallHit(ref ball.fXVelocity, ball.fHitPower + MB.fXVelocity);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            CrashingTime += Time.smoothDeltaTime;

            if (CrashingTime >= 0.1f)
                CrashObj.GetComponent<Ball>().ChangeState(Ball.STATE.DEAD);
        }
    }

    private void OnTriggerExit2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Ball ball = CrashObj.gameObject.GetComponent<Ball>();
            if (ball.BottomCrashed) ball.BottomCrashed = false;
            if (ball.LeftCrashed) ball.LeftCrashed = false;
            if (ball.RightCrashed) ball.RightCrashed = false;
            if (ball.TopCrashed) ball.TopCrashed = false;
            CrashingTime = 0f;
        }
    }
}
