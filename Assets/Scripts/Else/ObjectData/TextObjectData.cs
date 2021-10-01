using UnityEngine;

[System.Serializable]
public class TextObjectData
{
    public Vector2 Scale;
    public string Content;
    public TextObjectData(Vector2 scale, string content)
    {
        Scale = scale;
        Content = content;
    }
}
