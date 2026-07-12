using Godot;

public partial class SceneDoor : Area2D
{
	[Export] public string TargetScenePath;
	[Export] public string TargetSpawnPoint;

	private bool _used = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body) 
	{
		if (_used) // <- Tür nur einmal benutzen
			return;

		// ✅ NUR der Player darf Türen benutzen
		if (body is Player)
		{
			if (string.IsNullOrEmpty(TargetSpawnPoint)) // <- SpawnPoint MUSS gesetzt sein
			{
				GD.PrintErr("❌ TargetSpawnPoint ist leer!");
				return;
			}

			if (string.IsNullOrEmpty(TargetScenePath)) // <- TargetScenePath MUSS gesetzt sein
			{
				GD.PrintErr("❌ TargetScenePath ist leer!");
				return;
			}

			_used = true;

			GD.Print($"🚪 Door → {TargetScenePath}, Spawn: {TargetSpawnPoint}"); // Debug-Ausgabe für Türwechsel

			GameState.SpawnPointName = TargetSpawnPoint; // <- Setze den SpawnPoint im GameState
			CallDeferred(nameof(ChangeScene));
		}
	}

	private void ChangeScene() // Szene wechseln
	{
		GetTree().ChangeSceneToFile(TargetScenePath);
	}
}
