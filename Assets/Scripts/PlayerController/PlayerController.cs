using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

	[Header("Physics values")]
	private Rigidbody _RB;
	[SerializeField] private float _RideHeight;
	[SerializeField] private float _RideSpringStrength;
	[SerializeField] private float _RideSpringDamper;
	[SerializeField] private float _GroundedDistance;
	[SerializeField] private Transform _GroundedCheckPoint;
	[SerializeField] private float _uprightJoinSpringStrength;
	[SerializeField] private float _uprightJointSpringDamper;
	[SerializeField] private LayerMask _GroundLayers;
	[SerializeField] private float SlerpSpeed;
	[SerializeField] private Vector3 moveDirection;

	[Header("Input system")]
	[SerializeField] private InputActionAsset playerInput;
	private InputAction moveAction;
	//private InputAction jumpAction;

	[Header("Player input values")]
	[SerializeField] private Vector2 moveVector;
	public Vector2 lookVector;
	[SerializeField] private Vector3 m_UnitMoveGoal;
	[SerializeField] private float _maxSpeed;
	[SerializeField] private float _acceleration;
	[SerializeField] private float _maxAccelForce;
	[SerializeField] private float counterMovement;
	[SerializeField] private Vector3 _forceScale;

	float xRotation;

	[Header("Jumping")]
	public bool jumpPressed;
	bool isJumping;
	bool isGrounded;
	bool canJump;
	public float gravity;

	//[Header("Script references")]
	//[SerializeField] private WeaponController weaponController;

	//Script awake
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		_RB = GetComponent<Rigidbody>();

		moveAction = playerInput.FindActionMap("Player").FindAction("Move");
		//jumpAction = playerInput.FindActionMap("Player").FindAction("Jump");
	}
	//Input update
	private void Update()
	{
		moveVector = moveAction.ReadValue<Vector2>();
		//jumpPressed = jumpAction.IsPressed();

		if (jumpPressed && isGrounded == true && canJump == true)
		{
			isJumping = true;
			isGrounded = false;
			canJump = false;
			StopCoroutine(Jump());
			StartCoroutine(Jump());
		}

	}

	//Physics update
	private void FixedUpdate()
	{
		if (!isJumping)
		{
			UpdateGroundCheck();
		}

		UpdateUprightForce();
		UpdateMovement();
		Quaternion targetX = Quaternion.Euler(0, xRotation, 0);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetX, Time.deltaTime);
	}

	//Keeps rigidbody floating at the ride height using spring physics
	void UpdateGroundCheck()
	{
		if (Physics.Raycast(_GroundedCheckPoint.position, Vector3.down, out RaycastHit _rayHit, _GroundedDistance, _GroundLayers))
		{
			isGrounded = true;
			canJump = true;
			Vector3 vel = _RB.velocity;
			Vector3 rayDir = Vector3.down;

			Vector3 otherVel = Vector3.zero;
			Rigidbody hitBody = _rayHit.rigidbody;
			if (hitBody != null)
			{
				otherVel = hitBody.velocity;
			}

			float rayDirVel = Vector3.Dot(rayDir, vel);
			float otherDirVel = Vector3.Dot(rayDir, otherVel);

			float relVel = rayDirVel - otherDirVel;

			float x = _rayHit.distance - _RideHeight;

			float springForce = (x * _RideSpringStrength) - (relVel * _RideSpringDamper);

			_RB.AddForce(rayDir * springForce);

			if (hitBody != null && !hitBody.isKinematic)
			{
				hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
			}
		}
		else
		{
			isGrounded = false;
		}
	}

	//Keeps rigidbody rotated in the desired upright position using spring physics
	void UpdateUprightForce()
	{
		Quaternion toGoal = Quaternion.FromToRotation(transform.up, Vector3.up);


		toGoal.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
		rotAxis.Normalize();

		float rotRadians = rotDegrees * Mathf.Deg2Rad;

		_RB.AddTorque((rotAxis * (rotRadians * _uprightJoinSpringStrength)) - (_RB.angularVelocity * _uprightJointSpringDamper));
	}

	void UpdateMovement()
	{

		if (moveVector.sqrMagnitude > 0.1)
		{
			moveDirection = (transform.forward * -moveVector.y) + (transform.right * -moveVector.x);
			Vector3 goalVel = Vector3.ClampMagnitude(new Vector3(moveDirection.x, 0, moveDirection.z), _maxAccelForce);
			Vector3 horizontalVelocity = new(_RB.velocity.x, 0, _RB.velocity.z);
			_RB.AddForce(goalVel - (horizontalVelocity - moveDirection * _maxAccelForce) * _maxSpeed, ForceMode.Acceleration);
		}
		else
		{
			Vector3 stopDirection = new(-_RB.velocity.x, 0, -_RB.velocity.z);
			_RB.AddForce(stopDirection * counterMovement, ForceMode.Impulse);
		}

		_RB.AddForce(-Vector3.up * gravity, ForceMode.Acceleration);
	}

	IEnumerator Jump()
	{
		_RB.velocity = 10 * Vector3.up;
		yield return new WaitForSecondsRealtime(0.2f);
		isJumping = false;
	}

	public Vector3 GetCharacterMoveDirection()
	{
		return moveDirection;
	}

	public Vector3 GetCharacterMoveVector()
	{
		return moveVector;
	}

	public InputActionAsset GeteInputActionAsset()
	{
		return playerInput;
	}
}
