using UnityEngine;

[CreateAssetMenu(menuName = "Create Player Config")]
public class PlayerConfig : ScriptableObject
{
    public float walkSpeed;
    public float rotateSpeed;
    public float emojiTime;
    public float cameraDistance;
    public float cameraHeight;
    public Vector2 cameraTiltRange;
    public float cameraTiltSpeed;
}
