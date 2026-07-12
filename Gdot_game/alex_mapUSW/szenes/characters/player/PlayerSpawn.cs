using Godot;

public partial class PlayerSpawn : Node
{
	public override void _Ready()
	{
		CallDeferred(nameof(PlacePlayer)); // Verzögertes Platzieren des Spielers
    }

	private void PlacePlayer() // Spieler an Spawnpunkt platzieren
    {
		if (GameState.SpawnPointName == "")
			return;

		var spawn = GetTree().CurrentScene
			.GetNodeOrNull<Marker2D>(GameState.SpawnPointName);

		if (spawn == null)
			return;

		var player = GetTree().CurrentScene
			.GetNodeOrNull<CharacterBody2D>("Player");

		if (player == null)
			return;

		player.GlobalPosition = spawn.GlobalPosition;
		player.Velocity = Vector2.Zero;

		GameState.SpawnPointName = "";
	}
}
