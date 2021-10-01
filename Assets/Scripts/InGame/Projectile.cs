using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float fPassedTime = 0f;
    public float fProjSpeed = 0f;
    public float fDeleteTime = 0f;

    public enum STATE
    {
        CREATE, MOVE, DELETE
    }
    public STATE State = STATE.CREATE;

    private void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch(State)
        {
            case STATE.MOVE:
                GoToDir();
                break;
        }
    }

    public void ChangeState(STATE s, float speed = 0f, float deleteTime = 0f, float scale = 0f)
    {
        if (State == s) return;
        State = s;

        switch(s)
        {
            case STATE.CREATE:
                break;
            case STATE.MOVE:
                fProjSpeed = speed;
                fDeleteTime = deleteTime;
                transform.localScale = new Vector3(scale, scale, scale);
                break;
            case STATE.DELETE:
                Destroy(gameObject);
                break;
        }
    }
    private void GoToDir()
    {
        fPassedTime += Time.smoothDeltaTime;
        if (fPassedTime >= fDeleteTime)
            ChangeState(STATE.DELETE);

        transform.Translate(new Vector2(0f, fProjSpeed * Time.smoothDeltaTime));
    }
}
