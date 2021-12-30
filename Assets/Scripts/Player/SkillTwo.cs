using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTwo : MonoBehaviour
{
    public List<GameObject> enemiesHit;

    public float burstDamage = 40f;
    public Image targetCircle;


    // Start is called before the first frame update
    void Start()
    {
        enemiesHit = new List<GameObject>();
    }



    // Add to enemy array of enemies that will be hit with damage
    void OnTriggerEnter(Collider other)
    {
        if (targetCircle.enabled)
        {
            if (other.gameObject.GetComponent<EnemyCombat>() != null)
            {
                enemiesHit.Add(other.gameObject);
            }
        }
    }

    // Remove enemy from damage array
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyCombat>() != null)
        {
            enemiesHit.Remove(other.gameObject);
        }
    }

    // For each enemy in array it will do instant large damage
    public void DoBurstDamage(GameObject enemy)
    {
        enemy.GetComponent<Stats>().health -= burstDamage;
    }
}
