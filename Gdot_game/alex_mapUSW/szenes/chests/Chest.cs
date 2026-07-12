using Godot;

public partial class Chest : Area2D
{
	[Export] public string ChestId;
	[Export] public Control InteractHint;

	public override void _Ready() // Wird aufgerufen, wenn die Node dem Szenenbaum hinzugefügt wird
	{
		if (string.IsNullOrEmpty(ChestId)) // Überprüfung, ob die ChestId leer ist
		{
			GD.PrintErr("❌ Chest ohne ChestId!");
			return;
		}

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

		if (InteractHint != null)
			InteractHint.Visible = false;
	}

	private void OnBodyEntered(Node body) // Wird aufgerufen, wenn der Spieler den Bereich betritt
	{
		if (body is Player player) // Überprüfung, ob der Körper ein Spieler ist
		{
			if (InteractHint != null) // Überprüfung, ob der InteractHint nicht null ist
				InteractHint.Visible = true;

			player.SetNearbyChest(this); // Setzt die Referenz zur Truhe im Spieler
		}
	}

	private void OnBodyExited(Node body) // Wird aufgerufen, wenn der Spieler den Bereich verlässt
	{
		if (body is Player player) 
		{
			if (InteractHint != null) 
				InteractHint.Visible = false;

			player.SetNearbyChest(null); // Entfernt die Referenz zur Truhe im Spieler
			UI.Instance?.CallDeferred(nameof(UI.CloseChest)); // Schließt die Truhe im UI
		}
	}
}
