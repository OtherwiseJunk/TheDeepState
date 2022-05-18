namespace DeepState.Constants
{
	public class BotProperties
	{
		public static string InternalName = "DeepState";
#if DEBUG
		public static char CommandPrefix = '<';
#else
		public static char CommandPrefix = '>';
#endif
		public static readonly string SelfIdentifyingRegex = @"(de*r?p)\s*(sta*te*)";
	}
}
