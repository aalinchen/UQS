using Godot;

public partial class Item : Area2D
{
	[Export] public string ItemId = "logs";
	[Export] public int Amount = 1;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered; // Signal verbinden
	}

	private void OnBodyEntered(Node body) // Wird aufgerufen, wenn ein Körper den Bereich betritt
	{
		if (body is not Player) // Nur auf Spieler reagieren
			return;

		var inventory = GetNode<GlobalInventory>("/root/GlobalInventory").PlayerInventory; // Inventar abrufen

		if (inventory == null)
		{
			GD.PrintErr("❌ GlobalInventory.PlayerInventory ist NULL!");
			return;
		}

		inventory.AddItem(ItemId, Amount); // Item zum Inventar hinzufügen

		GD.Print($"✅ {ItemId} collected! Total: {inventory.GetItemCount(ItemId)}"); // Erfolgsmeldung ausgeben

		
		CallDeferred(Node.MethodName.QueueFree);// Item entfernen
	}
}
