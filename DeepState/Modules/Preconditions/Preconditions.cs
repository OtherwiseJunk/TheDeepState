using DeepState.Data.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules.Preconditions
{
	public class RequireDayOfMonthRange : PreconditionAttribute
	{
		private int StartingDayRange { get; set; }
		private int EndingDayRange { get; set; }

		public RequireDayOfMonthRange(int firstDay, int lastDay)
		{
			StartingDayRange = firstDay;
			EndingDayRange = lastDay;
		}
		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			int dayOfMonth = DateTime.Now.Day;
			if (dayOfMonth >= StartingDayRange && dayOfMonth <= EndingDayRange)
			{
				return Task.FromResult(PreconditionResult.FromSuccess());
			}
			return Task.FromResult(PreconditionResult.FromError($"Sorry, this command only works between the {StartingDayRange} and {EndingDayRange} of each month."));
		}
	}

	public class RequireLibcoinBalance : PreconditionAttribute
	{
		private double MinimumLibcoinBalance { get; set; }

		public RequireLibcoinBalance(double minimumLibcoinBalance)
		{
			MinimumLibcoinBalance = minimumLibcoinBalance;
		}

		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			try
			{
				UserRecordsService service = (UserRecordsService)services.GetService(typeof(UserRecordsService));
				double currentBalance = service.GetUserBalance(context.User.Id, context.Guild.Id);
				if (MinimumLibcoinBalance <= currentBalance)
				{
					return Task.FromResult(PreconditionResult.FromSuccess());
				}
				return Task.FromResult(PreconditionResult.FromError($"Sorry, this command costs {MinimumLibcoinBalance.ToString("F8")} libcoins, but you have {currentBalance.ToString("F8")}"));
			}
			catch
			{
				return Task.FromResult(PreconditionResult.FromError("ServiceProvider was empty :-("));
			}
		}
	}
}
