using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviourPun
{
    public static UIController uIController;
    
    public GameObject emoteMenu;
    public GameObject spawnedEmoteHolder;

    public GameObject escapeMenu;

    public PlayerController myPlayer;

    Coroutine emoteDisappearRoutine;

    PhotonView pv;
    
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        UIController.uIController = this;
    }


    public void ToggleEmoteMenu()
    {
        if (emoteMenu.activeInHierarchy)
        {
            emoteMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            emoteMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ToggleEscapeMenu()
    {
        if(escapeMenu.activeInHierarchy)
        {
            myPlayer.paused = false;
            escapeMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            myPlayer.paused = true;
            escapeMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SendEmote(Transform emote)
    {
        pv.RPC("SendEmoteRPC", RpcTarget.AllViaServer, myPlayer.pv.ViewID, emote.GetSiblingIndex());
    }

    [PunRPC]
    void SendEmoteRPC(int playerID, int emoteID)
    {
        PlayerController player = PhotonView.Find(playerID).GetComponent<PlayerController>();
        player.myEmote.GetComponent<MeshRenderer>().material.mainTexture = emoteMenu.transform.GetChild(emoteID).GetComponent<Image>().sprite.texture;
        player.myEmote.gameObject.SetActive(true);

        if (player.emoteDisappearRoutine != null)
        {
            StopCoroutine(player.emoteDisappearRoutine);
        }
        player.emoteDisappearRoutine = StartCoroutine(DisableEmote(player));
    }

    IEnumerator DisableEmote(PlayerController pc)
    {
        yield return new WaitForSeconds(pc.playerConfig.emojiTime);
        pc.myEmote.gameObject.SetActive(false);
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.LeaveRoom();
    }
}
