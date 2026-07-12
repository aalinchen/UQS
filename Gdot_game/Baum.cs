using Godot;

public partial class Baum : Node2D
{
	[Export] public int MaxHealth = 3;
	[Export] public PackedScene WoodDropScene;

	private int _health;

	public override void _Ready()
	{
		_health = MaxHealth;
	}

	public void Hit(int damage = 1)
	{
		_health -= damage;

		if (_health <= 0)
		{
			DropWood();
			QueueFree();
		}
	}

	private void DropWood()
	{
		if (WoodDropScene == null)
			return;

		Node2D wood = WoodDropScene.Instantiate<Node2D>();
		wood.GlobalPosition = GlobalPosition;
		GetParent().AddChild(wood);
	}
}
