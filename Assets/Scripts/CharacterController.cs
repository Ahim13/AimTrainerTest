using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private AnimationController _animationController;
	[SerializeField] private Transform _player;
	[SerializeField] private Transform _camera;

	[SerializeField] private float _mSensitivity = 10f;
	[SerializeField] private float _dmg;
	[SerializeField] private float _range;
	[SerializeField] private float _fireRate;
	[SerializeField] private float _clipSize;


	private float _xRotation;
	private float _timeToFire;
	private float _currentClip;
	private bool _isLoaded;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		_currentClip = _clipSize;
		_isLoaded = true;
	}

	void Update()
	{
		CameraMovement();
		Shooting();
	}

	private void Shooting()
	{
		if (!_animationController.IsReady)
			return;

		// if (Input.GetMouseButton(0) && Time.time >= _timeToFire)
		// {
		// 	//_characterAnimator.SetTrigger("Fire");
		// 	_timeToFire = _fireRate;
		// 	Debug.Log("FIREDDD!");
		// 	RaycastHit hit;
		// 	if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range))
		// 	{
		// 		Debug.Log(hit.transform.name);
		// 	}
		// }

		if (Input.GetMouseButton(0))
		{
			if (Input.GetMouseButtonDown(0))
			{
				_timeToFire = Time.time;
			}

			while (_timeToFire <= Time.time)
			{
				_timeToFire += 1f / _fireRate;
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		if (_currentClip == 0)
		{
			_animationController.Reload();
			return;
		}

		_animationController.Fire();
		RaycastHit hit;
		if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range))
		{
			if (hit.transform.CompareTag("Target"))
			{
				
			}
		}

		--_currentClip;
		if (_currentClip == 0)
		{
			_isLoaded = false;
		}
	}

	private void CameraMovement()
	{
		float mouseX = Input.GetAxis("Mouse X") * _mSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * _mSensitivity * Time.deltaTime;

		_xRotation -= mouseY;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		_camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
		_player.Rotate(Vector3.up * mouseX);
	}

	private void Reloaded()
	{
		_currentClip = _clipSize;
		_timeToFire = Time.time;
		_isLoaded = true;
	}

	private void OnEnable()
	{
		_animationController.ReloadedDone += Reloaded;
	}

	private void OnDisable()
	{
		_animationController.ReloadedDone -= Reloaded;
	}
}