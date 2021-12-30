using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] Image enemyHealthLeft3D;
    [SerializeField] Image enemyHealthRight3D;

    [SerializeField] Stats statsScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealthLeft3D.fillAmount = statsScript.GetHealthNormalized();
        enemyHealthRight3D.fillAmount = statsScript.GetHealthNormalized();

        statsScript.health = statsScript.maxHealth;
    }

    void LateUpdate()
    {
        // Look towards the camera location
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the enemy health UI beased on their current health
        enemyHealthLeft3D.fillAmount = statsScript.GetHealthNormalized();
        enemyHealthRight3D.fillAmount = statsScript.GetHealthNormalized();
    }
}
