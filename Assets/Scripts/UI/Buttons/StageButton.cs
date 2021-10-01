using UnityEngine;
using UnityEngine.UI;

public class StageButton : BasicButton
{
    public Stage stage;
    public RawImage BGI;
    public Texture2D BGIofButton;
    public RectTransform StageListRT;
    public Sprite CoinImage;
    public Image[] CoinUIPos;
    public StageButton[] NextStageButton;

    private new void Awake()
    {
        base.Awake();

        if (stage != null)
        {
            var data = GM.DM.LoadStageData(stage);
            if (data.Opened)
            {
                ChangeButtonState(BUTTONSTATE.UNLOCK);
            }

            if (CoinUIPos.Length != 0)
            {
                AddCoin(data.GotCoins);
            }
        }
    }

    public void PauseGame()
    {
        GM.Pause();
    }

    public void ResumeGame()
    {
        GM.Resume();
    }

    public void RetryGameStage()
    {
        GM.RetryStage();
    }

    public void LoadStage()
    {
        if (stage != null)
        {
            GM.PlayingStage = stage.gameObject;
            GM.SB = GetComponent<StageButton>();
            ChangeGMState(5);
        }
    }

    public void LoadNextStage()
    {
        GM.PlayingStage = GM.PlayingStage.GetComponent<Stage>().NextStage;
        GM.SB = GM.SB.NextStageButton[0];
        GM.FC.ResetCamera();
        ChangeGMState(5);
    }

    public void ExitGameStage()
    {
        GM.ExitStage();
    }

    public void ChangeBackGround()
    {
        BGI.texture = BGIofButton;
    }

    public void ShowStageList()
    {
        for (int i = 0; i < GM.NumOfArea; ++i)
        {
            if (StageListRT.GetChild(i) != UsingUI.transform)
                StageListRT.GetChild(i).gameObject.SetActive(false);
            else
                UsingUI.SetActive(true);
        }
    }

    public void AddCoin(int coin)
    {
        if (CoinUIPos.Length != 0)
        {
            for (int i = 0; i < coin; ++i)
            {
                CoinUIPos[i].sprite = CoinImage;
            }
        }
    }
}
