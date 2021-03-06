using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    private const float DIALOGUE_WAIT = 1f;
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

    private void OnResetBtn() {
        Debug.Log("Yo wat up I'm tryn to reset this here scene");

        SceneManager.LoadScene("BattleScene");
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
        attackBtn.style.display = DisplayStyle.Flex;
        attackBtn.RegisterCallback<ClickEvent>(ev => OnAttackBtn());

        var healBtn = controlsContainer.Q<Button>("HealBtn");
        healBtn.style.display = DisplayStyle.Flex;
        healBtn.RegisterCallback<ClickEvent>(ev => OnHealBtn());

        var resetBtn = controlsContainer.Q<Button>("ResetBtn");
        resetBtn.style.display = DisplayStyle.None;
        resetBtn.RegisterCallback<ClickEvent>(ev => OnResetBtn());

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

        // battleHud.SetDialogueText($"{enemyUnit.unitName} challenges you to learn math!");

        yield return new WaitForSeconds(DIALOGUE_WAIT);
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

    //Player and Enemy Attacks, each update the UI and call the BattleFSM
    private IEnumerator PlayerAttack()
    {
        battleHud.SetDialogueText($"You deal {playerUnit.damage} damage to {enemyUnit.unitName}");
        var previousHp = enemyUnit.currentHP;
        var isDead = enemyUnit.TakeDamage(playerUnit.damage);
        StartCoroutine(enemyUnit.DamageAnimation());
        battleHud.SetUnitHp(enemyUnit);

        if (isDead)
            state = BattleState.WON;
        else
            state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(DIALOGUE_WAIT);

        BattleFsm();
    }
    
    private IEnumerator PlayerHeal()
    {
        const int healAmount = 2; //hardcoded heal for right now, will change later.
        var previousHp = playerUnit.currentHP;
        playerUnit.Heal(healAmount);
        battleHud.SetUnitHp(playerUnit);
        battleHud.SetDialogueText($"You heal for {healAmount} points");        

        state = BattleState.ENEMYTURN;
        
        yield return new WaitForSeconds(DIALOGUE_WAIT);

        BattleFsm();
    }

    private IEnumerator EnemyAttack()
    {
        battleHud.SetDialogueText($"{enemyUnit.unitName} uses Chomp!\n You take {enemyUnit.damage} damage.");
        var previousHp = playerUnit.currentHP;
        var isDead = playerUnit.TakeDamage(enemyUnit.damage);
        StartCoroutine(enemyUnit.AttackAnimation());
        battleHud.SetUnitHp(playerUnit);
        
        if (isDead)
            state = BattleState.LOST;
        else
            state = BattleState.PLAYERTURN;

        yield return new WaitForSeconds(DIALOGUE_WAIT);

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
        battleHud.ShowResetBtn(OnResetBtn);
    }
}
