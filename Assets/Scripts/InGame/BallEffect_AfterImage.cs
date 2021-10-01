using UnityEngine;

public class BallEffect_AfterImage : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE
    }
    public STATE state = STATE.CREATE;
    public delegate void KindOfEffect();
    KindOfEffect KOE;
    public float fTime = 0f;
    public float ShowCycle = 0;
    public GameObject ObjectForEffect;
    public Sprite Skin;
    public Ball ball;
    public Color[] colors;

    private void Update()
    {
        StateProcess();
    }

    public void StateProcess()
    {
        switch(state)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                KOE();
                break;
        }
    }

    public void ChangeState(STATE s, int kindOfEffect)
    {
        if (state == s) return;
        state = s;

        switch(s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                WhatKindOfEffect(kindOfEffect);
                break;
        }
    }

    public void WhatKindOfEffect(int kind)
    {
        switch(kind)
        {
            case 1:
                Skin = ball.SR.sprite;
                KOE = AfterImage;
                break;
            case 2:
                Skin = ObjectForEffect.GetComponent<SpriteRenderer>().sprite;
                KOE = AfterImage_Asset;
                break;
        }
    }

    public void AfterImage()
    {
        fTime += Time.smoothDeltaTime;

        if (fTime >= ShowCycle)
        {
            fTime -= ShowCycle;

            SpriteRenderer obj = Instantiate(ObjectForEffect).GetComponent<SpriteRenderer>();
            obj.sprite = Skin;
            obj.transform.SetParent(ball.GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
    }

    public void AfterImage_Asset()
    {
        fTime += Time.smoothDeltaTime;

        if (fTime >= ShowCycle)
        {
            fTime -= ShowCycle;
            Vector3 randomPos = GetRandomPos();
            Vector2 randomScale = GetRandomScale();
            Vector3 ramdomRotation = GetRandomRotation();
            Color randomColor = GetRandomColor();

            SpriteRenderer obj = Instantiate(ObjectForEffect).GetComponent<SpriteRenderer>();
            obj.transform.SetParent(ball.GM.StageTR);
            obj.transform.position = randomPos;
            obj.transform.eulerAngles = ramdomRotation;
            obj.transform.localScale = randomScale;
            obj.color = randomColor;
            obj.sprite = Skin;
        }
    }

    public Vector3 GetRandomPos()
    {
        float x = Random.Range(-11f, 11f);
        float y = Random.Range(-11f, 11f);
        Vector3 pos = transform.position + new Vector3(x, y, 0f);

        return pos;
    }

    public Vector2 GetRandomScale()
    {
        float value = Random.Range(1f, 1.8f);
        Vector2 scale = new Vector2(value, value);

        return scale;
    }

    public Vector3 GetRandomRotation()
    {
        float value = Random.Range(-60f, 60f);
        Vector3 rot = new Vector3(0f, 0f, value);

        return rot;
    }

    public Color GetRandomColor()
    {
        int value = Random.Range(1, 5);
        Color c = Color.white;
        switch(value)
        {
            case 1:
                c = colors[0];
                break;
            case 2:
                c = colors[1];
                break;
            case 3:
                c = colors[2];
                break;
            case 4:
                c = colors[3];
                break;
        }

        return c;
    }
}
