using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;
	[SerializeField] private float rotationSpeed;
	private void LateUpdate()
	{

		Quaternion lookRotation = Quaternion.LookRotation(playerController.GetCharacterMoveDirection() * Time.deltaTime);

		if (playerController.GetCharacterMoveVector().sqrMagnitude > 0.1)
		{
			transform.localRotation = Quaternion.Lerp(transform.localRotation, lookRotation, rotationSpeed * Time.deltaTime);
		}
	}

}
