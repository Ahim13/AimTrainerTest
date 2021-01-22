using System;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
	[SerializeField] private Animator _characterAnimator;

	public event Action ReloadedDone;


	private bool _isReady;
	private bool _isReloading;
	public bool IsReady => _isReady;


	public void Ready()
	{
		_isReady = true;
	}

	public void Reload()
	{
		if (!_isReloading)
		{
			_isReloading = true;
			_characterAnimator.SetTrigger("Reload");
			_characterAnimator.ResetTrigger("Fire");
		}
	}

	public void Fire()
	{
		_characterAnimator.SetTrigger("Fire");
	}

	public void Reloaded()
	{
		_isReloading = false;
		ReloadedDone?.Invoke();
	}


	private void OnEnable()
	{
		_isReady = false;
		_characterAnimator.SetTrigger("FirstUse");
	}
}