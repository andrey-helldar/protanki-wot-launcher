using System;
using System.Drawing;
using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public sealed class WPRadioButton : RadioButton
	{
		private Image normalImage;

		public Image NormalImage
		{
			get { return normalImage; }
			set { Image = normalImage = value; }
		}

		public Image HoverImage { get; set; }
		public Image SelectedImage { get; set; }

		public WPRadioButton()
		{
			Appearance = Appearance.Button;
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			FlatAppearance.CheckedBackColor = Color.Transparent;
			FlatAppearance.MouseDownBackColor = Color.Transparent;
			FlatAppearance.MouseOverBackColor = Color.Transparent;
		}

		protected override bool ShowFocusCues
		{
			get { return false; }
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			if (Checked)
			{
				Image = SelectedImage;
				Cursor = Cursors.Arrow;
			}
			else
			{
				Image = NormalImage;
				Cursor = Cursors.Hand;
			}

			base.OnCheckedChanged(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			if (!Checked)
				Image = HoverImage;

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (!Checked)
				Image = NormalImage;

			base.OnMouseLeave(e);
		}

		protected override void OnTabStopChanged(EventArgs e)
		{
			base.OnTabStopChanged(e);

			if (TabStop)
				TabStop = false;
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