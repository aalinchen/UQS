using Godot;
using System;

public partial class Player : CharacterBody2D // durch CharacterBody2D bekommt der Player automatisch Velocity, MoveAndSlide(), Kollision etc.
{                                          //parcial = teilweiser zugriff auf die klasse (z.b. auf _Ready() in der engine)
	// ─── INVENTORY
	public Inventory PlayerInventory { get; private set; } // wird in _Ready() initialisiert, damit der Player Zugriff auf das globale Inventar hat, wird nur im Player gesetzt
	public bool CanMove = true;
	// ─── CHEST
	private Chest nearbyChest; // private --> nur innerhalb der Player-Klasse zugänglich

	[Export] public PackedScene WeedCropScene; //werden später inizialisiert (über den Editor) -- sind Szenen 
	[Export] public PackedScene TomatoCropScene;
	private Field currentField;


	[Export] public float Speed = 50f;

	private AnimatedSprite2D sprite;
	private Vector2 lastDirection = Vector2.Down; // Standardrichtung nach unten und wichtig für Idle-Animation
	private bool isUsingTool = false; // verhindert, dass der Spieler sich während der Werkzeugbenutzung bewegt

	// ─── TOOLS
	public enum ToolType // Werkzeugtypen (feste Auswahl)
	{
		None,
		Chopping,
		Tilling,
		Watering,
		WeedSeed,
		TomatoSeed
	}

	private ToolType currentTool = ToolType.None;

