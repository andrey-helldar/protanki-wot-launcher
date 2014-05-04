using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace WoTPinger.Core.Ping
{
	using Ping = System.Net.NetworkInformation.Ping;

	public sealed class PingManager : IDisposable
	{
		private readonly Ping ping;
		private readonly string address;
		private bool disposed;

		public event EventHandler<PingReceivedEventArgs> PingReceived;

		public PingManager(string address)
		{
			this.address = address;

			ping = new Ping();
			ping.PingCompleted += HandlePingComplete;
		}

		public void SendPing(int timeout = 1000)
		{
			try
			{
				// Time to live = 128
				// Buffer size  = 32 bytes (same as ping.exe)
				ping.SendAsync(address, timeout, null);
			}
			catch (PingException)
			{
				// An exception was thrown while sending or receiving the ICMP messages.
				OnPingReceived(new PingReceivedEventArgs(PingStatus.Error));
			}
			catch (SocketException)
			{
				// hostNameOrAddress could not be resolved to a valid IP address.
				OnPingReceived(new PingReceivedEventArgs(PingStatus.Error));
			}
			catch (InvalidOperationException)
			{
				// A call to SendAsync is in progress.
				OnPingReceived(new PingReceivedEventArgs(PingStatus.InProgress));
			}
		}

		private void HandlePingComplete(object sender, PingCompletedEventArgs e)
		{
			PingReceivedEventArgs eventArgs;

			if (e.Cancelled || e.Error != null)
			{
				// An asynchronous operation has been canceled
				// or error occurred during an asynchronous operation.
				eventArgs = new PingReceivedEventArgs(PingStatus.Error);
			}
			else
			{
				PingReply reply = e.Reply;

				eventArgs = (reply != null && reply.Status == IPStatus.Success)
					? new PingReceivedEventArgs(PingStatus.Success, reply.RoundtripTime)
					: new PingReceivedEventArgs(PingStatus.Error);
			}

			OnPingReceived(eventArgs);
		}

		private void OnPingReceived(PingReceivedEventArgs e)
		{
			var tempEvent = Interlocked.CompareExchange(ref PingReceived, null, null);
			if (tempEvent != null) tempEvent(this, e);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					ping.SendAsyncCancel();
					ping.Dispose();
				}
				disposed = true;
			}
		}

		~PingManager()
		{
			Dispose(false);
		}
	}
}