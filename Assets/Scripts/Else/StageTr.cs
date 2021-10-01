using UnityEngine;

public class StageTr : MonoBehaviour
{
    private void Awake()
    {
        transform.localScale = GameObject.Find("Canvas").transform.localScale;
    }
}
