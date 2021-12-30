using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowSkills : MonoBehaviour
{
    private float[] skillCooldowns;

    // SKill 1
    private bool isCooldown1;
    public int cooldown1Time;
    public int cooldown1Threshold;
    // Skiill 1 Canvas
    [SerializeField] Canvas skill1Canvas;
    [SerializeField] Image skillshot;


    // Skill 2
    private bool isCooldown2;
    public int cooldown2Time;
    public int cooldown2Threshold;
    // Skill 2 Canvas
    [SerializeField] Canvas skill2Canvas;
    [SerializeField] Image targetCircle;
    [SerializeField] Image radiusCircle;
    private Vector3 posUp;
    [SerializeField] float maxSkill2Dist;


    private SkillManager playerSkillScript;


    // Start is called before the first frame update
    void Awake()
    {
        playerSkillScript = FindObjectOfType<SkillManager>().GetComponent<SkillManager>();

        isCooldown1 = true;
        isCooldown2 = true;
        skillshot.enabled = false;
        targetCircle.enabled = false;
        radiusCircle.enabled = false;
        skillCooldowns = new float[2] { 5, 10 };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
