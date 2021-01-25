using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Statistics _statistics;
    [SerializeField] private TextMeshProUGUI _clip;
    [SerializeField] private TextMeshProUGUI _maxTargetNumber;
    [SerializeField] private TextMeshProUGUI _currentTargetNumber;
    [SerializeField] private TextMeshProUGUI _accuracy;
    [SerializeField] private TextMeshProUGUI _criticalAccuracy;
    [SerializeField] private TextMeshProUGUI _score;

    private void Start()
    {
	    _maxTargetNumber.text = "/ " + _statistics.InitialTargetsToKill;
    }

    void Update()
    {
       _clip.text = _statistics.CurrentClip + "/∞";
       _currentTargetNumber.text = _statistics.TargetsToKill.ToString();
       _accuracy.text = "Accuracy: " + _statistics.Accuracy.ToString("P1");
       _criticalAccuracy.text = "Critical accuracy: " +  _statistics.CriticalAccuracy.ToString("P1");
       _score.text = "Score: " +  _statistics.Score;
    }
}
