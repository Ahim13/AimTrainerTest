using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private GameObject _targetPrefab;
	[SerializeField] private PlayerController _player;
	[SerializeField] private Statistics _statistics;
	[SerializeField] private AnimationController _animationController;

	[Header("Debug")] [SerializeField] private float _maxDistanceToEnemies;
	
	private Parameters parameters;

	public PlayerController Player => _player;

	private void Awake()
	{
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;
		_statistics.GameStarted = false;
		Cursor.lockState = CursorLockMode.None;

		LoadParameters();
	}

	private void LoadParameters()
	{
		string json = File.ReadAllText(Application.dataPath + "/Technical Test/Example Data/ExampleParameters.json"); // would be better as a field or dynamic parameter
		parameters = JsonUtility.FromJson<Parameters>(json);
		_statistics.InitialTargetsToKill = parameters.GameParameters.targetsToKill;
		_statistics.TargetsToKill = parameters.GameParameters.targetsToKill;
		_statistics.FireRate = parameters.WeaponParameters.rateOfFire;
		_statistics.ClipSize = parameters.WeaponParameters.damage;
		_statistics.Dmg = parameters.WeaponParameters.damage;
		_statistics.Dmg /= _statistics.FireRate / 60f;
	}

	public void Setup()
	{

	}

	public void TargetKilled()
	{
		--_statistics.TargetsToKill;
	}

	public void StartGame()
	{
		StartCoroutine(TargetSpawning());
		Cursor.lockState = CursorLockMode.Locked;
		_statistics.GameStarted = true;
		_animationController.enabled = true;
	}

	private IEnumerator TargetSpawning()
	{
		GameObject target = null;
		while (_statistics.TargetsToKill > 0)
		{
			if (target == null)
			{
				target = Instantiate(_targetPrefab, GetRandomPointInsideMap(), Quaternion.identity);
				target.GetComponent<Target>().Health = parameters.TargetParameters.health;
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
		
		SaveAndReload();
	}

	private void SaveAndReload()
	{

		File.WriteAllText(Application.dataPath + "/Technical Test/Example Data/PlayerStatistics.json", 
			JsonUtility.ToJson(new Stats(new PlayerStatistics(_statistics.Score, _statistics.Accuracy, _statistics.CriticalAccuracy)))); // would be better as a field or dynamic parameter
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public Vector3 GetRandomPointInsideMap()
	{
		Vector2 randPos = Random.insideUnitCircle * _maxDistanceToEnemies;
		while (Vector3.Distance(randPos, Player.transform.position) < 2)
		{
			randPos = Random.insideUnitCircle * _maxDistanceToEnemies;
		}
		return new Vector3(randPos.x, 0.3f, randPos.y);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(Player.transform.position, _maxDistanceToEnemies);
	}
}