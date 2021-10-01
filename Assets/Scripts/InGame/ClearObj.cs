using System.Collections;
using UnityEngine;

public class ClearObj : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, MOVE, SHOWINGANI, DISAPPEAR, DONE
    }
    public STATE state = STATE.CREATE;

    public float fMoveSpeed = 0f;
    public int nScore = 0;

    public Transform DisappearCoinTr;
    public Vector3 TargetScale = Vector3.zero;

    ClearObj CO;
    ClearObj Obj_1;
    ClearObj Obj_2;

    GameManager GM;
    AudioSource AM;
    public AudioClip ClearSound;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        CO = GetComponent<ClearObj>();
        ChangeState(STATE.IDLE);
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
                break;
            case STATE.MOVE:
                switch (nScore)
                {
                    case 0:
                        ChangeState(STATE.DONE);
                        break;
                    default:
                        transform.SetParent(GM.MainCameraTr);
                        MoveToTarget(new Vector3(0f, 0f, 100f), TargetScale, STATE.SHOWINGANI);
                        break;
                }
                break;
            case STATE.SHOWINGANI:
                switch (nScore)
                {
                    case 2:
                        Obj_1 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr).GetComponent<ClearObj>();
                        Obj_1.gameObject.transform.localScale = transform.localScale;
                        Obj_1.MoveToTarget(new Vector3(-150f, 0f, 100f), TargetScale);
                        MoveToTarget(new Vector3(150f, 0f, 100f), TargetScale, STATE.DISAPPEAR);
                        break;
                    case 3:
                        Obj_1 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr).GetComponent<ClearObj>();
                        Obj_1.gameObject.transform.localScale = transform.localScale;
                        Obj_2 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr).GetComponent<ClearObj>();
                        Obj_2.gameObject.transform.localScale = transform.localScale;
                        Obj_1.MoveToTarget(new Vector3(-300f, 0f, 100f), TargetScale);
                        MoveToTarget(new Vector3(300f, 0f, 100f), TargetScale, STATE.DISAPPEAR);
                        break;
                    default:
                        ChangeState(STATE.DISAPPEAR);
                        break;
                }
                break;
            case STATE.DISAPPEAR:
                Vector3 disAppearCoinPos = DisappearCoinTr.position + new Vector3(0f, 0f, 100f);
                switch (nScore)
                {
                    case 1:
                        StartCoroutine(Disappear(disAppearCoinPos, CO, null, null));
                        break;
                    case 2:
                        Obj_1.StopAllCoroutines();
                        StartCoroutine(Disappear(disAppearCoinPos, Obj_1, CO, null));
                        break;
                    case 3:
                        Obj_1.StopAllCoroutines();
                        StartCoroutine(Disappear(disAppearCoinPos, Obj_1, Obj_2, CO));
                        break;
                }
                break;
            case STATE.DONE:
                GM.ShowClearUI(1f);
                if (Obj_1 != null) Destroy(Obj_1.gameObject);
                if (Obj_2 != null) Destroy(Obj_2.gameObject);
                Destroy(gameObject);
                break;
        }
    }

    public void MoveToTarget(Vector3 target, Vector3 targetScale, STATE s = STATE.CREATE, bool startCoinAni = false)
    {
        StartCoroutine(LerpToTarget(target, targetScale, s, startCoinAni));
    }

    public IEnumerator LerpToTarget(Vector3 target, Vector3 targetScale, STATE s, bool startCoinAni)
    {
        while (true)
        {
            if (transform != null)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, target, fMoveSpeed * Time.smoothDeltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, fMoveSpeed * Time.smoothDeltaTime);

                if (Mathf.Abs(transform.localScale.x - targetScale.x) < 0.03f &&
                    Vector3.Distance(transform.localPosition, target) < 10f)
                {
                    if (s != STATE.CREATE)
                        ChangeState(s);
                    if (startCoinAni)
                    {
                        DisappearCoinTr.gameObject.GetComponent<CoinUI>().BounceCoin();
                        Destroy(gameObject);
                    }
                    yield break;
                }
            }
            else
            {
                yield break;
            }

            yield return null;
        }
    }

    public IEnumerator Disappear(Vector3 disappearCoinPos, ClearObj obj1, ClearObj obj2, ClearObj obj3)
    {
        while (true)
        {
            if (obj2 != null) obj1.MoveToTarget(disappearCoinPos, Vector3.one, STATE.CREATE, true);
            else obj1.MoveToTarget(disappearCoinPos, Vector3.one, STATE.DONE, true);

            yield return new WaitForSeconds(0.3f);

            if (obj2 != null)
            {
                if (obj3 != null) obj2.MoveToTarget(disappearCoinPos, Vector3.one, STATE.CREATE, true);
                else obj2.MoveToTarget(disappearCoinPos, Vector3.one, STATE.DONE, true);
            }

            yield return new WaitForSeconds(0.3f);

            if (obj3 != null)
                obj3.MoveToTarget(disappearCoinPos, Vector3.one, STATE.DONE, true);

            yield break;
        }
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.CompareTag("Player"))
        {
            AM.PlayOneShot(ClearSound);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.SetParent(GM.StageTR);
            GM.CO = GetComponent<ClearObj>();
            GM.ChangeGameState(GameManager.GAMESTATE.CLEAR);
        }
    }
}
