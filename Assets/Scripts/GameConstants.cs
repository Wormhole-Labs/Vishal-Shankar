using UnityEngine;

public class GameConstants: MonoBehaviour
{
    public static GameConstants gameConstants;

    public string selectedCharacter;

    private void Awake()
    {
        GameConstants.gameConstants = this;
    }
}
