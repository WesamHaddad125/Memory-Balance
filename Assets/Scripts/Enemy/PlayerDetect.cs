using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    private EnemyCombat enemyCombatScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyCombatScript = GetComponentInParent<EnemyCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Detects when player is near, and stops the enemy patrol
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && !other.gameObject.CompareTag("Enemy Tower"))
        {
            enemyCombatScript.continuePatrol = false;
            
            if (other.gameObject.CompareTag("Player"))
            {
                enemyCombatScript.triggerCount++;
                enemyCombatScript.targetedPlayerOrTower = other.gameObject;
            }
            else if (other.gameObject.CompareTag("Player Tower"))
            {
                enemyCombatScript.triggerCount++;
                enemyCombatScript.targetedPlayerOrTower = other.gameObject;
            }
        }
    }

    // Lowers the trigger count once the player leaves the trigger collision
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && !other.gameObject.CompareTag("Enemy Tower"))
            enemyCombatScript.triggerCount--;
    }
}
