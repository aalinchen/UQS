using Godot;
using System.Collections.Generic;

public partial class GlobalChestManager : Node
{
	private Dictionary<string, Inventory> chests = new(); // Schlüssel: ChestId, Wert: Inventory der Truhe 

	public Inventory GetChestInventory(string chestId) // Methode zum Abrufen des Inventars einer Truhe anhand ihrer ID
	{
		if (!chests.ContainsKey(chestId)) // Wenn die Truhe noch nicht existiert, erstelle ein neues Inventar
			chests[chestId] = new Inventory(); // Neues Inventar für die Truhe erstellen

		return chests[chestId]; // Gibt das Inventar der Truhe zurück
	}
}
