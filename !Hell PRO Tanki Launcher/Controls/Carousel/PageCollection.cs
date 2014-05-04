using System.Windows.Forms;

namespace WoTPinger.UserInterface.Controls
{
	public sealed class PageCollection
	{
		private readonly Control holder;

		public PageCollection(Control holder)
		{
			this.holder = holder;
		}

		public void Add(Page page)
		{
			holder.Controls.Add(page);
		}

		public void AddRange(Page[] pages)
		{
			holder.Controls.AddRange(pages);
		}

		public void Remove(Page page)
		{
			holder.Controls.Remove(page);
		}

		public void RemoveAt(int index)
		{
			holder.Controls.RemoveAt(index);
		}
	}
}