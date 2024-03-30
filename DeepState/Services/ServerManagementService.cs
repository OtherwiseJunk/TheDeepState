using DartsDiscordBots.Modules.ServerManagement.Interfaces;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepState.Services
{
    internal class ServerManagementService : IServerManagmentService
    {
        public Regex hexColorValidator { get; set; } = new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");

        public async Task<IRole> ModifyUserRoleColor(string hexText, IUser user, ICommandContext Context)
        {
            Color roleColor = ParseDiscordColor(hexText);
            IRole usernameRole;
            int topPosition = Context.Guild.Roles.Count - 2;
            if (RoleExists(Context.Guild.Roles, user.Username, out usernameRole))
            {
                try
                {
                    await usernameRole.ModifyAsync(x =>
                    {
                        x.Color = roleColor;
                    });
                }
                catch
                {
                    await Context.User.SendMessageAsync("Sorry, something went wrong when applying the role color.");
                }
                try
                {
                    await usernameRole.ModifyAsync(x =>
                    {
                        x.Position = topPosition;
                    });
                }
                catch
                {
                    await Context.User.SendMessageAsync("Sorry, I created the role but I don't have permission to move its position, so your color might not change :shrug:");
                }
            }
            else
            {
                usernameRole = Context.Guild.CreateRoleAsync(user.Username, null, roleColor, false, null).Result;
                await usernameRole.ModifyAsync(x => x.Position = topPosition);
            }
            return usernameRole;
        }
        public Color ParseDiscordColor(string hexText)
        {
            string Hex = hexText.Substring(1);
            string B;
            string G;
            string R;
            if (hexText.Length == 7)
            {
                R = Hex.Substring(0, 2);
                G = Hex.Substring(2, 2);
                B = Hex.Substring(4, 2);
            }
            else
            {
                R = Hex.Substring(0, 1) + Hex.Substring(0, 1);
                G = Hex.Substring(1, 1) + Hex.Substring(1, 1);
                B = Hex.Substring(2, 1) + Hex.Substring(2, 1);
            }

            return new Color(int.Parse(R, System.Globalization.NumberStyles.HexNumber), int.Parse(G, System.Globalization.NumberStyles.HexNumber), int.Parse(B, System.Globalization.NumberStyles.HexNumber));
        }

        public bool RoleExists(IReadOnlyCollection<IRole> roles, string username, out IRole usernameRole)
        {
            foreach (IRole role in roles)
            {
                if (role.Name == username)
                {
                    usernameRole = role;
                    return true;
                }
            }
            usernameRole = null;
            return false;
        }
    }
}
