public class InventorySlot
{
	public string ItemId = "";
	public int Amount = 0;

	public bool IsEmpty => ItemId == "" || Amount <= 0; // Überprüft, ob der Slot leer ist

	public void Clear() // Leert den Slot
	{
		ItemId = "";
		Amount = 0;
	}
}
