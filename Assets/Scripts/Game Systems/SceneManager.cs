using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public Stats playerAncient;
    public Stats enemyAncient;
    public Text winLoseText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO, switch to different scenes for now it is just a print message
        if (playerAncient.health <= 0)
        {
            // LOSE GAME -> go to lose scene
            winLoseText.text = "Sorry, you lost! Restart App to try again!";
        } else if (enemyAncient.health <= 0)
        {
            // WIN GAME -> go to win scene
            winLoseText.text = "Congrats! You won! Restart App to try again!";
        }
    }
}
