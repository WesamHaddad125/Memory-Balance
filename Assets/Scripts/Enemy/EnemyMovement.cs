using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    private int currPatrolIdx;
    private bool travelling;
    private bool waiting;
    private bool patrolForward;
    float waitTimer;

    [SerializeField] bool patrolWaiting;

    [SerializeField]
    float totalWaitTime = 3f;

    public
    Waypoint[] patrolPoints;

    public float rotateVelocity;
    public float rotateSpeedMovement = 0.1f;

    private EnemyCombat enemyCombatScript;
    // Start is called before the first frame update

    void Awake()
    {
        // Grabs all waypoints in the scene and orders them alphabetically
        IComparer wayPointSorter = new WaypointSorter();
        patrolPoints = GameObject.FindObjectsOfType<Waypoint>();
        System.Array.Sort(patrolPoints, wayPointSorter);
        
    }
    void Start()
    {
        patrolForward = true;
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyCombatScript = GetComponentInChildren<EnemyCombat>();

        if (patrolPoints != null && patrolPoints.Length >= 2)
        {
            currPatrolIdx = 0;
            SetDestination();
        } else
        {
            Debug.Log("Not enough waypoints for minion travel");
        }

    }

    // Loops through each patrol point and set's the navmesh agent of the enemy to that location and once it gets close starts moving towards the next patrol point
    void Update()
    {
        if (enemyCombatScript.continuePatrol)
        {
            if (travelling && enemyAgent.remainingDistance <= 4.0f)
            {
                travelling = false;

                if (patrolWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    ChangePatrolPoint();
                    SetDestination();
                }
            }

            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= totalWaitTime)
                {
                    waiting = false;

                    ChangePatrolPoint();
                    SetDestination();
                }
            }
        }
    }

    private void SetDestination()
    {
        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currPatrolIdx].transform.position;
            enemyAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if (patrolForward && enemyCombatScript.continuePatrol)
        {
            currPatrolIdx = (currPatrolIdx + 1) % patrolPoints.Length;
        } else
        {
            if (--currPatrolIdx < 0)
            {
                currPatrolIdx = patrolPoints.Length - 1;
            }
        }
    }
}

// Sorts array alphabetically 
public class WaypointSorter : IComparer
{
    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((Waypoint)x).name, ((Waypoint)y).name));
    }
}
