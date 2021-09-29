using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
public class BattleHud : MonoBehaviour
{
    BaseUI baseUI;

    private void SetBaseUI() {        
        baseUI = GetComponent<BaseUI>();
    }

    private BaseUI GetBaseUI() {
        if (baseUI == null)
            SetBaseUI();

        return baseUI;
    }

    public void Start() {
        baseUI = GetComponent<BaseUI>();
    }

    public void setUnitHud(Unit unit)
    {
        var unitContainer = baseUI.RootElement.Q<VisualElement>(unit.unitUIContainer);
        unitContainer.Q<Label>("UnitName").text = unit.unitName;
        unitContainer.Q<Label>("UnitLevel").text = $"Lv. {unit.unitLevel}";
        
        SetUnitHp(unitContainer, unit);
    }
    private Length GetHpPercent(int currentHP, int maxHP) {
        float hpPercent = ((float)currentHP/(float)maxHP)*100f;
        return Length.Percent(hpPercent);
    }

    private void SetUnitHp(VisualElement unitContainer, Unit unit) 
    {
        // StartCoroutine(HpDamageAnimation(unit, previousHp));
        unitContainer.Q<VisualElement>("UnitHpFill").style.width = GetHpPercent(unit.currentHP, unit.maxHP);
    }

    public void SetUnitHp(Unit unit)
    {
        var unitContainer = baseUI.RootElement.Q<VisualElement>(unit.unitUIContainer);
        SetUnitHp(unitContainer, unit);
    }

    public void SetDialogueText(string text) 
    {
        GetBaseUI().ControlContainer.Q<Label>("DialogueText").text = text;
    }

    public void ShowResetBtn(Action onResetBtn)
    {
        baseUI.ControlContainer.Q<Button>("AttackBtn").style.display = DisplayStyle.None;
        baseUI.ControlContainer.Q<Button>("HealBtn").style.display = DisplayStyle.None;
        var resetBtn = baseUI.ControlContainer.Q<Button>("ResetBtn");
        resetBtn.style.display = DisplayStyle.Flex;

        resetBtn.RegisterCallback<ClickEvent>(ev => onResetBtn());
    }

    public void LoadQuestion() {
        
    }

    //Tween Animations

    // public IEnumerator HpDamageAnimation(Unit unit, int previousHp) {
    //     var unitContainer = RootElement.Q<VisualElement>(unit.unitUIContainer);
    //     var hpFill = unitContainer.Q<VisualElement>("UnitHpFill");
    //     float endWidth = (unit.currentHP/unit.maxHP)*100;
    //     var wat = hpFill.style.width.value.value;
    //     Debug.Log($"endwith: {endWidth}");
    //     Debug.Log($"worldBoundWidth: {100f-wat}");


    //     var hpReduce = DOTween.To(() => hpFill.style.width.value.value, x => hpFill.style.width = Length.Percent(x), endWidth, 5f).SetEase(Ease.Linear);
    //     yield return hpReduce.WaitForCompletion();

    //     var hpShake = unitContainer.transform;
    // }
}
