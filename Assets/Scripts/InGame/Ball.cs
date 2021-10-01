using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum STATE
    {
        CREATE, LIVE, WEIGHTLESS, DEAD
    }
    public STATE state = STATE.CREATE;

    public float fGravityScale = 6.25f;
    [SerializeField] private float fMoveSpeed = 6f;
    public float fRotateSpeed = 0f;
    public float fBouncePower = 450f;
    public float fHitPower = 120f;
    public float fXVelocity = 0f;
    public float fYVelocity = 0f;
    public float fRadius = 0f;
    public float fReverseGravity = 1f;
    public bool TopCrashed = false;
    public bool BottomCrashed = false;
    public bool LeftCrashed = false;
    public bool RightCrashed = false;
    [SerializeField] private float fMaxXVelocity = 200f;
    [SerializeField] private float fMaxYVelocity = 800f;
    [SerializeField] private float fDecreaseXVelocity = 2f;

    public bool IsMainStage = true;
    public Shop shop;

    //Effect
    public GameObject BallEffect;
    [SerializeField] private GameObject DeadEffect = null;

    //Sound
    public AudioClip BounceSound;
    [SerializeField] private AudioClip DeadSound = null;

    public SpriteRenderer SR;
    private AudioSource AM;
    public GameManager GM;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        SR = GetComponent<SpriteRenderer>();
        fRadius = GetComponent<CircleCollider2D>().radius;
        FollowCamera camera = GM.MainCameraTr.gameObject.GetComponent<FollowCamera>();
        camera.BallTr = transform;
        camera.ChangeState(FollowCamera.CAMERASTATE.FOLLOWING);
        fDecreaseXVelocity *= Time.fixedDeltaTime;
        fMaxXVelocity *= Time.fixedDeltaTime;
        fMaxYVelocity *= Time.fixedDeltaTime;
        fGravityScale *= Time.fixedDeltaTime;
        fBouncePower *= Time.fixedDeltaTime;
        fMoveSpeed *= Time.fixedDeltaTime;
        fHitPower *= Time.fixedDeltaTime;
        if (IsMainStage) LoadBallData();
        else LoadShopBallData();
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
                ChangeState(STATE.LIVE);
                break;
            case STATE.LIVE:
                Gravitypull();
                Move();
                break;
            case STATE.WEIGHTLESS:
                WeightlessMove();
                break;
            case STATE.DEAD:
                break;
        }
    }

    public void ChangeState(STATE s, float XVelo = 0f, float YVelo = 0f)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.LIVE:
                break;
            case STATE.WEIGHTLESS:
                fXVelocity = XVelo;
                fYVelocity = YVelo;
                break;
            case STATE.DEAD:
                Dead();
                break;
        }
    }

    private void Move()
    {
#if UNITY_ANDROID
        TouchInput();
#elif UNITY_IOS
        TouchInput();
#else
        if (fXVelocity > -fMaxXVelocity)
        {
            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.LeftArrow)) // 방향키 입력 및 최고 속도 제한
                fXVelocity -= fMoveSpeed;
        }
        if (fXVelocity < fMaxXVelocity)
        {
            if (Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.RightArrow))
                fXVelocity += fMoveSpeed;
        }
#endif
        if (fXVelocity > fDecreaseXVelocity) // 항상 X축 속도 감소
            fXVelocity -= fDecreaseXVelocity;
        else if (fXVelocity < -fDecreaseXVelocity)
            fXVelocity += fDecreaseXVelocity;
        else
            fXVelocity = 0.0f;

        transform.position += new Vector3(fXVelocity, 0f);
        Rotation();
    }

    private void Rotation()
    {
        transform.Rotate(0f, 0f, -fXVelocity * fRotateSpeed * fReverseGravity);
    }

    private void Gravitypull()
    {
        fYVelocity -= fGravityScale * fReverseGravity;

        if (fYVelocity > fMaxYVelocity) fYVelocity = fMaxYVelocity; // Y축 최고 속도 제한
        else if (fYVelocity < -fMaxYVelocity) fYVelocity = -fMaxYVelocity;

        transform.position += new Vector3(0.0f, fYVelocity);
    }

    private void WeightlessMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 TouchPos = GM.UICamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(TouchPos, Vector2.zero, 0f, GM.Layer_UI);

            if (!hit)
                ChangeState(STATE.LIVE);
        }
        transform.position += new Vector3(fXVelocity, fYVelocity);//계산 후 최종 이동
        Rotation();
    }

    private void Dead()
    {
        if (AM != null) AM.PlayOneShot(DeadSound);
        if (DeadEffect != null)
        {
            GameObject obj = Instantiate(DeadEffect);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }

        GM.DelayAndReset(); 

        Destroy(gameObject);
    }

    private void TouchInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 TouchPos = GM.UICamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(TouchPos, Vector2.zero, 0f, GM.Layer_UI);

            if (!hit)
            {
                if (TouchPos.x <= 0f && fXVelocity > -fMaxXVelocity) // 방향키 입력 및 최고 속도 제한
                    fXVelocity -= fMoveSpeed;
                if (TouchPos.x > 0f && fXVelocity < fMaxXVelocity)
                    fXVelocity += fMoveSpeed;
            }
        }
    }

    private void LoadBallData()
    {
        var data = GM.gameObject.GetComponent<DataManager>().LoadData<BallData>("BallData");
        if (data.Skin != null) SR.sprite = data.Skin;
        if (data.SE != null) BounceSound = data.SE;
        if (data.Effect != null)
        {
            BallEffect_AfterImage BE = data.Effect.GetComponent<BallEffect_AfterImage>();
            if (BE.ObjectForEffect != null)
            {
                GameObject effect = Instantiate(data.Effect);
                effect.transform.SetParent(transform);
                effect.transform.localPosition = Vector3.zero;
                BallEffect_AfterImage BE1 = effect.GetComponent<BallEffect_AfterImage>();
                BE1.ball = GetComponent<Ball>();
                BE1.ChangeState(BallEffect_AfterImage.STATE.IDLE, data.KindOfEffect);
            }
        }
    }

    private void LoadShopBallData()
    {
        if (shop.ShopBallSprite != null) SR.sprite = shop.ShopBallSprite;
        if (shop.ShopBallSE != null) BounceSound = shop.ShopBallSE;
        if (shop.ShopBallEffect != null)
        {
            BallEffect_AfterImage BE = shop.ShopBallEffect.GetComponent<BallEffect_AfterImage>();
            if (BE.ObjectForEffect != null)
            {
                GameObject effect = Instantiate(shop.ShopBallEffect);
                effect.transform.SetParent(transform);
                effect.transform.localPosition = Vector3.zero;
                BallEffect_AfterImage BE1 = effect.GetComponent<BallEffect_AfterImage>();
                BE1.ball = GetComponent<Ball>();
                BE1.ChangeState(BallEffect_AfterImage.STATE.IDLE, shop.KindOfShopBallEffect);
            }
        }
    }
}
