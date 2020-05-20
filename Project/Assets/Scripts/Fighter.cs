using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FighterType { PLAYER, ENEMY }

public class Fighter : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    public Image healthBar;
    public Image turnMeter;
    public float turnMeterValue;
    public float turnIncrease = 0f;
    public float emptyTurn = 0f;
    public bool isTurn;
    public FighterType fighterType; 
    void Start()
    {
        turnMeterValue = 0f;
        health = maxHealth;
        turnMeter.fillAmount = turnMeterValue / 100;
        healthBar.fillAmount = health / maxHealth;
    }

    private void Update()
    {
        turnMeter.fillAmount = turnMeterValue / 100;
        healthBar.fillAmount = health / maxHealth;
    }

    public bool TakeDamage(float damage) {

        health -= damage;

        if (health <= 0)
            return true;
        else
            return false;
    }
}
