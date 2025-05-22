namespace jcdcdev.Valheim.Signs.Models;

public class PlayerDeathInfo
{
    public int Deaths { get; set; }
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Offset { get; set; } = 0;
    public int GetDeaths() => Deaths + Offset;
}
