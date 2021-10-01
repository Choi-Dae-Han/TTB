using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        CREATE, TITLEMENU, STAGEMENU, SHOP, SHOPSTAGE, PLAYING, CLEAR
    }
    public GAMESTATE gamestate = GAMESTATE.CREATE;

    // 시간, 볼륨
    public float fVolume = 1f;
    public float fResetTime = 2f;
    public float fSec = 0f;
    public int nSec = 0;
    public int nMin = 0;

    // 공 오브젝트
    public GameObject Ball_Obj;
    public GameObject Ball_Obj_Shop;
    public GameObject UsingBall;

    // 일시정지 UI
    public GameObject PauseButtonUI;
    public GameObject PauseUI;
    public GameObject UsingPauseUI;

    // UI
    public GameObject OwnedCoinUI;
    public GameObject UsingOwnedCoinUI;
    public GameObject BackButtonUI;
    public GameObject ClearUI;
    public GameObject TimeUI;
    public GameObject AbilityButton;
    public GameObject AbilityUI;
    public GameObject ExitUI;
    public GameObject UsingExitUI;

    // 상태에 따른 큰 메뉴
    public GameObject TitleMenu;
    public GameObject StageMenu;
    public GameObject UsingStageMenu;
    public GameObject ShopMenu;
    public GameObject UsingShopMenu;

    //판넬
    public GameObject WhiteScreen;
    public GameObject UsingWhiteScreen;

    //스테이지
    public GameObject PlayingStage;
    public GameObject shopStage;
    public GameObject UsingStage;

    // 캔버스
    public RectTransform MainScreenTr;
    public RectTransform ObjectUIScreenTr;

    // 메인 카메라
    public Transform MainCameraTr;
    public FollowCamera FC;
    public Camera camera1;
    public Vector3 ResetCameraPos = new Vector3(0f, 0f, -100f);

    //UI 카메라
    public Camera UICamera;

    //기타
    public int NumOfArea = 6;
    public GameObject UsingStageBackGround;
    public GameObject FirstStartObj;
    public StageButton SB;
    public List<Transform> AbilityButtonPos = new List<Transform>();
    public List<AbilityButton> AbilityButtons = new List<AbilityButton>();
    public DataManager DM;
    public Transform StageTR;
    public Text TimeText;
    public ClearObj CO;
    public int NumOfAbility = 0;
    public int MaxNumOfAbility = 3;
    public int nAddScoreObj = 0;
    public float InputTime = 0f;
    public bool IsTouching = false;
    public int Layer_UI = 1 << 5;
    public Vector2 FirstTouchPos = Vector2.zero;
    public Vector2 SecondTouchPos = Vector2.zero;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //PlayerPrefs.SetInt("FirstStart", 0);
        if (PlayerPrefs.GetInt("FirstStart") != 1)
            Instantiate(FirstStartObj);
        for (int i = 0; i < MaxNumOfAbility; ++i)
        {
            AbilityButtonPos.Add(null);
            AbilityButtons.Add(null);
        }
        camera1 = MainCameraTr.gameObject.GetComponent<Camera>();
        FC = MainCameraTr.gameObject.GetComponent<FollowCamera>();
    }

    private void Start()
    {
        StageTR.localScale = Vector3.one;
        ObjectUIScreenTr.localScale = MainScreenTr.localScale;
        ObjectUIScreenTr.sizeDelta = MainScreenTr.sizeDelta;
    }

    void Update()
    {
        GameStateProcess();
    }

    void GameStateProcess()
    {
        switch (gamestate)
        {
            case GAMESTATE.TITLEMENU:
#if UNITY_ANDROID || UNITY_IOS
                if (Input.GetKeyUp(KeyCode.Escape))
                    CreateExitUI();
#endif
                break;
            case GAMESTATE.STAGEMENU:
#if UNITY_ANDROID || UNITY_IOS
                if (Input.GetKeyUp(KeyCode.Escape))
                    CreateExitUI();
#endif
                break;
            case GAMESTATE.SHOP:
#if UNITY_ANDROID || UNITY_IOS
                if (Input.GetKeyUp(KeyCode.Escape))
                    ChangeGameState(GAMESTATE.STAGEMENU);
#endif
                break;
            case GAMESTATE.SHOPSTAGE:
#if UNITY_ANDROID || UNITY_IOS
                if (Input.GetKeyUp(KeyCode.Escape))
                    ChangeGameState(GAMESTATE.SHOP);
#endif
                DoubleTabInput();
                break;
            case GAMESTATE.PLAYING:
#if UNITY_ANDROID || UNITY_IOS
                if (Input.GetKeyUp(KeyCode.Escape))
                    Pause();
#endif
                DoubleTabInput();
                CountTime();
                break;
        }
    }

    public void ChangeGameState(GAMESTATE s)
    {
        if (gamestate == s) return;
        gamestate = s;

        switch (s)
        {
            case GAMESTATE.TITLEMENU:
                LoadTitleMenu();
                break;
            case GAMESTATE.STAGEMENU:
                LoadStageMenu();
                break;
            case GAMESTATE.SHOP:
                LoadShopMenu();
                break;
            case GAMESTATE.SHOPSTAGE:
                LoadShopStage();
                break;
            case GAMESTATE.PLAYING:
                LoadStage();
                break;
            case GAMESTATE.CLEAR:
                StageClear();
                break;
        }
    }

    public void Pause()
    {
        if (UsingPauseUI == null)
        {
            Time.timeScale = 0.0f;
            UsingWhiteScreen = CreateUI(WhiteScreen, Vector3.zero);
            UsingWhiteScreen.GetComponent<RectTransform>().sizeDelta = MainScreenTr.sizeDelta;
            UsingPauseUI = CreateUI(PauseUI, Vector3.zero);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        if (UsingWhiteScreen != null) Destroy(UsingWhiteScreen);
    }

    public void CountTime()
    {
        fSec += Time.smoothDeltaTime;
        nSec = (int)fSec;
        if (fSec >= 60f)
        {
            nSec = 0;
            fSec -= 60f;
            ++nMin;
        }

        if (TimeText != null) TimeText.text = nMin + " : " + nSec;
    }

    public void ResetTime()
    {
        nSec = 0;
        fSec = 0f;
        nMin = 0;
    }

    public void ResetAbilitySlot()
    {
        for(int i = 0; i < AbilityButtonPos.Count; ++i)
        {
            AbilityButtonPos[i].transform.localRotation = Quaternion.identity;
        }
        for (int i = 0; i < AbilityButtons.Count; ++i)
        {
            if (AbilityButtons[i] != null)
                Destroy(AbilityButtons[i].gameObject);
        }
    }

    private void DoubleTabInput()
    {
        if (NumOfAbility != 0)
        {
            if (IsTouching)
            {
                InputTime += Time.smoothDeltaTime;
                if (InputTime >= 0.2f)
                    ResetDoubleTouchInput();

                if (Input.GetMouseButtonDown(0))
                {
                    SecondTouchPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(SecondTouchPos, Vector2.zero, 0f, Layer_UI);

                    if (Vector2.Distance(FirstTouchPos, SecondTouchPos) >= 100f)
                        ResetDoubleTouchInput();
                    else if (!hit)
                    {
                        if (UsingBall != null)
                            AbilityButtons[0].UseItem();
                        ResetDoubleTouchInput();
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                FirstTouchPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(FirstTouchPos, Vector2.zero, 0f, Layer_UI);
                if (!hit)
                    IsTouching = true;
            }
        }
    }

    private void ResetDoubleTouchInput()
    {
        InputTime = 0f;
        IsTouching = false;
    }

    public GameObject CreateObject(GameObject obj, Vector3 pos, Transform parentTR = null)
    {
        GameObject Temp = Instantiate(obj);
        if (parentTR != null)
            Temp.transform.SetParent(parentTR);
        Temp.transform.localScale = Vector3.one;
        Temp.transform.position = pos;

        return Temp;
    }

    public GameObject CreateUI(GameObject obj, Vector3 pos, Transform parentTr = null)
    {
        GameObject Temp = Instantiate(obj);
        if (parentTr == null)
            Temp.transform.SetParent(MainScreenTr);
        else
            Temp.transform.SetParent(parentTr);
        Temp.transform.localScale = Vector3.one;
        Temp.transform.localPosition = pos;

        return Temp;
    }

    public GameObject CreateAnchoredUI(GameObject obj, Vector2 aPos, Transform parentTr = null)
    {
        RectTransform Temp = Instantiate(obj).GetComponent<RectTransform>();
        if (parentTr == null)
            Temp.transform.SetParent(MainScreenTr);
        else
            Temp.transform.SetParent(parentTr);
        Temp.transform.localScale = Vector3.one;
        Temp.anchoredPosition = aPos;

        return Temp.gameObject;
    }

    public void ClearChild(Transform tr)
    {
        int ChildCount = tr.childCount;
        for (int i = 0; i < ChildCount; ++i)
        {
            Destroy(tr.GetChild(i).gameObject);
        }
    }

    public void LoadTitleMenu()
    {
        UICamera.clearFlags = CameraClearFlags.Depth;
        ClearChild(MainScreenTr);
        CreateUI(TitleMenu, Vector3.zero);
    }

    public void LoadStageMenu()
    {
        ClearChild(MainScreenTr);
        if (UsingStageMenu != null)
        {
            UsingStageMenu.transform.SetParent(MainScreenTr);
            UsingStageMenu.SetActive(true);
        }
        else
            UsingStageMenu = CreateUI(StageMenu, Vector3.zero);

        if (UsingOwnedCoinUI == null)
        {
            UsingOwnedCoinUI = CreateUI(OwnedCoinUI, Vector3.zero);
            UsingOwnedCoinUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(110f, -70f);
        }
        else
            UsingOwnedCoinUI.GetComponent<ShowOwnedCoin>().UpdateCoin();

        UsingOwnedCoinUI.transform.SetParent(UsingStageMenu.transform);
    }

    public void LoadShopMenu()
    {
        if (UsingStageMenu != null && UsingStageMenu.transform.parent != null)
        {
            UsingStageMenu.transform.SetParent(null);
            UsingStageMenu.SetActive(false);
        }
        if (UsingBall != null) UsingBall.tag = "Untagged";
        Destroy(UsingStageBackGround);
        ClearChild(MainScreenTr);
        ClearChild(StageTR);
        ClearChild(ObjectUIScreenTr);
        if (UsingShopMenu == null)
        {
            UsingShopMenu = CreateUI(ShopMenu, Vector3.zero);
            UsingOwnedCoinUI.transform.SetParent(UsingShopMenu.transform);
        }
        else
        {
            UsingShopMenu.transform.SetParent(MainScreenTr);
            UsingShopMenu.SetActive(true);
        }
        StopAllCoroutines();
        FC.ResetCamera();
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);
    }

    public void LoadStage()
    {
        UsingStageMenu.transform.SetParent(null);
        UsingStageMenu.SetActive(false);
        if (UsingBall != null) UsingBall.tag = "Untagged";
        nAddScoreObj = 0;
        NumOfAbility = 0;
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);
        Destroy(UsingStageBackGround);
        ClearChild(MainScreenTr);
        ClearChild(StageTR);
        ClearChild(ObjectUIScreenTr);
        StopAllCoroutines();
        ResetTime();
        CreateAnchoredUI(PauseButtonUI, new Vector2(-110f, -70f));
        CreateAnchoredUI(AbilityUI, new Vector2(-320f, -70f));
        TimeText = CreateAnchoredUI(TimeUI, new Vector2(0f, -70f)).GetComponent<Text>();
        if (PlayingStage != null)
        {
            UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);
            Stage s = UsingStage.GetComponent<Stage>();
            UsingStage.name = PlayingStage.name;
            DM.LoadStageData(s);
            DM.LoadMap(s);
            UsingBall = CreateObject(Ball_Obj, Vector3.zero, StageTR);
        }
        Resume();
    }

    public void LoadShopStage()
    {
        UsingShopMenu.transform.SetParent(null);
        UsingShopMenu.SetActive(false);
        NumOfAbility = 0;
        ClearChild(MainScreenTr);
        ClearChild(ObjectUIScreenTr);
        CreateAnchoredUI(BackButtonUI, new Vector2(130f, -130f));
        CreateAnchoredUI(AbilityUI, new Vector2(-320f, -70f));
        PlayingStage = shopStage;
        UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);
        UsingBall = CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
    }

    public void RetryStage()
    {
        if (gamestate != GAMESTATE.PLAYING)
            ChangeGameState(GAMESTATE.PLAYING);
        else
            LoadStage();
    }

    public void StageClear()
    {
        ClearChild(MainScreenTr);
        ShowOwnedCoin Temp = CreateAnchoredUI(OwnedCoinUI, new Vector2(110f, -70f)).GetComponent<ShowOwnedCoin>();
        CO.DisappearCoinTr = Temp.CoinTr;
        FC.BallTr = null;

        Stage s = UsingStage.GetComponent<Stage>();
        var data = DM.LoadData<StageData>(PlayingStage.name);

        int score = 1;
        if (nAddScoreObj == 0) score++;
        if (nMin * 60 + fSec < s.fLimitTime) score++;

        if (score > data.GotCoins)
        {
            SB.AddCoin(score);
            CO.nScore = score - data.GotCoins;
            DM.SaveStageData(PlayingStage.name, true, score);
            SavePlayerData(score - data.GotCoins);

            if (s.NextStage != null)
            {
                var nextStageData = DM.LoadData<StageData>(s.NextStage.name);
                if (!nextStageData.Opened)
                {
                    for (int i = 0; i < SB.NextStageButton.Length; ++i)
                    {
                        if (SB.NextStageButton[i] != null)
                        {
                            SB.NextStageButton[i].ChangeButtonState(BasicButton.BUTTONSTATE.UNLOCK);
                            SB.NextStageButton[i].transform.localScale = Vector3.one; // SetActive가 null일때 참조하려하면 스케일이 0이되는 버그때문에 해놓음
                        }
                    }
                    nextStageData.Opened = true;
                    DM.SaveStageData(s.NextStage.name, true, 0);
                }
            }
        }
        CO.ChangeState(ClearObj.STATE.MOVE);
    }

    public void ExitStage()
    {
        if (UsingBall != null) UsingBall.tag = "Untagged";
        PlayingStage = null;
        TimeText = null;
        CO = null;
        nAddScoreObj = 0;
        NumOfAbility = 0;
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);
        Destroy(UsingStageBackGround);
        ClearChild(StageTR);
        ClearChild(ObjectUIScreenTr);
        FC.ResetCamera();
        ResetTime();
        Resume();
        ChangeGameState(GAMESTATE.STAGEMENU);
    }

    public void ShowClearUI(float delay)
    {
        StartCoroutine(ShowClearUI_Cor(delay));
    }

    public IEnumerator ShowClearUI_Cor(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CreateUI(ClearUI, Vector2.zero);
            yield break;
        }
    }

    public void DelayAndReset()
    {
        StartCoroutine(DelayAndReset_Cor());
    }

    IEnumerator DelayAndReset_Cor()
    {
        while (true)
        {
            yield return new WaitForSeconds(fResetTime);

            if (gamestate != GAMESTATE.PLAYING &&
                gamestate != GAMESTATE.SHOPSTAGE)
                yield break;

            ClearChild(StageTR);
            Destroy(UsingStageBackGround);
            ClearChild(ObjectUIScreenTr);

            UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);
            Stage s = UsingStage.GetComponent<Stage>();
            UsingStage.name = PlayingStage.name;

            switch (gamestate)
            {
                case GAMESTATE.PLAYING:
                    DM.LoadStageData(s);
                    DM.LoadMap(s);
                    ResetTime();
                    nAddScoreObj = 0;
                    NumOfAbility = 0;
                    ResetAbilitySlot();
                    UsingBall =
                        CreateObject(Ball_Obj, Vector3.zero, StageTR);
                    break;
                case GAMESTATE.SHOPSTAGE:
                    NumOfAbility = 0;
                    ResetAbilitySlot();
                    UsingBall =
                        CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
                    break;
            }

            yield break;
        }
    }

    public void SavePlayerData(int coin)
    {
        var data = DM.LoadData<PlayerData>("PlayerData");
        data.OwnedCoin += coin;
        string jsonData = DM.ObjectToJson(data);
        DM.SaveData(jsonData, "PlayerData");
    }

    public void CreateExitUI()
    {
        if (UsingExitUI == null)
            UsingExitUI = CreateUI(ExitUI, Vector2.zero);
    }
}
