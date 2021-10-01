using UnityEngine;
using UnityEngine.UI;

public class ShopButton : BasicButton
{
    public enum GOODSSTATE
    {
        UNSOLD, SOLD
    }
    public GOODSSTATE goodsState = GOODSSTATE.UNSOLD;

    Shop shop;
    public Transform GoodsImgTR;
    public RectTransform ContentRT;
    [SerializeField] private GameObject IapBtn;
    [SerializeField] private GameObject SellingEffect = null;
    [SerializeField] private GameObject SellingSE = null;
    [SerializeField] private GameObject SellingSkin = null;
    [SerializeField] private GameObject DoneUI = null;
    [SerializeField] private GameObject CancelUI = null;
    [SerializeField] private int KindOfEffect = 0;
    [SerializeField] private Text GoodsName = null;

    private new void Awake()
    {
        base.Awake();
        shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();

        if (SellingEffect != null)
        {
            var data = GM.DM.LoadData<GoodsData>(SellingEffect.name + "(Effect)");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
        if (SellingSE != null)
        {
            var data = GM.DM.LoadData<GoodsData>(SellingSE.name + "(SoundEffect)");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
        if (SellingSkin != null)
        {
            var data = GM.DM.LoadData<GoodsData>(SellingSkin.name + "(Skin)");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
    }

    public void ChangeGoodsStage(GOODSSTATE s)
    {
        if (goodsState == s) return;
        goodsState = s;

        switch (s)
        {
            case GOODSSTATE.UNSOLD:
                break;
            case GOODSSTATE.SOLD:
                ShowSoldImg();
                break;
        }
    }

    public void ShowShopList()
    {
        if (UsingUI == null)
        {
            ContentRT.GetChild(0).GetComponent<ChildUI>().ParentButton.GetComponent<BasicButton>()
                .ChangeButtonState(BUTTONSTATE.UNLOCK);
            ChangeButtonState(BUTTONSTATE.SELECTEDLOCK);
            GM.ClearChild(ContentRT);
            ContentRT.transform.localPosition = Vector3.zero;
            UsingUI = Instantiate(UI);
            UsingUI.GetComponent<ChildUI>().ParentButton = gameObject;
            UsingUI.transform.SetParent(ContentRT);
            UsingUI.transform.localPosition = ContentRT.rect.center;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void ShowGoodsInfo()
    {
        if (UsingUI == null)
        {
            GM.UsingWhiteScreen = GM.CreateUI(GM.WhiteScreen, Vector3.zero);
            GM.UsingWhiteScreen.GetComponent<RectTransform>().sizeDelta = GM.MainScreenTr.sizeDelta;
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;

            shop.Goodsif = UsingUI.GetComponent<GoodsInfo>();
            GoodsInfo GI = shop.Goodsif;

            GameObject btn1 = Instantiate(IapBtn);
            btn1.transform.SetParent(GI.BuyButtonTR);
            btn1.transform.localPosition = Vector3.zero;
            btn1.transform.localScale = Vector3.one;
            GameObject btn2 = Instantiate(shop.BuyButton1);
            btn2.transform.SetParent(GI.BuyButtonTR1);
            btn2.transform.localPosition = Vector3.zero;
            btn2.transform.localScale = Vector3.one;

            switch (goodsState)
            {
                case GOODSSTATE.UNSOLD:
                    btn1.GetComponent<Button>().interactable = true;
                    btn2.GetComponent<Button>().interactable = true;
                    GameObject btn23 = Instantiate(shop.TestApplyButton);
                    btn23.transform.SetParent(UsingUI.transform);
                    btn23.transform.position = GI.TestApplyButtonTR.position;
                    btn23.transform.localScale = Vector3.one;
                    break;
                case GOODSSTATE.SOLD:
                    GameObject btn3 = Instantiate(shop.ApplyButton);
                    btn3.transform.SetParent(UsingUI.transform);
                    btn3.transform.position = GI.ApplyButtonTR.position;
                    btn3.transform.localScale = Vector3.one;
                    break;
            }
            string goodsName = "";

            if (SellingSkin != null)
            {
                GI.SellingSkin = SellingSkin;
                goodsName = SellingSkin.name + "(Skin)";
            }
            if (SellingEffect != null)
            {
                GI.SellingEffect = SellingEffect;
                GI.KindOfEffect = KindOfEffect;
                goodsName = SellingEffect.name + "(Effect)";
            }
            if (SellingSE != null)
            {
                GI.SellingSE = SellingSE;
                goodsName = SellingSE.name + "(SoundEffect)";
            }
            GI.ParentButton = gameObject;
            GI.GoodsName.text = GoodsName.text;
            GI.GoodsImage.texture = gameObject.transform.GetChild(0).GetComponent<RawImage>().texture;

            var data = GM.DM.LoadData<GoodsData>(goodsName);
            GI.CoinPrice.text = data.Price + "";
            GI.MoneyPrice.text = data.MoneyPrice + "";
        }
    }

    public void BuyGoods_Coin()
    {
        var pData = GM.DM.LoadData<PlayerData>("PlayerData");
        GoodsInfo GI = transform.parent.parent.GetComponent<GoodsInfo>();
        int tempPrice = 0;
        if (GI.SellingSkin != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingSkin.name + "(Skin)");

            if (data.Price > pData.OwnedCoin)
            {
                GM.CreateUI(CancelUI, Vector3.zero);
            }
            else
            {
                if (!data.IsHaving)
                {
                    SkinGoods sprite1 = GI.SellingSkin.GetComponent<SkinGoods>();
                    GM.DM.SaveSkinGoodsData(GI.SellingSkin.name, data.Price, data.MoneyPrice, true);
                    tempPrice = data.Price;
                    ChangeBallSkin(sprite1.SellingSkin);
                    shop.ShopBallSprite = sprite1.SellingSkin;
                    shop.Text_Skin.text = sprite1.SellingSkin.name;
                    shop.BGIofShopBallSprite.color = Color.white;
                    GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                    GM.CreateUI(DoneUI, Vector3.zero);
                    Destroy(GI.gameObject);
                }
            }
        }
        if (GI.SellingEffect != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingEffect.name + "(Effect)");
            if (data.Price > pData.OwnedCoin)
            {
                GM.CreateUI(CancelUI, Vector3.zero);
            }
            else
            {
                if (!data.IsHaving)
                {
                    EffectGoods effect1 = GI.SellingEffect.GetComponent<EffectGoods>();
                    GM.DM.SaveEffectGoodsData(GI.SellingEffect.name, true, data.Price, data.MoneyPrice, effect1.KindOfEffect);
                    tempPrice = data.Price;
                    ChangeBallEffect(effect1.SellingEffect, effect1.KindOfEffect);
                    shop.ShopBallEffect = effect1.SellingEffect;
                    shop.KindOfShopBallEffect = effect1.KindOfEffect;
                    shop.Text_Effect.text = effect1.SellingEffect.name;
                    shop.BGIofShopBallEffect.color = Color.white;
                    GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                    GM.CreateUI(DoneUI, Vector3.zero);
                    Destroy(GI.gameObject);
                }
            }
        }
        if (GI.SellingSE != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingSE.name + "(SoundEffect)");
            if (data.Price > pData.OwnedCoin)
            {
                GM.CreateUI(CancelUI, Vector3.zero);
            }
            else
            {
                if (!data.IsHaving)
                {
                    SoundEffectGoods se1 = GI.SellingSE.GetComponent<SoundEffectGoods>();
                    GM.DM.SaveSoundEffectGoodsData(GI.SellingSE.name, data.Price, data.MoneyPrice, true);
                    tempPrice = data.Price;
                    ChangeBallSE(se1.SellingSE);
                    shop.ShopBallSE = se1.SellingSE;
                    shop.Text_SoundEffect.text = se1.SellingSE.name;
                    shop.BGIofShopBallSE.color = Color.white;
                    GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                    GM.CreateUI(DoneUI, Vector3.zero);
                    Destroy(GI.gameObject);
                }
            }
        }

        pData.OwnedCoin -= tempPrice;
        GM.DM.SaveData(GM.DM.ObjectToJson(pData), "PlayerData");
        GM.UsingOwnedCoinUI.GetComponent<ShowOwnedCoin>().UpdateCoin();
    }

    public void BuyGoods_Money()
    {
        GoodsInfo GI = shop.Goodsif;

        if (GI.SellingSkin != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingSkin.name + "(Skin)");
            if (!data.IsHaving)
            {
                SkinGoods sprite1 = GI.SellingSkin.GetComponent<SkinGoods>();
                GM.DM.SaveSkinGoodsData(GI.SellingSkin.name, data.Price, data.MoneyPrice, true);
                ChangeBallSkin(sprite1.SellingSkin);
                shop.ShopBallSprite = sprite1.SellingSkin;
                shop.Text_Skin.text = sprite1.SellingSkin.name;
                shop.BGIofShopBallSprite.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                GM.CreateUI(DoneUI, Vector3.zero);
                Destroy(GI.gameObject);
            }

        }
        if (GI.SellingEffect != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingEffect.name + "(Effect)");
            if (!data.IsHaving)
            {
                EffectGoods effect1 = GI.SellingEffect.GetComponent<EffectGoods>();
                GM.DM.SaveEffectGoodsData(GI.SellingEffect.name, true, data.Price, data.MoneyPrice, effect1.KindOfEffect);
                ChangeBallEffect(effect1.SellingEffect, effect1.KindOfEffect);
                shop.ShopBallEffect = effect1.SellingEffect;
                shop.KindOfShopBallEffect = effect1.KindOfEffect;
                shop.Text_Effect.text = effect1.SellingEffect.name;
                shop.BGIofShopBallEffect.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                GM.CreateUI(DoneUI, Vector3.zero);
                Destroy(GI.gameObject);
            }
        }
        if (GI.SellingSE != null)
        {
            var data = GM.DM.LoadData<GoodsData>(GI.SellingSE.name + "(SoundEffect)");
            if (!data.IsHaving)
            {
                SoundEffectGoods se1 = GI.SellingSE.GetComponent<SoundEffectGoods>();
                GM.DM.SaveSoundEffectGoodsData(GI.SellingSE.name, data.Price, data.MoneyPrice, true);
                ChangeBallSE(se1.SellingSE);
                shop.ShopBallSE = se1.SellingSE;
                shop.Text_SoundEffect.text = se1.SellingSE.name;
                shop.BGIofShopBallSE.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
                GM.CreateUI(DoneUI, Vector3.zero);
                Destroy(GI.gameObject);
            }
        }

        GM.UsingOwnedCoinUI.GetComponent<ShowOwnedCoin>().UpdateCoin();
        Destroy(transform.parent.gameObject);
    }

