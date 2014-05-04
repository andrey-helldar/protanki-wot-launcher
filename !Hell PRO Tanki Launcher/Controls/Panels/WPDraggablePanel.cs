using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public sealed class WPDraggablePanel : WPPanel
	{
		public int TopBorderHeight { get; set; }

		public WPDraggablePanel()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Y < TopBorderHeight)
			{
				Capture = false;
				Message msg = WindowHelper.CreateDragMessage(Parent.Handle);
				DefWndProc(ref msg);
			}

			base.OnMouseDown(e);
		}
	}
}