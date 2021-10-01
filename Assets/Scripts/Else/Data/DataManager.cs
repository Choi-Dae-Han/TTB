using UnityEngine;
using System.IO;

[System.Serializable]
public class DataManager : MonoBehaviour
{
#if UNITY_ANDROID
    public static string DataPath = "/";
#elif UNITY_IOS
    public static string DataPath = "/";
#else
    public static string DataPath = "/";
#endif

    public string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public void SaveData(string jsonData, string fileName)
    {
        File.WriteAllText(Application.persistentDataPath + DataPath + fileName + ".json", jsonData.ToString());
    }

    public T LoadData<T>(string fileName)
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + DataPath + fileName + ".json");

        return JsonUtility.FromJson<T>(jsonString);
    }

    public void SaveStageData(string stageName, bool opend, int gotCoin)
    {
        var data = LoadData<StageData>(stageName);
        data.Opened = opend;
        data.GotCoins = gotCoin;
        string jsonData = ObjectToJson(data);

        SaveData(jsonData, stageName);
    }

    public StageData LoadStageData(Stage s)
    {
        var data = LoadData<StageData>(s.gameObject.name);
        s.GotCoins = data.GotCoins;
        s.Opened = data.Opened;

        return data;
    }

    public void ResetStageData(string stageName)
    {
        TextAsset to = Resources.Load("Data/" + stageName) as TextAsset;
        var data = JsonUtility.FromJson<StageData>(to.text);
        string jsonData = ObjectToJson(data);
        SaveData(jsonData, stageName);
    }

    public void LoadMap(Stage s)
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + DataPath + s.gameObject.name + ".json");
        s.Data = JsonUtility.FromJson<StageData>(jsonString); // 로드
        s.CreateBackGround();
        s.CreateGrounds();
        s.CreateObj();
        s.CreateDeadLine();
    }

    public void SaveSkinGoodsData(string goodsname, int price, int moneyPrice, bool having)
    {
        GoodsData SG = new GoodsData(having, price, moneyPrice);
        string jsonData = ObjectToJson(SG);
        SaveData(jsonData, goodsname + "(Skin)");
    }

    public void SaveSoundEffectGoodsData(string goodsname, int price, int moneyPrice, bool having)
    {
        GoodsData GD = new GoodsData(having, price, moneyPrice);
        string jsonData = ObjectToJson(GD);
        SaveData(jsonData, goodsname + "(SoundEffect)");
    }

    public void SaveEffectGoodsData(string goodsname, bool having, int price, int moneyPrice, int kindOfEff)
    {
        GoodsData GD = new GoodsData(having,price, moneyPrice, kindOfEff);
        string jsonData = ObjectToJson(GD);
        SaveData(jsonData, goodsname + "(Effect)");
    }
}
