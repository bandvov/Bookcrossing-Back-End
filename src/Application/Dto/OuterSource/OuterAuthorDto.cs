﻿using System.Xml.Serialization;

namespace Application.Dto.OuterSource
{
    public class OuterAuthorDto
    {
        [XmlElement("name")]
        public string FullName { get; set; }
    }
}
