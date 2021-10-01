using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveAllData : MonoBehaviour
{
    public Sprite Skin;
    public AudioClip SE;
    public GameObject Effect;
    public int KindOfBallEffect = 0;
    public SkinGoods[] SkinGoods;
    public EffectGoods[] EffectGoods;
    public SoundEffectGoods[] SoundEffectGoods;
    public List<Stage> Stages = new List<Stage>();

    private void Start()
    {
        ResetData();
        PlayerPrefs.SetInt("FirstStart", 1);
        Destroy(gameObject);
    }

    public void ResetData()
    {
        //Debug.Log(PlayerPrefs.GetInt("FirstStart"));
        //PlayerPrefs.DeleteAll();

        DataManager DM = GetComponent<DataManager>();

        // 공 데이터 초기화
        BallData BD = new BallData(Effect, SE, Skin, KindOfBallEffect);
        string jsonData = DM.ObjectToJson(BD);
        DM.SaveData(jsonData, "BallData");

        // 플레이어 데이터 초기화
        PlayerData PD = new PlayerData(0);
        string jsonData1 = DM.ObjectToJson(PD);
        DM.SaveData(jsonData1, "PlayerData");

        // 스테이지 데이터 초기화
        for (int i = 0; i < Stages.Count; ++i)
        {
            DM.ResetStageData(Stages[i].gameObject.name);
        }

        // 스킨 상품 데이터 초기화
        DM.SaveSkinGoodsData(SkinGoods[0].gameObject.name, SkinGoods[0].Price, SkinGoods[0].MoneyPrice, true);
        for (int i = 1; i < SkinGoods.Length; ++i)
        {
            DM.SaveSkinGoodsData(SkinGoods[i].gameObject.name, SkinGoods[i].Price, SkinGoods[i].MoneyPrice, false);
        }

        DM.SaveEffectGoodsData(EffectGoods[0].gameObject.name, true, EffectGoods[0].Price, EffectGoods[0].MoneyPrice, 0);
        // 이펙트 상품 데이터 초기화
        for (int i = 1; i < EffectGoods.Length; ++i)
        {
            DM.SaveEffectGoodsData(EffectGoods[i].gameObject.name, false, EffectGoods[i].Price, EffectGoods[i].MoneyPrice, 0);
        }

        // 효과음 상품 데이터 초기화
        DM.SaveSoundEffectGoodsData(SoundEffectGoods[0].gameObject.name, SoundEffectGoods[0].Price, SoundEffectGoods[0].MoneyPrice, true);
        for (int i = 1; i < SoundEffectGoods.Length; ++i)
        {
            DM.SaveSoundEffectGoodsData(SoundEffectGoods[i].gameObject.name, SoundEffectGoods[i].Price, SoundEffectGoods[i].MoneyPrice, false);
        }
    }

#if UNITY_EDITOR
    public void OpenAllStage()
    {
        DataManager DM = GetComponent<DataManager>();
        for (int i = 0; i < Stages.Count; ++i)
        {
            DM.SaveStageData(Stages[i].gameObject.name, true, 0);
        }
    }
#endif
}
