using UnityEngine;

public class ShowOwnedCoin : MonoBehaviour
{
    public TMPro.TMP_Text textt;
    public Transform CoinTr;
    public int NumberOfCoin = 0;
    DataManager DM;

    private void Awake()
    { 
        DM = GameObject.Find("GameManager").GetComponent<DataManager>();
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        var data = DM.LoadData<PlayerData>("PlayerData");
        NumberOfCoin = data.OwnedCoin;
        textt.text = NumberOfCoin + "";
    }
}