	// ─── READY
	public override void _Ready() // wird aufgerufen, wenn die Szene geladen ist
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); //holt das Sprite-Kind aus der Szene
		sprite.AnimationFinished += OnAnimationFinished; // wenn die Animation endert wird diese Methode aufgerufen

		PlayerInventory = GetNode<GlobalInventory>("/root/GlobalInventory").PlayerInventory; //holt das globale Inventar, Inventar bleibt erhalten beim Szenenwechsel

	}

	// ─── PHYSICS
	public override void _PhysicsProcess(double delta) //Bewegungslogik, wird in jedem Physik-Frame aufgerufen
	{
		if (!CanMove) // verhindert Bewegung, wenn CanMove false ist
		{
			Velocity = Vector2.Zero;
			return;
		}
		
		if (isUsingTool) // verhindert Bewegung während Werkzeugbenutzung
			return;

		Vector2 direction = GetMovementInput();

		if (direction != Vector2.Zero) //merkt sich die letzte Richtung
			lastDirection = direction;

		Velocity = direction * Speed; //setzt die Geschwindigkeit (bewegt den Spieler)
		MoveAndSlide();

		UpdateMovementAnimation(direction); //spielt die passende Animation ab

		// Use tool via left mouse button (keeps previous behavior)
		if (Input.IsMouseButtonPressed(MouseButton.Left))
			UseTool();

		//  Ernten (example)
		if (Input.IsActionJustPressed("harvest"))
		{
			if (currentField != null)
				currentField.Harvest(PlayerInventory);
		}
	}

	// ─── MOVEMENT
	private Vector2 GetMovementInput() //Bewegungseingabe abfragen
	{
		Vector2 dir = Vector2.Zero;

		if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up)) dir.Y -= 1;
		if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down)) dir.Y += 1;
		if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left)) dir.X -= 1;
		if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right)) dir.X += 1;

		return dir.Normalized();
	}

	private void UpdateMovementAnimation(Vector2 dir) //spielt die passende Bewegungsanimation ab
	{
		if (dir == Vector2.Zero)
		{
			PlayIdle();
			return;
		}

		if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
			sprite.Play(dir.X > 0 ? "walk_right" : "walk_left");
		else
			sprite.Play(dir.Y > 0 ? "walk_front" : "walk_back");
	}

	private void PlayIdle()
	{
		if (Mathf.Abs(lastDirection.X) > Mathf.Abs(lastDirection.Y))
			sprite.Play(lastDirection.X > 0 ? "idle_right" : "idle_left");
		else
			sprite.Play(lastDirection.Y > 0 ? "idle_front" : "idle_back");
	}

	// ─── TOOLS
	private void UseTool() // Werkzeugbenutzung (Hauptmethode) 
	{
		if (currentTool == ToolType.None || isUsingTool) //wenn kein Werkzeug ausgewählt ist oder bereits benutzt wird, abbrechen
			return;

		bool isSeed = //unterscheidet zwischen Samen und anderen
			currentTool == ToolType.WeedSeed ||
			currentTool == ToolType.TomatoSeed;

		if (!isSeed) 
		{
			isUsingTool = true;
			Velocity = Vector2.Zero;
		}

		string dir = GetDirectionSuffix(); //bestimmt die Richtung für die Animation 

		// Play animation
		string anim = currentTool switch
		{
			ToolType.Chopping => $"chopping_{dir}",
			ToolType.Tilling => $"tilling_{dir}",
			ToolType.Watering => $"watering_{dir}",
			_ => ""
		};

		if (anim != "")
			sprite.Play(anim); //spielt die entsprechende Werkzeuganimation ab

		// Chopping logic
		if (currentTool == ToolType.Chopping)
		{
			// HitComponent holen
			var hitComponent = GetNodeOrNull<HitComponent>("HitComponent"); 
			if (hitComponent != null)
			{
				//Tool setzten - Info an Hitbox übergeben
				hitComponent.CurrentTool = Tools.Chopping;

				//Hit damage 
				hitComponent.HitDamage = 1;

				hitComponent.Swing(dir);
			}
			else
			{
				GD.PrintErr("HitComponent not found on Player - check the node path.");
			}
		}

		// Farming logic
		if (currentField != null)
		{
			switch (currentTool)
			{
				case ToolType.Tilling:
					currentField.Till();
					break;

				case ToolType.WeedSeed:
					PlantSeed("weed_seed", WeedCropScene);
					isUsingTool = false;
					break;

				case ToolType.TomatoSeed:
					PlantSeed("tomato_seed", TomatoCropScene);
					isUsingTool = false;
					break;

				case ToolType.Watering:
					currentField.Water();
					break;
			}
		}
	}

	private string GetDirectionSuffix()
	{
		if (Mathf.Abs(lastDirection.X) > Mathf.Abs(lastDirection.Y))
			return lastDirection.X > 0 ? "right" : "left";
		else
			return lastDirection.Y > 0 ? "front" : "back";
	}

	private void OnAnimationFinished()
	{
		// wenn die Animation fertig ist, kann der Spieler sich wieder bewegen
		isUsingTool = false;
	}

	// ─── UI → PLAYER
	public void SetToolByIndex(int index)
	{
		currentTool = index switch
		{
			1 => ToolType.Chopping,
			2 => ToolType.Tilling,
			3 => ToolType.Watering,
			4 => ToolType.WeedSeed,
			5 => ToolType.TomatoSeed,
			_ => ToolType.None
		};

		GD.Print("Player Tool gesetzt: " + currentTool);
	}

	public void SetCurrentField(Field field) => currentField = field;

	private void PlantSeed(string seedId, PackedScene cropScene)
	{
		if (!PlayerInventory.HasItem(seedId, 1)) //prüft ob der Spieler den Samen im Inventar hat
		{
			GD.Print("❌ No seeds left!");
			return;
		}

		if (currentField.State != Field.FieldState.Tilled) //prüft ob das Feld vorbereitet/bestellt ist
			return;

		currentField.Plant(cropScene); //pflanzt die Pflanze im Feld
		PlayerInventory.AddItem(seedId, -1); //entfernt einen Samen aus dem Inventar

	}

	public void SetNearbyChest(Chest chest) => nearbyChest = chest;
	public Chest GetNearbyChest() => nearbyChest;

	public override void _Input(InputEvent e)
	{
		if (e.IsActionPressed("interact")&& nearbyChest != null&& IsInstanceValid(nearbyChest)) //Interact mit Truhe mit E
		{
			UI.Instance.OpenChest(nearbyChest); //öffnet die Truhe im UI
		}
	}
	
	public override void _ExitTree() //Reset beim Verlassen der Szene
	{
		nearbyChest = null;
	}
}
