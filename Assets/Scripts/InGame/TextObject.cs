using UnityEngine;

public class TextObject : MonoBehaviour
{
    void Start()
    {
        GameManager GM =GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.SetParent(GM.ObjectUIScreenTr);
    }
}
