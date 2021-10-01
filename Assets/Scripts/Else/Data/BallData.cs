using UnityEngine;

public class BallData
{
    public AudioClip SE;
    public Sprite Skin;
    public GameObject Effect;
    public int KindOfEffect;

    public BallData(GameObject effect = null, AudioClip clip = null, Sprite skin = null, int kindOfEffet = 0)
    {
        SE = clip;
        Skin = skin;
        Effect = effect;
        KindOfEffect = kindOfEffet;
    }
}
