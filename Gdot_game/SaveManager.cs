using Godot;

public static class SaveManager
{
	private const string SAVE_PATH = "user://savegame.save";

	public static void SaveGame( // Spielstand speichern
		Vector2 playerPosition, // Spielerposition
		Godot.Collections.Dictionary<string, int> inventory // Inventar-Daten
	)
	{
		var saveData = new Godot.Collections.Dictionary // Spielstand-Daten
		{
			{ "day", GameManager.Instance.CurrentDay },
			{ "day_progress", GameManager.Instance.GetDayProgress() },
			{ "player_position", playerPosition },
			{ "inventory", inventory }
		};

		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write); // Datei zum Schreiben öffnen
		file.StoreVar(saveData);

		GD.Print("💾 Game saved");
	}

	public static Godot.Collections.Dictionary LoadGame() // Spielstand laden
	{
		if (!FileAccess.FileExists(SAVE_PATH)) // Überprüfen, ob die Speicherdatei existiert
		{
			GD.Print("No savegame found");
			return null;
		}

		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read); // Datei zum Lesen öffnen
		return (Godot.Collections.Dictionary)file.GetVar();
	}
	public static bool HasSave() // Überprüfen, ob ein Spielstand existiert
	{
		return FileAccess.FileExists("user://savegame.save");
	}
}
