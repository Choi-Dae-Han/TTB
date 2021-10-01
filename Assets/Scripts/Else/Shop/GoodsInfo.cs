using UnityEngine;
using UnityEngine.UI;

public class GoodsInfo : MonoBehaviour
{
    public RawImage GoodsImage;
    public GameObject ParentButton;
    public Transform TestApplyButtonTR;
    public Transform BuyButtonTR;
    public Transform BuyButtonTR1;
    public Transform ApplyButtonTR;

    public Text GoodsName; 
    public TMPro.TMP_Text MoneyPrice; 
    public TMPro.TMP_Text CoinPrice;

    public GameObject SellingEffect;
    public int KindOfEffect = 0;
    public GameObject SellingSE;
    public GameObject SellingSkin;
}
