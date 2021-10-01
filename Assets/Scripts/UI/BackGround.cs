using UnityEngine;

public class BackGround : MonoBehaviour
{
    private void Start()
    {
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GM.gamestate == GameManager.GAMESTATE.PLAYING ||
            GM.gamestate == GameManager.GAMESTATE.SHOPSTAGE)
        {
            float XRatio = GM.MainScreenTr.sizeDelta.x / 1280f;
            float YRatio = GM.MainScreenTr.sizeDelta.y / 720f;
            GM.UsingStageBackGround = gameObject;
            gameObject.transform.SetParent(GM.MainCameraTr);
            gameObject.transform.localPosition = new Vector3(0f, 0f, 100f);
            gameObject.transform.localScale = new Vector3(XRatio * GM.MainScreenTr.localScale.x, YRatio * GM.MainScreenTr.localScale.y, 1f); 
        }
        else
            GetComponent<RectTransform>().sizeDelta = GM.MainScreenTr.sizeDelta;
    }
}
