using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargeting : MonoBehaviour
{
    public GameObject selectedHero;
    public bool heroPlayer;
    RaycastHit hit;

    private int playerLayer;
    // Start is called before the first frame update
    void Awake()
    {
        selectedHero = GameObject.FindGameObjectWithTag("Player");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Minion Targeting - Lets you tap an enemy or tower to set them as targetted enemy to be auto attacked
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            Ray targetRay = Camera.main.ScreenPointToRay(touchPos);
            // Take finger off screen
            if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(targetRay, out hit, Mathf.Infinity, playerLayer, QueryTriggerInteraction.Ignore))
                {
                    
                    // If the minion is targetable
                    if (hit.collider.GetComponent<Targetable>() != null)
                    {
                        
                        if (hit.collider.gameObject.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
                        {
                            selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                        }
                    }

                    else if (hit.collider.gameObject.GetComponent<Targetable>() == null)
                    {
                        selectedHero.GetComponent<HeroCombat>().targetedEnemy = null;
                    }
                }
            }
        }
    }
}
