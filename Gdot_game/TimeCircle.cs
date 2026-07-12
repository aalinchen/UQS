using Godot;

public partial class TimeCircle : Control
{
	public float Progress = 0f; // 0.0 - 1.0

	public override void _Draw()
	{
		Vector2 center = Size / 2;
		float radius = Size.X / 2 - 5;

		// Hintergrund
		DrawArc(center, radius, 0, Mathf.Tau, 64, Colors.DarkGray, 5);

		// Fortschritt (Start oben)
		DrawArc(
			center,
			radius,
			-Mathf.Pi / 2,
			-Mathf.Pi / 2 + Mathf.Tau * Progress,
			64,
			Colors.Green,
			5
		);
	}

	public void SetProgress(float value)
	{
		Progress = Mathf.Clamp(value, 0, 1);
		QueueRedraw();
	}
}
