using Godot;

public partial class WorldItem : Area2D
{
	[Export] public string ItemId = "logs";
	[Export] public int Amount = 1;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body) // Wird aufgerufen, wenn ein Körper den Bereich betritt
	{
		if (body is not Player)
			return;

		// Globales Inventar holen
		var inventory = GetNode<GlobalInventory>("/root/GlobalInventory").PlayerInventory;

		if (inventory == null)
		{
			GD.PrintErr("❌ GlobalInventory.PlayerInventory ist NULL!");
			return;
		}

		// Item hinzufügen
		inventory.AddItem(ItemId, Amount);

		GD.Print($"✅ {ItemId} collected! Total: {inventory.GetItemCount(ItemId)}");

		// Physics-sicher löschen
		CallDeferred(Node.MethodName.QueueFree);
	}
}
