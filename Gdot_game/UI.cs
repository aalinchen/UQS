using Godot;

public partial class UI : CanvasLayer //CanvasLayer = UI Ebene über der Spielwelt
{
	// ─── MINIMAP ───────────────────────────
	[Export] public SubViewport MiniMapViewport;

	// ─── AUDIO ─────────────────────────────
	[Export] public HSlider MusicSlider;
	[Export] public HSlider SfxSlider;
	[Export] public Button BackFromSettingsButton;

	// ─── ZEIT & TAG ────────────────────────
	[Export] public Label DayLabel;
	[Export] public Label TimeLabel;

	// ─── PANELS ────────────────────────────
	[Export] public Control InventoryPanel;
	[Export] public Control PauseMenu;
	[Export] public Control SettingsMenu;

	// ─── TOOLS ─────────────────────────────
	[Export] public Button EmptyTool;
	[Export] public Button AxeButton;
	[Export] public Button TillingButton;
	[Export] public Button WateringButton;
	[Export] public Button CornSeedButton;
	[Export] public Button TomatoSeedButton;
	
	// ─── INVENTORY LABELS ───────────────────
	private Label logsLabel;
	private Label stoneLabel;
	private Label weedLabel;
	private Label tomatoLabel;
	private Label eggLabel;
	private Label milkLabel; 
	private Label weedSeedLabel;
	private Label tomatoSeedLabel;

	private Button[] toolButtons;
	private int currentToolIndex = 0;
	private Inventory inventory;


	
	
	// Chests 
	private Player player;
	private Chest currentChest;
	public static UI Instance; // Singleton-Instanz für globalen Zugriff (damit Player UI öffnen kann)
	private int selectedSlotIndex = -1;
	private string selectedPlayerItemId = null;
	[Export] public Control InteractHint; // damit der Hint angezeigt werden kann



	private Control chestInventoryPanel;
	[Export] public Texture2D woodIcon;
	[Export] public Texture2D stoneIcon;
	[Export] public Texture2D weedIcon;
	[Export] public Texture2D tomatoIcon;
	[Export] public Texture2D eggIcon;
	[Export] public Texture2D milkIcon;
	[Export] public Texture2D weedSeedIcon;
	[Export] public Texture2D tomatoSeedIcon;

	// ─── Drag & Drop ───────────────────────── funktioniert noch nicht komplett
	public InventorySlot DraggedSlot = null;
	public int DraggedFromIndex = -1;

	[Export] public TextureRect DragIcon;

	// ─── Zugriff für SlotButtons ─────────────andere Klassen können darauf zugreifen, sie aber nicht verändern
	public Chest CurrentChest => currentChest;
	public Player Player => player;
	

	// ───────────────────────── READY ─────────────────────────