    public void Apply()
    {
        GoodsInfo GI = transform.parent.GetComponent<GoodsInfo>();
        var data = GM.DM.LoadData<BallData>("BallData");

        if (GI.SellingSkin != null)
        {
            Sprite sprite1 = GI.SellingSkin.GetComponent<SkinGoods>().SellingSkin;
            data.Skin = sprite1;
            shop.ShopBallSprite = sprite1;
            shop.Text_Skin.text = sprite1.name;
            shop.BGIofShopBallSprite.color = Color.white;
        }
        if (GI.SellingEffect != null)
        {
            EffectGoods EG = GI.SellingEffect.GetComponent<EffectGoods>();
            GameObject effect1 = EG.SellingEffect;
            data.Effect = effect1;
            data.KindOfEffect = EG.KindOfEffect;
            shop.ShopBallEffect = effect1;
            shop.KindOfShopBallEffect = EG.KindOfEffect;
            shop.Text_Effect.text = effect1.name;
            shop.BGIofShopBallEffect.color = Color.white;
        }
        if (GI.SellingSE != null)
        {
            AudioClip se1 = GI.SellingSE.GetComponent<SoundEffectGoods>().SellingSE;
            data.SE = se1;
            shop.ShopBallSE = se1;
            shop.Text_SoundEffect.text = se1.name;
            shop.BGIofShopBallSE.color = Color.white;
        }

        string jsonData = GM.DM.ObjectToJson(data);
        GM.DM.SaveData(jsonData, "BallData");
    }

