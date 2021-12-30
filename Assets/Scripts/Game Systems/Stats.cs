using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Unit Stats
    [SerializeField] public float maxHealth;
    [SerializeField] public float health;
    [SerializeField] public float attackDmg;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackTime;

    HeroCombat heroCombatScript;
    EnemyCombat enemyCombatScript;

    private GameObject player;
    public float expValue;

    // Start is called before the first frame update
    void Start()
    {
        heroCombatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyCombatScript = GetComponent<EnemyCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        // Death Event
        if (health <= 0)
        {
            heroCombatScript.targetedEnemy = null;
            heroCombatScript.performMeleeAttack = false;
            if (GetComponent<EnemyCombat>() != null)
                enemyCombatScript.targetedPlayerOrTower = null;
            // Give Exp
            player.GetComponent<LevelUpStatus>().SetExperience(expValue);
            if (GetComponent<HeroCombat>() == null)
            {
                Destroy(gameObject);
            }
            else
            {
                // If player dies, hide it and reset it's position
                player.SetActive(false);
            }
        }
    }

    public float GetHealthNormalized()
    {
        return health / maxHealth;
    }
}
