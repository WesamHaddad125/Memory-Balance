using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    // Variables
    public enum HeroAttackType { Melee, Ranged };
    public HeroAttackType heroAttackType;


    public GameObject targetedEnemy;
    public float attackRange;
    public float rotateSpeedForAttack;

    private PlayerMovement moveScript;
    private Stats statsScript;

    public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool performMeleeAttack = true;

    [SerializeField] ParticleSystem meleeVisual;

    public float regenAmount = 0.8f;

    // Start is called before the first frame update
    void Awake()
    {
        moveScript = GetComponent<PlayerMovement>();
        statsScript = GetComponent<Stats>();
        StartCoroutine(AutoRegen());
    }

    // Update is called once per frame
    void Update()
    {
        // If no targetted Enemy then we can start auto attack
        if (targetedEnemy == null)
        {
            // Not targetting enemy
            performMeleeAttack = true;
        }

        // If we have selected a target then it will move close to that unit and begin auto attack
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) > attackRange)
            {
                moveScript.agent.SetDestination(targetedEnemy.transform.position);
                moveScript.agent.stoppingDistance = attackRange;

                // Rotation
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref moveScript.rotateVelocity,
                    rotateSpeedForAttack * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            } 
            else
            {
                if (heroAttackType == HeroAttackType.Melee)
                {
                    if (performMeleeAttack)
                    {
                        // ATTACK
                        StartCoroutine(MeleeAttackInterval());
                    }
                }
            }
        }
    }

    // Run auto attack with time in between attacks, and also play a visual animation to show attack
    IEnumerator MeleeAttackInterval()
    {
        while (true)
        {
            performMeleeAttack = false;
            // Activate Particle
            meleeVisual.transform.Rotate(UnityEngine.Random.Range(-30f, 30f), 0f, 0f);
            meleeVisual.Play();
            if (meleeVisual.IsAlive())
            {
                MeleeAttack();
            }

            yield return new WaitForSeconds(statsScript.attackTime / ((100 + statsScript.attackTime) * 0.01f));

            if (targetedEnemy == null)
            {
                // Not targetting enemy
                performMeleeAttack = true;
                break;
            }
        }
    }

    // Deal damage to targetted unit
    public void MeleeAttack()
    {
        if (targetedEnemy != null)
        {
            if (targetedEnemy.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy.GetComponent<Stats>().health -= statsScript.attackDmg;
                if (targetedEnemy.GetComponent<Stats>().health <= 0)
                {
                    targetedEnemy = null;
                    performMeleeAttack = true;
                }
            }
        }
    }

    // Auto regenerate health every 2 seconds so you can heal outside of battle
    IEnumerator AutoRegen()
    {
        while (true)
        {
            if (statsScript.health < statsScript.maxHealth)
            {
                statsScript.health += regenAmount;
            }

            yield return new WaitForSeconds(1);
        }
    }
}
