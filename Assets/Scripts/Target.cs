using System;
using UnityEngine;


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
		GameManager.Instance.TargetKilled();
	}

	private void Start()
	{
		_nextRandomPosition = GameManager.Instance.GetRandomPointInsideMap();
		_playerPos = new Vector3(GameManager.Instance.Player.position.x, transform.position.y, GameManager.Instance.Player.position.z);
	}

	private void Update()
	{
		transform.LookAt(_playerPos);
		
		float step =  _speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, _nextRandomPosition, step);
		if(Vector3.Distance(transform.position, _nextRandomPosition) < 0.01f)
			_nextRandomPosition = GameManager.Instance.GetRandomPointInsideMap();
	}
}