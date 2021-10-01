using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Vector2 TargetPos = Vector2.zero;
    Vector2 OriginPos = Vector2.zero;
    public float ItemSpeed = 0f;
    bool Dir = true;

    public GameManager GM;
    public AudioSource AM;
    public AudioClip GetSound;
    public AudioClip UseSound;
    public Ball ball;
    public Sprite ItemSprite;

    public void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        ItemSprite = GetComponent<SpriteRenderer>().sprite;
        ball = GM.UsingBall.GetComponent<Ball>();
        AM = GM.GetComponent<AudioSource>();
        OriginPos = transform.position;
        TargetPos += OriginPos;
    }

    private void Update()
    {
        ItemMove();
    }

    void ItemMove()
    {
        if (Dir)
        {
            transform.position = Vector2.Lerp(transform.position, TargetPos, ItemSpeed * 
                (Vector2.Distance(transform.position, OriginPos) + 0.1f) * Time.smoothDeltaTime);
            if (Vector2.Distance(transform.position, TargetPos) < 0.5f)
                Dir = false;
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, OriginPos, ItemSpeed *
                (Vector2.Distance(transform.position, TargetPos) + 0.1f) * Time.smoothDeltaTime);
            if (Vector2.Distance(transform.position, OriginPos) < 0.5f)
                Dir = true;
        }
    }

    public void GetItem(AbilityButton.Ability skill)
    {
        GM.NumOfAbility++;
        AM.PlayOneShot(GetSound);
        if (GM.NumOfAbility <= GM.MaxNumOfAbility)
        {
            for (int i = 0; i < GM.AbilityButtons.Count; ++i)
            {
                if (GM.AbilityButtons[i] == null)
                {
                    AbilityButton AB = GM.CreateUI(GM.AbilityButton, Vector3.zero, GM.AbilityButtonPos[i]).GetComponent<AbilityButton>();
                    AB.gameObject.GetComponent<Image>().sprite = ItemSprite;
                    AB.ability = skill;
                    AB.ItemUseSound = UseSound;
                    GM.AbilityButtons[i] = AB;
                    AB.gameObject.transform.localRotation = Quaternion.identity;
                    break;
                }
            }
        }

        Destroy(gameObject);
    }
}
