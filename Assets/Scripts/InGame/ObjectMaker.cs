using UnityEngine;

public class ObjectMaker : InGameObj
{
    public GameObject ObjectToCreate;
    public float fCycle = 0f;
    public Vector2 Velo = Vector2.zero;
    public float fTime = 0f;
    public AudioSource AM;
    public AudioClip Sound = null;
    private GameManager GM;

    new void Start()
    {
        base.Start();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GetComponent<AudioSource>();
    }

    void Update()
    {
        MakeObject();
    }

    private void MakeObject()
    {
        if(fTime >= fCycle)
        {
            fTime -= fCycle;
            GameObject obj = Instantiate(ObjectToCreate);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<Rigidbody2D>().velocity = Velo;
            AM.PlayOneShot(Sound);
        }
        fTime += Time.smoothDeltaTime;
    }
}
