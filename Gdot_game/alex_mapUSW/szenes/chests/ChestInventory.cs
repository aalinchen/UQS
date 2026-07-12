using Godot;
using System.Collections.Generic;

public partial class ChestInventory : Node
{
	[Export] public string ChestId;
	[Export] public int SlotCount = 26;

	public List<InventorySlot> Slots; // Referenz auf die Inventarplätze der Truhe

	public override void _Ready()
	{
		if (GameState.Instance == null) // Sicherheitsabfrage
		{
			GD.PrintErr("❌ GameState.Instance ist NULL");
			return;
		}

		if (string.IsNullOrEmpty(ChestId))
		{
			GD.PrintErr("❌ ChestInventory ohne ChestId gefunden! Szene: " + GetPath());
			return;
		}

		if (!GameState.Instance.Chests.ContainsKey(ChestId)) // Truhe noch nicht im GameState vorhanden
		{
			var list = new List<InventorySlot>(); // Neue Liste für die Inventarplätze der Truhe erstellen
			for (int i = 0; i < SlotCount; i++) // Inventarplätze entsprechend SlotCount hinzufügen
				list.Add(new InventorySlot()); // Neuer leerer Inventarplatz

			GameState.Instance.Chests[ChestId] = list; // Truhe mit ihren Inventarplätzen zum GameState hinzufügen
		}

		Slots = GameState.Instance.Chests[ChestId]; // Referenz zu den Inventarplätzen der Truhe im GameState
	}
}
