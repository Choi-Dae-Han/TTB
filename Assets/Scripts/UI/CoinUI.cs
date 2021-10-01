using System.Collections;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public float Speed = 0f;
    bool IsSmaller = false;
    public Vector2 TargetScale = Vector2.one;
    Vector2 TempTargetScale = Vector2.one;
    public TMPro.TMP_Text textt;
    AudioSource AM;
    public AudioClip GetSound;
    public ShowOwnedCoin SOC;

    private void Awake()
    {
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            BounceCoin();
    }

    public void AddCoinAni()
    {
        textt.text = SOC.NumberOfCoin + 1 + "";
        SOC.NumberOfCoin += 1;
    }

    public void BounceCoin()
    {
        IsSmaller = false;
        transform.localScale = Vector2.one;
        TempTargetScale = TargetScale;
        AM.PlayOneShot(GetSound);
        AddCoinAni();
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        while (true)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, TempTargetScale, Speed * Time.smoothDeltaTime);
            if (transform.localScale.x > TempTargetScale.x - 0.01f)
            {
                TempTargetScale = Vector2.one;
                IsSmaller = true;
            }

            if (IsSmaller && transform.localScale.x < TempTargetScale.x + 0.01f)
            {
                transform.localScale = Vector3.one;
                yield break;
            }

            yield return null;
        }
    }
}
