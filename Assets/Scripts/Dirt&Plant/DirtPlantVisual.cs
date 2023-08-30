using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtPlantVisual : MonoBehaviour
{
	[SerializeField] private PlantSO plantSO;

	[SerializeField] private GameObject currentPlantObject;

	[SerializeField] private DirtPlantGrowth dirtPatch;

	[SerializeField] private int currentGrowthIndex;

	[SerializeField] private GameObject currentPlantInstantiatedModel;

	void Start()
	{
		dirtPatch.OnPlantGrowAction += DirtPlantGrowth_OnPlantGrowAction;
		dirtPatch.OnNewSeedPlanted += DirtPlantGrowth_OnNewSeedPlanted;
		dirtPatch.OnPlantDestroyed += DirtPlantGrowth_OnPlantDestroyed;

		if (dirtPatch.GetCurrentPlant() != null)
		{
			plantSO = dirtPatch.GetCurrentPlant();

		}
	}

	private void DirtPlantGrowth_OnPlantDestroyed(object sender, EventArgs e)
	{
		//Reset field to null
		plantSO = null;
		currentPlantObject = null;
		if (currentPlantInstantiatedModel != null)
		{
			Destroy(currentPlantInstantiatedModel);
		}

		Debug.Log("Destroyed plant visual");
	}

	private void DirtPlantGrowth_OnNewSeedPlanted(object sender, EventArgs e)
	{
		//Reset to accomodate new seed
		plantSO = dirtPatch.GetCurrentPlant();
		currentPlantObject = plantSO.seed;

		if (currentPlantInstantiatedModel != null)
		{
			Destroy(currentPlantInstantiatedModel);
		}
		currentPlantInstantiatedModel = Instantiate(currentPlantObject, transform);
	}

	private void DirtPlantGrowth_OnPlantGrowAction(object sender, EventArgs e)
	{
		if (plantSO != null)
		{
			CyclePlantVisualArray();
		}
	}

	void CyclePlantVisualArray()
	{
		if (currentPlantInstantiatedModel != null)
		{
			Destroy(currentPlantInstantiatedModel);
		}

		if (dirtPatch.isHalfGrown)
		{
			currentPlantObject = plantSO.seedling;
		}

		if (dirtPatch.isFullyGrown)
		{
			currentPlantObject = plantSO.harvestable;
		}

		currentPlantInstantiatedModel = Instantiate(currentPlantObject, transform);

		currentPlantInstantiatedModel.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
	}
}
