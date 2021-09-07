using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleHud : MonoBehaviour
{

    VisualElement rootEle;
    VisualElement controlsContainer;
    // Start is called before the first frame update
    void Start()
    {
        rootEle = GetComponent<UIDocument>().rootVisualElement;
        controlsContainer = rootEle.Q<VisualElement>("ControlsContainer");
        controlsContainer.Q<Button>("AttackBtn").RegisterCallback<ClickEvent>(ev => PlayerAttack());
    }

    private void PlayerAttack()
    {

    }

    public void setUnitHud(Unit unit)
    {
        var unitContainer = rootEle.Q<VisualElement>(unit.unitUIContainer);
        unitContainer.Q<Label>("UnitName").text = unit.unitName;
        unitContainer.Q<Label>("UnitLevel").text = $"Lv. {unit.unitLevel}";
        
        SetUnitHp(unitContainer, unit);
    }

    private void SetUnitHp(VisualElement unitContainer, Unit unit) {
        float hpPercent = ((float)unit.currentHP/(float)unit.maxHP)*100f;

        unitContainer.Q<VisualElement>("UnitHpFill").style.width = Length.Percent(hpPercent);
    }

    public void SetUnitHp(Unit unit)
    {
        var unitContainer = rootEle.Q<VisualElement>(unit.unitUIContainer);
        SetUnitHp(unitContainer, unit);
    }

    public void SetDialogueText(string text) {
        var controlsContainer = rootEle.Q<VisualElement>("ControlsContainer");
        controlsContainer.Q<Label>("DialogueText").text = text;
    }
}
