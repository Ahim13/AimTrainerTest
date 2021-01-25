using UnityEngine;

public enum TargetPointType
{
	Normal,
	Critical,
}
public class TargetPoint : MonoBehaviour
{
	[Range(0, 3)] [SerializeField] private float _dmgMultiplier;
	[SerializeField] private Target _target;
	[SerializeField] private TargetPointType _type;

	public TargetPointType Type => _type;

	public void Hit(float dmg)
	{
		_target.Health -= dmg * _dmgMultiplier;

		if (_target.Health <= 0)
			Destroy(_target.gameObject);
	}
}