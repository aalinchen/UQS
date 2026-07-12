using Godot;

public partial class DayNightLight : CanvasModulate
{
	public override void _Process(double delta)
	{
		if (GameManager.Instance == null)
			return;

		float t = GameManager.Instance.GetDayProgress(); 

        // Farbe je nach Tageszeit
        Color night = new Color(0.15f, 0.18f, 0.35f);
		Color day = Colors.White;

		// Kurve: Tag → Nacht → Tag
		float blend = Mathf.Abs(t * 2f - 1f);

		Color = day.Lerp(night, blend);
	}
}
