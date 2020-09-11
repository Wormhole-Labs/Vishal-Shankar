using UnityEngine;

public class GameConstants: MonoBehaviour
{
    public static GameConstants gameConstants;

    public string selectedCharacter;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(GameConstants.gameConstants == null)
        {
            GameConstants.gameConstants = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
