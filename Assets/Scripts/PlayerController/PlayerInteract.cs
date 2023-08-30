using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField] private float interactDistance = 2f;
	[SerializeField] private LayerMask interactLayer;
	[SerializeField] private Transform interactionPoint;
	[SerializeField] private PlayerController playerController;
	private InputActionAsset playerInput;
	private InputAction interactAction;
	private InputAction interactAlternateAction;

	[SerializeField] private IInteractable currentHighlightedObject;
	[SerializeField] private IInteractable previousHighlightedObject;
	// Update is called once per frame

	void Start()
	{
		playerInput = playerController.GeteInputActionAsset();
		interactAction = playerInput.FindActionMap("Player").FindAction("Interact");
		interactAlternateAction = playerInput.FindActionMap("Player").FindAction("InteractAlternate");
	}
	void Update()
	{
		HandleInteractions();
	}

	private void HandleInteractions()
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		mousePos.z = Camera.main.farClipPlane * interactDistance;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.position + worldPoint, out RaycastHit hit, interactDistance, interactLayer))
		{
			if (hit.transform.TryGetComponent(out IInteractable interactable))
			{


				Debug.Log(hit.transform.gameObject);

				if (interactable != previousHighlightedObject)
				{
					//If object changes
					previousHighlightedObject = currentHighlightedObject;
					currentHighlightedObject = interactable;

					if (previousHighlightedObject != null) { previousHighlightedObject.OnRemoveHighlight(); }

					Debug.Log("Object changed");
					currentHighlightedObject.OnHighlight();
				}
				else
				{
					//Object is the same
					Debug.Log("Hitting same object");
				}

				if (interactAction.WasPerformedThisFrame())
				{
					currentHighlightedObject.OnInteract();
					Debug.Log("Interacted with" + currentHighlightedObject);
				}

				if (interactAlternateAction.WasPerformedThisFrame())
				{
					currentHighlightedObject.OnInteractAlternate();
					Debug.Log("Interacted with" + currentHighlightedObject);
				}

			}
			else
			{
				{
					//Object is null
					if (previousHighlightedObject != null) { previousHighlightedObject.OnRemoveHighlight(); }
					currentHighlightedObject = null;
					previousHighlightedObject = null;
					Debug.Log("No object hit");
				}
			}
		}
	}
}
