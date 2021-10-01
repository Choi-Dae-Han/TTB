using UnityEngine;

public class Portal : InGameObj
{
    public Transform TargetPos;
    public GameObject ExitPortal;
    [SerializeField] private AudioClip TeleportSound = null;
    private AudioSource AM;

    new void Start()
    {
        base.Start();
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }
    private void Teleportation(Transform obj)
    {
        AM.PlayOneShot(TeleportSound);
        obj.position = TargetPos.position;
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if(CrashObj.gameObject.CompareTag("Player") ||
            CrashObj.gameObject.CompareTag("Monster"))
            Teleportation(CrashObj.gameObject.transform);
    }
}