	public override void _Ready()
	{
		Instance = this; // Singleton-Instanz setzen
		inventory = GetNode<GlobalInventory>("/root/GlobalInventory").PlayerInventory;

		if (inventory == null)
			GD.PrintErr("❌ GlobalInventory.PlayerInventory ist NULL");
		else
			GD.Print("✅ UI hat GlobalInventory");
			
			
		GD.Print("Seeds Weed:", inventory.GetItemCount("weed_seed"));
		GD.Print("Seeds Tomato:", inventory.GetItemCount("tomato_seed"));

			
		
		player = GetTree().GetFirstNodeInGroup("Player") as Player; // Player aus Gruppe holen
		if (player == null)
			GD.PrintErr("❌ Player nicht gefunden (Group 'Player')");

		if (InventoryPanel != null) InventoryPanel.Visible = false;
		if (PauseMenu != null) PauseMenu.Visible = false;
		if (SettingsMenu != null) SettingsMenu.Visible = false;

		toolButtons = new Button[] //sammelt die Tool Buttons in einem Array, damit man sie gemeinsam steuern kann
		{
			EmptyTool,
			AxeButton,
			TillingButton,
			WateringButton,
			CornSeedButton,
			TomatoSeedButton
		};

		// Tool Buttons klickbar machen
		for (int i = 0; i < toolButtons.Length; i++) //jeder Button hat einen Index, aktualiesierte Anzeige und informiert den Player
		{
			int index = i;
			if (toolButtons[i] != null)
			{
				toolButtons[i].Pressed += () =>
				{
					currentToolIndex = index;
					UpdateToolVisuals();
				};
			}
		}

		if (MusicSlider != null)
			MusicSlider.ValueChanged += OnMusicVolumeChanged;

		if (SfxSlider != null)
			SfxSlider.ValueChanged += OnSfxVolumeChanged;

		if (BackFromSettingsButton != null)
			BackFromSettingsButton.Pressed += _on_back_button_pressed;

		UpdateToolVisuals();

		if (MiniMapViewport != null) //Minimap initialisieren / verbinden mit der Welt
			MiniMapViewport.World2D = GetTree().Root.GetWorld2D();
			
		// Inventory Labels holen
		logsLabel   = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Logs/logsLabel");
		stoneLabel  = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Stone/stoneLabel");
		weedLabel   = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Weed/weedLabel");
		tomatoLabel = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Tomato/tomatoLabel");
		eggLabel    = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Egg/eggLabel");
		milkLabel = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/Milk/milkLabel");
		weedSeedLabel = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/weedSeed/weedSeedLabel");
		tomatoSeedLabel = GetNode<Label>("InventoryPanel/MarginContainer/GridContainer/tomatoSeed/tomatoSeedLabel");

		// Inventar-Signal verbinden
		if (inventory != null)
		{
			inventory.InventoryChanged += UpdateInventoryUI;
			UpdateInventoryUI(); // initialer Refresh
		}
		
		//Chest
		
		chestInventoryPanel = GetNode<Control>("ChestInventoryPanel");
		
		for (int i = 0; i < 26; i++)
		{
			int index = i;
			string path = $"ChestInventoryPanel/MarginContainer/PanelContainer/VBoxContainer/MarginContainer/GridContainer/SlotButton_{i}";
			Button btn = GetNode<Button>(path);

			btn.Pressed += () => OnChestSlotPressed(index);
		}
		
		
		// Logs Slot
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Logs/ClickButton").Pressed += () => SelectPlayerItem("logs");

		// Stone Slot
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Stone/ClickButton").Pressed += () => SelectPlayerItem("stone");

		// Weed Slot
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Weed/ClickButton").Pressed += () => SelectPlayerItem("weed");

		// Seeds
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/weedSeed/ClickButton").Pressed += () => SelectPlayerItem("weed_seed");

		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/tomatoSeed/ClickButton").Pressed += () => SelectPlayerItem("tomato_seed");
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Milk/ClickButton").Pressed += () => SelectPlayerItem("milk");
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Egg/ClickButton").Pressed += () => SelectPlayerItem("egg");
		GetNode<Button>("InventoryPanel/MarginContainer/GridContainer/Tomato/ClickButton").Pressed += () => SelectPlayerItem("tomato");
		

		
			
	}

	// ───────────────────────── PROCESS ─────────────────────────

	public override void _Process(double delta) // aktualisiert Zeit & Tag, Drag Icon Position jeden Frame
	{
		if (GameManager.Instance == null)
			return;

		if (DayLabel != null)
			DayLabel.Text = $"DAY {GameManager.Instance.CurrentDay}";

		if (TimeLabel != null)
			TimeLabel.Text = GameManager.Instance.GetTimeString();
			
		if (DragIcon != null && DragIcon.Visible)
		{
			DragIcon.GlobalPosition = GetViewport().GetMousePosition();
		}
	}

	// ───────────────────────── INPUT ─────────────────────────

	public override void _Input(InputEvent e)
	{
		// Chest schließen (E ODER ESC)
		if (currentChest != null)
		{
			if (e.IsActionPressed("interact") || e.IsActionPressed("ui_cancel"))
			{
				CloseChest();
				return; // ganz wichtig
			}
		}
		
		// Inventar öffnen/schließen
		if (e.IsActionPressed("inventory"))
			InventoryPanel.SetDeferred("visible", !InventoryPanel.Visible);
			
		//  Pause
		if (e.IsActionPressed("ui_cancel"))
			TogglePause();
		
		// Tools wechseln (Mausrad)
		if (e is InputEventMouseButton mb && mb.Pressed)
		{
			if (mb.ButtonIndex == MouseButton.WheelUp)
				SelectPreviousTool();

			if (mb.ButtonIndex == MouseButton.WheelDown)
				SelectNextTool();
		}
		
		
	}

	// ───────────────────────── TOOLS ─────────────────────────

