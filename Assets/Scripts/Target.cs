using System;
using UnityEngine;


public class Target : MonoBehaviour
{
	[SerializeField] private float _health;

	public float Health
	{
		get => _health;
		set => _health = value;
	}

	private void OnDestroy()
	{
		GameManager.Instance.TargetKilled();
	}
}