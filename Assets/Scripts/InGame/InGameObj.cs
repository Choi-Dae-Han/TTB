using UnityEngine;

public class InGameObj : MonoBehaviour
{
    public MovingBox MB;

    public void Start()
    {
        if (GetComponent<MovingBox>() != null)
            MB = GetComponent<MovingBox>();
    }
}
