using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    private bool canMove = true;
    [SerializeField] public NavMeshAgent agent;

    private int enemyLayer;

    public float rotateVelocity;
    public float rotateSpeedMovement = 0.1f;

    private HeroCombat heroCombatScript;
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<Camera>();
        agent = GetComponent<NavMeshAgent>();
        heroCombatScript = GetComponent<HeroCombat>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (heroCombatScript.targetedEnemy != null)
        {
            if (heroCombatScript.targetedEnemy.GetComponent<HeroCombat>() != null)
            {
                if (heroCombatScript.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                {
                    heroCombatScript.targetedEnemy = null;
                }
            }
        }


        // Count number of touches on screen
        if (Input.touchCount > 0 && canMove)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            Ray moveRay = cam.ScreenPointToRay(touchPos);
            RaycastHit hit;
            // Take finger off screen
            if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(moveRay, out hit, Mathf.Infinity, enemyLayer, QueryTriggerInteraction.Ignore))
                {

                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        // If you tap the ground then it will move the player toward that position
                        agent.SetDestination(hit.point);

                        agent.stoppingDistance = 0f;

                        Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                            rotationToLookAt.eulerAngles.y,
                            ref rotateVelocity,
                            rotateSpeedMovement * (Time.deltaTime * 5));

                        transform.eulerAngles = new Vector3(0, rotationY, 0);
                    }
                }
            }
        }
    }

    public void SetCanMove(bool cm)
    {
        canMove = cm;
    }

    public bool GetCanMove()
    {
        return canMove;
    }
}
