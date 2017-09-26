using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum ItemSlot
{
	None, Dust, HealthPotion, ManaPotion, Dagger, Bow, Armor 
}

[System.Serializable]
public enum ItemQuality
{
	Common, Rare, Epic, Legendary
}




public class Item : MonoBehaviour {
	public string description;
	public ItemSlot slot;
	public ItemQuality itemQuality;
	public GameObject icon;
	public GameObject displayModel;
	public GameObject groundModel;
	public List<Stat> stats;

}
