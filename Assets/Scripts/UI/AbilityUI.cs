using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    GameManager GM;
    public Transform[] ButtonPos;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i = 0; i < ButtonPos.Length; ++i)
            GM.AbilityButtonPos[i] = ButtonPos[i];
    }
}