    public void TestApply()
    {
        GoodsInfo GI = transform.parent.GetComponent<GoodsInfo>();
        if (GI.SellingSkin != null)
        {
            Sprite sprite1 = GI.SellingSkin.GetComponent<SkinGoods>().SellingSkin;
            shop.ShopBallSprite = sprite1;
            shop.Text_Skin.text = sprite1.name;
            shop.BGIofShopBallSprite.color = Color.blue;
        }
        if (GI.SellingEffect != null)
        {
            EffectGoods EG = GI.SellingEffect.GetComponent<EffectGoods>();
            GameObject effect1 = EG.SellingEffect;
            shop.ShopBallEffect = effect1;
            shop.KindOfShopBallEffect = EG.KindOfEffect;
            shop.Text_Effect.text = effect1.name;
            shop.BGIofShopBallEffect.color = Color.blue;
        }
        if (GI.SellingSE != null)
        {
            AudioClip se1 = GI.SellingSE.GetComponent<SoundEffectGoods>().SellingSE;
            shop.ShopBallSE = se1;
            shop.Text_SoundEffect.text = se1.name;
            shop.BGIofShopBallSE.color = Color.blue;
        }
    }

    public void ResetApply()
    {
        Shop shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();
        var data = GM.DM.LoadData<BallData>("BallData");

        if (data.Skin != null)
        {
            shop.ShopBallSprite = data.Skin;
            shop.Text_Skin.text = data.Skin.name;
            shop.BGIofShopBallSprite.color = Color.white;
        }
        if (data.Effect != null)
        {
            shop.ShopBallEffect = data.Effect;
            shop.KindOfShopBallEffect = data.KindOfEffect;
            shop.Text_Effect.text = data.Effect.name;
            shop.BGIofShopBallEffect.color = Color.white;
        }
        if (data.SE != null)
        {
            shop.ShopBallSE = data.SE;
            shop.Text_SoundEffect.text = data.SE.name;
            shop.BGIofShopBallSE.color = Color.white;
        }
    }

