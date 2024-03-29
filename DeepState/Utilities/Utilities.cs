﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DeepState.Constants;
using Discord;
using Discord.WebSocket;
using static DeepState.Constants.SharedConstants;

namespace DeepState.Utilities
{
	public static class Utilities
	{
		public static bool PercentileCheck(int successCheck)
		{
			return CreateSeededRandom().Next(1, 100) <= successCheck;
		}
		public static bool IsMentioningMe(SocketMessage discordMessage, SocketSelfUser currentUser)
		{
			IMessage replyingToMessage = discordMessage.Reference != null ? discordMessage.Channel.GetMessageAsync(discordMessage.Reference.MessageId.Value).Result : null;

			if (discordMessage.MentionedUsers.Contains(currentUser))
			{
				return true;
			}
			if (replyingToMessage != null && replyingToMessage.Author.Id == currentUser.Id)
			{
				return true;
			}
			if (Regex.IsMatch(discordMessage.Content, BotProperties.SelfIdentifyingRegex, RegexOptions.IgnoreCase))
			{
				return true;
			}
			return false;
		}

		public static Random CreateSeededRandom()
		{
			return new Random(Guid.NewGuid().GetHashCode());
		}

		public static bool IsSus(string message)
		{
			return Regex.IsMatch(message, SharedConstants.SusRegex, RegexOptions.IgnoreCase);
		}

		public static bool ContainsTwitterLink(string message)
        {
			return Regex.IsMatch(message, SharedConstants.TwitterLinkRegex, RegexOptions.IgnoreCase);
		}

		public static string ReplaceTwitterWithFXTwitter(string message)
        {
			if(message.Contains("x.com", StringComparison.OrdinalIgnoreCase))
			{
				return message.Replace("x.com", "c.vxtwitter.com", StringComparison.OrdinalIgnoreCase);
            }
			return message.Replace("twitter.com", "c.vxtwitter.com", StringComparison.OrdinalIgnoreCase);
		}

		public static Dictionary<PronounConjugations, List<string>> GetUserPronouns(IGuildUser user, IGuild guild)
		{
			Dictionary<PronounConjugations, List<string>> pronouns = new Dictionary<PronounConjugations, List<string>>();
			List<ConfiguredPronouns> pronounsToApply = new List<ConfiguredPronouns>();

			if (null == user)
			{
				pronounsToApply.Add(ConfiguredPronouns.Nongendered);
				pronouns[PronounConjugations.Subjective] = GetSubjectivePronouns(pronounsToApply);
				pronouns[PronounConjugations.Objective] = GetObjectivePronouns(pronounsToApply);
				pronouns[PronounConjugations.Possessive] = GetPossesivePronouns(pronounsToApply);
				pronouns[PronounConjugations.PossessiveAdjective] = GetPossesiveAdjective(pronounsToApply);
				pronouns[PronounConjugations.Reflexive] = GetReflexivePronouns(pronounsToApply);
				return pronouns;
			}

			List<ulong> guildPronounRoles = guild.Roles.Where(role => role.Name.ToLower() == MasculinePronounRoleName || role.Name.ToLower() == FemininePronnounRoleName || role.Name.ToLower() == NongenderedPronounRolename).Select(role => role.Id).ToList();

			List<ulong> pronounRoleIds = user.RoleIds.Where(roleId => guildPronounRoles.Contains(roleId)).ToList();

			if (pronounRoleIds.Count > 0)
			{
				foreach (ulong roleId in pronounRoleIds)
				{
					string roleName = guild.GetRole(roleId).Name.ToLower();
					if (roleName.Contains(MasculinePronounRoleName))
					{
						pronounsToApply.Add(ConfiguredPronouns.Masculine);
					}
					else if (roleName.Contains(FemininePronnounRoleName))
					{
						pronounsToApply.Add(ConfiguredPronouns.Feminine);
					}
					else if (roleName.Contains(NongenderedPronounRolename))
					{
						pronounsToApply.Add(ConfiguredPronouns.Nongendered);
					}
				}
			}
			else
			{
				pronounsToApply.Add(ConfiguredPronouns.Nongendered);
			}

			pronouns[PronounConjugations.Subjective] = GetSubjectivePronouns(pronounsToApply);
			pronouns[PronounConjugations.Objective] = GetObjectivePronouns(pronounsToApply);
			pronouns[PronounConjugations.Possessive] = GetPossesivePronouns(pronounsToApply);
			pronouns[PronounConjugations.PossessiveAdjective] = GetPossesiveAdjective(pronounsToApply);
			pronouns[PronounConjugations.Reflexive] = GetReflexivePronouns(pronounsToApply);

			return pronouns;
		}

