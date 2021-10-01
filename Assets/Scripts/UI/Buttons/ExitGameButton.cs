using UnityEngine;

public class ExitGameButton : BasicButton
{
    private new void Awake()
    {
        base.Awake();
    }

    public void ExitAppication()
    {
        Application.Quit();
    }
}
