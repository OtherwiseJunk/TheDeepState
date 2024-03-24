using DeepState.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Utilities;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Handlers
{
	public static class OnComponentInteractionHandlers
	{
		public static async Task ProgressiveSharesDistributionHandler(SocketMessageComponent component, IServiceProvider serviceProvider)
		{
			string[] componentIdParts = component.Data.CustomId.Split(':');
			if (componentIdParts.Length != 4 || component.Message.Embeds.Count != 1)
			{
				return;
			}
			string operation = componentIdParts[0];
			double distributionAmount = double.Parse(componentIdParts[1]);
			ulong distributingUserId = ulong.Parse(componentIdParts[2]);
			double distributionMaximum = double.Parse(componentIdParts[3]);
			int succesfulPage;
			int currentPage = int.Parse(component.Message.Embeds.First().Footer.ToString());

			UserRecordsService _service = (UserRecordsService)serviceProvider.GetService(typeof(UserRecordsService));
			IGuild guild = ((IGuildChannel)component.Message.Channel).Guild;

			List<UserRecord> activeUsers = _service.GetActiveUserRecords(guild);
			UserRecord triggeringUserRecord = activeUsers.FirstOrDefault(u => u.DiscordUserId == distributingUserId);
			List<UserProgressiveShare> shares;
			if (triggeringUserRecord != null)
			{
				activeUsers.Remove(triggeringUserRecord);
			}

			switch (operation)
			{
				case PagedEmbedConstants.ProgressiveDistributionPreviousPage:
					if (currentPage != 0)
					{
						shares = _service.GetPagedProgressiveShares(activeUsers, distributionAmount, distributionMaximum, out succesfulPage, --currentPage);
						_ = component.UpdateAsync(msg =>
						{
							msg.Embed = LibcoinUtilities.BuildProgressiveSharesEmbed(shares, succesfulPage, guild);
						});
					}
					else
					{
						_ = component.DeferAsync();
					}
					break;
				case PagedEmbedConstants.ProgressiveDistributionNextPage:
					shares = _service.GetPagedProgressiveShares(activeUsers, distributionAmount, distributionMaximum, out succesfulPage, ++currentPage);
					_ =component.UpdateAsync(msg =>
					{
						msg.Embed = LibcoinUtilities.BuildProgressiveSharesEmbed(shares, succesfulPage, guild);
					});
					break;
			}

			
		}
	}
}
