using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public string unitName;
	public int unitLevel;

	public int damage;

	public int maxHP;
	public int currentHP;

	public string unitUIContainer;

	public Vector3 punch;
	public float duration;
	public float strength;
	public int vibrato;
	public float elasticity;

	public SpriteRenderer unitSprite;

	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}

	public IEnumerator DamageAnimation() {
		Tween damageAni = transform.DOPunchScale(new Vector3(-0.75f, -0.25f), 0.25f, vibrato, elasticity);
		yield return damageAni.WaitForCompletion();
	}

	public IEnumerator AttackAnimation() {
		// transform.DOPunchPosition(new Vector3(0f, -1f, 0f), 0.25f, 0);
		var attackAni = transform.DOPunchScale(new Vector3(0.75f, 0.75f), 0.5f, 0, 0);
		yield return attackAni.WaitForCompletion();
	}

}
