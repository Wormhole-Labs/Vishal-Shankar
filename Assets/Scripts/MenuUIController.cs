using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    public GameObject joinButton;
    public GameObject previewCharacter;

    public List<GameObject> characterList;

    public void SelectCharacter(Transform selectedCharacter)
    { 
        GameObject go = Instantiate(characterList.Find(x => x.name == selectedCharacter.name), previewCharacter.transform.position, previewCharacter.transform.rotation);
        Destroy(go.GetComponent<PlayerController>());
        Destroy(go.GetComponent<CharacterController>());
        Destroy(previewCharacter);
        previewCharacter = go;
        GameConstants.gameConstants.selectedCharacter = selectedCharacter.name;
        joinButton.SetActive(true);
    }

    public void StartGame()
    {
        if(GameConstants.gameConstants.selectedCharacter != null)
        {
            SceneManager.LoadScene(1);
        }
    }
}
