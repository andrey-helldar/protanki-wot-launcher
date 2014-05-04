using System;

namespace WoTPinger.Core.Ping
{
	public sealed class PingReceivedEventArgs : EventArgs
	{
		public long Time { get; private set; }
		public PingStatus Status { get; private set; }

		internal PingReceivedEventArgs(PingStatus status, long time = 0)
		{
			Status = status;
			Time = time;
		}
	}
}