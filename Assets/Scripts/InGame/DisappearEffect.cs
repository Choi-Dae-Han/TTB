using UnityEngine;

public class DisappearEffect : MonoBehaviour
{
    public SpriteRenderer SR;
    public float DisappearSpeed = 0f;

    private void Start()
    {
        SR.color -= new Color(0f, 0f, 0f, 0.4f);
    }

    void FixedUpdate()
    {
        Disappear();
    }

    void Disappear()
    {
        SR.color -= new Color(0f, 0f, 0f, DisappearSpeed * Time.smoothDeltaTime);

        if (SR.color.a <= 0f) Destroy(gameObject);
    }
}
