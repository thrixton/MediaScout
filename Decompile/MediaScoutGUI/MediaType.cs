using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

[GeneratedCode("svcutil", "4.0.30319.1"), XmlType(Namespace = "http://www.sublight.si/")]
[Serializable]
public enum MediaType
{
	CD,
	DVD,
	HD_DVD,
	BlueRay,
	BluRay,
	Unknown
}