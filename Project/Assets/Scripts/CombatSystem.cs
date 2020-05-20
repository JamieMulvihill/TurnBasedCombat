using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CombatState { START, WON, LOST, PLAYERTURN, ENEMYTURN}

public class CombatSystem : MonoBehaviour
{
    public CombatState state;
    public GameObject[] EnemySpawnPoints;
    public GameObject[] PlayerSpawnPoints;
    public GameObject fighter;
    public GameObject enemyTarget;

    List<Fighter> fighters;
    List<Fighter> enemies;
    public List<Fighter> totalPlayers;
    public GameObject attacker;

    SelectTarget targetSelect;

    bool hasFightStarted;
    void Start()
    {
        state = CombatState.START;
        enemies = new List<Fighter>();
        fighters = new List<Fighter>();
        totalPlayers = new List<Fighter>();
        hasFightStarted = false;
        targetSelect = gameObject.GetComponent<SelectTarget>();
        StartCoroutine(StartCombatEncoutner());
    }

    IEnumerator StartCombatEncoutner() {

        Debug.Log("Combat Encounter!!");

        foreach (GameObject spawnPoint in EnemySpawnPoints)
        {
            GameObject enemyGO = Instantiate(fighter, spawnPoint.transform);
            enemyGO.GetComponent<Fighter>().fighterType = FighterType.ENEMY;
            enemies.Add(enemyGO.GetComponent<Fighter>());
            totalPlayers.Add(enemyGO.GetComponent<Fighter>());
        }

        foreach (GameObject spawnPoint in PlayerSpawnPoints)
        {
            GameObject playerGO = Instantiate(fighter, spawnPoint.transform);
            playerGO.GetComponent<Fighter>().fighterType = FighterType.PLAYER;
            fighters.Add(playerGO.GetComponent<Fighter>());
            totalPlayers.Add(playerGO.GetComponent<Fighter>());
        }

        yield return new WaitForSeconds(1f);
        
        PlayerTurn();
    }

    IEnumerator PlayerAttack() {

        bool isDead = targetSelect.target.GetComponent<Fighter>().TakeDamage(65f);

        Debug.Log("Attack Success");

        yield return new WaitForSeconds(1f);

        if (isDead) {
            enemies.Remove(targetSelect.target.GetComponent<Fighter>());
            totalPlayers.Remove(targetSelect.target.GetComponent<Fighter>());
            Destroy(targetSelect.target);
            targetSelect.target = null;
            targetSelect.targetSelected = false;
        }

        if (enemies.Count == 0)
        {
            state = CombatState.WON;
        }
        else
        {
            PlayerTurn();
        }
    }

    void PlayerTurn() {
        GameObject turnFighter = CalculateTurnMeter();

        if (turnFighter.GetComponent<Fighter>().fighterType == FighterType.PLAYER)
        {
            state = CombatState.PLAYERTURN;
            Debug.Log("Player Turn!!");
        }
        else {
            state = CombatState.ENEMYTURN;
            EnemyTurn();
        }
    }

    public void AttackButtonClick() {
        if (state != CombatState.PLAYERTURN || targetSelect.target == null)
            return;

        StartCoroutine(PlayerAttack());
    }

    void EnemyTurn() {

        Debug.Log("EnemyTurn");

        int targetIndex = Random.Range(0, fighters.Count-1);
        enemyTarget = fighters[targetIndex].gameObject;
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack() {

        bool isDead = enemyTarget.GetComponent<Fighter>().TakeDamage(65f);

        Debug.Log("Enemy Attack Sucess");

        yield return new WaitForSeconds(1f);

        
        if (isDead){

            fighters.Remove(enemyTarget.GetComponent<Fighter>());
            totalPlayers.Remove(enemyTarget.GetComponent<Fighter>());
            Destroy(enemyTarget);
           
        }
        if (fighters.Count == 0)
        {
            state = CombatState.LOST;
        }
        else { 
            PlayerTurn();
        }
    }

    private GameObject CalculateTurnMeter() {

        if (!hasFightStarted)
        {
            {
                /*
                 foreach (Fighter fighter in totalPlayers)
                {
                    fighter.turnMeterValue = fighter.gameObject.GetComponent<Speed>().speedStat / 350;
                }
                hasFightStarted = true;
                totalPlayers.Sort((f1, f2) => f1.GetComponent<Fighter>().turnMeterValue.CompareTo(f2.GetComponent<Fighter>().turnMeterValue));
                totalPlayers.Reverse();
                attacker = totalPlayers[0].gameObject;
                */
            }

            totalPlayers.Sort((f1, f2) => f1.gameObject.GetComponent<Speed>().speedStat.CompareTo(f2.gameObject.GetComponent<Speed>().speedStat));
            totalPlayers.Reverse();
            float ratio = 100f / totalPlayers[0].gameObject.GetComponent<Speed>().speedStat;
            List<float> normalizedList = totalPlayers.Select(i => i.gameObject.GetComponent<Speed>().speedStat * ratio).ToList();

            for (int i = 0; i < totalPlayers.Count; i++) {
                totalPlayers[i].turnMeterValue = normalizedList[i];
            }

            totalPlayers[0].isTurn = true;
            attacker = totalPlayers[0].gameObject;
            attacker.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            hasFightStarted = true;

            return attacker;
        }

        else {

            foreach (Fighter fighter in totalPlayers)
            {
                if (!fighter.isTurn)
                {
                    fighter.emptyTurn = (100 - fighter.turnMeterValue) / fighter.gameObject.GetComponent<Speed>().speedStat;
                }
                else
                {
                    fighter.turnMeterValue = 0;
                    fighter.emptyTurn = 100;
                    fighter.isTurn = false;
                }
            }
            if (attacker)
            {
                attacker.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                
            }

            totalPlayers.Sort((f1, f2) => f1.GetComponent<Fighter>().emptyTurn.CompareTo(f2.GetComponent<Fighter>().emptyTurn));
            attacker = totalPlayers[0].gameObject;
            totalPlayers[0].isTurn = true;
            attacker.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);

            foreach (Fighter fighter in totalPlayers){
                
                if (attacker.GetComponent<Fighter>() == fighter) {
                    fighter.turnIncrease = 100 - fighter.turnMeterValue;
                    fighter.turnMeterValue += fighter.turnIncrease;
                }
                else {
                    fighter.turnIncrease = (attacker.GetComponent<Fighter>().turnIncrease) / attacker.GetComponent<Speed>().speedStat * fighter.GetComponent<Speed>().speedStat;
                    fighter.turnMeterValue += fighter.turnIncrease;
                }            
            }
            return attacker;
        }
           
   
    }
}
