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
	public static bool GameStarted = false;

	[SerializeField] private GameObject _targetPrefab;
	[SerializeField] private PlayerController _player;

	[Header("Debug")] [SerializeField] private float _maxDistanceToEnemies;

	private int _initialTargetsToKill = 2;
	private int _targetsToKill = 2;
	private int _score;
	private Parameters parameters;

	public int TargetsToKill => _targetsToKill;
	public int InitialTargetsToKill => _initialTargetsToKill;
	public int Score => _score;

	public PlayerController Player => _player;

	private void Awake()
	{
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;

		LoadParameters();
	}

	private void LoadParameters()
	{
		string json = File.ReadAllText(Application.dataPath + "/Technical Test/Example Data/ExampleParameters.json");
		parameters = JsonUtility.FromJson<Parameters>(json);
		Setup();
		_player.SetUp(parameters.WeaponParameters.rateOfFire, parameters.WeaponParameters.clipSize, parameters.WeaponParameters.damage);
	}

	public void Setup()
	{
		_initialTargetsToKill = parameters.GameParameters.targetsToKill;
		_targetsToKill = _initialTargetsToKill;
	}

	public void TargetKilled()
	{
		_targetsToKill = TargetsToKill - 1;
		_score += 200; //would be better in a parameter
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
				target.GetComponent<Target>().Health = parameters.TargetParameters.health;
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
		
		yield return new WaitForSeconds(3);
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