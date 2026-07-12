using Godot;
using System;

public partial class HurtComponent : Area2D
{
	[Export]
	public Tools Tool { get; set; } = Tools.None;

	[Signal]
	public delegate void HurtEventHandler(int damage);

	public override void _Ready()
	{
		Monitoring = true; // ensure this area is monitoring
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		// Debug: log the entering area
		GD.Print($"[HurtComponent:{Name}] AreaEntered by '{area.Name}' (type: {area.GetType().Name})");

		// Check if the area is a HitComponent
		if (area is HitComponent hitComponent)
		{
			GD.Print($"[HurtComponent:{Name}] Detected HitComponent (Tool={hitComponent.CurrentTool}, Damage={hitComponent.HitDamage})");

			// Check if the tool matches
			if (Tool == hitComponent.CurrentTool)
			{
				// Emit the existing signal so any editor connections still work
				EmitSignal(nameof(Hurt), hitComponent.HitDamage);
				GD.Print($"[HurtComponent:{Name}] Valid hit: applying {hitComponent.HitDamage} damage");

				// Robust: try to apply damage directly to the parent node (e.g. a tree)
				Node? owner = GetParent();
				if (owner != null)
				{
					// If the parent has a TakeDamage(int) method (e.g. kleinesbaum), call it
					if (owner.HasMethod("TakeDamage"))
					{
						owner.Call("TakeDamage", hitComponent.HitDamage);
						GD.Print($"[HurtComponent:{Name}] Called {owner.Name}.TakeDamage({hitComponent.HitDamage})");
					}
					else
					{
						// Otherwise try to find a DamageComponent child and apply damage there
						var dmgComp = owner.GetNodeOrNull<DamageComponent>("DamageComponent");
						if (dmgComp != null)
						{
							dmgComp.ApplyDamage(hitComponent.HitDamage);
							GD.Print($"[HurtComponent:{Name}] Applied damage to DamageComponent on {owner.Name}");
						}
						else
						{
							GD.PrintErr($"[HurtComponent:{Name}] No TakeDamage method and no DamageComponent found on parent '{owner.Name}'");
						}
					}
				}
			}
			else
			{
				GD.Print($"[HurtComponent:{Name}] Tool mismatch: Hurt.Tool={Tool} vs Hit.CurrentTool={hitComponent.CurrentTool}");
			}
		}
	}
}
