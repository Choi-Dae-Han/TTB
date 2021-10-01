using UnityEngine;

public class Box_Simple : InGameObj
{
    [SerializeField] private float fAddBouncePower = 0f;
    private float HalfBoxSize = 0f;
    private float CrashingTime = 0f;

    [SerializeField] private RectTransform[] EventTr = null;
    [SerializeField] private GameObject Effect = null;
    [SerializeField] private AudioClip SE_Wall = null;
    [SerializeField] private AudioClip SE_Ground = null;
    private GameManager GM;
    private AudioSource AM;

    new void Start()
    {
        base.Start();
        GameObject gm = GameObject.Find("GameManager");
        GM = gm.GetComponent<GameManager>();
        AM = gm.GetComponent<AudioSource>();
        HalfBoxSize = GetComponent<RectTransform>().rect.height * 0.5f;
        fAddBouncePower *= Time.fixedDeltaTime;
    }

    public void CreateEffect(Vector3 pos)
    {
        GameObject obj = Instantiate(Effect);
        obj.transform.SetParent(GM.StageTR);
        obj.transform.localScale = transform.localScale;
        obj.transform.position = pos;
    }

    private void BallBounce(Ball ball, Vector3 effectPos)
    {
        ball.gameObject.transform.position = new Vector3(ball.gameObject.transform.position.x,
                                                         transform.position.y + ball.fReverseGravity * (HalfBoxSize + ball.fRadius), 
                                                         0f);
        float crashPower = ball.fReverseGravity * (ball.fBouncePower + fAddBouncePower);
        if (MB != null) crashPower += MB.fYVelocity;
        ball.fYVelocity = crashPower;
        if (SE_Ground == null) AM.PlayOneShot(ball.BounceSound);
        else AM.PlayOneShot(SE_Ground);
        if (Effect != null) CreateEffect(effectPos);
    }

    private void BallHit(ref float ballVelo, float hitPower)
    {
        ballVelo = hitPower;
        AM.PlayOneShot(SE_Wall);
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
                if (ball.TopCrashed) ball.ChangeState(Ball.STATE.DEAD);
                ball.BottomCrashed = true;
                if (ball.fReverseGravity > 0) BallBounce(ball, Contact);
                else
                {
                    if(MB == null) BallHit(ref ball.fYVelocity, ball.fHitPower);
                    else BallHit(ref ball.fYVelocity, ball.fHitPower + MB.fYVelocity);
                }
            }
            else if (fDistBottom <= fDistLeft && fDistBottom <= fDistRight)
            {
                if (ball.BottomCrashed) ball.ChangeState(Ball.STATE.DEAD);
                ball.TopCrashed = true;
                if (ball.fReverseGravity > 0)
                {
                    if (MB == null) BallHit(ref ball.fYVelocity, -ball.fHitPower);
                    else BallHit(ref ball.fYVelocity, -ball.fHitPower + MB.fYVelocity);
                }
                else BallBounce(ball, Contact);
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
            if (ball.TopCrashed) ball.TopCrashed = false;
            if (ball.LeftCrashed) ball.LeftCrashed = false;
            if (ball.RightCrashed) ball.RightCrashed = false;
            CrashingTime = 0f;
        }
    }
}
