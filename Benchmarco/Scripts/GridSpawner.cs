using Godot;
using System;

public partial class GridSpawner : Node3D
{
	[Export] PackedScene objTemplate;

	[Export] float dist;
	[Export] int rowNum;

	[Export] Label infoLabel;

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
		
		Node3D nNode = objTemplate.Instantiate<Node3D>();
		AddChild(nNode);
		nNode.GlobalPosition = nPos;
		currentIdx += 1;

		infoLabel.Text = $"{currentIdx.ToString()}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsPhysicalKeyPressed(Key.Space))
		{
			SpawnObj();
		}

	}
}
