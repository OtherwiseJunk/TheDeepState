﻿using DeepState.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Constants
{
	public class RPGConstants
	{
		public const double NewCharacterCost = 1.00;
		public const string AvatarFolder = "RPGAvatars";
		public static List<string> Names = new List<string>
		{
			"Ash",
			"Cameron",
			"Charlie",
			"Frankie",
			"Morgan",
			"Finley",
			"Jaime",
			"Harley",
			"Sidney",
			"Jackie",
			"Alex",
			"Avery",
			"Devin",
			"Dylan",
			"Ezra",
			"Francis",
			"Hayden",
			"Jessie",
			"Keegan",
			"Max",
			"Noel",
			"Quinn",
			"Rene",
			"Ripley",
			"Sam",
			"Wren"
		};
		public static List<string> titles = new List<string>
		{
			"Unknowable",
			"Playful",
			"Romantic",
			"Terrific",
			"Horrible",
			"Great",
			"Wicked",
			"Wild",
			"Prescient",
			"Satiric",
			"Sweet",
			"Florid",
			"Vivid",
			"Esoteric",
			"Paranoid",
			"Hypothetical",
			"Humorous",
			"Short King",
			"Beanpole",
			"Weird",
			"Puerile",
			"Joyous",
			"Colorful",
			"Impulsive",
			"Extravagent",
			"Idle",
			"Recognizable",
			"Bizarre",
			"Feathered",
			"Zany",
			"Crazy",
			"Ridiculous",
			"Thoroughly Odd",
			"Wonderful",
			"Hot",
			"Generic",
			"Gritty",
			"Cold",
			"Mostly Heroic",
			"Poignant",
			"Sensitive",
			"Ancient",
			"Glossy",
			"Deeply Problematic",
			"Lighthearted",
			"Decadent",
			"Chimerical",
			"Musical",
			"Witty",
			"Messianic",
			"Serious",
			"Wide-Ranging",
			"Ambiguous",
			"Confused",
			"Depraved"
		};
		public static List<string> ObituaryTitles = new List<string>
		{
			"Look what the cat dragged in",
			"Hey kids you wanna see a dead body?",
			"F's in the chat for this one",
			"new corpse who this",
			"Fuck they owed me like 20 Libcoin too...",
			"They will be missed. I assume.",
			"They will be missed. Probably.",
			"They will be missed. Right?",
			"They won't be missed.",
			"Ok who wants to tell their kids",
			"Taken too soon",
			"Taken too late",
			"RIP Bozo. Rest in piss you won't be missed."
		};
		public static List<int[]> StartingStatArrays = new List<int[]>
		{
			new int[]{ 1, 2, 3 },
			new int[]{ 2, 1, 3 },
			new int[]{ 3, 2, 1 },
			new int[]{ 3, 1, 2 },
			new int[]{ 1, 3, 2 },
			new int[]{ 2, 3, 1 },
			new int[]{ 1, 2, 3 },
			new int[]{ 2, 1, 3 },
			new int[]{ 3, 2, 1 },
			new int[]{ 3, 1, 2 },
			new int[]{ 1, 3, 2 },
			new int[]{ 2, 3, 1 },
			new int[]{ 1, 2, 3 },
			new int[]{ 2, 1, 3 },
			new int[]{ 3, 2, 1 },
			new int[]{ 3, 1, 2 },
			new int[]{ 1, 3, 2 },
			new int[]{ 2, 3, 1 },
			new int[]{ 2, 2, 3 },
			new int[]{ 3, 2, 2 },
			new int[]{ 2, 3, 2 },
		};
		public static List<int[]> LevelUpArrays = new List<int[]>
		{
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 1, 0 },
			new int[]{ 0, 0, 1 },
			new int[]{ 1, 0, 0 },
			new int[]{ 0, 2, 0 },
			new int[]{ 0, 0, 2 },
			new int[]{ 2, 0, 0 },
		};

		#region Items
		public static Item StrangeMeat = new HealingPotion
		{
			Name = "Strange Meat",
			Description = "Heals a character for 1 or 2 points of health. Smells a bit... Human-y.",
			Uses = 1,
			HealingDiceSize = 2,
			Price = 0
		};
		public static Item SmallHealingPotion = new HealingPotion
		{
			Name = "Small Healing Potion",
			Description = "Heals a character for 1 to 6 points of health. Smells and tastes awful, but you can't argue with the results..",
			Uses = 1,
			HealingDiceSize = 6,
			Price = 2
		};
		#endregion
	}
}
