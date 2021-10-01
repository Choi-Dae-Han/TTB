using UnityEngine;
using UnityEngine.UI;


public class BasicButton : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    public enum BUTTONSTATE
    {
        LOCK, SELECTEDLOCK, UNLOCK
    }
    public BUTTONSTATE buttonState = BUTTONSTATE.UNLOCK;

    public GameObject UI;
    public GameObject UsingUI;
    public AudioClip OnMouseSE;
    public AudioSource AM;
    public GameManager GM;
    public GameObject UsingLockImg;
    Vector2 ButtonScale = Vector2.zero;

    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.gameObject.GetComponent<AudioSource>();
        ButtonScale = new Vector2(transform.localScale.x, transform.localScale.y);

        switch (buttonState)
        {
            case BUTTONSTATE.LOCK:
                GetComponent<Button>().interactable = false;
                break;
            case BUTTONSTATE.SELECTEDLOCK:
                GetComponent<Button>().interactable = false;
                transform.localScale = ButtonScale * 1.1f;
                break;
            case BUTTONSTATE.UNLOCK:
                break;
        }
    }

    public void ChangeButtonState(BUTTONSTATE s)
    {
        buttonState = s;

        switch (s)
        {
            case BUTTONSTATE.LOCK:
                GetComponent<Button>().interactable = false;
                break;
            case BUTTONSTATE.SELECTEDLOCK:
                GetComponent<Button>().interactable = false;
                transform.localScale = ButtonScale * 1.1f;
                break;
            case BUTTONSTATE.UNLOCK:
                GetComponent<Button>().interactable = true;
                transform.localScale = ButtonScale;
                if (UsingLockImg != null) Destroy(UsingLockImg);
                break;
        }
    }

    public void UISound(AudioClip sound)
    {
        AM.PlayOneShot(sound);
    }

    public void ShowUI()
    {
        if (UsingUI == null)
        {
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.parent);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void ShowExitUI()
    {
        GM.CreateExitUI();
    }

    public void DeleteUI()
    {
        if (GM.UsingWhiteScreen != null) Destroy(GM.UsingWhiteScreen);
        Destroy(transform.parent.gameObject);
    }

    public void DeleteUI2()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ChangeGMState(int i)
    {
        GM.ChangeGameState((GameManager.GAMESTATE)i);
    }

    // 폰에서는 없는게 나을듯
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (buttonState == BUTTONSTATE.UNLOCK)
    //    {
    //        transform.localScale = ButtonScale * 1.1f;
    //        AM.PlayOneShot(OnMouseSE);
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (buttonState == BUTTONSTATE.UNLOCK)
    //    {
    //        transform.localScale = ButtonScale;
    //    }
    //}
}
