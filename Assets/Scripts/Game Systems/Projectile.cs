using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 15f;
    private Vector3 projDirection;
    public float moveSpeed = 20f;
    private Rigidbody rb;
    private HeroCombat playerPos;
    public float rotateSpeed;
    Vector3 direction;
    GameObject targettedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPos = FindObjectOfType<HeroCombat>();
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Shoot toward either the enemy or player
        if (gameObject.CompareTag("Enemy Tower Projectile"))
        {
            direction = (playerPos.transform.position - transform.position).normalized;
        } else if (gameObject.CompareTag("Player Tower Projectile"))
        {
            direction = (targettedEnemy.transform.position - transform.position).normalized;
        }

        rb.velocity = direction * moveSpeed;
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player Tower Projectile"))
        {
            other.gameObject.GetComponent<Stats>().health -= damage;
            Destroy(gameObject);
        } else if (other.gameObject.GetComponent<Targetable>() != null && other.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy Tower Projectile"))
        {
            other.gameObject.GetComponent<Stats>().health -= damage;
            Destroy(gameObject);
        }
    }

    public void Setup(GameObject enemy)
    {
        targettedEnemy = enemy;
    }

    IEnumerator Despawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}
