using Godot;
using System;

public partial class KeyPressSpawner : Node3D
{

	[Export] PackedScene cubeTemplate;
	[Export] Label infosLabel;

	public override void _Ready()
	{
		GD.Randomize();
	}

	int cubesSpawned = 0;
	void SpawnCube()
	{
		Vector3 nPos = new Vector3((float)GD.RandRange(-5.0, 5.0), 0f, (float)GD.RandRange(-5.0, 5.0));
		RigidBody3D nRB = cubeTemplate.Instantiate() as RigidBody3D;
		AddChild(nRB);
		nRB.GlobalPosition = GlobalPosition + nPos;

		cubesSpawned += 1;
		infosLabel.Text = cubesSpawned.ToString();
	}

	public override void _Process(double delta)
	{
		if (Input.IsPhysicalKeyPressed(Key.Space))
		{
			//GD.Print("ooo");
			SpawnCube();
		}
	}
}
