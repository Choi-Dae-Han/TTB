using UnityEngine;
using UnityEngine.UI;

public class AwakeGame : MonoBehaviour
{
    private enum STATE
    {
        APPEARING, SHOWING, DISAPPEARING
    }
    private STATE state = STATE.APPEARING;

    [SerializeField] private Image Intro1 = null;
    [SerializeField] private Image Intro2 = null;
    [SerializeField] private float fSpeed = 0.8f;
    private GameManager GM;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        GetComponent<Transform>().localScale = GM.MainScreenTr.localScale;
    }

    private void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        GM.fSec += Time.smoothDeltaTime;
        if (GM.fSec >= 7f || Input.anyKeyDown && GM.fSec >= 2f)
        {
            GM.fSec = 0.0f;
            GM.ChangeGameState(GameManager.GAMESTATE.TITLEMENU);
        }

        switch (state)
        {
            case STATE.APPEARING:
                if (GM.fSec > 1f) ShowImage(Intro1);
                if (Intro1.color.a >= 1f) ShowImage(Intro2);
                if (Intro2.color.a >= 1f) ChangeState(STATE.SHOWING);
                break;
            case STATE.SHOWING:
                if (GM.fSec >= 5f) ChangeState(STATE.DISAPPEARING);
                break;
            case STATE.DISAPPEARING:
                DisappearText(Intro1);
                DisappearText(Intro2);
                break;
        }
    }

    private void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.APPEARING:
                break;
            case STATE.SHOWING:
                GetComponent<AudioSource>().Play();
                break;
            case STATE.DISAPPEARING:
                break;
        }
    }

    private void ShowImage(Image Intro)
    {
        if (Intro.color.a < 1f)
            Intro.color += new Color(0f, 0f, 0f, fSpeed * Time.smoothDeltaTime);
    }

    private void DisappearText(Image Intro)
    {
        if (Intro.color.a > 0f)
            Intro.color -= new Color(0f, 0f, 0f, fSpeed * Time.smoothDeltaTime);
    }
}
