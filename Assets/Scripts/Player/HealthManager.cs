using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image healthLeft3D;
    [SerializeField] Image healthRight3D;
    [SerializeField] Image healthLeft2D;
    [SerializeField] Image healthRight2D;

    Stats statsScript;

    
    void Start()
    {
        statsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

        healthLeft3D = GameObject.FindGameObjectWithTag("HealthLeft3d").GetComponent<Image>();
        healthRight3D = GameObject.FindGameObjectWithTag("HealthRight3d").GetComponent<Image>();
        healthLeft2D = GameObject.FindGameObjectWithTag("HealthLeft2d").GetComponent<Image>();
        healthRight2D = GameObject.FindGameObjectWithTag("HealthRight2d").GetComponent<Image>();

        healthLeft2D.fillAmount = statsScript.GetHealthNormalized();
        healthRight2D.fillAmount = statsScript.GetHealthNormalized();
        healthLeft3D.fillAmount = statsScript.GetHealthNormalized();
        healthRight3D.fillAmount = statsScript.GetHealthNormalized();

        statsScript.health = statsScript.maxHealth;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    void Update()
    {
        // Updates health over player, and health in on screen HUD depending on player health from Stats script
        healthLeft2D.fillAmount = statsScript.GetHealthNormalized();
        healthRight2D.fillAmount = statsScript.GetHealthNormalized();
        healthLeft3D.fillAmount = statsScript.GetHealthNormalized();
        healthRight3D.fillAmount = statsScript.GetHealthNormalized();
    }


}
