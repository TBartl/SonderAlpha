using UnityEngine;
using System.Collections;

[System.Serializable]
public enum StatType
{
	MaxHealth, MaxMana, DaggerSpeed, DaggerDamage, BowSpeed, BowDamage, Armor
}

[System.Serializable]
public class Stat {
	public StatType statType;
	public float amount;
	
}
