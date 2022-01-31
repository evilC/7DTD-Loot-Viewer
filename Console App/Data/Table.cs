namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// Old data structure only used by OldLootParser
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Key is Item Name, value is a HashSet (List) of Containers that the Item can spawn in
        /// </summary>
        public Dictionary<string, HashSet<string>> ItemContainers { get; } = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// Key is Container Name, value is a HashSet (List) of Items that can spawn in that Container
        /// </summary>
        public Dictionary<string, HashSet<string>> ContainerItems { get; } = new Dictionary<string, HashSet<string>>();
    }
}
