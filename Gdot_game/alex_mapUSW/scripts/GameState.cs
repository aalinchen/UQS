using Godot;
using System.Collections.Generic;

public partial class GameState : Node
{
	public static GameState Instance; 

	public Dictionary<string, List<InventorySlot>> Chests = new();

	public bool TutorialDone = false;
	public static string SpawnPointName = "";

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadGameState() // Lädt den Spielstand und setzt Spielerposition und Inventar
	{
		var data = SaveManager.LoadGame();
		if (data == null)
			return;

		var player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		if (player != null)
			player.GlobalPosition = (Vector2)data["player_position"];

		var inventory = GetTree().GetFirstNodeInGroup("Inventory") as InventoryManager;
		if (inventory != null)
			inventory.Items = (Godot.Collections.Dictionary<string, int>)data["inventory"];

		GD.Print("Game loaded");
	}
}
