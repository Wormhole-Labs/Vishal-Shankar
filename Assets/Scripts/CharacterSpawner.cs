using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public List<GameObject> characterList;

    private void Start()
    {
        Instantiate(characterList.Find(x => x.name == GameConstants.gameConstants.selectedCharacter), new Vector3(0, 1.1f, 0), Quaternion.identity);
    }
}
