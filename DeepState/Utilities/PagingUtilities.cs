using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Utilities
{
	public static class PagingUtilities
	{
		public static Embed TributeEmbedPagingCallback(IMessage msg, IServiceProvider serviceProvider, int currentPage, bool incrementPage)
		{
			HungerGamesService service = ((HungerGamesService)serviceProvider.GetService(typeof(HungerGamesService)));
			IChannel channel = msg.Channel;
			IGuild guild = ((IGuildChannel)channel).Guild;
			List<HungerGamesTribute> tributes;
			if (incrementPage)
			{
				tributes = service.GetPagedTributeList(guild.Id, out currentPage, ++currentPage);
			}
			else
			{
				tributes = service.GetPagedTributeList(guild.Id, out currentPage, --currentPage);
			}

			return HungerGameUtilities.BuildTributeEmbed(tributes, currentPage, guild);
		}

		public static Embed OpenRequestsPaginingCallback(IMessage msg, IServiceProvider serviceProvider, int currentPage, bool incrementPage)
		{
			ModTeamRequestService service = ((ModTeamRequestService)serviceProvider.GetService(typeof(ModTeamRequestService)));
			IChannel channel = msg.Channel;
			IGuild guild = ((IGuildChannel)channel).Guild;
			List<ModTeamRequest> requests;
			if (incrementPage)
			{
				requests = service.GetOpenModTeamRequestPage(guild.Id, out currentPage, ++currentPage);
			}
			else
			{
				requests = service.GetOpenModTeamRequestPage(guild.Id, out currentPage, --currentPage);
			}

			return service.BuildRequestsEmebed(requests, currentPage, guild, true);
		}

		public static Embed ClosedRequestsPaginingCallback(IMessage msg, IServiceProvider serviceProvider, int currentPage, bool incrementPage)
		{
			ModTeamRequestService service = ((ModTeamRequestService)serviceProvider.GetService(typeof(ModTeamRequestService)));
			IChannel channel = msg.Channel;
			IGuild guild = ((IGuildChannel)channel).Guild;
			List<ModTeamRequest> requests;
			if (incrementPage)
			{
				requests = service.GetClosedModTeamRequestPage(guild.Id, out currentPage, ++currentPage);
			}
			else
			{
				requests = service.GetClosedModTeamRequestPage(guild.Id, out currentPage, --currentPage);
			}

			return service.BuildRequestsEmebed(requests, currentPage, guild, false);
		}
	}
}
