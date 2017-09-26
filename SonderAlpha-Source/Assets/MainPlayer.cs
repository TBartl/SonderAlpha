using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainPlayer : Player {
	/*
	public Transform cameraPoint, cameraHolder;
	private Vector3 targetCameraRot;
	public float cameraRotationSpeed, cameraSmoothing, zoomSensitivity, zoomMin, zoomMax;
	public float maxSpeed, jumpPower, gravity, gravityReduction, turnSpeed;

	private float justJumped;
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
	*/

	public float moveAcceleration = 10.0f;
	public float airAcceleration = 10.0f;
	public float wallClampAcceleration = 100f;
	public float wallGravityReduction = .3f;
	public float airDrag = 5f;

	//public float gravity = 10.0f;
	public float minimumJumpTime = .8f;
	public float minimumJumpHeight = 2.0f;

	private float justJumped = 0f;

	
	private Transform cameraPoint, cameraHolder;
	private Vector3 targetCameraRot, currentCameraRot;
	private float targetZoomOut = 5f;
	public float cameraRotationSpeed = 6f;
	public float cameraSmoothing = 2f;
	public float zoomSpeed = 10f;
	public float zoomSmoothing = 1f;
	public bool debugMode = false;

	
	void Start()
	{
	}
	
	public void Awake () {
		base.Awake();
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
		cameraPoint = transform.FindChild("CameraPoint");
		cameraHolder = GameObject.Find("CameraHolder").transform;
		targetCameraRot = Vector3.zero;
	}
	void FixedUpdate () {
		base.FixedUpdate ();
		UpdateCamera ();


		//Movement
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


		/*
		if (input.magnitude >= .1f)
			transform.rotation = Quaternion.Euler(new Vector3(0,cameraHolder.rotation.eulerAngles.y,0));
		*/
		input = cameraHolder.transform.rotation * input;
		//input = cameraHolder.TransformDirection(input);
		if (groundState != GroundState.inAir)
			rigidbody.AddForce (input.normalized * moveAcceleration, ForceMode.Acceleration);
		else
			rigidbody.AddForce (input.normalized * airAcceleration, ForceMode.Acceleration);
		Vector3 lastVelocity = new Vector3 (rigidbody.velocity.x, 0, rigidbody.velocity.z);
		Vector3 friction = -lastVelocity.normalized * lastVelocity.magnitude / maxSpeed;
		if (groundState == GroundState.onGround)
			rigidbody.AddForce (friction * moveAcceleration, ForceMode.Acceleration);
		else
			rigidbody.AddForce (friction * airDrag, ForceMode.Acceleration);
		if (groundState == GroundState.onWall) {
			Vector3 normalNoY = new Vector3(surfaceNormal.x, 0, surfaceNormal.z).normalized;
			rigidbody.AddForce (-normalNoY * wallClampAcceleration, ForceMode.Acceleration);
		}
		if (groundState != GroundState.inAir && Input.GetButton("Jump") && justJumped  <= 0) {
			if (groundState == GroundState.onGround)
				rigidbody.velocity += surfaceNormal*CalculateJumpVerticalSpeed();
			else
			{
				Vector3 newNormal = (surfaceNormal + Vector3.up/2f).normalized;
				rigidbody.velocity += newNormal*CalculateJumpVerticalSpeed()*wallJumpModifier;
			}
			justJumped = .2f;
		}
		justJumped -= Time.deltaTime;
		if (groundState == GroundState.onWall)
			rigidbody.AddForce (new Vector3 (0, -CalculateAcceleration()*wallGravityReduction, 0), ForceMode.Acceleration);
		else
			rigidbody.AddForce (new Vector3 (0, -CalculateAcceleration(), 0), ForceMode.Acceleration);

		surfaceNormal = Vector3.zero;
		contactPoints = new List<Vector3>();


		if (Input.GetKeyDown (KeyCode.O))
			debugMode = !debugMode;

	}

	void OnCollisionStay(Collision collisionInfo) {
		base.OnCollisionStay (collisionInfo);
	}

	float CalculateAcceleration()
	{
		return 8f * minimumJumpHeight / (minimumJumpTime * minimumJumpTime);
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * minimumJumpHeight * CalculateAcceleration());
	}

	void UpdateCamera()
	{
		//Camera
		cameraHolder.position = Vector3.Lerp(cameraHolder.position, cameraPoint.position, Time.deltaTime*cameraSmoothing);
		targetCameraRot += new Vector3( -Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*cameraRotationSpeed*Time.deltaTime;
		currentCameraRot = Vector3.Lerp (currentCameraRot, targetCameraRot, cameraSmoothing * Time.deltaTime);
		/* TODO: Smooth Camera to Wall Surface
		if (!touchingSurface || groundNotWall)
						currentCameraRot = Vector3.Lerp (currentCameraRot, targetCameraRot, cameraSmoothing * Time.deltaTime);
		else {
			float newRotation = Mathf.Atan2(surfaceNormal.x,-surfaceNormal.y)*Mathf.Rad2Deg;
			currentCameraRot = new Vector3(0, newRotation, 0);
		}
		*/
		cameraHolder.rotation = Quaternion.Euler (currentCameraRot);
		targetZoomOut -= Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime*zoomSpeed;
		cameraHolder.localScale = Vector3.Lerp(cameraHolder.localScale,Vector3.one*targetZoomOut, Time.deltaTime*zoomSmoothing); 
	}

	void OnGUI()
	{
		if (debugMode) {
			if (GUI.Button(new Rect(100, 100, 250, 100), "Teleport to item test."))
				transform.position = new Vector3(0,-40,200);
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Teleport back to map."))
				transform.position = new Vector3(0,50,0);
		}
	}
}
