using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    public GameObject joinButton;
    public GameObject characterSelectionScreen;
    public GameObject previewCharacter;

    public List<GameObject> characterList;

    public Text connectionStatsText;

    public static MenuUIController menuUIController;

    public void Awake()
    {
        MenuUIController.menuUIController = this;
    }

    public void ToggleJoinButton(bool active)
    {
        joinButton.SetActive(active);
    }

    public void ToggleCharacterSelectionScreen(bool active)
    {
        characterSelectionScreen.SetActive(active);
    }

    public void SelectCharacter(Transform selectedCharacter)
    { 
        GameObject go = Instantiate(characterList.Find(x => x.name == selectedCharacter.name), previewCharacter.transform.position, previewCharacter.transform.rotation);
        Destroy(go.GetComponent<Photon.Pun.PhotonView>());
        Destroy(go.GetComponent<PlayerController>());
        Destroy(go.GetComponent<CharacterController>());
        Destroy(previewCharacter);
        previewCharacter = go;
        GameConstants.gameConstants.selectedCharacter = selectedCharacter.name;
        joinButton.SetActive(true);
    }

    public void DisplayConnectionMessage(string message)
    {
        connectionStatsText.text = message;
    }

    public void StartGame()
    {
        if(GameConstants.gameConstants.selectedCharacter != null)
        {
            PhotonNetwork.LoadLevel(PhotonLobby.photonLobby.MainSceneIndex);
        }
    }
}
