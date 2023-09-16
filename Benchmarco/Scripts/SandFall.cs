using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class SandFall : Sprite2D
{
	[Export] Vector2I imageSize;
	[Export] Camera2D cam;
	Image simImage;

	[Export] int placementDist = 1;
	[Export] int placementNum = 1;

	[Export] Color[] colorsCatalogue;


	[Serializable]
	enum ColorKeys
	{
		AIR = 0,
		SAND = 1
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		simImage = new Image();
		simImage = Image.Create(imageSize.X, imageSize.Y, false, Image.Format.Rgba8);
		//simImage.Fill(colorsCatalogue[(int)ColorKeys.AIR]);
		
		InitializeGrid();
		
		ApplyTexture();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	    Paint();

	    Stopwatch stopwatch = new Stopwatch();
	    stopwatch.Start();
		UpdateSandFall(true);
		//UpdateSandFall(true);
		stopwatch.Stop();
		GD.Print($"1:{stopwatch.ElapsedMilliseconds.ToString()}");
		
		
		ApplyTexture();
		
		
		GC.Collect();
		
	}
	
	//NEW BEHAVIOR

	int[] grid;
	byte[] imgData;
	byte[] colorBuffer = new byte[4];

	void InitializeGrid()
	{
		grid = new int[imageSize.X * imageSize.Y];
		imgData = new byte[imageSize.X * imageSize.Y * 4];
		for (int i = 0; i < grid.Length; i++) 
		{
			imgData[i * 4] = Convert.ToByte(colorsCatalogue[grid[0]].R8);
			imgData[i * 4 + 1] = Convert.ToByte(colorsCatalogue[grid[0]].G8);
			imgData[i * 4 + 2] = Convert.ToByte(colorsCatalogue[grid[0]].B8);
			imgData[i * 4 + 3] = Convert.ToByte(colorsCatalogue[grid[0]].A8);
		}
	}


	//Behavior

	void Paint()
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			Vector2I mPos = (Vector2I)(GetViewport().GetMousePosition() / cam.Zoom);
			int[] randPlacements = GetRandomsInSquare(mPos.X, mPos.Y, placementDist, placementNum);
			for (int i = 0; i < randPlacements.Length; i++)
			{
				if (randPlacements[i] != -1)
				{
					grid[randPlacements[i]] = 1;
					paintedPixels.Add(randPlacements[i]);
				}
			}
			//grid[To1DPos(mPos.X, mPos.Y)] = 1;
			//GD.Print(mPos);
			//simImage.SetPixelv(mPos, colorsCatalogue[(int)ColorKeys.SAND]);
		}
	}

	HashSet<int> paintedPixels = new HashSet<int>();
	HashSet<int> addedPixels = new HashSet<int>();
	HashSet<int> removedPixels = new HashSet<int>();
	void UpdateSandFall(bool updateGraphics = true)
	{
		addedPixels.Clear();
		removedPixels.Clear();
		
		int len = grid.Length;
		for (int i = 0; i < len; i++) 
		{
			if (grid[i] == 1 && !addedPixels.Contains(i))
			{
				if (i / imageSize.X < imageSize.Y - 1)//if not last pixel y move down
				{
					if (grid[i + imageSize.X] == 0)
					{
						grid[i] = 0;
						grid[i + imageSize.X] = 1;
						addedPixels.Add(i + imageSize.X);
						removedPixels.Add(i);
					}
					else if (i % imageSize.X != 0)//not first column
					{
						if (grid[i + imageSize.X - 1] == 0)
						{
							grid[i] = 0;
							grid[i + imageSize.X - 1] = 1;
							addedPixels.Add(i + imageSize.X - 1);
							removedPixels.Add(i);
						}
						else if (i % imageSize.X != imageSize.X - 1)//not last column
						{
							if (grid[i + imageSize.X + 1] == 0)
							{
								grid[i] = 0;
								grid[i + imageSize.X + 1] = 1;
								addedPixels.Add(i + imageSize.X + 1);
								removedPixels.Add(i);
							}
						}
					}
				}
			}

			if (updateGraphics)
			{
				if (addedPixels.Contains(i) || removedPixels.Contains(i) || paintedPixels.Contains(i))
				{
					Color col = colorsCatalogue[grid[i]];
					imgData[i * 4] = Convert.ToByte(col.R8);
					imgData[i * 4 + 1] = Convert.ToByte(col.G8);
					imgData[i * 4 + 2] = Convert.ToByte(col.B8);
					imgData[i * 4 + 3] = Convert.ToByte(col.A8);
				}
			}
			
		}
		
		paintedPixels.Clear();
		
		
		//Old code
		//
		// byte[] byters = simImage.GetData();
		//
		// updatedPixels.Clear();
		// for (int x = 0; x < imageSize.X; x++)
		// {
		// 	for (int y = 0; y < imageSize.Y; y++)
		// 	{
		// 		if (simImage.GetPixel(x, y).Compare(colorsCatalogue[(int)ColorKeys.SAND]))
		// 		{
		// 			//GD.Print(simImage.GetPixel(x, y).ToString());
		// 			if (GetColAt(x, y + 1).Compare(colorsCatalogue[(int)ColorKeys.AIR]))
		// 			{
		// 				if (!updatedPixels.Contains(To1DPos(x,y)))
		// 				{
		// 					simImage.SetPixel(x, y+1, colorsCatalogue[(int)ColorKeys.SAND]);
		// 					simImage.SetPixel(x, y, colorsCatalogue[(int)ColorKeys.AIR]);
		// 					updatedPixels.Add(To1DPos(x, y+1));
		// 				}
		// 			}
		// 			else if (GetColAt(x-1, y + 1).Compare(colorsCatalogue[(int)ColorKeys.AIR]))
		// 			{
		// 				if (!updatedPixels.Contains(To1DPos(x, y)))
		// 				{
		// 					simImage.SetPixel(x-1, y+1, colorsCatalogue[(int)ColorKeys.SAND]);
		// 					simImage.SetPixel(x, y, colorsCatalogue[(int)ColorKeys.AIR]);
		// 					updatedPixels.Add(To1DPos(x-1, y+1));
		// 				}
		// 			}
		// 			else if (GetColAt(x+1, y + 1).Compare(colorsCatalogue[(int)ColorKeys.AIR]))
		// 			{
		// 				if (!updatedPixels.Contains(To1DPos(x, y)))
		// 				{
		// 					simImage.SetPixel(x+1, y+1, colorsCatalogue[(int)ColorKeys.SAND]);
		// 					simImage.SetPixel(x, y, colorsCatalogue[(int)ColorKeys.AIR]);
		// 					updatedPixels.Add(To1DPos(x+1, y+1));
		// 				}
		// 			}
		// 		}
		// 	}
		// }
		

		
	}
	
	bool CheckBounds(int inx, int iny)
	{
		return inx >= 0 && inx < imageSize.X && iny >= 0 && iny < imageSize.Y;
	}

	Color GetValAt(int inx, int iny)
	{
		if (CheckBounds(inx, iny))
		{
			return simImage.GetPixel(inx, iny);
		}
		else
		{
			return new Color(0f, 0f, 0f, 0f);
		}
	}

	int To1DPos(int x, int y)
	{
		return x + y * imageSize.X;
	}

	int[] GetRandomsInSquare(int x, int y, int radius, int num)
	{
		int[] rands = new int[num];
		for (int i = 0; i < num; i++)
		{
			int nX = (int)GD.Randi() % (radius * 2) - radius;
			nX += x;
			int nY = (int)GD.Randi() % (radius * 2) - radius;
			nY += y;
			
			if (!CheckBounds(nX, nY))
			{
				rands[i] = -1;
			}
			else
			{
				rands[i] = To1DPos(nX, nY);
			}
		}
		return rands;
	}

	void ApplyTexture()
	{
		simImage.SetData(imageSize.X, imageSize.Y, false, Image.Format.Rgba8, imgData);
		this.Texture = ImageTexture.CreateFromImage(simImage);
	}

}
