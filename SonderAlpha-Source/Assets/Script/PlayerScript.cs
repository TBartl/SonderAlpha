using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	/*
	public Transform cameraPoint, cameraHolder;
	private Vector3 targetCameraRot;
	public float cameraRotationSpeed, cameraSmoothing, speed, jumpPower, gravity,turnSpeed, zoomSensitivity;
	private float justJumped;
	private CharacterController characterController;
	private Animator animator;
	private Vector3 lastVelocity;
	private Vector2 lastInput;
	private bool doubleJump,grounded;
	public GameObject doubleJumpClouds;
	public bool debugOnline = false;
	public GameObject hudGameObject;
	public HUD hud;
	public int weaponCycle;
	public Item dagger, bow;
	public float health,mana;
	public int healthPotions, manaPotions;
	public GameObject[] restoreParticles;
	public Transform rightHand;
	
	// Use this for initialization
	void Start () {
		
		grounded = false;
		targetCameraRot = Vector3.zero;
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		lastVelocity = Vector3.zero;
		lastInput = Vector2.zero;
		hudGameObject = GameObject.Find("HudCamera");
		hud = hudGameObject.GetComponent<HUD>();
		UpdateItems();
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (networkView.isMine || debugOnline)
		{	
			if (cameraHolder == null)
				cameraHolder = GameObject.Find("CameraHolder").transform;
			cameraHolder.position = cameraPoint.position;
			targetCameraRot += new Vector3( -Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*cameraRotationSpeed*Time.deltaTime;
			targetCameraRot = Quaternion.Euler(targetCameraRot).eulerAngles;
			cameraHolder.rotation =Quaternion.Lerp(cameraHolder.rotation, Quaternion.Euler(targetCameraRot), cameraSmoothing*Time.deltaTime);
			cameraHolder.localScale = cameraHolder.localScale - Vector3.one*Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime*zoomSensitivity;
			Vector2 input = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			input = input.normalized;
			Vector2 velocity = Vector2.Lerp(lastInput, input,Time.deltaTime*turnSpeed);
			lastInput = velocity;
			if (Input.GetButtonDown("CycleWeapon"))
			{
				if (!(dagger.slot != itemSlot.Dagger && bow.slot != itemSlot.Bow))
				{
					if (dagger.name == "NoItem" || bow.name == "NoItem")
						weaponCycle = (weaponCycle+3)%2;
					else
						weaponCycle = (weaponCycle+4)%3;
					hud.weaponCycle = weaponCycle;

				}
			}
			if (Input.GetButtonDown("UseHealthPotion"))
			{
				if (healthPotions >0)
				{
					healthPotions -= 1;
					health = Mathf.Min(1.0f, health +.5f);
					UpdateItems();
					GameObject temp = (GameObject)Instantiate(restoreParticles[0],transform.position, Quaternion.identity);
					temp.transform.parent = transform;
					temp = (GameObject)Instantiate(restoreParticles[1],transform.position, Quaternion.identity);
					temp.transform.parent = transform;
				}
				
			}
			if (Input.GetButtonDown("UseManaPotion"))
			{
				if (manaPotions >0)
				{
					manaPotions -= 1;
					mana = Mathf.Min(1.0f, mana +.5f);
					UpdateItems();
					GameObject temp = (GameObject)Instantiate(restoreParticles[2],transform.position, Quaternion.identity);
					temp.transform.parent = transform;
					temp = (GameObject)Instantiate(restoreParticles[3],transform.position, Quaternion.identity);
					temp.transform.parent = transform;
				}
				
			}
			float upVelocity = lastVelocity.y;
			if (grounded)
			{
				upVelocity = Mathf.Max(0f, upVelocity);
				doubleJump=true;
			}
			justJumped -= Time.deltaTime;
			if (Input.GetButton("Jump"))
		    {
				if (grounded)
				{
					upVelocity = jumpPower;
					justJumped = .2f;
				}
				else if(doubleJump && justJumped <= 0f)
				{
					upVelocity = jumpPower;
					doubleJump = false;
					Instantiate(doubleJumpClouds,transform.position,Quaternion.Euler(-90,0,0));
				}
			}
			upVelocity -= gravity*Time.deltaTime;
			animator.SetFloat("velocityForward", velocity.y);
			animator.SetFloat("velocityStrafe", velocity.x);
			animator.SetFloat("directionChecker", Mathf.Sign(velocity.y+.1f)*(Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y)));
			animator.SetFloat("upVelocity", upVelocity);
			animator.SetBool("grounded", grounded);
			if (velocity.magnitude >= .1f)
				transform.rotation = Quaternion.Euler(new Vector3(0,cameraHolder.rotation.eulerAngles.y,0));
			velocity = velocity*speed*Time.deltaTime;
			lastVelocity = new Vector3(velocity.x, upVelocity ,velocity.y);
			characterController.Move(transform.TransformDirection(lastVelocity));
			grounded = false;
		}
	}

	void CheckGrounded(bool isGrounded)
	{
		grounded = true;
	}

	void UpdateItems()
	{
		hud.SetDagger(dagger.slot == itemSlot.Dagger);
		hud.SetBow(bow.slot == itemSlot.Bow);
		hud.SetHealth(health);
		hud.SetMana(mana);
		hud.SetHealthPotions(healthPotions);
		hud.SetManaPotions(manaPotions);
	}

	void EquipDagger()
	{

	}

	public int GetWeaponEquipped()
	{
		if (weaponCycle == 0)
			return 0;
		else if (dagger.name == "NoItem")
			return 2;
		return 1;

	}
	*/

}
