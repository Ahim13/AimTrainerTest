using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private GameObject _targetPrefab;
	[SerializeField] private GameObject _flyingTargetPrefab;

	private int _targetsToKill;

	private void Awake()
	{
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;
	}

	public void TargetKilled()
	{
		_targetsToKill--;
	}
}