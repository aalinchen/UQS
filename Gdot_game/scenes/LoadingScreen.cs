using Godot;

public partial class LoadingScreen : Control
{
	private ProgressBar bar;
	private AnimatedSprite2D elior;

	private string worldPath =
		"res://alex_mapUSW/szenes/Tutorial/Tutorial.tscn";

	private float loadProgress = 0f;
	private float loadSpeed = 25f;

	private float barStartX;
	private float barEndX;

	private bool sceneChanged = false;

	public override async void _Ready()
	{
		bar = GetNodeOrNull<ProgressBar>("ProgressBar");
		elior = GetNodeOrNull<AnimatedSprite2D>("Node2D/Elior");

		if (bar == null)
		{
			GD.PrintErr("❌ ProgressBar nicht gefunden");
			return;
		}

		if (elior == null)
		{
			GD.PrintErr("❌ Elior (AnimatedSprite2D) nicht gefunden");
			return;
		}

		bar.Value = 0;

		// 1 Frame warten, damit Control-Layout korrekt ist
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		// Start- & Endpunkt des Balkens
		barStartX = bar.GlobalPosition.X;
		barEndX = bar.GlobalPosition.X + bar.Size.X;

		// Elior an Start setzen
		elior.GlobalPosition = new Vector2(
			barStartX - 12f,
			elior.GlobalPosition.Y
		);

		// Laufanimation starten
		if (elior.SpriteFrames.HasAnimation("walk"))
			elior.Play("walk");
	}

	public override void _Process(double delta)
	{
		if (sceneChanged) // Szene wurde bereits gewechselt
			return;

		// Ladefortschritt
		loadProgress += loadSpeed * (float)delta;
		loadProgress = Mathf.Min(loadProgress, 100f);
		bar.Value = loadProgress;

		// Elior zieht den Balken
		float t = loadProgress / 100f;
		float targetX = Mathf.Lerp(barStartX, barEndX, t) + 12f; // +12f, damit Elior vor dem Balken läuft

		elior.GlobalPosition = new Vector2(
			Mathf.Lerp(elior.GlobalPosition.X, targetX, 0.2f),
			elior.GlobalPosition.Y
		);

		// Fertig → Szene wechseln
		if (loadProgress >= 100f)
		{
			sceneChanged = true;

			if (elior.SpriteFrames.HasAnimation("idle")) // Idle-Animation spielen
				elior.Play("idle");
			else
				elior.Stop();

			GetTree().ChangeSceneToFile(worldPath);
		}
	}
}