	public static List<string> GetSubjectivePronouns(List<ConfiguredPronouns> pronounsToAssign)
	{
		List<string> pronouns = new List<string>();
		foreach (ConfiguredPronouns pronoun in pronounsToAssign)
		{
			switch (pronoun)
			{
				case ConfiguredPronouns.Nongendered:
					pronouns.Add(SharedConstants.SubjectiveNonGenderedPronoun);
					break;
				case ConfiguredPronouns.Feminine:
					pronouns.Add(SharedConstants.SubjectiveFemininePronoun);
					break;
				case ConfiguredPronouns.Masculine:
					pronouns.Add(SharedConstants.SubjectiveMasculinePronoun);
					break;
			}
		}

		return pronouns;
	}

	public static List<string> GetObjectivePronouns(List<ConfiguredPronouns> pronounsToAssign)
	{
		List<string> pronouns = new List<string>();
		foreach (ConfiguredPronouns pronoun in pronounsToAssign)
		{
			switch (pronoun)
			{
				case ConfiguredPronouns.Nongendered:
					pronouns.Add(SharedConstants.ObjectiveNonGenderedPronoun);
					break;
				case ConfiguredPronouns.Feminine:
					pronouns.Add(SharedConstants.ObjectiveFemininePronoun);
					break;
				case ConfiguredPronouns.Masculine:
					pronouns.Add(SharedConstants.ObjectiveMasculinePronoun);
					break;
			}
		}

		return pronouns;
	}

	public static List<string> GetPossesivePronouns(List<ConfiguredPronouns> pronounsToAssign)
	{
		List<string> pronouns = new List<string>();
		foreach (ConfiguredPronouns pronoun in pronounsToAssign)
		{
			switch (pronoun)
			{
				case ConfiguredPronouns.Nongendered:
					pronouns.Add(SharedConstants.PossesiveNonGenderedPronoun);
					break;
				case ConfiguredPronouns.Feminine:
					pronouns.Add(SharedConstants.PossesiveFemininePronoun);
					break;
				case ConfiguredPronouns.Masculine:
					pronouns.Add(SharedConstants.PossesiveMasculinePronoun);
					break;
			}
		}

		return pronouns;
	}

	public static List<string> GetPossesiveAdjective(List<ConfiguredPronouns> pronounsToAssign)
	{
		List<string> pronouns = new List<string>();
		foreach (ConfiguredPronouns pronoun in pronounsToAssign)
		{
			switch (pronoun)
			{
				case ConfiguredPronouns.Nongendered:
					pronouns.Add(SharedConstants.PossessiveAdjectiveNonGenderedPronoun);
					break;
				case ConfiguredPronouns.Feminine:
					pronouns.Add(SharedConstants.PossessiveAdjectiveFemininePronoun);
					break;
				case ConfiguredPronouns.Masculine:
					pronouns.Add(SharedConstants.PossessiveAdjectiveMasculinePronoun);
					break;
			}
		}

		return pronouns;
	}

	public static List<string> GetReflexivePronouns(List<ConfiguredPronouns> pronounsToAssign)
	{
		List<string> pronouns = new List<string>();
		foreach (ConfiguredPronouns pronoun in pronounsToAssign)
		{
			switch (pronoun)
			{
				case ConfiguredPronouns.Nongendered:
					pronouns.Add(SharedConstants.ReflexiveNonGenderedPronoun);
					break;
				case ConfiguredPronouns.Feminine:
					pronouns.Add(SharedConstants.ReflexiveFemininePronoun);
					break;
				case ConfiguredPronouns.Masculine:
					pronouns.Add(SharedConstants.ReflexiveMasculinePronoun);
					break;
			}
		}

		return pronouns;
	}
}
}
