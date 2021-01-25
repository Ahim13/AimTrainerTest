using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static bool GameStarted = false;

	[SerializeField] private GameObject _targetPrefab;
	[SerializeField] private Transform _player;

	[Header("Debug")] [SerializeField] private float _maxDistanceToEnemies;

	private int _initialTargetsToKill = 2;

	private int _targetsToKill = 2;

	public int TargetsToKill => _targetsToKill;
	public int InitialTargetsToKill => _initialTargetsToKill;

	public Transform Player => _player;

	private void Awake()
	{
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;
	}

	public void Setup(int targetCount)
	{
		_initialTargetsToKill = targetCount;
		_targetsToKill = targetCount;
	}

	public void TargetKilled()
	{
		_targetsToKill = TargetsToKill - 1;
	}

	public void StartGame()
	{
		StartCoroutine(TargetSpawning());
		Cursor.lockState = CursorLockMode.Locked;
		GameStarted = true;
	}

	private IEnumerator TargetSpawning()
	{
		GameObject target = null;
		while (_targetsToKill > 0)
		{
			if (target == null)
			{
				target = Instantiate(_targetPrefab, GetRandomPointInsideMap(), Quaternion.identity);
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
	}

	public Vector3 GetRandomPointInsideMap()
	{
		Vector2 randPos = Random.insideUnitCircle * _maxDistanceToEnemies;
		while (Vector3.Distance(randPos, Player.position) < 2)
		{
			randPos = Random.insideUnitCircle * _maxDistanceToEnemies;
		}
		return new Vector3(randPos.x, 0.3f, randPos.y);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(Player.position, _maxDistanceToEnemies);
	}
}