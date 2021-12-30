using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerRespawn : MonoBehaviour
{
    public float respawnTime;
    public GameObject playerPrefab;
    public GameObject currentPlayer;
    private Stats currPlayerStats;
    private HeroCombat currHeroCombatScript;
    public CameraFollow cameraLookAt;

    private float maxHealthMult = 10;
    private float attackDmgMult = 2;
    private float regenMult = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        respawnTime = 10f;
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        currPlayerStats = currentPlayer.GetComponent<Stats>();
        currHeroCombatScript = currentPlayer.GetComponent<HeroCombat>();
        cameraLookAt = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        // Once player dies, start coroutine
        if (currPlayerStats.health <= 0)
        {
            StartCoroutine(Respawn(currentPlayer.GetComponent<LevelUpStatus>().level));
        }
    }

    // When player dies they are hidden from scene, and after the respawn time, they are warped back to 
    // start position, reset their current health, and show player on scene. After a few seconds they can move
    IEnumerator Respawn(int level)
    {
        while(true)
        {
            yield return new WaitForSeconds(respawnTime);
            currentPlayer.GetComponent<NavMeshAgent>().Warp(transform.position);
            currPlayerStats.health = currPlayerStats.maxHealth;
            currentPlayer.SetActive(true);
            break;
        }
    }
}
