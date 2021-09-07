using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform _Dynamic;
    public Transform battleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public UIDocument battleHudDoc;
    public BattleHud battleHud;
    VisualElement rootHudEle;
    VisualElement playerContainer;
    VisualElement enemyContainer;
    VisualElement controlsContainer;

    public BattleState state;
    
    // UIElement CallBack functions
     public void OnAttackBtn() {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }    

    private void OnHealBtn()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    void Start()
    {
        //set inital state
        state = BattleState.START;

        //get the root UIElement
        rootHudEle = battleHudDoc.rootVisualElement;

        //set callbacks for the UI buttons
        controlsContainer = rootHudEle.Q<VisualElement>("ControlsContainer");
        var attackBtn = controlsContainer.Q<Button>("AttackBtn");
        attackBtn.RegisterCallback<ClickEvent>(ev => OnAttackBtn());

        var healBtn = controlsContainer.Q<Button>("HealBtn");
        healBtn.RegisterCallback<ClickEvent>(ev => OnHealBtn());

        BattleFsm();
    }

    void BattleFsm() {
        switch (state)
        {
            case BattleState.START:
                StartCoroutine(SetupBattle());
                break;

            case BattleState.PLAYERTURN:
                PlayerTurn();
                break;

            case BattleState.ENEMYTURN:
                EnemyTurn();
                break;

            case BattleState.WON:
                WinBattle();
                return;

            case BattleState.LOST:
                LoseBattle();
                return;
            default:
                break;
        }
    }
    
    //Sets up the enemy and player UIs
    IEnumerator SetupBattle()
    {
        var playerGO = Instantiate(playerPrefab, _Dynamic);
        playerUnit = playerGO.GetComponent<Unit>();

        var enemyGO = Instantiate(enemyPrefab, battleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        rootHudEle = battleHudDoc.rootVisualElement;
        battleHud.setUnitHud(playerUnit);
        battleHud.setUnitHud(enemyUnit);

        battleHud.SetDialogueText($"{enemyUnit.unitName} challenges you to learn math!");

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        BattleFsm();
    }

    //Enemy and player turns
    private void EnemyTurn()
    {        
        StartCoroutine(EnemyAttack());
    }

    private void PlayerTurn()
    {
        battleHud.SetDialogueText("Select an action:");
    }

    //Player and Enemy Attcks, each update the UI and call the BattleFSM
    private IEnumerator PlayerAttack()
    {
        battleHud.SetDialogueText($"You deal {playerUnit.damage} damage to {enemyUnit.unitName}");
        var isDead = enemyUnit.TakeDamage(playerUnit.damage);
        battleHud.SetUnitHp(enemyUnit);

        if (isDead)
            state = BattleState.WON;
        else
            state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(2f);
        BattleFsm();
    }
    
    private IEnumerator PlayerHeal()
    {
        const int healAmount = 2; //hardcoded heal for right now, will change later.
        playerUnit.Heal(healAmount);
        battleHud.SetUnitHp(playerUnit);
        battleHud.SetDialogueText($"You heal for {healAmount} points");
        
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        BattleFsm();
    }

    private IEnumerator EnemyAttack()
    {
        battleHud.SetDialogueText($"{enemyUnit.unitName} uses Chomp!\n You take {enemyUnit.damage} damage.");
        var isDead = playerUnit.TakeDamage(enemyUnit.damage);
        battleHud.SetUnitHp(playerUnit);

        if (isDead)
            state = BattleState.LOST;
        else
            state = BattleState.PLAYERTURN;

        yield return new WaitForSeconds(2f);

        BattleFsm();
    }

    //Win lose methods
    private void LoseBattle()
    {        
        battleHud.SetDialogueText($"{enemyUnit.unitName} has defeated you!\n Game Over");
    }

    private void WinBattle()
    {
        battleHud.SetDialogueText("Yay! You win!");
    }
}
