using Godot;
using System;

public partial class GridSpawner : Node3D
{
	[Export] PackedScene objTemplate;

	[Export] float dist;
	[Export] int rowNum;

	int currentIdx = 0;

	void SpawnObj()
	{
		Vector3 nPos = GlobalPosition + new Vector3
		(
			(float)(currentIdx % rowNum), 
			(float)(currentIdx / (rowNum * rowNum)) * dist,
			(float)((currentIdx % (rowNum * rowNum)) / rowNum) * dist
		);
		nPos += (float)(currentIdx / (rowNum * rowNum * rowNum)) * dist * (float)rowNum * Vector3.Right;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
