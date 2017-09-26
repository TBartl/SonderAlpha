using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public Transform healthBar, manaBar;
	private float healthBarTargetScale, manaBarTargetScale;
	public GameObject healthPotionIcon, manaPotionIcon;
	public TextMesh healthPotionCount, manaPotionCount;
	public GameObject fist, dagger, bow;
	public int weaponCycle;

	public float speed;
	// Use this for initialization
	void Start () {
		healthBarTargetScale = 1f;
		manaBarTargetScale = 1f;
		SetHealth(.5f);
		SetMana(.2f);
		SetHealthPotions(3);
		SetManaPotions(0);
	}

	
	// Update is called once per frame
	void Update () {
		healthBar.transform.localScale = new Vector3(Mathf.Lerp(healthBar.transform.localScale.x, healthBarTargetScale, Time.deltaTime*speed),1,1);
		manaBar.transform.localScale = new Vector3(Mathf.Lerp(manaBar.transform.localScale.x, manaBarTargetScale, Time.deltaTime*speed),1,1);
		fist.transform.localPosition = Vector3.Lerp(fist.transform.localPosition,GetCyclePosition(0),Time.deltaTime*speed);
		if (dagger.activeSelf)
			dagger.transform.localPosition = Vector3.Lerp(dagger.transform.localPosition,GetCyclePosition(1),Time.deltaTime*speed);
		if (bow.activeSelf)
			bow.transform.localPosition = Vector3.Lerp(bow.transform.localPosition,GetCyclePosition(2),Time.deltaTime*speed);
		fist.renderer.material.color = new Color(1-.25f*fist.transform.localPosition.z,1-.25f*fist.transform.localPosition.z,1-.25f*fist.transform.localPosition.z);
		dagger.renderer.material.color = new Color(1-.25f*dagger.transform.localPosition.z,1-.25f*dagger.transform.localPosition.z,1-.25f*dagger.transform.localPosition.z);
		bow.renderer.material.color = new Color(1-.25f*bow.transform.localPosition.z,1-.25f*bow.transform.localPosition.z,1-.25f*bow.transform.localPosition.z);
	}

	public void SetHealth(float f)
	{
		healthBarTargetScale = f;
	}
	public void SetMana(float f)
	{
		manaBarTargetScale = f;
	}

	public void SetHealthPotions(int x)
	{
		if (x == 0)
		{
			healthPotionCount.text = "";
			healthPotionIcon.SetActive(false);
		}
		else
		{
			healthPotionCount.text = "x"+x.ToString();
			healthPotionIcon.SetActive(true);
		}
	}
	public void SetManaPotions(int x)
	{
		if (x == 0)
		{
			manaPotionCount.text = "";
			manaPotionIcon.SetActive(false);
		}
		else
		{
			manaPotionCount.text = "x"+x.ToString();
			manaPotionIcon.SetActive(true);
		}
	}

	public void SetDagger(bool b)
	{
		dagger.SetActive(b);
		weaponCycle = 0;
	}
	public void SetBow(bool b)
	{
		bow.SetActive(b);
		weaponCycle = 0;
	}

	Vector3 GetCyclePosition(int weapon)
	{
		int x = 0;
		if (!dagger.activeSelf || !bow.activeSelf)
		{
			if (weapon == 0)
			{
				x = -weaponCycle;
			}
			else
			{
				x = -((1+weaponCycle)%2);
			}
		}
		else
		{
			x = TriCycledPosition((weapon - weaponCycle+3)%3);
		}
		return new Vector3(x,0,Mathf.Abs(x));

	}

	int TriCycledPosition(int x)
	{
		if (x == 0)
			return 0;
		else if (x == 1)
			return -1;
		return 1;
	}


}
