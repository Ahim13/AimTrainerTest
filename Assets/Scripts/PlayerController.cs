using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private AnimationController _animationController;
	[SerializeField] private Transform _player;
	[SerializeField] private Transform _camera;
	[SerializeField] private LayerMask _layermask;

	[SerializeField] private float _mSensitivity = 10f;
	[SerializeField] private float _dmg;
	[SerializeField] private float _range;
	[SerializeField] private float _fireRate;
	[SerializeField] private float _clipSize;


	private float _xRotation;
	private float _timeToFire;
	private float _currentClip;
	private bool _isLoaded;

	public float CurrentClip => _currentClip;

	private void Awake()
	{
		_currentClip = _clipSize;
		_isLoaded = true;
		_dmg /= _fireRate / 60f;
	}

	void Update()
	{
		if(!GameManager.GameStarted)
			return;
		
		CameraMovement();
		if (_animationController.IsReady)
		{
			Shooting();
			if(Input.GetKeyDown(KeyCode.R))
				Reload();
		}
	}

	private void Reload()
	{
		_isLoaded = false;
		_animationController.Reload();
	}

	private void Shooting()
	{
		if (Input.GetMouseButton(0) && _isLoaded)
		{
			if (Input.GetMouseButtonDown(0))
			{
				_timeToFire = Time.time;
			}

			while (_timeToFire <= Time.time)
			{
				_timeToFire += 60f / _fireRate;
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		_animationController.Fire();
		RaycastHit hit;
		if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range, _layermask))
		{
			Debug.Log(hit.transform.name);
			if (hit.transform.CompareTag("Target"))
			{
				hit.transform.GetComponent<TargetPoint>().Hit(_dmg);
			}
		}
		_currentClip = CurrentClip - 1;
		if (CurrentClip == 0)
		{
			_isLoaded = false;
			_animationController.Reload();
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