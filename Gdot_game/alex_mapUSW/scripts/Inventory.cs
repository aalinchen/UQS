using Godot;

public partial class Inventory : Node
{
	[Signal]
	public delegate void InventoryChangedEventHandler(); // Signal, das ausgelöst wird, wenn sich das Inventar ändert

	[Export] public int Size = 26;
	public InventorySlot[] Slots;

	public override void _Ready()
	{
		Slots = new InventorySlot[Size];
		for (int i = 0; i < Size; i++)
			Slots[i] = new InventorySlot();
	}

	public InventorySlot GetSlot(int index) // Gibt den Inventarplatz an der angegebenen Indexposition zurück
	{
		if (index < 0 || index >= Slots.Length)
			return null;

		return Slots[index];
	}

	public void AddItem(string itemId, int amount = 1) // Fügt dem Inventar einen Artikel hinzu
	{
		if (amount <= 0)
			return;

		foreach (var slot in Slots)
		{
			if (!slot.IsEmpty && slot.ItemId == itemId)
			{
				slot.Amount += amount;
				EmitSignal(nameof(InventoryChanged));
				return;
			}
		}

		foreach (var slot in Slots) // Sucht nach einem leeren Slot
		{
			if (slot.IsEmpty)
			{
				slot.ItemId = itemId;
				slot.Amount = amount;
				EmitSignal(nameof(InventoryChanged));
				return;
			}
		}
	}
	
	public void AddItemAtStart(string itemId, int amount) // Fügt dem Inventar einen Artikel am Anfang hinzu
	{
		if (string.IsNullOrEmpty(itemId) || amount <= 0)
			return;

		for (int i = 0; i < Slots.Length; i++)
		{
			if (Slots[i].IsEmpty)
			{
				Slots[i].ItemId = itemId;
				Slots[i].Amount = amount;
				EmitSignal(nameof(InventoryChanged));
				return;
			}
		}
	}
	

	public int GetItemCount(string itemId) // Gibt die Gesamtanzahl eines bestimmten Artikels im Inventar zurück
	{
		int total = 0;
		foreach (var slot in Slots)
		{
			if (slot.ItemId == itemId)
				total += slot.Amount;
		}
		return total;
	}

	public bool HasItem(string itemId, int amount = 1) // Überprüft, ob das Inventar mindestens eine bestimmte Menge eines Artikels enthält
	{
		int total = 0;
		foreach (var slot in Slots)
		{
			if (!slot.IsEmpty && slot.ItemId == itemId)
				total += slot.Amount;

			if (total >= amount)
				return true;
		}
		return false;
	}

	public void RemoveItem(string itemId, int amount) // Entfernt eine bestimmte Menge eines Artikels aus dem Inventar
	{
		if (string.IsNullOrEmpty(itemId) || amount <= 0)
			return;

		for (int i = 0; i < Slots.Length; i++) // Durchläuft alle Slots im Inventar
		{
			var slot = Slots[i];
			if (slot.IsEmpty || slot.ItemId != itemId)
				continue;

			if (slot.Amount > amount) // Wenn der Slot mehr als die zu entfernende Menge enthält
			{
				slot.Amount -= amount;
				EmitSignal(nameof(InventoryChanged));
				return;
			}
			else // Wenn der Slot weniger oder genau die zu entfernende Menge enthält
			{
				amount -= slot.Amount;
				slot.Clear();

				if (amount <= 0) // Wenn die gesamte Menge entfernt wurde
				{
					EmitSignal(nameof(InventoryChanged));
					return;
				}
			}
		}
	}
}
