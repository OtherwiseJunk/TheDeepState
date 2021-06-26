using DeepState.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Utilities
{
	public static class PagingUtilities
	{
		public static List<T> GetPagedList<T>(List<T> dataToPage, out int successfulPage, int page = 0) where T : class
		{
			int startingItem = 0 + (page * SharedConstants.PageSize);
			List<T> pageData;
			int itemCount = dataToPage.Count();
			try
			{
				if (itemCount >= startingItem && (itemCount - startingItem) < SharedConstants.PageSize)
				{
					pageData = dataToPage.GetRange(startingItem, itemCount - startingItem);
				}
				else
				{
					pageData = dataToPage.GetRange(startingItem, SharedConstants.PageSize);
				}

			}
			catch
			{
				startingItem = 0 + (--page * SharedConstants.PageSize);
				pageData = dataToPage.GetRange(startingItem, SharedConstants.PageSize);
			}
			successfulPage = page;
			return pageData;
		}
	}
}
