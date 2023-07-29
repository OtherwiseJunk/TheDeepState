using DartsDiscordBots.Modules.AnimalCrossing.Interfaces;
using DartsDiscordBots.Modules.AnimalCrossing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Services
{
    public class AnimalCrossingDataService : IAnimalCrossingDataService
    {
        public void ChangeTownBorderStatus(ulong mayorDiscordUserId, bool isOpen, string dodoCode = "")
        {
            throw new NotImplementedException();
        }

        public void CloseAllOpenTowns()
        {
            throw new NotImplementedException();
        }

        public List<Town> GetAllTowns(bool includeFruitData = false, bool includeTurnipData = false)
        {
            throw new NotImplementedException();
        }

        public Town GetTownByDiscordUserId(ulong mayorDiscordUserId, bool includeFruitData = false, bool includeTurnipData = false)
        {
            throw new NotImplementedException();
        }

        public Town GetTownById(int townId)
        {
            throw new NotImplementedException();
        }

        public Town GetTownByName(string townName)
        {
            throw new NotImplementedException();
        }

        public void RegisterFruit(ulong mayorDiscordUserId, string fruitName)
        {
            throw new NotImplementedException();
        }

        public void RegisterTown(ulong mayorDiscordUserId, string townName)
        {
            throw new NotImplementedException();
        }

        public void RegisterTurnipBuyPrice(ulong mayorDiscordUserId, int turnipPrice)
        {
            throw new NotImplementedException();
        }

        public void RegisterTurnipSellPrice(ulong mayorDiscordUserId, int turnipPrice)
        {
            throw new NotImplementedException();
        }

        public void RegisterWishListItem(ulong mayorDiscordUserId, string itemName)
        {
            throw new NotImplementedException();
        }

        public void RemoveWishLIstItemById(ulong mayorDiscordUserId, int itemId)
        {
            throw new NotImplementedException();
        }

        public void RemoveWishLIstItemByName(ulong mayorDiscordUserId, string itemName)
        {
            throw new NotImplementedException();
        }

        public void SetHemisphere(ulong mayorDiscordUserId, bool isNorthern)
        {
            throw new NotImplementedException();
        }

        public void SetNativeFruit(ulong mayorDiscordUserId, string fruitName)
        {
            throw new NotImplementedException();
        }

        public void SetRealName(ulong mayorDiscordUserId, string realName)
        {
            throw new NotImplementedException();
        }
    }
}
