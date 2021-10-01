using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public delegate void Ability();
    public Ability ability;
    public AudioClip ItemUseSound;

    GameManager GM;
    AudioSource AM;
    Ball ball;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        ball = GM.UsingBall.GetComponent<Ball>();
    }

    public void UseItem()
    {
        if (ball != null && ability != null)
        {
            ability();
            AM.PlayOneShot(ItemUseSound);
            ability = null;
            --GM.NumOfAbility;

            if (GM.NumOfAbility >= 1)
            {
                GM.AbilityButtons[1].transform.SetParent(GM.AbilityButtonPos[0]);
                GM.AbilityButtons[1].transform.localPosition = Vector3.zero;
                GM.AbilityButtons[0] = GM.AbilityButtons[1];
                GM.AbilityButtons[1] = null;
            }
            if (GM.NumOfAbility >= 2)
            {
                GM.AbilityButtons[2].transform.SetParent(GM.AbilityButtonPos[1]);
                GM.AbilityButtons[2].transform.localPosition = Vector3.zero;
                GM.AbilityButtons[1] = GM.AbilityButtons[2];
                GM.AbilityButtons[2] = null;
            }

            Destroy(gameObject);
        }
    }
}
