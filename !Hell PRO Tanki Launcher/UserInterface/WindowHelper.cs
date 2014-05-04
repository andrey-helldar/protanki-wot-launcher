using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using _Hell_PRO_Tanki_Launcher;


namespace WoTPinger.UserInterface
{
	public static class WindowHelper
	{
		private static readonly IntPtr Two = new IntPtr(2);
		private static bool helpIsShown;

		public static Icon GetAssemblyIcon()
		{
			return Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
		}

		public static Message CreateDragMessage(IntPtr handle)
		{
			return System.Windows.Forms.Message
				.Create(handle, (int)fPing.NonClientButtonDown, Two, IntPtr.Zero);
		}

		public static void Error(string errorMessage)
		{
			Message(errorMessage, MessageBoxIcon.Error);
		}

		public static void Warning(string message)
		{
			Message(message, MessageBoxIcon.Warning);
		}

		internal static void Message(string message, MessageBoxIcon icon)
		{
			MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, icon);
		}
	}
}