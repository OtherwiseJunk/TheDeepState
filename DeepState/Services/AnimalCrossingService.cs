using DartsDiscordBots.Modules.AnimalCrossing.Interfaces;
using DartsDiscordBots.Modules.AnimalCrossing.Models;
using DeepState.Data.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Services
{
    public class AnimalCrossingService : IAnimalCrossingService
    {
        IAnimalCrossingDataService _acData;
        public AnimalCrossingService(IAnimalCrossingDataService acDataService) {
            _acData = acDataService;
        }
        string IAnimalCrossingService.CloseTownBorder(ulong mayorDiscordUserId)
        {
            try
            {
                _acData.ChangeTownBorderStatus(mayorDiscordUserId, false);
                return "Ok, I'll let everyone know so they can stop by!";
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Animal Crossing Service] Encountered exception attempting to close town border: ${ex.Message}");
                return "Sorry, something went wrong trying to set your town as open.";
            }
        }

        Embed IAnimalCrossingService.GetFruitList(List<IGuildUser> users)
        {
            EmbedBuilder builder = new();
            List<ulong> userIds = users.Select(guild => guild.Id).ToList();
            builder.WithTitle("Town Fruits");
            builder.WithThumbnailUrl("https://dodo.ac/np/images/9/92/Fruit_Basket_NH_Icon.png");
            foreach(Town town in _acData.GetAllTowns(includeFruitData: true))
            {
                bool includeRealName = userIds.Contains(town.MayorDiscordId) && !String.IsNullOrEmpty(town.MayorRealName);
                bool hasNativeFruitSet = !String.IsNullOrEmpty(town.NativeFruit);

                string townFruitFieldTitle = includeRealName ? $"{town.TownName} - {town.MayorRealName}" : town.TownName;
                string townFruitFieldValue = hasNativeFruitSet ? $"" : String.Join(", ", town.Fruits);

                builder.AddField(townFruitFieldTitle, townFruitFieldValue);
            }

            return builder.Build();
        }

        Town IAnimalCrossingService.GetTown(int townId)
        {
            throw new NotImplementedException();
        }

        Town IAnimalCrossingService.GetTown(string townName)
        {
            throw new NotImplementedException();
        }

        Town IAnimalCrossingService.GetTown(ulong mayorDiscordUserId)
        {
            throw new NotImplementedException();
        }

        Embed IAnimalCrossingService.GetTownList(List<IGuildUser> users)
        {
            throw new NotImplementedException();
        }

        Embed IAnimalCrossingService.GetTurnipPricesForWeek(ulong mayorDiscordUserId)
        {
            throw new NotImplementedException();
        }

        Embed IAnimalCrossingService.GetTurnipStats(ulong mayorDiscordUserId)
        {
            throw new NotImplementedException();
        }

        Embed IAnimalCrossingService.GetWishlist(List<IGuildUser> users)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.OpenTownBorder(ulong mayorDiscordUserId, string dodoCode)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RegisterFruit(ulong mayorDiscordUserId, string fruitName)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RegisterTown(ulong mayorDiscordUserId, string townName)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RegisterTurnipBuyPrice(ulong mayorDiscordUserId, int turnipPrice)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RegisterTurnipSellPrice(ulong mayorDiscordUserId, int turnipPrice)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RegisterWishlistItem(ulong mayorDiscordUserId, string itemName)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RemoveWishlistItemById(ulong mayorDiscordUserId, int itemId)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.RemoveWishlistItemByName(ulong mayorDiscordUserId, string itemName)
        {
            throw new NotImplementedException();
        }

        void IAnimalCrossingService.SendTurnipPriceList(List<IGuildUser> users, ICommandContext context)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.SetHemisphere(ulong mayorDiscordUserId, bool isNorthern)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.SetNativeFruit(ulong mayorDiscordUserId, string fruitName)
        {
            throw new NotImplementedException();
        }

        string IAnimalCrossingService.SetRealName(ulong mayorDiscordUserId, string realName)
        {
            throw new NotImplementedException();
        }

        bool IAnimalCrossingService.UserHasTown(ulong mayorDiscordUserId)
        {
            throw new NotImplementedException();
        }
    }
}
