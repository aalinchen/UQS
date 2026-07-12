using Godot;

public partial class IntroDialogue : Control
{
	private RichTextLabel textLabel;
	private TextureRect arrow;
	private AnimationPlayer arrowAnim;

	private string[] dialogue = 
	{
		"Endlich angekommen.\nJetzt beginnt etwas Neues.",
		"Diese Insel kennt keine Eile.\nAlles wächst in seinem eigenen Tempo.",
		"Das Land ist bereit.\nMit jedem Samen formst du deine Farm.",
		"Das Haus ist alt, aber stabil.\nEs wird dein Zuhause sein.",
		"Hier bestimmst du den Rhythmus.\nArbeit, Ruhe, neue Tage.",
		"Kein Druck.\nNur du und das, was du erschaffst.",
		"Dies ist dein Anfang.",
		"Willkommen auf deiner Farm."
	};

	private int page = 0;
	private int charIndex = 0;
	private bool isTyping = false;
	private bool skipTyping = false;

	private float speed = 0.04f;
	private string currentLine = "";

	public override void _Ready() 
	{
		textLabel = GetNode<RichTextLabel>("DialogueBox/TextLabel");
		arrow = GetNode<TextureRect>("DialogueBox/ContinueArrow");
		arrowAnim = GetNode<AnimationPlayer>("DialogueBox/ArrowAnimation");

		textLabel.Text = "";
		arrow.Visible = false;
		arrowAnim.Stop();

		StartTyping(); 
	}

	public override void _Input(InputEvent @event) // Eingabeverarbeitung
    {
		if (!@event.IsActionPressed("ui_accept"))
			return;

		if (isTyping)
		{
			// Tipp-Effekt überspringen
			skipTyping = true;
		}
		else
		{
			arrow.Visible = false;
			arrowAnim.Stop();

			page++; // Nächste Seite
            if (page >= dialogue.Length) // Ende des Dialogs
            {
				GetTree().ChangeSceneToFile(
					"res://Scenes/LoadingScreen.tscn");
			}
            else // Nächste Zeile tippen
            {
				StartTyping();
			}
		}
	}

	private async void StartTyping() // Beginnt den Tipp-Effekt
    {
		currentLine = dialogue[page]; // Aktuelle Zeile
        charIndex = 0;
		isTyping = true;
		skipTyping = false; 

        arrow.Visible = false;
		arrowAnim.Stop();

		// Abstand zwischen Dialogblöcken
		if (textLabel.Text.Length > 0)
			textLabel.Text += "\n\n";

		while (charIndex < currentLine.Length) // Tipp-Effekt
        {
			if (skipTyping)
				break;

			textLabel.Text += currentLine[charIndex]; // Nächstes Zeichen anhängen
            charIndex++;

			textLabel.ScrollToLine(textLabel.GetLineCount()); // Scrollen

            await ToSignal( // Warten
                GetTree().CreateTimer(speed),
				"timeout");
		}

		// Falls übersprungen → Rest sauber anhängen
		if (skipTyping && charIndex < currentLine.Length)
		{
			textLabel.Text += currentLine.Substring(charIndex); // Rest der Zeile anhängen
        }

		isTyping = false;
		arrow.Visible = true;
		arrowAnim.Play("blink");
	}
}
