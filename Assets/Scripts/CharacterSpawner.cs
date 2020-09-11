using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public List<GameObject> characterList;

    public PlayerController SpawnCharacter()
    {
        return Photon.Pun.PhotonNetwork.Instantiate(characterList.Find(x => x.name == GameConstants.gameConstants.selectedCharacter).name, new Vector3(0, 1.1f, 0), Quaternion.identity, 0).GetComponent<PlayerController>();
    }
}
