using Godot;

public partial class Crop : Node2D
{
	[Export] public string CropItemId;
	[Export] public string SeedItemId;
	[Export] public Texture2D[] GrowthStages;

	private int stage = 0;
	private Sprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D"); //holt das Sprite-Kind aus der Szene

		if (GrowthStages == null || GrowthStages.Length == 0) 
		{
			GD.PrintErr("❌ Crop has no GrowthStages!");
			return;
		}

		sprite.Texture = GrowthStages[stage]; //setzt das Anfangsbild


	}

	

	public void Grow() //wird aufgerufen, um die Pflanze wachsen zu lassen
	{
		if (CanHarvest())
			return;

		stage++;
		if (stage >= GrowthStages.Length) //stellt sicher, dass das Stadium nicht über das Maximum hinausgeht
			stage = GrowthStages.Length - 1; //setzt das Stadium auf das Maximum, wenn es überschritten wird

		sprite.Texture = GrowthStages[stage]; //setzt das Bild entsprechend dem Wachstumsstadium
	}

	public bool CanHarvest() //überprüft, ob die Pflanze erntereif ist
	{
		return stage == GrowthStages.Length - 1; //die Pflanze ist erntereif, wenn sie das letzte Wachstumsstadium erreicht hat
	}

	public void Harvest(Inventory inventory) //wird aufgerufen, um die Pflanze zu ernten
	{
		if (!CanHarvest())
			return;

		inventory.AddItem(CropItemId, 1); //fügt dem Inventar das geerntete Produkt hinzu
		inventory.AddItem(SeedItemId, 1); 

		if (GD.Randf() < 0.25f) //25% Chance, einen zusätzlichen Samen zu erhalten :)
			inventory.AddItem(SeedItemId, 1);

		QueueFree(); //entfernt die Pflanze aus der Szene nach der Ernte
	}
}
