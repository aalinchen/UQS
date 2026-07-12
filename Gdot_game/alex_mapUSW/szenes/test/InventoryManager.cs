using Godot;

public partial class InventoryManager : Node
{
	// ItemID -> Menge
	public Godot.Collections.Dictionary<string, int> Items =
		new Godot.Collections.Dictionary<string, int>();

	// ─── GRUNDLAGEN ──────────────────────────

	public void AddItem(string itemId, int amount = 1) // Fügt dem Inventar einen Artikel hinzu
	{
		if (Items.ContainsKey(itemId)) // Wenn der Artikel/das Item bereits im Inventar ist
			Items[itemId] += amount; // Erhöhe die Menge
		else
			Items[itemId] = amount; // Füge den Artikel/das Item mit der angegebenen Menge hinzu

		GD.Print($"Item added: {itemId} x{amount}");
	}

	public bool RemoveItem(string itemId, int amount = 1) // Entfernt eine bestimmte Menge eines Artikels/vom Item aus dem Inventar
	{
		if (!Items.ContainsKey(itemId)) 
			return false;

		Items[itemId] -= amount;

		if (Items[itemId] <= 0)
			Items.Remove(itemId);

		GD.Print($"Item removed: {itemId} x{amount}");
		return true;
	}

	public bool HasItem(string itemId, int amount = 1) // Überprüft, ob das Inventar mindestens eine bestimmte Menge eines Artikels enthält
	{
		return Items.ContainsKey(itemId) && Items[itemId] >= amount;
	}

	public int GetItemCount(string itemId) // Gibt die Gesamtanzahl eines bestimmten Artikels im Inventar zurück
	{
		return Items.ContainsKey(itemId) ? Items[itemId] : 0;
	}

	public void ClearInventory() // Leert das gesamte Inventar
	{
		Items.Clear();
		GD.Print("Inventory cleared");
	}

	// ─── SAVE / LOAD ─────────────────────────

	public Godot.Collections.Dictionary<string, int> GetSaveData() // Gibt die Daten des Inventars zum Speichern zurück
	{
		return Items.Duplicate();
	}

	public void LoadSaveData(Godot.Collections.Dictionary<string, int> data) // Lädt die Inventardaten aus dem gespeicherten Spielstand
	{
		Items = data.Duplicate();
		GD.Print("Inventory loaded");
	}

	// ─── DEBUG / TEST ────────────────────────

	public void PrintInventory() // Gibt den aktuellen Inhalt des Inventars in der Konsole aus
	{
		GD.Print("=== INVENTORY ===");
		foreach (var item in Items)
			GD.Print($"{item.Key}: {item.Value}");
	}
}
