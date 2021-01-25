using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Target : MonoBehaviour
{
	[SerializeField] private float _health;
	[SerializeField] private float _speed = 3;

	private Vector3 _nextRandomPosition;
	private Vector3 _playerPos;

	public float Health
	{
		get => _health;
		set => _health = value;
	}

	private void OnDestroy()
	{
		if (GameManager.Instance != null)
			GameManager.Instance.TargetKilled();
	}

	private void Start()
	{
		if (GameManager.Instance != null)
		{
			_nextRandomPosition = GameManager.Instance.GetRandomPointInsideMap();
			var position = GameManager.Instance.Player.transform.position;
			_playerPos = new Vector3(position.x, transform.position.y, position.z);
			_speed = Random.Range(1, 4);
		}
	}

	private void Update()
	{
		if (GameManager.Instance != null)
		{
			transform.LookAt(_playerPos);

			float step = _speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, _nextRandomPosition, step);
			if (Vector3.Distance(transform.position, _nextRandomPosition) < 0.01f)
				_nextRandomPosition =
					GameManager.Instance.GetRandomPointInsideMap(); //Move to a random point inside the circle area - if there are obstacles this needs rework, navmesh is an option
		}
	}
}