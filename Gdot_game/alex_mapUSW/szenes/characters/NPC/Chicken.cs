using Godot;
using System;

public partial class Chicken : CharacterBody2D
{
	// ✅ NUR EIN sprite, per Inspector gesetzt
	[Export] private AnimatedSprite2D sprite;

	// Bewegung
	[Export] public float Speed = 40f;

	// Eier
	[Export] public PackedScene EggScene;
	[Export] public float MinEggTime = 10f;
	[Export] public float MaxEggTime = 20f;

	private Vector2 direction = Vector2.Zero;
	private float changeTime = 0f;

	private Timer eggTimer;

	public override void _Ready()
	{
		// 🔐 Sicher abspielen
		PlayAnim("idle");

		// Timer erstellen
		eggTimer = new Timer();
		eggTimer.OneShot = true;
		AddChild(eggTimer);

		eggTimer.Timeout += OnEggTimerTimeout;
		StartEggTimer();
	}

	public override void _PhysicsProcess(double delta)
	{
		changeTime -= (float)delta;

		if (changeTime <= 0f)
			ChooseNewDirection();

		Velocity = direction * Speed;
		MoveAndSlide();

		if (GetSlideCollisionCount() > 0)
			ChooseNewDirection();

		if (direction.X != 0 && sprite != null)
			sprite.FlipH = direction.X < 0;
	}

	private void ChooseNewDirection()
	{
		changeTime = (float)GD.RandRange(1.0, 3.0);

		if (GD.Randf() < 0.3f)
		{
			direction = Vector2.Zero;
			PlayAnim("idle");
		}
		else
		{
			direction = new Vector2(
				(float)GD.RandRange(-1.0, 1.0),
				(float)GD.RandRange(-1.0, 1.0)
			).Normalized();

			PlayAnim("walk");
		}
	}

	// 🥚 TIMER LOGIK
	private void StartEggTimer()
	{
		eggTimer.WaitTime = (float)GD.RandRange(MinEggTime, MaxEggTime);
		eggTimer.Start();
	}

	private void OnEggTimerTimeout()
	{
		SpawnEgg();
		StartEggTimer();
	}

	private void SpawnEgg()
	{
		if (EggScene == null)
		{
			GD.PrintErr("EggScene not assigned!");
			return;
		}

		Node2D egg = EggScene.Instantiate<Node2D>();
		egg.GlobalPosition = GlobalPosition + new Vector2(0, 12);
		GetParent().AddChild(egg);

		GD.Print("🥚 Egg dropped!");
	}

	// 🔐 Sichere Animationsmethode
	private void PlayAnim(string name)
	{
		if (sprite == null || sprite.SpriteFrames == null)
			return;

		if (!sprite.SpriteFrames.HasAnimation(name))
			return;

		if (sprite.Animation != name)
			sprite.Play(name);
	}
}
