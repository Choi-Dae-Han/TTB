using UnityEngine;

public class FireMuzzle : MonoBehaviour
{
    public enum STATE
    {
        CREATE, FIRSTFIRE, IDLE, ATTACK
    }
    public STATE state = STATE.CREATE;

    public float fTime = 0f;
    public float fFirstFireTime = 0f;
    public float fDeleteTime = 0f;
    public float fFireDelay = 0f;
    public float ProjSpeed = 100f;
    public float ProjScale = 1f;
    public GameObject ProjObj = null;
    public AudioSource AM = null;
    public GameManager GM = null;
    public AudioClip SoundEffect = null;

    private void Start()
    {
        AM = transform.parent.GetComponent<AudioSource>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                ChangeState(STATE.FIRSTFIRE);
                break;
            case STATE.FIRSTFIRE:
                fTime += Time.smoothDeltaTime;
                if (fTime >= fFireDelay)
                    ChangeState(STATE.ATTACK);
                break;
            case STATE.IDLE:
                fTime += Time.smoothDeltaTime;
                if (fTime >= fFireDelay)
                    ChangeState(STATE.ATTACK);
                break;
            case STATE.ATTACK:
                break;
        }
    }

    private void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.FIRSTFIRE:
                fTime = fFirstFireTime;
                break;
            case STATE.IDLE:
                fTime = 0.0f;
                break;
            case STATE.ATTACK:
                fire();
                ChangeState(STATE.IDLE);
                break;
        }
    }

    private void fire()
    {
        if (AM != null)
        {
            AM.clip = SoundEffect;
            AM.Play();
        }
        Projectile proj = Instantiate(ProjObj).GetComponent<Projectile>();
        proj.transform.SetParent(GM.StageTR);
        proj.transform.localScale = transform.localScale;
        proj.transform.rotation = transform.rotation;
        proj.transform.position = transform.position;
        proj.ChangeState(Projectile.STATE.MOVE, ProjSpeed, fDeleteTime, ProjScale);
    }
}
