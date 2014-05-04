using System.Collections.Generic;

namespace WoTPinger.Core.Configuration
{
	public interface IConfiguration
	{
		int GoodPing { get; }
		int BadPing { get; }
		int PingInterval { get; }

		IList<Server> GetServers();
	}
}