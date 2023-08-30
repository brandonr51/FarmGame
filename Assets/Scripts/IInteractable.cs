using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

	void OnHighlight();

	void OnRemoveHighlight();

	void OnInteract();

	void OnInteractAlternate();
}
