using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Corrosive Sand - DOT(Damage Over Time) Damage Skillshot ability 
public class SkillOne : MonoBehaviour
{
    public List<GameObject> enemiesHit;

    public float timeBetweenTicks = 0.1f; // Time between damage ticks
    public float damagePerTick = 7f;
    public float dotLength = 4f; // How long does DOT damage last
    public Image skillShotImage;


    // Start is called before the first frame update
    void Start()
    {
        enemiesHit = new List<GameObject>();
    }



    // Add enemy to the array to enemies that will be dealt damage
    void OnTriggerEnter(Collider other)
    {
        if (skillShotImage.enabled)
        {
            if (other.gameObject.GetComponent<EnemyCombat>() != null)
            {
                enemiesHit.Add(other.gameObject);
            }
        }
    }

    // Remove enemy from array that would of dealt damage to
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyCombat>() != null)
        {
            enemiesHit.Remove(other.gameObject);
        }
    }

    // Deal damage to enemy for a certain length of time, and deals damage every few milliseconds until the end length
    public IEnumerator StartDotDamage(GameObject enemy)
    {
        float currCount = 0; // Keep track of how many ticks of damage have happened
        while (currCount < dotLength)
        {
            enemy.GetComponent<Stats>().health -= damagePerTick;
            yield return new WaitForSeconds(timeBetweenTicks);
            currCount += timeBetweenTicks;
        }
    }
}
