using System;
using System.Drawing;
using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public class WPButton : Button
	{
		public Image NormalImage { get; set; }
		public Image HoverImage { get; set; }

		public WPButton()
		{
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			FlatAppearance.MouseDownBackColor = Color.Transparent;
			FlatAppearance.MouseOverBackColor = Color.Transparent;
		}

		protected override bool ShowFocusCues
		{
			get { return false; }
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			BackgroundImage = HoverImage;
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			BackgroundImage = NormalImage;
			base.OnMouseLeave(e);
		}

		public override void NotifyDefault(bool value)
		{
			base.NotifyDefault(false);
		}

		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			base.OnPreviewKeyDown(e);

			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
				e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				e.IsInputKey = true;
			}
		}
	}
}