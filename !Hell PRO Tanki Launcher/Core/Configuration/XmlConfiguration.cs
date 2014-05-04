using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WoTPinger.Core.Configuration
{
	public sealed class XmlConfiguration : IConfiguration
	{
		private static ConfigFile config;

		public XmlConfiguration(string fileName)
		{
			using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				config = (ConfigFile)new XmlSerializer(typeof(ConfigFile)).Deserialize(stream);
			}
		}

		public int GoodPing
		{
			get { return config.GoodPing; }
		}

		public int BadPing
		{
			get { return config.BadPing; }
		}

		public int PingInterval
		{
			get { return Math.Max(config.Interval, 500); }
		}

		public IList<Server> GetServers()
		{
			var servers = new List<Server>();

			foreach (var server in config.Servers)
			{
				if (server == null || String.IsNullOrEmpty(server.Address))
					continue;

				bool serverIsDuplicated = false;

				foreach (var existingServer in servers)
				{
					if (existingServer.Equals(server))
					{
						serverIsDuplicated = true;
						break;
					}
				}

				if (!serverIsDuplicated)
					servers.Add(server);
			}

			return servers;
		}
	}
}