using UnityEngine;

public class NeedleSetting : MonoBehaviour
{
    public Needle[] Neddles;
    public FireMuzzle[] Muzzles;
    public float Cycle = 0f;
    public float AddTime = 0f;
    public float Diffrence = 0f;

    public void SetNeedle_Ascending()
    {
        for (int i = 0; i < Neddles.Length; ++i)
        {
            Neddles[i].fCycle = Cycle;
            Neddles[i].fFistAppearTime = AddTime + Diffrence * i;
        }
    }

    public void SetNeedle_Decending()
    {
        int j = 0;

        for (int i = Neddles.Length - 1; i >= 0; --i)
        {
            Neddles[i].fCycle = Cycle;
            Neddles[i].fFistAppearTime = AddTime + Diffrence * j;
            ++j;
        }
    }

    public void SetMuzzle_Ascending()
    {
        for(int i = 0; i < Muzzles.Length; ++i)
        {
            Muzzles[i].fFireDelay = Cycle;
            Muzzles[i].fFirstFireTime = AddTime + Diffrence + i;
        }
    }

    public void SetMuzzle_Decending()
    {
        int j = 0;

        for (int i = Muzzles.Length - 1; i >= 0; --i)
        {
            Muzzles[i].fFireDelay = Cycle;
            Muzzles[i].fFirstFireTime = AddTime + Diffrence * j;
            ++j;
        }
    }
}
