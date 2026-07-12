using Godot;

public partial class MiniMapCamera : Camera2D
{
	private Node2D player;

	public override void _Ready()
	{
		GD.Print("MiniMapCamera World2D ID: ", GetWorld2D().GetInstanceId());
		player = GetTree().GetFirstNodeInGroup("Player") as Node2D;

		if (player == null)
			GD.PrintErr("❌ Player NICHT gefunden");
		else
			GD.Print("✅ Player gefunden");
	}

	public override void _Process(double delta) // wird jeden Frame aufgerufen
	{
		
		if (player != null)
			GlobalPosition = player.GlobalPosition; // Kamera folgt der Spielerposition

	}
}
