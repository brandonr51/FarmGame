using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantSO : ScriptableObject
{
	[Header("Stages of Life")]
	public GameObject seed;
	public GameObject seedling;
	public GameObject harvestable;

	public enum CropState
	{
		Seed, Seedling, Harvestable
	}

	public int secondsToGrowAverage; //In seconds

	public float secondsToGrowVariation; //In seconds
}
