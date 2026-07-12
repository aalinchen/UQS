using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance;

	[Export] public float DayLength = 600f; // Sekunden pro Tag (10 Minuten)
	[Export] public int StartHour = 6; // Spiel startet um 6:00
	public int CurrentDay { get; private set; } = 1;

	private float currentTime = 0f; // 0 → DayLength

	public override void _Ready()
	{
		Instance = this;
		// ⏰ Startzeit setzen (z. B. 6:00)
		currentTime = (StartHour / 24f) * DayLength;
		
		var player = GetTree().GetFirstNodeInGroup("Player") as Player;
		var spawn = GetTree().GetFirstNodeInGroup("PlayerSpawn") as Node2D;

		if (player == null)
		{
			GD.PrintErr("❌ Player nicht gefunden");
			return;
		}

		// WICHTIG: Nur setzen, wenn KEIN Save geladen wird
		if (!SaveManager.HasSave() && spawn != null)
		{
			player.GlobalPosition = spawn.GlobalPosition;
		}
		
	}

	public override void _Process(double delta)
	{
		currentTime += (float)delta;

		if (currentTime >= DayLength)
		{
			currentTime = 0f;
			NextDay();
		}
	}

	// Neuer Tag
	private void NextDay()
	{
		CurrentDay++;
		GD.Print("🌅 Neuer Tag: " + CurrentDay);

		NotifyFieldsNewDay();
	}

	// Felder informieren
	private void NotifyFieldsNewDay()
	{
		foreach (Node node in GetTree().GetNodesInGroup("Fields"))
		{
			if (node is Field field)
				field.OnNewDay();
		}
	}

	// 0.0 → 1.0 (für Effekte)
	public float GetDayProgress()
	{
		return currentTime / DayLength;
	}

	// Uhrzeit als Text (z. B. 13:45)
	public string GetTimeString()
	{
		float dayPercent = GetDayProgress();
		float hours = dayPercent * 24f;

		int h = Mathf.FloorToInt(hours);
		int m = Mathf.FloorToInt((hours - h) * 60f);

		return $"{h:00}:{m:00}";
	}
	
	
	
	
}