    public void ShowUI2()
    {
        if (UsingUI == null)
        {
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(shop.Goodsif.transform);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void ChangeBallEffect(GameObject effect, int kindOfEffect)
    {
        var data = GM.DM.LoadData<BallData>("BallData");
        data.Effect = effect;
        data.KindOfEffect = kindOfEffect;
        string jsonData = GM.DM.ObjectToJson(data);
        GM.DM.SaveData(jsonData, "BallData");
    }

    public void ChangeBallSkin(Sprite skin)
    {
        var data = GM.DM.LoadData<BallData>("BallData");
        data.Skin = skin;
        string jsonData = GM.DM.ObjectToJson(data);
        GM.DM.SaveData(jsonData, "BallData");
    }

    public void ChangeBallSE(AudioClip se)
    {
        var data = GM.DM.LoadData<BallData>("BallData");
        data.SE = se;
        string jsonData = GM.DM.ObjectToJson(data);
        GM.DM.SaveData(jsonData, "BallData");
    }

    public void ShowSoldImg()
    {
        GameObject obj = Instantiate(shop.SoldImg);
        obj.transform.SetParent(GoodsImgTR);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    public void LoadShopStage()
    {
        GM.shopStage.GetComponent<ShopStage>().shop = shop;
        GM.Ball_Obj_Shop.GetComponent<Ball>().shop = shop;
        ChangeGMState(4);
    }
}
