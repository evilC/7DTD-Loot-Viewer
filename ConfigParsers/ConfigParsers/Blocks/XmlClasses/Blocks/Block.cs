﻿using System.Xml.Serialization;

namespace ConfigParsers.Blocks.XmlClasses
{
    public class Block
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement(ElementName = "property")]
        public List<BlockProperty> Properties { get; set; } = new List<BlockProperty>();

        [XmlElement(ElementName = "drop")]
        public List<BlockDrop> Drops { get; set; } = new List<BlockDrop>();
    }
}
