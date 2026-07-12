using Godot;

public partial class Field : Area2D
{
	public enum FieldState // Feldzustände
	{
		Normal,
		Tilled,
		Planted
	}

	[Export] public Texture2D NormalTexture;
	[Export] public Texture2D TilledTexture;

	private Sprite2D sprite;
	public FieldState State = FieldState.Normal; // aktueller Zustand des Feldes

	private Crop currentCrop;
	private Label harvestLabel;
	private bool isWatered = false;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D"); 
		UpdateVisual();

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		
		harvestLabel = GetNode<Label>("Label");
		harvestLabel.Visible = false;
		
		AddToGroup("Fields");

	}

	// Hacken
	public void Till()
	{
		if (State != FieldState.Normal)
			return;

		State = FieldState.Tilled;
		UpdateVisual();
	}

	// Pflanzen
	public void Plant(PackedScene cropScene)
	{
		if (State != FieldState.Tilled) // nur bepflanzen, wenn der Zustand "Gehackt" ist
			return;

		currentCrop = cropScene.Instantiate<Crop>();
		if (currentCrop == null)
		{
			GD.PrintErr("❌ Failed to instantiate crop!");
			return;
		}

		AddChild(currentCrop); 
		currentCrop.Position = Vector2.Zero;

		State = FieldState.Planted; // Zustand auf "Bepflanzt" setzen
		UpdateHarvestHint();
	}

	// Ernten
	public void Harvest(Inventory inventory)
	{
		if (State != FieldState.Planted)
			return;

		if (currentCrop == null || !currentCrop.CanHarvest())
			return;

		// Crop kümmert sich um Inventar + QueueFree
		currentCrop.Harvest(inventory);

		currentCrop = null;
		State = FieldState.Tilled;
		UpdateVisual();
		UpdateHarvestHint();
		
		// Feld zurück auf normalen Boden
		State = FieldState.Normal;
		UpdateVisual();
	}

	private void UpdateVisual() 
	{
		sprite.Texture = State == FieldState.Tilled
			? TilledTexture
			: NormalTexture;
	}

	// Player erkennen
	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.SetCurrentField(this);
			GD.Print("🟩 Player entered field");
			UpdateHarvestHint();
		}
	}

	private void OnBodyExited(Node body) 
	{
		if (body is Player player)
		{
			player.SetCurrentField(null);
			GD.Print("⬜ Player left field");
		}
	}
	
	private void UpdateHarvestHint()  // Ernte-Hinweis aktualisieren
	{
		if (currentCrop != null && currentCrop.CanHarvest()) // wenn die Pflanze erntereif ist
			harvestLabel.Visible = true;
		else 
			harvestLabel.Visible = false; // sonst ausblenden
	}
	public void Water()
	{
		if (State == FieldState.Planted) // nur gießen, wenn eine Pflanze gepflanzt ist
		{
			isWatered = true;
			currentCrop.Grow(); // sofort sichtbar
			GD.Print("💧 Field watered + instant grow");
		}
		else
		{
			GD.Print("❌ Water failed - no planted crop");
		}
	}
	
	public void OnNewDay() // wird vom GameManager aufgerufen
	{
		if (currentCrop == null)
			return;

		currentCrop.Grow(); // normales Wachstum
		currentCrop.Grow(); // extra Wachstum

		if (isWatered) // zusätzliches Wachstum, wenn gegossen
		{
			currentCrop.Grow();
			currentCrop.Grow(); // extra Wachstum
			isWatered = false; // zurücksetzen für den nächsten Tag
		}
	}
}
