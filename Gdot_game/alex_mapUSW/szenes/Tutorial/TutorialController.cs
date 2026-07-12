using Godot;
using System.Collections.Generic;

public partial class TutorialController : Node
{
	[Export] private NodePath PlayerPath;
	[Export] private NodePath TutorialUiPath;

	private Player _player;
	private TutorialUi _ui;

	private int _stepIndex = 0;

	private class TutorialStep // Einzelschritt im Tutorial
	{
		public string Text;
		public Vector2 Position;

		public TutorialStep(string text, Vector2 position) // Konstruktor
		{
			Text = text;
			Position = position;
		}
	}

	private List<TutorialStep> _steps; // Liste aller Tutorial-Schritte

	public override void _Ready()
	{
		// 🔒 Sicherheit
		if (PlayerPath.IsEmpty || TutorialUiPath.IsEmpty) 
		{
			GD.PushError("TutorialController: PlayerPath oder TutorialUiPath nicht gesetzt!");
			return;
		}

		_player = GetNode<Player>(PlayerPath); // Spieler-Node holen
		_ui = GetNode<TutorialUi>(TutorialUiPath); // Tutorial-UI-Node holen

		if (_ui.DialogueBox == null || _ui.DialogueLabel == null || _ui.ContinueButton == null) // UI-Komponenten prüfen
		{
			GD.PushError("TutorialUi ist nicht korrekt im Inspector zugewiesen!");
			return;
		}

		_player.CanMove = false; // Spielerbewegung deaktivieren

		_ui.ContinueButton.Pressed += NextStep;// Weiter-Button Event verbinden

		BuildSteps();
		ShowStep();
	}

	private void BuildSteps() // Tutorial-Schritte erstellen
	{
		Vector2 top = new(400, 40);
		Vector2 left = new(40, 300);
		Vector2 right = new(GetViewport().GetVisibleRect().Size.X - 440, 300);
		Vector2 bottom = new(400, GetViewport().GetVisibleRect().Size.Y - 220);

		_steps = new List<TutorialStep> // Liste der Schritte füllen
		{
			new("Willkommen in Under Quiet Skies.\nBewege dich mit [W][A][S][D].", top),
			new("Öffne dein Inventar mit [I].", left),
			new("Wechsle Tools mit dem Mausrad.", right),
			new("Drücke [ESC], um das Pause-Menü zu öffnen.", top),
			new("Items sammelst du, indem du darüber läufst.", bottom),
			new("Öffne eine Kiste mit [E].", bottom),
			new("Lege Items per Klick in die Kiste.", left),
			new("Mit [SHIFT] + Klick holst du Items zurück.", left),
			new("Mit der Axt bekommst du Holz von Bäumen.", bottom),
			new("Pflanzen:\nBoden → Samen → Gießen → Ernten.", bottom),
			new("Betritt das Haus, indem du zur Tür läufst.", bottom),
			new("Viel Erfolg!", top)
		};
	}

	private void ShowStep() // Aktuellen Schritt anzeigen
	{
		_ui.DialogueLabel.Text = _steps[_stepIndex].Text;
		_ui.DialogueBox.GlobalPosition = _steps[_stepIndex].Position;
	}

	private void NextStep() // Zum nächsten Schritt wechseln
	{
		_stepIndex++;

		if (_stepIndex >= _steps.Count)
			EndTutorial();
		else
			ShowStep();
	}

	private void EndTutorial() // Tutorial beenden
	{
		_player.CanMove = true; // Spielerbewegung aktivieren
		GetNode<GameState>("/root/GameState").TutorialDone = true;
		GetTree().ChangeSceneToFile("res://alex_mapUSW/szenes/test/test_scene_default.tscn");
	}

	public override void _UnhandledInput(InputEvent e) // Eingaben verarbeiten
	{
		if (e.IsActionPressed("ui_accept")) // Eingabe für "Weiter"
			NextStep();
	}
}
