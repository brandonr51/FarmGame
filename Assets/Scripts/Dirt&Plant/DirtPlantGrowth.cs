using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirtPlantGrowth : MonoBehaviour
{
	public event EventHandler OnPlantGrowAction;
	public event EventHandler OnNewSeedPlanted;
	public event EventHandler OnPlantDestroyed;
	public enum plantState
	{
		Empty,
		Seed,
		HalfGrown,
		FullGrown,
		Dead
	}

	public enum dirtState
	{
		Untilled,
		Tilled,
		Watered,
		Fertilized
	}

	public plantState currentState;

	[SerializeField] private PlantSO currentPlant;

	[SerializeField] private float currentPlantAge; //In seconds

	private float determinedPlantLife;

	public bool isHalfGrown = false;
	public bool isFullyGrown = false;
	public PlantSO testPlant;

	private void Awake()
	{
		currentState = plantState.Empty;
	}
	private void Start()
	{
		DayManager.Instance.OnPlantPeriodPassed += DayManager_OnPlantPeriodPassed;
	}

	private void DayManager_OnPlantPeriodPassed(object sender, EventArgs e)
	{
		if (currentPlant != null)
		{
			currentPlantAge = currentPlantAge + 1f;
			GrowPlant();
		}
	}
	private void GrowPlant()
	{
		//if the plant is half grown
		if (currentPlantAge > (determinedPlantLife / 2) && currentPlantAge < determinedPlantLife && !isHalfGrown)
		{
			currentState = plantState.HalfGrown;
			isHalfGrown = true;
			OnPlantGrowAction?.Invoke(this, EventArgs.Empty);
			Debug.Log("Plant half grown!");
		}

		//if the plant is full grown
		if (currentPlantAge >= determinedPlantLife && !isFullyGrown)
		{
			currentState = plantState.FullGrown;
			isHalfGrown = false;
			isFullyGrown = true;
			Debug.Log("Plant fully grown!");
			OnPlantGrowAction?.Invoke(this, EventArgs.Empty);
		}
	}

	public void PlantNewSeed(PlantSO plantSO)
	{
		currentState = plantState.Seed;
		currentPlant = plantSO;
		currentPlantAge = 0f;

		float plantLifeInDays = currentPlant.secondsToGrowAverage;
		float plantLifeVariation = currentPlant.secondsToGrowVariation;

		isHalfGrown = false;
		isFullyGrown = false;

		determinedPlantLife = plantLifeInDays + UnityEngine.Random.Range(plantLifeVariation, -plantLifeVariation);

		OnNewSeedPlanted?.Invoke(this, EventArgs.Empty);

		Debug.Log(determinedPlantLife);
	}

	public void DestroyPlant()
	{
		currentState = plantState.Empty;
		if (currentPlant != null)
		{
			Destroy(currentPlant);
		}
		currentPlant = null;
		currentPlantAge = 0f;
		isHalfGrown = false;
		isFullyGrown = false;
		OnPlantDestroyed?.Invoke(this, EventArgs.Empty);

		Debug.Log("Destroyed plant");
	}

	public void TillDirt()
	{

	}

	public void WaterDirt()
	{

	}

	public void HarvestPlant()
	{

	}

	public void FertilizeDirt()
	{

	}

	public PlantSO GetCurrentPlant()
	{
		return currentPlant;
	}
}
