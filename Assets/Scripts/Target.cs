using System;
using UnityEngine;


public class Target : MonoBehaviour
{
	[SerializeField] private float _health;

	public void GotHit(float dmg)
	{
		Debug.Log("OUCH");
		_health -= dmg;
		
		if(_health <= 0)
			Destroy(gameObject);
	}

	private void OnDestroy()
	{
		GameManager.Instance.TargetKilled();
	}
}