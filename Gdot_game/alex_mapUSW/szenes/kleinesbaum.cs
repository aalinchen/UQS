using Godot;

public partial class kleinesbaum : Sprite2D
{
	[Export] public int MaxHealth = 3;
	[Export] public PackedScene LogScene;

	private int _currentHealth;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;

		if (LogScene == null)
		{
			GD.PrintErr("LogScene ist nicht gesetzt! Bitte im Inspector zuweisen.");
		}
	}

	/// <summary>
	/// Diese Methode kann von außen aufgerufen werden,
	/// z.B. wenn der Spieler den Baum schlägt.
	/// </summary>
	public void TakeDamage(int damage = 1)
	{
		_currentHealth -= damage;
		GD.Print($"Baum getroffen. Verbleibende HP: {_currentHealth}");

		if (_currentHealth <= 0)
		{
			DestroyTree();
		}
	}

	private void DestroyTree()
	{
		GD.Print("Baum wurde zerstört.");

		SpawnLog();
		QueueFree();
	}

	private void SpawnLog()
	{
		if (LogScene == null)
			return;

		Node2D logInstance = LogScene.Instantiate<Node2D>();
		logInstance.GlobalPosition = GlobalPosition;

		GetParent().AddChild(logInstance);
	}
}
