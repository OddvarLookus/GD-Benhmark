using Godot;
using System;

public partial class KeyPressSpawner : Node3D
{

	[Export] PackedScene cubeTemplate;
	
	public override void _Ready()
	{
		GD.Randomize();
	}

	void SpawnCube()
	{
		Vector3 nPos = new Vector3((float)GD.RandRange(-5.0, 5.0), 0f, (float)GD.RandRange(-5.0, 5.0));
		RigidBody3D nRB = cubeTemplate.Instantiate() as RigidBody3D;
		nRB.GlobalPosition = GlobalPosition + nPos;
	}

	public override void _Process(double delta)
	{
		if (Input.IsPhysicalKeyPressed(Key.Space))
		{
			SpawnCube();
		}
	}
}
