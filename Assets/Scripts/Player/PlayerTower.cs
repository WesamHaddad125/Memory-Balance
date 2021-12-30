using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public GameObject targettedEnemy;
    private List<GameObject> enemiesNear;
    private int enemyIdx;
    public GameObject projectile;
    public Transform projStart;
    public float projSpeed = 20f;
    bool startFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        enemiesNear = new List<GameObject>();
        enemyIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (targettedEnemy != null)
        {
            if (startFiring)
                StartCoroutine(FireProjectile());

            if (targettedEnemy.GetComponent<Stats>().health <= 0)
            {
                targettedEnemy = enemiesNear[enemyIdx + 1];
                startFiring = true;
                enemyIdx++;
                if (enemyIdx >= enemiesNear.Count)
                {
                    enemyIdx = 0;
                }
            }
        }
    }


    // Adds all enemies near it to an array
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Enemy"))
        {
            enemiesNear.Add(other.gameObject);
            if (enemiesNear.Count <= 1)
            {
                targettedEnemy = other.gameObject;
                startFiring = true;
            }
        }
    }

    // Removes enemy from array
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Enemy"))
            enemiesNear.Remove(other.gameObject);
    }

    // Fires a projectile toward an enemy and deals damage, shoots every 2 seconds
    IEnumerator FireProjectile()
    {
        while (true) {
            startFiring = false;

            GameObject proj = Instantiate(projectile, projStart.position, transform.rotation);
            Vector3 projDir = targettedEnemy.transform.position - projStart.position;
            proj.GetComponent<Projectile>().Setup(targettedEnemy);
            
            yield return new WaitForSeconds(2);
            if (targettedEnemy == null)
                break;
        }
    }
}
