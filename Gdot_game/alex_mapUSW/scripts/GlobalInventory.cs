using Godot;

public partial class GlobalInventory : Node
{
	public Inventory PlayerInventory { get; private set; }
	private bool initialized = false;

	public override void _Ready() // wird aufgerufen, wenn die Szene geladen ist
	{
		if (initialized)
			return;

		PlayerInventory = new Inventory();
		AddChild(PlayerInventory); // ⭐ extrem wichtig

		PlayerInventory.AddItemAtStart("weed_seed", 10);
		PlayerInventory.AddItemAtStart("tomato_seed", 10);

		initialized = true;

		GD.Print("GlobalInventory Ready - Slots:");
		GD.Print(PlayerInventory.GetItemCount("weed_seed"));
		GD.Print(PlayerInventory.GetItemCount("tomato_seed"));
	}
}
