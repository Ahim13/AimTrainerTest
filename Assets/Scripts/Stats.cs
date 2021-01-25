using System;

[Serializable]
public struct PlayerStatistics
{
	public int Score;
	public float Accuracy;
	public float CriticalAccuracy;

	public PlayerStatistics(int score, float accuracy, float criticalAccuracy)
	{
		Score = score;
		Accuracy = accuracy;
		CriticalAccuracy = criticalAccuracy;
	}
}
[Serializable]
public class Stats
{
	public PlayerStatistics PlayerStatistics;

	public Stats(PlayerStatistics playerStatistics)
	{
		PlayerStatistics = playerStatistics;
	}
}