	private void SelectPreviousTool() // Werkzeug vorheriges auswählen
	{
		currentToolIndex--;
		if (currentToolIndex < 0)
			currentToolIndex = toolButtons.Length - 1;

		UpdateToolVisuals();
	}

	private void SelectNextTool() // Werkzeug nächstes auswählen
	{
		currentToolIndex++;
		if (currentToolIndex >= toolButtons.Length)
			currentToolIndex = 0;

		UpdateToolVisuals();
	}

	private void UpdateToolVisuals()
	{
		for (int i = 0; i < toolButtons.Length; i++)
		{
			if (toolButtons[i] == null)
				continue;

			bool active = (i == currentToolIndex);
			toolButtons[i].ButtonPressed = active;

			// Aufleuchten
			toolButtons[i].Modulate = active
				? new Color(1f, 1f, 1f, 1f)
				: new Color(0.4f, 0.4f, 0.4f, 1f);
		}

		// PLAYER INFORMIEREN
		player?.SetToolByIndex(currentToolIndex);

		GD.Print("UI Tool gesetzt: " + currentToolIndex);
	}

	// ───────────────────────── PAUSE ─────────────────────────

	private void TogglePause()
	{
		bool paused = !GetTree().Paused; //pausiert das Spiel / unpausiert es
		GetTree().Paused = paused;
		PauseMenu.SetDeferred("visible", paused);

		if (!paused)
			SettingsMenu.Visible = false;
	}

	// ───────────────────────── BUTTONS ───────────────────────

	public void _on_continue_button_pressed()
	{
		TogglePause();
	}

	public void _on_settings_button_pressed()
	{
		SettingsMenu.Visible = true;
	}

	public void _on_back_button_pressed()
	{
		SettingsMenu.Visible = false;
		PauseMenu.Visible = true;
	}

	public void _on_save_quit_button_pressed()
	{
		var inventory = GetTree().GetFirstNodeInGroup("Inventory");

		if (player != null && inventory != null)
		{
			SaveManager.SaveGame(
				player.GlobalPosition,
				((InventoryManager)inventory).Items
			);
		}

		GetTree().Quit();
	}

	// ───────────────────────── AUDIO ─────────────────────────

	private void OnMusicVolumeChanged(double value)
	{
		SetBusVolume("Music", value);
	}

	private void OnSfxVolumeChanged(double value)
	{
		SetBusVolume("SFX", value);
	}

	private void SetBusVolume(string busName, double linearValue)
	{
		int bus = AudioServer.GetBusIndex(busName);
		float v = Mathf.Clamp((float)linearValue, 0f, 1f);
		AudioServer.SetBusVolumeDb(bus, Mathf.LinearToDb(v));
	}
	
	
	// ───────────────────────── INVENTORY UI ─────────────────────────

	private void UpdateInventoryUI()
	{
		if (inventory == null || !inventory.IsInsideTree())
			return;

		logsLabel.Text = inventory.GetItemCount("logs").ToString();
		stoneLabel.Text = inventory.GetItemCount("stone").ToString();
		weedLabel.Text = inventory.GetItemCount("weed").ToString();
		tomatoLabel.Text = inventory.GetItemCount("tomato").ToString();
		eggLabel.Text = inventory.GetItemCount("egg").ToString();
		milkLabel.Text = inventory.GetItemCount("milk").ToString();
		weedSeedLabel.Text = inventory.GetItemCount("weed_seed").ToString();
		tomatoSeedLabel.Text = inventory.GetItemCount("tomato_seed").ToString();
	}
	
