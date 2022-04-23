﻿using System.Xml.Serialization;

namespace CensureFiles.Entities
{
    public class File
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Extension")]
        public string Extension { get; set; }

        [XmlIgnore]
        public string FullName => Name + Extension;

    }
}
