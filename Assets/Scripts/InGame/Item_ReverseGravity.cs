using UnityEngine;

public class Item_ReverseGravity : Item
{
    private new void Start()
    {
        base.Start();
    }

    public void ItemAbility()
    {
        if (ball != null)
        {
            ball.ChangeState(Ball.STATE.LIVE);
            ball.fReverseGravity *= -1f;

            for (int j = 0; j < GM.AbilityButtonPos.Count; ++j)
            {
                if (GM.AbilityButtonPos[j] != null)
                    GM.AbilityButtonPos[j].transform.Rotate(new Vector3(180f, 0f, 0f));
            }
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
