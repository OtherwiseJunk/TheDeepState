using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	public class ModTeamRequest
	{
		[Key]
		public int RequestId { get; set; }
		public DateTime CreationDatetime { get; set; }
		public DateTime UpdateDatetime { get; set; }
		public ulong RequestingUserDiscordId { get; set; }
		public ulong DiscordGuildId { get; set; }
		public string Request { get; set; }
		public RequestStatus Status { get; set; }
		public double? Price { get; set; }
		public ulong modifyingModDiscordId { get; set; }
		public string ClosingMessage { get; set; }
	}

	public enum RequestStatus
	{
		Opened,
		Priced,
		Completed,
		Rejected
	}
}
