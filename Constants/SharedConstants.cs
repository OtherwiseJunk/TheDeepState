using System;
using System.Collections.Generic;
using System.Text;

namespace TheDeepState.Constants
{
	public class SharedConstants
	{
		#region String Formats
		public static string ReplacedMessageFormat(string username, string modifiedMessage) => $"**{username}:** {modifiedMessage}";
		#endregion
	}
}
