using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private AnimationController _animationController;
	[SerializeField] private Statistics _statistics;
	[SerializeField] private Transform _player;
	[SerializeField] private Transform _camera;
	[SerializeField] private LayerMask _layermask;

	[SerializeField] private float _mSensitivity = 10f;
	[SerializeField] private float _range;

	private float _xRotation;
	private float _timeToFire;
	private bool _isLoaded;
	

	private void Awake()
	{
		_isLoaded = true;
	}

	private void Start()
	{
		_statistics.CurrentClip = _statistics.ClipSize;
		ResetStats();
	}

	private void ResetStats()
	{
		_statistics.Score = 0;
		_statistics.AllShots = 0;
		_statistics.CritricalShots = 0;
		_statistics.NormalShots = 0;
	}

	void Update()
	{
		if (!_statistics.GameStarted)
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
				_timeToFire += 60f / _statistics.FireRate;
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		_animationController.Fire();
		RaycastHit hit;
		++_statistics.AllShots;
		if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range, _layermask))
		{
			if (hit.transform.CompareTag("Target"))
			{
				var targetPoint = hit.transform.GetComponent<TargetPoint>();
				targetPoint.Hit(_statistics.Dmg);
				if (targetPoint.Type == TargetPointType.Normal)
				{
					++_statistics.NormalShots;
					_statistics.Score += 20;
				}
				else if (targetPoint.Type == TargetPointType.Critical)
				{
					++_statistics.CritricalShots;
					_statistics.Score += 40;
				}
			}
		}

		_statistics.CurrentClip = _statistics.CurrentClip - 1;
		if (_statistics.CurrentClip == 0)
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
		_statistics.CurrentClip = _statistics.ClipSize;
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