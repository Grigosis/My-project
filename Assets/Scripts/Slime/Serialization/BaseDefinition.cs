﻿using System.Xml.Serialization;

namespace ROR.Core.Serialization
{
    public class BaseDefinition : Definition
    {
        [XmlAttribute]
        public string Name = "";
        [XmlAttribute]
        public string Icon = "";
        
        public string Description = "";
        public string Type = "";
    }
}