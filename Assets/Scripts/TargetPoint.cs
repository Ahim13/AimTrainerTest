using UnityEngine;


public class TargetPoint : MonoBehaviour
{
	[Range(0, 3)] [SerializeField] private float _dmgMultiplier;
	[SerializeField] private Target _target;

	public void Hit(float dmg)
	{
		_target.Health -= dmg * _dmgMultiplier;

		if (_target.Health <= 0)
			Destroy(_target.gameObject);
	}
}