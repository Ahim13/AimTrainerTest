using System;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
	[SerializeField] private Animator _characterAnimator;
	
	[Header("Audio")]
	[SerializeField] private AudioSource _fire;
	[SerializeField] private AudioSource _reload;
	[SerializeField] private AudioSource _ready;

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
			_reload.Play();
			_characterAnimator.SetTrigger("Reload");
			_characterAnimator.ResetTrigger("Fire");
		}
	}

	public void Fire()
	{
		_fire.Play();
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
		_ready.Play();
		_characterAnimator.SetTrigger("FirstUse");
	}
}