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
	[SerializeField] private float _range;
	
	[SerializeField] private float _dmg;
	[SerializeField] private float _fireRate;
	[SerializeField] private float _clipSize;

	private float _allShots = 0;
	private float _critricalShots = 0;
	private float _normalShots = 0;
	
	private float _xRotation;
	private float _timeToFire;
	private float _currentClip;
	private bool _isLoaded;

	public float CurrentClip => _currentClip;
	public float Accracy => (_normalShots + _critricalShots) / _allShots;
	public float CriticalAccuracy => _critricalShots / _allShots;

	private void Awake()
	{
		_isLoaded = true;
	}

	public void SetUp(float fireRate, float clipSize, float damage)
	{
		_fireRate = fireRate;
		_clipSize = clipSize;
		_dmg = damage;
		_dmg /= _fireRate / 60f;
		_currentClip = _clipSize;
	}

	void Update()
	{
		if (!GameManager.GameStarted)
			return;

		CameraMovement();
		if (_animationController.IsReady)
		{
			Shooting();
			if (Input.GetKeyDown(KeyCode.R))
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
		++_allShots;
		if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range, _layermask))
		{
			if (hit.transform.CompareTag("Target"))
			{
				var targetPoint = hit.transform.GetComponent<TargetPoint>();
				targetPoint.Hit(_dmg);
				if (targetPoint.Type == TargetPointType.Normal)
				{
					++_normalShots;
					GameManager.Instance.Score += 20;
				}
				else if (targetPoint.Type == TargetPointType.Critical)
				{
					++_critricalShots;
					GameManager.Instance.Score += 40;
				}
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