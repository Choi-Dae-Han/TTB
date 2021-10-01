using UnityEngine;

public class Item_Dash : Item
{
    public float SavingXVelo = 0f;
    public float SavingYVelo = 0f;

    private new void Start()
    {
        base.Start();
        SavingXVelo *= Time.fixedDeltaTime;
        SavingYVelo *= Time.fixedDeltaTime;
    }

    public void ItemAbility()
    {
        if (ball != null)
        {
            ball.ChangeState(Ball.STATE.LIVE);
            ball.fXVelocity = SavingXVelo;
            ball.fYVelocity = SavingYVelo * ball.fReverseGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D crashObj)
    {
        if (crashObj.gameObject.CompareTag("Player"))
        {
            if (ball != null & GM.NumOfAbility < GM.MaxNumOfAbility)
                GetItem(ItemAbility);
        }
    }
}
