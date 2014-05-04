using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public sealed class WPFlowLayoutPanel : FlowLayoutPanel
	{
		public WPFlowLayoutPanel()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.UserPaint, true);
		}
	}
}