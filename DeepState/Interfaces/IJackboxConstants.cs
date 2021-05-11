using System.Collections.Generic;

namespace DeepState.Interfaces
{
	public interface IJackboxConstants
	{
		public static Dictionary<int, List<string>> JackboxGameListByNumber { get; set; }
		private static List<string> JackboxOneGames { get; set; }
		private static List<string> JackboxTwoGames { get; set; }
		private static List<string> JackboxThreeGames { get; set; }
		private static List<string> JackboxFourGames { get; set; }
		private static List<string> JackboxFiveGames { get; set; }
		private static List<string> JackboxSixGames { get; set; }
		private static List<string> JackboxSevenGames { get; set; }
	}
}
