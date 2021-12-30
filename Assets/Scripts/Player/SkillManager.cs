using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillManager : MonoBehaviour
{
    [SerializeField] public Image[] skillList = new Image[3];
    private float[] skillCooldowns;
    private bool isCooldown1;
    private bool isCooldown2;
    private PlayerMovement playerMov;
    private HeroCombat heroCombatScript;


    // TODO Change to touch controls
    public KeyCode skill1;

   
    [Header("Skill 1")]
    // Skill 1 Touch Variables
    [SerializeField] Collider2D skill1Col;
    [SerializeField] Image skill1Image;
    private bool dragSkill1Allowed;
    private Transform skill1StartPos;
    private SkillOne skillOneScript;
    // Skill 1 Input Variables
    Vector3 position;
    [SerializeField] GameObject skill1Canvas;
    [SerializeField] Image skillshot;
    [SerializeField] Transform player;

    [Header("Skill 2")]
    // SKill 2 Touch Variables
    [SerializeField] Collider2D skill2Col;
    [SerializeField] Image skill2Image;
    private bool dragSkill2Allowed;
    private Transform skill2StartPos;
    private SkillTwo skillTwoScript;
    // Skill 2 Input Variables
    [SerializeField] Image targetCircle;
    [SerializeField] Image raduisCircle;
    [SerializeField] GameObject skill2Canvas;
    private Vector3 posUp;
    [SerializeField] float maxSkill2Dist;
    private Animator skill2Anim;
    private GameObject skill2AnimPos;
    public GameObject skill2AnimGO;

    // Extra Skill Variables
    private bool resetSkill1;
    private bool resetSkill2;

    private int playerLayer;


    // Start is called before the first frame update

    void Awake()
    {
        // Skill 1 Initialize
        skillList[0] = GameObject.FindGameObjectWithTag("Skill 1 Cooldown").GetComponent<Image>();
        skill1Col = GameObject.FindGameObjectWithTag("Skill 1").GetComponent<BoxCollider2D>();
        skill1Image = GameObject.FindGameObjectWithTag("Skill 1").GetComponent<Image>();
        skill1Canvas = GameObject.FindGameObjectWithTag("Skillshot Canvas");
        skillshot = GameObject.FindGameObjectWithTag("Skillshot").GetComponent<Image>();
        // SKill 2 Initilize
        skillList[1] = GameObject.FindGameObjectWithTag("Skill 2 Cooldown").GetComponent<Image>();
        skill2Col = GameObject.FindGameObjectWithTag("Skill 2").GetComponent<BoxCollider2D>();
        skill2Image = GameObject.FindGameObjectWithTag("Skill 2").GetComponent<Image>();
        skill2Canvas = GameObject.FindGameObjectWithTag("Circle Canvas");
        targetCircle = GameObject.FindGameObjectWithTag("Circle").GetComponent<Image>();
        raduisCircle = GameObject.FindGameObjectWithTag("Radius").GetComponent<Image>();
        skill2Anim = GameObject.FindGameObjectWithTag("Skill 2 Anim").GetComponent<Animator>();
        skill2AnimPos = GameObject.FindGameObjectWithTag("Skill 2 Anim");
        skill2AnimPos.SetActive(false);
        resetSkill1 = false;
        resetSkill2 = false;
        dragSkill1Allowed = false;
        dragSkill2Allowed = false;
        isCooldown1 = true;
        isCooldown2 = true;
        skillshot.enabled = false;
        targetCircle.enabled = false;
        raduisCircle.enabled = false;

        skillTwoScript = GetComponentInChildren<SkillTwo>();
        skillOneScript = GetComponentInChildren<SkillOne>();
        heroCombatScript = GetComponent<HeroCombat>();
        playerLayer = LayerMask.NameToLayer("Player");
        playerMov = GetComponent<PlayerMovement>();
        skillCooldowns = new float[3] { 5, 10, 7 };
        skillList[0].fillAmount = 0;
        skillList[1].fillAmount = 0;
        skillList[2].fillAmount = 0;

        skill1StartPos = skillList[0].transform;
        skill2StartPos = skillList[1].transform;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        DragSkill1();
        DragSkill2();    
        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            // Skill 1 Inputs
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer, QueryTriggerInteraction.Ignore))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            // Skill 2 Inputs
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject != this.gameObject)
                {
                    posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                    position = hit.point;
                }
            }

            // Skill 1 Canvas Inputs
            Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
            transRot.eulerAngles = new Vector3(0f, transRot.eulerAngles.y, transRot.eulerAngles.z);
            skill1Canvas.transform.rotation = Quaternion.Lerp(transRot, skill1Canvas.transform.rotation, 0f);

            // Skill 2 Canvas Inputs
            var hitPosDir = (hit.point - transform.position).normalized;
            float distance = Vector3.Distance(hit.point, transform.position);
            distance = Mathf.Min(distance, maxSkill2Dist);

            var newHitPos = transform.position + hitPosDir * distance;
            skill2Canvas.transform.position = new Vector3(newHitPos.x, 6f, newHitPos.z);
            skill2AnimPos.transform.position = newHitPos;
        }
    }

    // Allows you to drag skill 1 icon and if you don't reset it, once you let go of tapping screen it will damage players it is covering
    private void DragSkill1()
    {
        if (Input.touchCount > 0 && !isCooldown1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = (touch.position);
            
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);

                if (skill1Col == touchedCollider)
                {
                    playerMov.SetCanMove(false);
                    dragSkill1Allowed = true;
                }
            }
            
            // Move skill icon
            if (touch.phase == TouchPhase.Moved)
            {

                if (dragSkill1Allowed && !dragSkill2Allowed)
                {
                    skill1Image.transform.position = new Vector3(touchPosition.x, touchPosition.y, skill1Image.transform.position.z);
                    skill2Image.transform.position = skill2StartPos.position;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                skill1Image.transform.position = skill1StartPos.position;
                dragSkill1Allowed = false;
                // Player Cast SKill 1
                Debug.Log(resetSkill2);
                // If skill icon is outside of skill HUD, and they stop tap it starts skill 1
                if (!resetSkill1)
                {
                    // Do Damage
                    foreach (GameObject enemy in skillOneScript.enemiesHit)
                    {
                        // Start skill 1 coroutine
                        if (enemy != null)
                            StartCoroutine(skillOneScript.StartDotDamage(enemy));
                    }
                    EndSkill1();
                }
                resetSkill1 = false;
            }
        }

        if (isCooldown1)
        {
            skillList[0].fillAmount -= 1 / skillCooldowns[0] * Time.deltaTime;
            skillshot.enabled = false;

            if (skillList[0].fillAmount <= 0)
            {
                skillList[0].fillAmount = 0;
                isCooldown1 = false;
            }
        }

    }

    // Allows you to drag skill 2 icon and if you don't reset it, once you let go of tapping screen it will damage players it is covering
    private void DragSkill2()
    {
        if (Input.touchCount > 0 && !isCooldown2)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = (touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (skill2Col == touchedCollider)
                {
                    playerMov.SetCanMove(false);
                    dragSkill2Allowed = true;
                }

            }

            // Move skill icon
            if (touch.phase == TouchPhase.Moved)
            {

                if (dragSkill2Allowed && !dragSkill1Allowed)
                {
                    skill2Image.transform.position = new Vector3(touchPosition.x, touchPosition.y, skill2Image.transform.position.z);
                    skill1Image.transform.position = skill1StartPos.position;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                skill2Image.transform.position = skill2StartPos.position;
                dragSkill2Allowed = false;
                // Player Cast SKill 2
                // If skill icon is outside of skill HUD, and they stop tap it starts skill 2
                if (!resetSkill2)
                {
                    // Do Damage
                    foreach (GameObject enemy in skillTwoScript.enemiesHit)
                    {
                        if (enemy != null) 
                            skillTwoScript.DoBurstDamage(enemy);
                    }
                    EndSkill2();
                }
                resetSkill2 = false;
            }
        }

        if (isCooldown2)
        {
            skillList[1].fillAmount -= 1 / skillCooldowns[1] * Time.deltaTime;
            targetCircle.enabled = false;
            raduisCircle.enabled = false;

            if (skillList[1].fillAmount <= 0)
            {
                skillList[1].fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }

    // End skill 1 and begins it's cooldown timer
    private void EndSkill1()
    {
        StartCoroutine(SetPlayerCanMove());
        if (skillshot.enabled == true)
        {
            isCooldown1 = true;
            skillList[0].fillAmount = 1;
        }     
    }

    // End skill 2 and begins it's cooldown timer
    private void EndSkill2()
    {
        if (!playerMov.GetCanMove())
            StartCoroutine(SetPlayerCanMove());
        
        if (targetCircle.enabled == true)
        {
            StartCoroutine(ResetSkill2Anim());
            isCooldown2 = true;
            skillList[1].fillAmount = 1;
        }
    }

    // Once the skill is started, shows the skillshot canvas on the ground for aiming
    public void StartSkill1()
    {
        resetSkill1 = false;
        if (isCooldown1 == false)
        {
            skillshot.enabled = true;

            // Disable other skill UIs
            raduisCircle.enabled = false;
            targetCircle.enabled = false;

        }
    }

    // Once the skill is started, shows the circle and radius canvas on the ground for aiming
    public void StartSkill2()
    {
        resetSkill2 = false;
        if (isCooldown2 == false)
        {
            targetCircle.enabled = true;
            raduisCircle.enabled = true;
            // Disable other skill UIs
            skillshot.enabled = false;

        }
    }

    // If player doesn't want to use skill they reset
    public void ResetSkill1()
    {
        resetSkill1 = true;

        skillshot.enabled = false;
    }

    // If player doesn't want to use skill they reset
    public void ResetSkill2()
    {
        resetSkill2 = true;

        raduisCircle.enabled = false;
        targetCircle.enabled = false;
    }

    // Let player move after skill is done
    IEnumerator SetPlayerCanMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            playerMov.SetCanMove(true);
            break;
        }
    }

    // Set animation to start on skill 2 start
    IEnumerator ResetSkill2Anim()
    {
        while (true)
        {
            //skill2AnimPos.SetActive(true);
            //skill2Anim.SetTrigger("sandAttack");
            Debug.Log("RUNNING");
            GameObject effect = Instantiate(skill2AnimGO, skill2AnimPos.transform.position, Quaternion.identity) as GameObject;
            Debug.Log(effect.transform.position);
            yield return new WaitForSeconds(0.7f);
            //skill2AnimPos.SetActive(false);
            Destroy(effect);
            break;
        }
    }
}
