using UnityEngine;
using Random = System.Random;

public static class RandomExtentions {
    public static Vector2 Between(this Random random, Vector2 min, Vector2 max)
    {
        return new Vector2(
            (max.x - min.x) * (float)random.NextDouble() + min.x,
            (max.y - min.y) * (float)random.NextDouble() + min.y);
    }

    public static double Between(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
    
    public static Vector3 Between(this Random random, Vector3 min, Vector3 max)
    {
        return new Vector3(
            (max.x - min.x) * (float)random.NextDouble() + min.x,
            (max.y - min.y) * (float)random.NextDouble() + min.y,
            (max.z - min.z) * (float)random.NextDouble() + min.z);
    }
}