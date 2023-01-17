using DartsDiscordBots.Utilities;
using DeepState.Constants;
using Discord;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;

namespace DeepState.Utilities
{
    public static class TwitterUtilities
    {
        private static async Task<Embed> GetTweetContentsEmbed(long tweetId, string sendingDiscordUser)
        {
            var twitter = await GetTwitterClient();
            var tweet = (await twitter.TweetsV2.GetTweetAsync(tweetId)).Tweet;
            if(tweet != null)
            {
                var author = (await twitter.UsersV2.GetUserByIdAsync(tweet.AuthorId)).User;
                EmbedBuilder eb = new();
                eb.Title = $"{author.Name} - ({author.Username})".Uwuify();
                eb.Description =  tweet.Text.HTMLDecode().Uwuify();
                eb.ThumbnailUrl = author.ProfileImageUrl;
                eb.WithFooter($"Original discord message sent by: {sendingDiscordUser}".Uwuify());
                eb.WithUrl($"https://twitter.com/{author.Username}/status/{tweetId}");
                if(tweet.Attachments.MediaKeys.Length > 0)
                {
                    Console.WriteLine("Logging all media keys...");
                    foreach(string mediaKey in tweet.Attachments.MediaKeys)
                    {
                        Console.WriteLine(mediaKey);
                    }
                }

                return eb.Build();
            }
            return null;
        }

        private static async Task<TwitterClient> GetTwitterClient()
        {
            string key = Environment.GetEnvironmentVariable("TWITTER_KEY");
            string secret = Environment.GetEnvironmentVariable("TWITTER_SECRET");
            var twitter = new TwitterClient(key, secret);
            return new TwitterClient(key, secret, await twitter.Auth.CreateBearerTokenAsync());
        }

        public static long GetTweetId(string twitterUrl)
        {
            if (twitterUrl.Split('?').Length == 2)
            {
                twitterUrl = twitterUrl.Split('?')[0];
            }
            return long.Parse(twitterUrl.Split('/').Last());
        }

        public static async Task<Embed> GetUwuifiedTwitterEmbed(string twitterUrl, string sendingUserDisplayname)
        {
            long tweetId = TwitterUtilities.GetTweetId(twitterUrl);
            return await TwitterUtilities.GetTweetContentsEmbed(tweetId, sendingUserDisplayname);
        }

        public static bool MessageExclusivelyContainsTweetURL(string message)
        {
            Match match = Regex.Match(message, SharedConstants.TwitterStatusDetector);
            return match.Success && match.Length == message.Length;
        }

        public static bool MessageExclusivelyContainsFlaggedUserTweetURL(string message)
        {
            Match match = Regex.Match(message, SharedConstants.FlaggedTwitterUserDetector,RegexOptions.IgnoreCase);
            return match.Success && match.Length == message.Length;
        }
    }
}
