using System;
using System.Xml.Serialization;

namespace WoTPinger.Core
{
	public sealed class Server : IEquatable<Server>
	{
		[XmlAttribute("address")]
		public string Address { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		public bool Equals(Server other)
		{
			return other != null && Address.Equals(other.Address, StringComparison.OrdinalIgnoreCase);
		}
	}
}