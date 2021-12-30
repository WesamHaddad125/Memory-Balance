using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSkill : MonoBehaviour
{

    private SkillManager skillManager;
    // Start is called before the first frame update
    void Awake()
    {
        skillManager = FindObjectOfType<SkillManager>().GetComponent<SkillManager>();
    }

    // When a skill icon moves out of it's HUD then it will start the skill's canvas for aiming and useage
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Skill 1"))
        {
            skillManager.StartSkill1();
        } else if (other.gameObject.CompareTag("Skill 2"))
        {
            skillManager.StartSkill2();
        }
    }

    // If the skill icon is put back into the HUD then it will reset it's position and not activate the skill
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Skill 1"))
        {
            skillManager.ResetSkill1();
        }
        else if (other.gameObject.CompareTag("Skill 2"))
        {
            skillManager.ResetSkill2();
        }
    }
}
