using System.Collections.Generic;
using System.Xml.Serialization;

namespace WoTPinger.Core.Configuration
{
	// Should be with internal visibility. It's public only due to serialization.

	[XmlRoot("Configuration", Namespace = "")]
	public sealed class ConfigFile
	{
		public int GoodPing { get; set; }

		public int BadPing { get; set; }

		public int Interval { get; set; }

		[XmlArray("Servers"), XmlArrayItem("Server")]
		public List<Server> Servers { get; set; }
	}
}