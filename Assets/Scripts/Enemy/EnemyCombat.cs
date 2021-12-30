using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    // Enemy Info
    public enum EnemyAttackType { Melee, Ranged };
    public EnemyAttackType enemyAttackType;

    public GameObject targetedPlayerOrTower;
    public float attackRange;
    public float rotateSpeedForAttack;

    private EnemyMovement moveScript;
    private Stats statsScript;

    public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool performMeleeAttack = true;

    public bool continuePatrol = true;

    [SerializeField] ParticleSystem meleeVisual;

    public int triggerCount = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponentInParent<EnemyMovement>();
        statsScript = GetComponentInParent<Stats>();
        targetedPlayerOrTower = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Moves on to the next player or tower to attack once either dies
        if (targetedPlayerOrTower != null)
        {
            if (targetedPlayerOrTower.GetComponent<Stats>().health <= 0)
            {
                targetedPlayerOrTower = null;
                triggerCount--;
            }
        }

        // No targettable units near, continue patrol towards end
        if (triggerCount <= 0)
        {
            continuePatrol = true;
            targetedPlayerOrTower = null;
        }

        // If enemy is targetting a unity it checks if it is close enough to start auto attacking the unit
        if (!continuePatrol)
        {
            if (targetedPlayerOrTower != null)
            {
                if (Vector3.Distance(gameObject.transform.position, targetedPlayerOrTower.transform.position) > attackRange)
                {
                    moveScript.enemyAgent.SetDestination(targetedPlayerOrTower.transform.position);
                    moveScript.enemyAgent.stoppingDistance = attackRange;

                    // Rotation
                    Quaternion rotationToLookAt = Quaternion.LookRotation(targetedPlayerOrTower.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                        rotationToLookAt.eulerAngles.y,
                        ref moveScript.rotateVelocity,
                        rotateSpeedForAttack * (Time.deltaTime * 5));

                    transform.eulerAngles = new Vector3(0f, rotationY, 0f);
                } else
                {
                    if (enemyAttackType == EnemyAttackType.Melee)
                    {
                        // Start auto attacking
                        if (performMeleeAttack)
                        {
                            StartCoroutine(MeleeAttackInterval());
                        }
                    }
                }
            }
        }
    }

    // Deal damage every couple seconds to the targetted unit and plays visual
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

            if (targetedPlayerOrTower == null)
            {
                // Hide particle
                // Not targetting enemy
                performMeleeAttack = true;
                break;
            }
        }
    }

    // Deals damage
    public void MeleeAttack()
    {
        if (targetedPlayerOrTower != null)
        {
            if (targetedPlayerOrTower.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Player)
            {
                targetedPlayerOrTower.GetComponent<Stats>().health -= statsScript.attackDmg;
            }
        }
    }

}
