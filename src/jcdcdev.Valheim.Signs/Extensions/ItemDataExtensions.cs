namespace jcdcdev.Valheim.Signs.Extensions;

public static class ItemDataExtensions
{
    public static string GetName(this ItemDrop.ItemData item) => item.m_shared.m_name.Replace("$item_", "");
    public static string GetLabel(this ItemDrop.ItemData item) => item.m_shared.m_name.Replace("$item_", "");
    public static int GetStackSize(this ItemDrop.ItemData item) => item.m_shared.m_maxStackSize;
}
