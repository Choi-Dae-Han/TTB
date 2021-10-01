using UnityEngine;

[System.Serializable]
public class ObjectMakerData
{
    public string ObjectToCreatePath;
    public float fCycle = 0f;
    public Vector2 Velo = Vector2.zero;
    public float fTime = 0f;

    public ObjectMakerData(string objectToCreatePath, float cycle, float time, Vector2 velo)
    {
        ObjectToCreatePath = objectToCreatePath;
        fCycle = cycle;
        fTime = time;
        Velo = velo;
    }
}
