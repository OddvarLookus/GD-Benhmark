using Godot;
using System;

public static class Extentions
{

    public static bool Compare(this Color c0, Color c1)
    {
        Vector4 v0 = new Vector4(c0.R, c0.G, c0.B, c0.A);
        Vector4 v1 = new Vector4(c1.R, c1.G, c1.B, c1.A);
        return v0.DistanceTo(v1) <= 0.01f;
    }


}
