public class GoodsData 
{
    public bool IsHaving = false;
    public int Price;
    public int MoneyPrice;
    public float KinfOfEffect = 0; 

    public GoodsData(bool ishaving, int price, int moneyPrice, int kinfOfEff = -1)
    {
        IsHaving = ishaving;
        if (kinfOfEff != -1)
            KinfOfEffect = kinfOfEff;
        Price = price;
        MoneyPrice = moneyPrice;
    }
}
