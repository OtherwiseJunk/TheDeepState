using DartsDiscordBots.Utilities;
using Discord;
using System;
using System.Threading.Tasks;
using Tweetinvi;

namespace DeepState.Utilities
{
    public static class TwitterUtilities
    {
        public static async Task<Embed> GetTweetContents(long tweetId, string sendingDiscordUser)
        {
            var twitter = await GetTwitterClient();
            var tweet = (await twitter.TweetsV2.GetTweetAsync(tweetId)).Tweet;
            if(tweet != null)
            {
                var author = (await twitter.UsersV2.GetUserByIdAsync(tweet.AuthorId)).User;
                EmbedBuilder eb = new();
                eb.Title = $"{author.Name} - ({author.Username})";
                eb.Description = tweet.Text.Uwuify();
                eb.ThumbnailUrl = author.ProfileImageUrl;
                eb.WithFooter($"Original discord message sent by: {sendingDiscordUser}");
                eb.WithUrl($"https://twitter.com/{author.Username}/status/{tweetId}");

                return eb.Build();
            }
            return null;
        }

        public static async Task<TwitterClient> GetTwitterClient()
        {
            string key = Environment.GetEnvironmentVariable("TWITTER_KEY");
            string secret = Environment.GetEnvironmentVariable("TWITTER_SECRET");
            var twitter = new TwitterClient(key, secret);
            return new TwitterClient(key, secret, await twitter.Auth.CreateBearerTokenAsync());
        }
    }
}
