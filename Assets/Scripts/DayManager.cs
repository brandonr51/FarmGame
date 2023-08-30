using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayManager : MonoBehaviour
{
	public event EventHandler OnDayPassedAction;
	public event EventHandler OnPlantPeriodPassed;

	public static DayManager Instance;

	public float plantUpdatePeriod = 1f;

	private int currentDay;

	public float currentDayTime;

	public float dayLengthInMinutes;
	private float currentPlantUpdateTime;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Update()
	{
		currentDayTime += Time.deltaTime;
		currentPlantUpdateTime += Time.deltaTime;
		float secondsToMinutes = 60f;

		if (currentPlantUpdateTime > plantUpdatePeriod)
		{
			OnPlantPeriodPassed?.Invoke(this, EventArgs.Empty);
			currentPlantUpdateTime = 0;
		}

		if (currentDayTime > (dayLengthInMinutes * secondsToMinutes))
		{
			StartNextDay();
		}
	}

	private void StartNextDay()
	{
		currentDay++;
		currentDayTime = 0;
		OnDayPassedAction?.Invoke(this, EventArgs.Empty);
		Debug.Log("Starting a new day!");
	}
}
