using Godot;

public partial class InventorySlotButton : Button
{
	[Export] public int SlotIndex;
	[Export] public bool IsChestSlot = true;

	private TextureRect icon;
	private Label amountLabel;

	public override void _Ready() 
	{
		icon = GetNode<TextureRect>("TextureRect"); // Pfad zum Icon-TextureRect
		amountLabel = GetNode<Label>("Label");

		ButtonDown += OnButtonDown; // Signal für Button gedrückt
		ButtonUp += OnButtonUp; // Signal für Button losgelassen
	}

	private void OnButtonDown() // Wird aufgerufen, wenn der Button gedrückt wird
	{
		UI ui = UI.Instance;
		if (ui == null)
			return;

		var slot = GetSlot(); // Holt den entsprechenden Inventar-Slot
		if (slot == null || slot.IsEmpty)
			return;

		ui.DraggedSlot = slot; // Setzt den gezogenen Slot im UI
		ui.DraggedFromIndex = SlotIndex;

		ui.DragIcon.Texture = icon.Texture;
		ui.DragIcon.Visible = true;
	}

	private void OnButtonUp() // Wird aufgerufen, wenn der Button losgelassen wird
	{
		UI ui = UI.Instance;
		if (ui == null || ui.DraggedSlot == null)
			return;

		var targetSlot = GetSlot();
		if (targetSlot == null)
		{
			ui.CancelDrag();
			return;
		}

		// Tauschen
		(targetSlot.ItemId, ui.DraggedSlot.ItemId) =
			(ui.DraggedSlot.ItemId, targetSlot.ItemId);

		(targetSlot.Amount, ui.DraggedSlot.Amount) =
			(ui.DraggedSlot.Amount, targetSlot.Amount);

		ui.EndDrag();
	}

	private InventorySlot GetSlot() // Holt den entsprechenden Inventar-Slot basierend auf dem SlotIndex und ob es sich um eine Truhe handelt
	{
		UI ui = UI.Instance; // Singleton-Instanz des UI
		if (ui == null) // Überprüfung, ob die UI-Instanz null ist
			return null;

		if (IsChestSlot) // Wenn es sich um einen Truhen-Slot handelt
		{
			var chest = ui.CurrentChest;
			if (chest == null)
				return null;

			var inv = chest.GetNode<ChestInventory>("ChestInventory");
			return inv.Slots[SlotIndex];
		}
		else // Wenn es sich um einen Spieler-Inventar-Slot handelt
		{
			return GetNode<GlobalInventory>("/root/GlobalInventory").PlayerInventory.GetSlot(SlotIndex);
		}
	}

}
