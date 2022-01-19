using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("References")]
    public Player player;
    public Shooter shooter;

    [Header("UI")]
    public Text ammoText;
    public Text healthText;
    public Text infoText;
    private bool playerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        infoText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateHUD();
    }

    void updateHUD()
    {
        ammoText.text = "Ammo: " + shooter.Ammo;
        healthText.text = "Health: " + player.Health;
        if(player.Health ==0)
        {
            die();
        }

        if(shooter.GameFinished)
        {
            win();
        }
    }

    void die()
    {
        shooter.Ammo = 0;
        infoText.text = "YOU DIED.";
        infoText.gameObject.SetActive(true);
    }

    void win()
    {
        infoText.text = "YOU WIN!!! :-)";
        infoText.gameObject.SetActive(true);
    }
}
