using Godot;
using System;

public partial class HitComponent : Area2D
{
	// ─── TOOL & DAMAGE
	[Export]
	public Tools CurrentTool { get; set; } = Tools.None;

	[Export]
	public int HitDamage { get; set; } = 1;

	// ─── HITBOX OFFSETS
	[Export] public Vector2 OffsetFront = new Vector2(0, 16);
	[Export] public Vector2 OffsetBack = new Vector2(0, -16);
	[Export] public Vector2 OffsetLeft = new Vector2(-16, 0);
	[Export] public Vector2 OffsetRight = new Vector2(16, 0);

	private CollisionShape2D collisionShape;

	public override void _Ready()
	{
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		// Start disabled / not monitoring so it doesn't trigger accidentally
		if (collisionShape != null)
			collisionShape.Disabled = true;

		Monitoring = false;
		Hide();
	}

	/// <summary>
	/// Move the hitbox based on player swing direction and activate it briefly
	/// </summary>
	public void Swing(string direction)
	{
		Vector2 offset = direction switch
		{
			"front" => OffsetFront,
			"back" => OffsetBack,
			"left" => OffsetLeft,
			"right" => OffsetRight,
			_ => Vector2.Zero
		};

		Position = offset;

		// Activate area monitoring + collision shape so AreaEntered fires
		Show();
		Monitoring = true;
		if (collisionShape != null)
			collisionShape.Disabled = false;

		GD.Print($"[HitComponent] Swing at {direction} (Tool={CurrentTool}, Damage={HitDamage}). Position={GlobalPosition + Position}");

		// Disable after 0.2 seconds
		GetTree().CreateTimer(0.2).Timeout += () =>
		{
			// ensure we disable monitoring to avoid lingering collisions
			Monitoring = false;
			if (collisionShape != null)
				collisionShape.Disabled = true;
			Hide();
			GD.Print("[HitComponent] Swing ended");
		};
	}
}
