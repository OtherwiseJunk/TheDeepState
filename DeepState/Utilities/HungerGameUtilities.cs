using DartsDiscordBots.Utilities;
using DeepState.Constants;
using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DeepState.Constants.SharedConstants;
using Utils = DeepState.Utilities.Utilities;

namespace DeepState.Utilities
{
	public enum CauseOfDeathCategories
	{
		Tribute,
		TributeTeamup,
		Environmental
	}
	public class HungerGameUtilities
	{
		#region Lists
		//70% chance for tribute kill, 10% chance for tribute teamup, 20% chance for environmental kills
		public static List<CauseOfDeathCategories> ProbableCauses = new List<CauseOfDeathCategories> {
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Tribute,
			CauseOfDeathCategories.Environmental,
			CauseOfDeathCategories.Environmental,
			CauseOfDeathCategories.Environmental
		};
		#endregion
		public static Embed BuildTributeEmbed(List<HungerGamesTribute> tributes, int currentPage, IGuild guild)
		{
			EmbedBuilder embed = new EmbedBuilder();
			embed.Title = HungerGameConstants.HungerGameTributesEmbedTitle;
			foreach (HungerGamesTribute tribute in tributes)
			{
				IGuildUser user = guild.GetUserAsync(tribute.DiscordUserId).Result;
				embed.AddField(user.Nickname ?? user.Username, "Status: Alive");
				embed.WithFooter($"{currentPage}");
			}

			return embed.Build();
		}

		public static void DailyEvent(HungerGamesService service, IDiscordClient client)
		{
			foreach (HungerGamesServerConfiguration config in service.GetAllConfigurations())
			{
				List<HungerGamesTribute> tributes = service.GetTributeList(config.DiscordGuildId);

				HungerGamesTribute victim = tributes.GetRandom();

				IGuild guild = client.GetGuildAsync(config.DiscordGuildId).Result;
				IGuildUser victimUser = guild.GetUserAsync(victim.DiscordUserId).Result;

				Dictionary<PronounConjugations, List<string>> pronouns = Utils.GetUserPronouns(victimUser, guild);
				string causeOfDeath = GetCauseOfDeathDescription(victimUser, guild, tributes, pronouns);
			}
		}

		public static string GetCauseOfDeathDescription(IGuildUser victim, IGuild guild, List<HungerGamesTribute> tributes, Dictionary<PronounConjugations, List<string>> victimPronounsByConjugation)
		{
			List<HungerGamesTribute> usualSuspects = tributes.Where(t => t.DiscordUserId != victim.Id).ToList();
			string goreyDetails = "";
			switch (ProbableCauses.GetRandom())
			{
				case CauseOfDeathCategories.Tribute:
					HungerGamesTribute murderer = usualSuspects.GetRandom();
					IGuildUser murdererUser = guild.GetUserAsync(murderer.DiscordUserId).Result;
					goreyDetails = GetTributeKillDetails(murdererUser,victimPronounsByConjugation);
					break;
				case CauseOfDeathCategories.TributeTeamup:
					//TODO. Don't want to bother with Tribute Teamups for the first round.
					break;
				case CauseOfDeathCategories.Environmental:
					goreyDetails = GetEnvironmentalKillDetails(victimPronounsByConjugation);
					break;
			}
			return goreyDetails;
		}

		public static string GetTributeKillDetails(IGuildUser murderer, Dictionary<PronounConjugations, List<string>> victiomPronounsByConjugation)
		{
			Random rand = Utils.CreateSeededRandom();
			string murdererName = murderer.Nickname ?? murderer.Username;
			List<string> tributeKillDetails = new List<string>
			{
				$"{murdererName} took {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} by clonking {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} with the Sunday Edition of the New York Times.",
				$"{murdererName} punched {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} clean in half! Shit was crazy.",
				$"Strangled in {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} sleep by {murdererName}.",
				$"Sacrified on an altar to Dagon, by {murdererName} seeking favor from The Old Gods.",
				$"{murdererName} accidentally crushed {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} head underfoot while {victiomPronounsByConjugation[PronounConjugations.Subjective]} hid in a pile of leaves.",
				$"Ripped in half by {murdererName}. {murdererName} just kind of grabbed {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} by either buttcheek and tore. Fucking Brutal, yanno?",
				$"{murdererName} jammed a beehive on {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} head and watched them run off a cliff.",
				$"{murdererName} conjured a massive Fireball and chucked it at {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()}.",
				$"{victiomPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} got dropped into an active volcano by a murder of crows, trained by {murdererName}",
				$"{murdererName} offered a teamup, then poisoned {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} food that evening.",
				$"{murdererName} shot {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} in the head {rand.Next(17,69)} times. I thought for sure {victiomPronounsByConjugation[PronounConjugations.Subjective].GetRandom()} might make it, but that last bullet really did {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} in.",
				$"After a particularly sick burn by {murdererName}, {victiomPronounsByConjugation[PronounConjugations.Subjective].GetRandom()} continued to insist 'im not owned! im not owned!!', as they slowly shrunk and transform into a cob of corn.",
				$"Telefragged by {murdererName}. Such a mess...",
				$"{murdererName} R1'd {victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom()} real quick, into non-existance. BainCapitalist shed a single, pride-filled tear.",
				$"Impaled through the chest from behind by {murdererName} just as {victiomPronounsByConjugation[PronounConjugations.Subjective].GetRandom()} got to the climax of {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} monologue. Hate it when that happens."
			};
			//add 5 "chances" for generic random tribute weapon kills.
			tributeKillDetails.AddRange(Enumerable.Repeat(HungerGameConstants.RandomTributeWeaponKill, 5));

			string goreyDetails = tributeKillDetails.GetRandom();

			if(goreyDetails != HungerGameConstants.RandomTributeWeaponKill)
			{
				return goreyDetails;
			}

			return String.Format(HungerGameConstants.RandomTributeWeaponKillFormat, murdererName, victiomPronounsByConjugation[PronounConjugations.Objective].GetRandom(), HungerGameConstants.Weapons.GetRandom());
		}