	//Chest öffnen 
	public void OpenChest(Chest chest) // wird von Player aufgerufen, speichert die aktuelle Chest und zeigt das UI an, aktualisiert die Inventare
	{
		currentChest = chest;
		InventoryPanel.Visible = true;
		chestInventoryPanel.Visible = true;

		GD.Print("🧺 Chest UI opened");
		UpdateInventories(); //liest die Inventare aus der Chest und dem Spielerinventar aus und aktualisiert die UI
	}
	private void UpdateInventories()
	{
		if (currentChest == null)
			return;

		var chestInv = currentChest.GetNodeOrNull<ChestInventory>("ChestInventory"); //ChestInventory in der Chest holen
		if (chestInv == null)
		{
			GD.PrintErr("❌ ChestInventory node fehlt in der Chest!");
			return;
		}

		for (int i = 0; i < chestInv.Slots.Count; i++)// geht alle Slots im Chest-Inventar durch und aktualisiert die UI entsprechend
		{
			string slotPath = $"ChestInventoryPanel/MarginContainer/PanelContainer/VBoxContainer/MarginContainer/GridContainer/SlotButton_{i}";
			Button slotButton = GetNodeOrNull<Button>(slotPath);
			if (slotButton == null)
				continue;

			TextureRect icon = slotButton.GetNodeOrNull<TextureRect>("TextureRect"); //Icon im Slot
			Label label = slotButton.GetNodeOrNull<Label>("Label");
			if (icon == null || label == null)
				continue;

			InventorySlot slot = chestInv.Slots[i];
			if (slot == null || slot.IsEmpty)
			{
				icon.Texture = null;
				label.Text = "";
			}
			else
			{
				icon.Texture = GetItemIcon(slot.ItemId);
				label.Text = slot.Amount.ToString();
			}
		}
	}

	
	private Texture2D GetItemIcon(string itemId)
	{
		return itemId switch
		{
			"logs" => woodIcon,
			"stone" => stoneIcon,
			"weed" => weedIcon,
			"tomato" => tomatoIcon,
			"egg" => eggIcon,
			"milk" => milkIcon,
			"weed_seed" => weedSeedIcon,
			"tomato_seed" => tomatoSeedIcon,
			_ => null
		};
	}
	
	//Slots verbinden 
	private void OnChestSlotPressed(int slotIndex)
	{
		if (currentChest == null)
			return;

		var chestInv = currentChest.GetNode<ChestInventory>("ChestInventory");
		var slot = chestInv.Slots[slotIndex];

		// SHIFT → ALLES rüber
		if (Input.IsKeyPressed(Key.Shift))
		{
			if (slot.IsEmpty)
				return;

			inventory.AddItem(slot.ItemId, slot.Amount);
			slot.Clear();
			UpdateInventories();
			return;
		}

		// Spieler → Chest
		if (!slot.IsEmpty)
		{
			// Slot belegt → auswählen
			selectedPlayerItemId = slot.ItemId;
			return;
		}

		if (string.IsNullOrEmpty(selectedPlayerItemId))
			return;

		// STACKING LOGIK
		int amount = inventory.GetItemCount(selectedPlayerItemId);
		if (amount <= 0)
			return;

		slot.ItemId = selectedPlayerItemId;
		slot.Amount = amount;

		inventory.RemoveItem(selectedPlayerItemId, amount);
		selectedPlayerItemId = null;

		UpdateInventories();
	}





	
	public void CloseChest()
	{
		currentChest = null;
		selectedSlotIndex = -1;
		selectedPlayerItemId = null;

		chestInventoryPanel.SetDeferred("visible", false);
		InventoryPanel.SetDeferred("visible", false);
		
	}
	public void SelectPlayerItem(string itemId)
	{
		if (inventory == null)
		{
			GD.PrintErr("❌ Inventory ist NULL in SelectPlayerItem");
			return;
		}

		if (inventory.GetItemCount(itemId) <= 0)
		{
			GD.Print($"⚠ Kein Item mehr: {itemId}");
			selectedPlayerItemId = null;
			return;
		}

		selectedPlayerItemId = itemId;
		GD.Print("🎯 Player selected item: " + itemId);
	}
	
	//Chest Hint -- Taste E 
	public void ShowInteractHint(string key)
	{
		InteractHint.Visible = true;
	}

	public void HideInteractHint()
	{
		InteractHint.Visible = false;
	}
	
	//Drag&Drop 
	public void EndDrag()
	{
		DraggedSlot = null;
		DraggedFromIndex = -1;
		DragIcon.Visible = false;
		UpdateInventories();
	}

	public void CancelDrag()
	{
		EndDrag();
	}
	public override void _UnhandledInput(InputEvent e)
	{
		if (e is InputEventMouseButton mb && !mb.Pressed)
		{
			if (DraggedSlot != null)
				CancelDrag();
		}
	}
	public override void _ExitTree()
	{
		if (inventory != null && inventory.IsInsideTree())
		{
			inventory.InventoryChanged -= UpdateInventoryUI;
		}
	}

	
}
