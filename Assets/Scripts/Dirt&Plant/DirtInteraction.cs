using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtInteraction : MonoBehaviour, IInteractable
{
	[SerializeField] GameObject highlightObject;
	[SerializeField] private DirtPlantGrowth dirtPlantGrowth;
	//[SerializeField] 
	public void OnHighlight()
	{
		highlightObject.SetActive(true);
	}

	public void OnRemoveHighlight()
	{
		highlightObject.SetActive(false);
	}

	public void OnInteract()
	{
		dirtPlantGrowth.PlantNewSeed(dirtPlantGrowth.testPlant);
	}

	public void OnInteractAlternate()
	{
		dirtPlantGrowth.DestroyPlant();
	}


}
