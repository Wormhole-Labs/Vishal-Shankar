using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController uIController;
    
    public GameObject emoteMenu;
    public GameObject spawnedEmoteHolder;

    public PlayerController myPlayer;

    Coroutine emoteDisappearRoutine;
    
    private void Awake()
    {
        UIController.uIController = this;
    }

    public void ToggleEmoteMenu()
    {
        if(emoteMenu.activeInHierarchy)
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

    public void SendEmote(Sprite spr)
    {
        myPlayer.myEmote.GetComponent<MeshRenderer>().material.mainTexture = spr.texture;
        myPlayer.myEmote.gameObject.SetActive(true);

        if(emoteDisappearRoutine != null)
        {
            StopCoroutine(emoteDisappearRoutine);
        }
        emoteDisappearRoutine = StartCoroutine(DisableEmote());
    }

    IEnumerator DisableEmote()
    {
        yield return new WaitForSeconds(myPlayer.playerConfig.emojiTime);
        myPlayer.myEmote.gameObject.SetActive(false);
    }
}
