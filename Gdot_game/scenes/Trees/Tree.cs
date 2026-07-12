using Godot;

public partial class Tree : Node2D
{
	public override async void _Ready()
	{
		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		while (true)
		{
			// Warten (zufällig)
			await ToSignal(GetTree().CreateTimer(
				GD.RandRange(4f, 10f)), "timeout");

			// Animation abspielen
			sprite.Play("idle_tree");

			// Dauer der Animation
			await ToSignal(GetTree().CreateTimer(2.5f), "timeout");

			// Stoppen
			sprite.Stop();
			sprite.Frame = 0;
		}
	}
}
