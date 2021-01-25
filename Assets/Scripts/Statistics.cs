using UnityEngine;


[CreateAssetMenu(fileName = "Statistics", menuName = "Statistics", order = 0)]
public class Statistics : ScriptableObject
{
	public int TargetsToKill;
	public int InitialTargetsToKill;
	public int Score;
	public float CurrentClip;

	public float AllShots = 0;
	public float CritricalShots = 0;
	public float NormalShots = 0;

	public float Dmg;
	public float FireRate;
	public float ClipSize;
	
	public bool GameStarted;

	public float Accuracy => (NormalShots + CritricalShots) / AllShots;
	public float CriticalAccuracy => CritricalShots / AllShots;
}