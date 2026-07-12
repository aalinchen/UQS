using Godot;
using System;

public partial class DamageComponent : Node2D
{
	// ─── EXPORT VARIABLES
	[Export]
	public int MaxDamage { get; set; } = 1;

	[Export]
	public int CurrentDamage { get; set; } = 0;

	// ─── SIGNAL
	[Signal]
	public delegate void MaxDamagedReachedEventHandler();

	/// <summary>
	/// Apply damage to this component.
	/// Emits MaxDamagedReached signal if damage reaches max.
	/// </summary>
	public void ApplyDamage(int damage)
	{
		CurrentDamage = Mathf.Clamp(CurrentDamage + damage, 0, MaxDamage);

		if (CurrentDamage == MaxDamage)
		{
			EmitSignal(nameof(MaxDamagedReached));
		}
	}
}
