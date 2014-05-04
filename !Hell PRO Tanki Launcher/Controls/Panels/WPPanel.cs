using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public class WPPanel : Panel
	{
		public WPPanel()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.UserPaint, true);
		}
	}
}