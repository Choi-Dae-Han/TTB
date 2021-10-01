using System.Collections;
using UnityEngine;

public class Rubble : MonoBehaviour
{
    public void BreakAni(float xVelo, float yVelo, float rotSpeed)
    {
        StartCoroutine(BreakAni_Cor(xVelo, yVelo, rotSpeed));
    }

    IEnumerator BreakAni_Cor(float xVelo, float yVelo, float rotSpeed)
    {
        float fTime = 0f;

        while (true)
        {
            fTime += Time.smoothDeltaTime;

            transform.position += new Vector3(xVelo * Time.smoothDeltaTime, yVelo * Time.smoothDeltaTime);
            transform.Rotate(0f, 0f, rotSpeed * Time.smoothDeltaTime);
            if (xVelo > 0.1) xVelo -= 100f * Time.smoothDeltaTime;
            if (xVelo < -0.1) xVelo += 100f * Time.smoothDeltaTime;
            yVelo -= 625f * Time.smoothDeltaTime;

            if (fTime >= 2f)
            {
                Destroy(transform.parent.gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
