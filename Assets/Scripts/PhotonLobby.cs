using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public string globalRoomName;

    public static PhotonLobby photonLobby;

    public PlayerController localPlayer;
    
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(PhotonLobby.photonLobby == null)
        {
            PhotonLobby.photonLobby = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        MenuUIController.menuUIController.DisplayConnectionMessage("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       if(scene.buildIndex == 1 && localPlayer == null)
        {
            PhotonNetwork.JoinRoom(globalRoomName);
       }
    }
    
    public override void OnConnectedToMaster()
    {
        MenuUIController.menuUIController.ToggleCharacterSelectionScreen(true);
        MenuUIController.menuUIController.connectionStatsText.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room : " + PhotonNetwork.CurrentRoom.Name);
        localPlayer = FindObjectOfType<CharacterSpawner>().SpawnCharacter();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room. Creating new..");
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsOpen = true;
        roomOps.IsVisible = true;
        PhotonNetwork.CreateRoom(globalRoomName, roomOps, null);
    }


    public override void OnLeftRoom()
    {
        Debug.Log("Disconnected.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LoadLevel(0);
        StartCoroutine(ReconnectToServers());
    }

    IEnumerator ReconnectToServers()
    {
        yield return new WaitForSeconds(1f);

        MenuUIController.menuUIController.connectionStatsText.gameObject.SetActive(true);
        MenuUIController.menuUIController.connectionStatsText.text = "Lost connection. Attempting to reconnect..";

        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitForSeconds(0.5f);
        }

        PhotonNetwork.Reconnect();
        MenuUIController.menuUIController.connectionStatsText.gameObject.SetActive(false);
        MenuUIController.menuUIController.ToggleCharacterSelectionScreen(true);
    }
}
