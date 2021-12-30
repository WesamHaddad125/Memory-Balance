using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : MonoBehaviour
{
    public GameObject targettedPlayer;
    public GameObject projectile;
    public Transform projStart;
    public float projSpeed = 20f;
    bool startFiring = false;


    // If player is close to tower, start firing coroutine
    void Update()
    {
        if (targettedPlayer != null)
        {
            if (startFiring)
                StartCoroutine(FireProjectile());

            if (targettedPlayer.GetComponent<Stats>().health <= 0)
            {
                startFiring = true;
            }
        }
    }

    // Gets the player game object
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Player"))
        {
            targettedPlayer = other.gameObject;
            startFiring = true;
        }
    }

    // Sets the targetted player to null
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Player"))
        {
            targettedPlayer = null;
        }
    }

    IEnumerator FireProjectile()
    {
        while (true)
        {
            startFiring = false;
            GameObject proj = Instantiate(projectile, projStart.position, projStart.rotation);           

            yield return new WaitForSeconds(2);
            if (targettedPlayer == null)
                break;

            if (!targettedPlayer.activeSelf)
                targettedPlayer = null;
        }
    }
}
