using DeepState.Utilities;
using NUnit.Framework;
using System;
using Utils = DeepState.Utilities.HungerGameUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepState.Data.Models;

namespace DeepState.Tests
{
	class HungerGamesTests
	{
		[Test]
		[TestCase("2022-01-01", EventStage.FirstDayRegistrationPeriod)]
		[TestCase("2022-01-02", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-03", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-04", EventStage.FirstActiveGameDay)]
		[TestCase("2022-01-05", EventStage.ActiveGame)]
		[TestCase("2022-01-06", EventStage.ActiveGame)]
		[TestCase("2022-01-07", EventStage.ActiveGame)]
		[TestCase("2022-01-08", EventStage.ActiveGame)]
		[TestCase("2022-01-09", EventStage.ActiveGame)]
		[TestCase("2022-01-10", EventStage.ActiveGame)]
		[TestCase("2022-01-11", EventStage.FirstDayRegistrationPeriod)]
		[TestCase("2022-01-12", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-13", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-14", EventStage.FirstActiveGameDay)]
		[TestCase("2022-01-15", EventStage.ActiveGame)]
		[TestCase("2022-01-16", EventStage.ActiveGame)]
		[TestCase("2022-01-17", EventStage.ActiveGame)]
		[TestCase("2022-01-18", EventStage.ActiveGame)]
		[TestCase("2022-01-19", EventStage.ActiveGame)]
		[TestCase("2022-01-20", EventStage.ActiveGame)]
		[TestCase("2022-01-21", EventStage.FirstDayRegistrationPeriod)]
		[TestCase("2022-01-22", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-23", EventStage.RegistrationPeriod)]
		[TestCase("2022-01-24", EventStage.FirstActiveGameDay)]
		[TestCase("2022-01-25", EventStage.ActiveGame)]
		[TestCase("2022-01-26", EventStage.ActiveGame)]
		[TestCase("2022-01-27", EventStage.ActiveGame)]
		[TestCase("2022-01-28", EventStage.ActiveGame)]
		[TestCase("2022-01-29", EventStage.ActiveGame)]
		[TestCase("2022-01-30", EventStage.ActiveGame)]
		[TestCase("2022-01-31", EventStage.ActiveGame)]
		public void ValidateEventStageDetermination(DateTime time, EventStage expectedStage)
		{
			Assert.AreEqual(expectedStage,
				Utils.DetermineEventStage(time,
				new List<HungerGamesTribute> {
					new HungerGamesTribute { IsAlive = true },
					new HungerGamesTribute { IsAlive = true }
				}));
		}
		[Test]
		[TestCase("2022-01-01", 9)]
		[TestCase("2022-01-02", 8)]
		[TestCase("2022-01-03", 7)]
		[TestCase("2022-01-04", 6)]
		[TestCase("2022-01-05", 5)]
		[TestCase("2022-01-06", 4)]
		[TestCase("2022-01-07", 3)]
		[TestCase("2022-01-08", 2)]
		[TestCase("2022-01-09", 1)]
		[TestCase("2022-01-10", 0)]
		[TestCase("2022-01-11", 9)]
		[TestCase("2022-01-12", 8)]
		[TestCase("2022-01-13", 7)]
		[TestCase("2022-01-14", 6)]
		[TestCase("2022-01-15", 5)]
		[TestCase("2022-01-16", 4)]
		[TestCase("2022-01-17", 3)]
		[TestCase("2022-01-18", 2)]
		[TestCase("2022-01-19", 1)]
		[TestCase("2022-01-20", 0)]
		[TestCase("2022-01-21", 9)]
		[TestCase("2022-01-22", 8)]
		[TestCase("2022-01-23", 7)]
		[TestCase("2022-01-24", 6)]
		[TestCase("2022-01-25", 5)]
		[TestCase("2022-01-26", 4)]
		[TestCase("2022-01-27", 3)]
		[TestCase("2022-01-28", 2)]
		[TestCase("2022-01-29", 1)]
		[TestCase("2022-01-30", 0)]
		[TestCase("2022-01-31", 0)]
		[TestCase("2022-02-28", 0)]
		[TestCase("2022-02-27", 1)]
		[TestCase("2024-02-29", 0)]
		[TestCase("2024-02-28", 1)]
		public void ValidateCalculatedDaysRemaining(DateTime time, int expectedRemainingDays)
		{
			Assert.AreEqual(expectedRemainingDays, Utils.CalculateDaysRemaining(time));
		}
	}
}
