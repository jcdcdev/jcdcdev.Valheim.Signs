using UnityEngine;

namespace jcdcdev.Valheim.Signs.Models;

public class GameObjectDto
{
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public Vector3 Position => new(X, Y, Z);
}
