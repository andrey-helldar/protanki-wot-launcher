using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public sealed class WPLinkButton : WPButton
	{
		public string Link { get; set; }

		public WPLinkButton()
		{
			Cursor = Cursors.Hand;
		}

		protected override void OnClick(EventArgs e)
		{
			if (Link != null)
			{
				TryOpenLink();
			}

			base.OnClick(e);
		}

		private void TryOpenLink()
		{
            try
            {
                Process.Start(Link);
            }
            finally { }
		}
	}
}