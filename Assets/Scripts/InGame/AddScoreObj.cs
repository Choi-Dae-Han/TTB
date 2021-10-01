using System.Collections;
using UnityEngine;

public class AddScoreObj : InGameObj
{
    [SerializeField] private float fMoveSpeed = 0.0f;
    [SerializeField] private Vector3 Target = Vector3.zero;

    private GameManager GM;
    private AudioSource AM;
    public AudioClip SoundEffect;

    new void Start()
    {
        base.Start();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        fMoveSpeed *= Time.fixedDeltaTime;
        AM = GM.GetComponent<AudioSource>();
        GM.nAddScoreObj++;
    }

    private IEnumerator MoveAndDelete()
    {
        GetComponent<Collider2D>().enabled = false;
        Vector3 TargetPos = transform.position + Target;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, fMoveSpeed);

            if (transform.position == TargetPos)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            GM.nAddScoreObj--;
            AM.PlayOneShot(SoundEffect);
            StartCoroutine(MoveAndDelete());
        }
    }
}
