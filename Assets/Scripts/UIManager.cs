﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _clip;
    [SerializeField] private TextMeshProUGUI _maxTargetNumber;
    [SerializeField] private TextMeshProUGUI _currentTargetNumber;
    [SerializeField] private TextMeshProUGUI _accuracy;
    [SerializeField] private TextMeshProUGUI _criticalAccuracy;

    private void Start()
    {
	    _maxTargetNumber.text = "/ " + GameManager.Instance.InitialTargetsToKill;
    }

    void Update()
    {
       _clip.text = _playerController.CurrentClip + "/∞";
       _currentTargetNumber.text = GameManager.Instance.TargetsToKill.ToString();
    }
}
