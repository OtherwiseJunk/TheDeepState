using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.Jackbox
{
    public partial class JackboxInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JackboxGames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordGuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JackboxVersion = table.Column<int>(type: "int", nullable: false),
                    MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    MinPlayers = table.Column<int>(type: "int", nullable: false),
                    VotingEmoji = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAudience = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JackboxGames", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GameRatings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    JackboxGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRatings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_GameRatings_JackboxGames_JackboxGameId",
                        column: x => x.JackboxGameId,
                        principalTable: "JackboxGames",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JackboxGames",
                columns: new[] { "ID", "Description", "DiscordGuildId", "HasAudience", "JackboxVersion", "MaxPlayers", "MinPlayers", "Name", "PlayerName", "VotingEmoji" },
                values: new object[,]
                {
                    { 1, "The comedy trivia sensation returns with hundreds of new questions that you can tackle on a night in by yourself or when you’re joined by friends.", 0m, false, 1, 4, 1, "You Don't Know Jack 2015", "Know-It-Alls", ":smirk:" },
                    { 23, "The competitive drawing, create odd inventions to solve bizzare problems. Features Drawing", 0m, true, 5, 8, 3, "Patently Stupid", "Idiots", ":blue_square:" },
                    { 24, "The lyric-writing game, channel your inner MC as a rap battling robot", 0m, true, 5, 8, 3, "Mad Verse City", "Rappers", ":microphone:" },
                    { 25, "The deadliest game show in the Crab Nebula! Fling yourself at bloodthirsty aliens to win millions of Zubabucks! Manual Dexterity Required.", 0m, false, 5, 6, 1, "Zeeple Dome", "Abductees", ":alien:" },
                    { 26, "Try to survive the bizarre new minigames against a trivia-obessed killer.", 0m, true, 6, 8, 1, "Trivia Murder Party 2", "Soon-To-Be-Corpses", ":dagger:" },
                    { 27, "The offbeat personality test game, find out who you really are. (Or at least what your friends think of you.)", 0m, true, 6, 6, 3, "Role Models", "Impressionable Youths", ":bar_chart:" },
                    { 28, "The comedy contest, Craft one-liners for a cruise ship talent show.", 0m, true, 6, 8, 3, "Joke Boat", "Comedians", ":rowboat:" },
                    { 29, "The weird word circus, may the funniest definition win.", 0m, true, 6, 8, 3, "Dictionarium", "Wordcrafters", ":blue_book:" },
                    { 22, "The what-if game, create strange and divisive hypothetical situations.", 0m, true, 5, 8, 3, "Split the Room", "Trolls", ":cat:" },
                    { 30, "The hidden identity game, can you discover the aliens in time? Social Deduction/Lying Game", 0m, false, 6, 10, 4, "Push The Button", "Button-Pushers", ":black_square_button:" },
                    { 32, "The pop culture guessing game, describe your secret prompt with a very limited vocabulary and hope that someone can figure it out in time. It’s a “GOOD” “FUN TIME” “EXPERIENCE.”", 0m, true, 7, 6, 2, "Blather 'Round", "Blathermouths", ":tongue:" },
                    { 33, "The collaborative chaos game, you’re a family of devils working together to survive in suburbia. Can you handle the daily torture of human life?", 0m, true, 7, 8, 3, "The Devils and the Details", "Devils", ":imp:" },
                    { 34, "The drawing fighting game, create absurd characters that will battle over unusual titles. Can you take down the heavy favorite? Features Drawing", 0m, true, 7, 8, 3, "Champ'd Up", "Champs", ":muscle:" },
                    { 35, "The say-anything threequel, a head-to-head battle of the wits as you give hilarious responses to quirky prompts while everyone else votes for their favorite!", 0m, true, 7, 8, 3, "Quiplash 3", "Funny People", ":rofl:" },
                    { 36, "It’s alive! The guessing game with terrible drawings and hilariously wrong answers makes a dynamic return. In this revamped title, players create looping, two-frame animations based on weird and random titles.", 0m, true, 8, 10, 3, "Drawful Animate", "Underpaid Animators", ":pencil2:" },
                    { 37, "Trivia has never been so large! A fantastic, mystical wheel challenges you with a variety of trivia prompts. Winners are awarded slices of the Wheel’s face with a chance to win big with each nail-biting spin. In the end, one player will have their most burning question answered by the great Wheel.", 0m, true, 8, 8, 2, "The Wheel of Enormous Proportions", "Wonderers", ":pie:" },
                    { 38, "Use other people’s words to create unique and funny answers to classic job interview questions. Go head to head to see who scores the job!", 0m, true, 8, 10, 3, "Job Job", "Employees", ":moneybag:" },
                    { 31, "The on-the-spot public-speaking game, give a speech responding to picture slides you’ve never seen before or be the Assistant and approve pictures as fast as you can. Just keep talking whether it makes sense or not.", 0m, true, 7, 8, 3, "Talking Points", "Pundits", ":loud_sound:" },
                    { 21, "The classic pop-culture trivia mash-up returns, full of wild new surprises.", 0m, true, 5, 8, 1, "You Dont Know Jack: Full Stream", "Know-It-Alls", ":nerd:" },
                    { 20, "The one-up art game where you compete to improve the town murals.", 0m, true, 4, 8, 3, "Civic Doodle", "Drawers", ":paintbrush:" },
                    { 19, "The deranged debate match where you place smart bets on stupid arguments", 0m, true, 4, 16, 3, "Bracketeering", "Mad Marchers", ":medal:" },
                    { 2, "The hilarious bluffing game, convince your friends that you know the answers to odd trivia questions OR aim to win the Thumbs Cup with the funniest answer.", 0m, false, 1, 8, 2, "Fibbage XL", "$#%!ing LIars", ":liar:" },
                    { 3, "The first installment of the wildly popular drawing game allows you to draw bizarre doodles on your phone or tablet.", 0m, false, 1, 8, 3, "Drawful", "Doodlers", ":pencil:" },
                    { 4, "Test your vocabulary chops in the racy-as-you-want-it-to-be fill-in-the-blank word game.", 0m, false, 1, 8, 2, "Word Spud", "Potatos", ":potato:" },
                    { 5, "Need a game for a big group? Grab yourself or a crowd and play true-or-false with a timer. Don’t get swatted in this wacky fact-filled game! Grab the original Jackbox Party Pack to experience the storied Jackbox Games franchises in their first form.", 0m, false, 1, 100, 1, "Lie Swatter", "Fly Killers", ":fly:" },
                    { 6, "The runaway hit bluffing game with over 500 brand-new questions, more than 2x the original! Plus new features, like the deFIBrillator!", 0m, true, 2, 8, 2, "Fibbage 2", "$#%!ing LIars", ":confused:" },
                    { 7, "The absurd art auction game where you draw right there on your phone or tablet. Outbid your opponents for weird art pieces – drawn by players themselves – and win this strangely competitive auction game! Don’t be a bidiot!", 0m, false, 2, 6, 3, "Bidiots", "Spenders", ":money_mouth:" },
                    { 8, "The bomb-defusing nailbiter of a party game! As interns at Bomb Corp., you must defuse random bombs in the office in order to keep your jobs. You’ll probably die, but it’ll be good work experience!", 0m, false, 2, 4, 1, "Bomb Corp.", "Defusers", ":bomb:" },
                    { 9, "The hear-larious sound-effects game that will leave you up to your ears in laughter! Cow moo? Huge explosion? Or tiny fart? Which to choose?", 0m, true, 2, 8, 3, "Earwax", "Q-Tippers", ":ear:" },
                    { 10, "The say-anything, gut-busting game, which includes everything in Quiplash, Quip Pack 1, AND over 100 brand-new prompts!", 0m, true, 2, 8, 3, "Quiplash XL", "EXTRA LARGE Funny People", ":joy:" },
                    { 11, "The say-anything sequel, a head-to-head battle of the wits as you give hilarious responses to quirky prompts while everyone else votes for their favorite!", 0m, true, 3, 8, 3, "Quiplash 2", "Funny People", ":laughing:" },
                    { 12, "A deadly quiz show where you match wits with a trivia-obsessed killer.", 0m, true, 3, 8, 1, "Triva Murder Party", "Soon-To-Be-Corpses", ":scream:" },
                    { 13, "The brain-battering data-mining guessing game. Answer questions like 'What percentage of people are stomache sleepers?'", 0m, true, 3, 8, 2, "Gusspionage", "Undercover Guessers", ":spy:" },
                    { 14, "The t-shirt slugfest where you battle your custom t-shirts to the death! Features Drawing", 0m, true, 3, 8, 3, "Tee K.O.", "Silk-screeners", ":shirt:" },
                    { 15, "One of your friends has something to hide in this sneaky game for tricksters.", 0m, true, 3, 6, 3, "Fakin' It", "BIG PHONIES", ":japanese_goblin:" },
                    { 16, "The blanking fun sequel with all-new question types and the game mode Fibbage: Enough About You where you guess the weird facts about your friends.", 0m, true, 4, 8, 2, "Fibbage 3", "$#%!ing LIars", ":face_with_raised_eyebrow:" },
                    { 17, "The web-based frame game where you twist your friends’ “online” comments in hilarious ways.", 0m, true, 4, 8, 3, "Survive the Internet", "NEETs", ":desktop:" },
                    { 18, "The spooky date-a-thon where you message and date fellow monsters with special powers. Social Deduction Game", 0m, true, 4, 7, 3, "Monster Seeking Monsters", "Hot Abominations", ":purple_heart:" },
                    { 39, "A survey game that’s all about YOU! Split into teams and see who can escape from the witch’s lair! Players individually rank their choices to a difficult question, then must guess how the group answered as a whole. How well do you know your friends?!", 0m, true, 8, 10, 2, "The Poll Mine", "Miners", ":pick:" },
                    { 40, "A social deduction game where everyone is both a murderer and a detective. Players doodle all the clues, hiding a letter from their name in the weapon drawings. Can you solve murders while trying to get away with your own?", 0m, true, 8, 8, 4, "Lie Swatter", "Detective/Murderers", ":mag:" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRatings_JackboxGameId",
                table: "GameRatings",
                column: "JackboxGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRatings");

            migrationBuilder.DropTable(
                name: "JackboxGames");
        }
    }
}
