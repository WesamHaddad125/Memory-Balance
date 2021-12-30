using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject minionGroup;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMinions());
    }

    // Spawn group of enemies every 45 seconds for player to fight
    IEnumerator SpawnMinions()
    {
        while(true)
        {
            GameObject group = Instantiate(minionGroup, transform.position, transform.rotation);
            yield return new WaitForSeconds(45);
        }
    }
}