		public static string GetEnvironmentalKillDetails(Dictionary<PronounConjugations, List<string>> victimPronounsByConjugation)
		{
			Random rand = Utils.CreateSeededRandom();

			List<string> environmentalKillDetails = new List<string>
			{
				"Smote by God for being far, far, too horny.",
				$"Attacked by a roving pack of enraged twitter users, who cancelled {victimPronounsByConjugation[PronounConjugations.Objective].GetRandom()}, permanently.",
				"Chased off a cliff by a swarm of Japanese Murder Hornets.",
				$"Tripped while descending down a steep slope, breaking {rand.Next(47,206)} bones.",
				$"Shot by Donald Trump Jr., who took {victimPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} head and pelt as trophies.",
				"Burned alive by an unseasonable wildfire.",
				"Struck by lightning.",
				//$"Fatally injured by {victiomPronounsByConjugation[PronounConjugations.PossessiveAdjectiveAdjective].GetRandom()} malfunctioning {}",
				"Died of Cancer. Bad luck bud.",
				"Pricked to death.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} walked into a cactus whilst trying to escape a Creeper.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} drowned.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} drowned while trying to escape a Drowned Zombie.",
				"Experienced kinetic energy.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} blew up.",
				"Killed by Intentional Game Design.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} hit the ground too hard.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} fell off a ladder.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} fell from a high place.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} fell while climbing.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} fell off some vines.",
				"Impaled on a stalagmite.",
				"Squashed by a falling anvil whilst fighting The Wither.",
				"Skewered by a falling stalactite",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} went up in flames.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} burned to death.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} burnt to a crisp whilst fightinng a Ghast.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} went off with a bang.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} tried to swim in lava. {victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} made a good run of it, honestly.",
				"Discovered the floor was, in fact, lava.",
				$"A Wizard Killed {victimPronounsByConjugation[PronounConjugations.Objective].GetRandom()}.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} froze to death.",
				"Stung to death.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} starved to death.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} suffocated in a wall.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} fell out of the world.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} withered away...",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} disappeared when Thanos snapped his fingers.",
				"Roasted by Dragon Breath.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} got shot by a Lost-In-Time Red Coat wielding a musket. Three days later {victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom()} succumbed to an infection.",
				$"Now One With The Universe ({victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} drank 420 gallon of LSD).",
				"Just kinda died 🤷",
				"Was Fashed",
				"Was R1'd",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} drank Bone Hurting Juice. Yum Yum!",
				"Died of Boredom.",
				"McNuked.",
				"Died of cringe.",
				$"{victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom().ToPascalCase()} got ratioed.",
				"Was redistricted out of the ⛈️T H U N D E R D O M E⛈️.",
				$"Julienned by 🇺🇸President Joe Biden's🇺🇸 DEVASTATING LAZER EYES after Joe mistook {victimPronounsByConjugation[PronounConjugations.Objective].GetRandom()} for God.",
				$"{victimPronounsByConjugation[PronounConjugations.Objective].GetRandom().ToPascalCase()} gained insight into the illusory nature of the self and popped out of existence.",
			};

			return environmentalKillDetails.GetRandom();
		}

		public static string GetObituary(Dictionary<PronounConjugations, List<string>> victimPronounsByConjugation)
		{

			Random rand = Utils.CreateSeededRandom();
			List<string> obituaries = new List<string> {
				$"Survived by {victimPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} {rand.Next(69, 420)} cats.",
				$"Survived by a jar of Mayo {victimPronounsByConjugation[PronounConjugations.Subjective].GetRandom()} left in {victimPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} fridge.",
				$"Survived by {victimPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} {rand.Next(1,20)} siblings, both parents, both sets of grand parents, and all 16 of {victimPronounsByConjugation[PronounConjugations.PossessiveAdjective].GetRandom()} great grand parents."
			};

			return obituaries.GetRandom();
		}

		public static Embed BuildTributeDeathEmbed(IGuildUser victim, string goreyDetails, string obituary, int district)
		{
			
			EmbedBuilder embed = new EmbedBuilder();
			string victimName = victim.Nickname ?? victim.Username;
			embed.WithTitle($"Tribute {victimName} has fallen!");
			embed.WithImageUrl(victim.GetAvatarUrl());
			embed.WithColor(Color.DarkPurple);
			embed.WithDescription(obituary);
			embed.AddField("Cause of Death", goreyDetails);
			embed.AddField("District", district);
			return embed.Build();
		}


	}
}
