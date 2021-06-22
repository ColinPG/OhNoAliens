/* HighScoreScreen.cs
 * Description: HighScoreScreen is a class that inherits from the SubMenuScreen class. 
 * It is a Screen that displays a list of information about the players with the highest scores.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// HighScoreScreen: A class that inherits from the SubMenuScreen class. 
    /// It is a Screen that displays a list of information about the players with the highest scores.
    /// </summary>
    class HighScoreScreen : SubMenuScreen
    {
        private Game1 game;
        private const string filepath = "Content/highscores.txt";

        private Score[] highScoresStats;
        private List<object> namesList;
        private List<object> killsList;
        private List<object> wavesList;
        private List<object> totalList;
        private MenuItemList namesMenuList;
        private MenuItemList killsMenuList;
        private MenuItemList wavesMenuList;
        private MenuItemList totalMenuList;

        private const int SCORES_TO_KEEP = 5;
        private const int SPACE_BETWEEN_MENUITEMS = 30;
        private const int PULSE_AMOUNT = 0; // No pulsing
        private const int PULSE_RATE = 0;

        private MenuItem highScoreTitle;
        private const string HighScoreTitleText = "High Scores";

        /// <summary>
        /// The Primary constructor for the HighScoreScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public HighScoreScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            this.game = (Game1)game;
            //Title at top of the Screen
            highScoreTitle = new MenuItem(game,
                screenManager,
                this.game.PositionOnScreen(0.50f, 0.05f, xOffsetInPixels: -(screenManager.HighLightMenuFont.MeasureString(HighScoreTitleText).X / 2)),
                HighScoreTitleText,
                screenManager.HighLightMenuFont);
            this.Components.Add(highScoreTitle);

            namesList = new List<object>
            { "Name" };
            killsList = new List<object>
            { "Kills" };
            wavesList = new List<object>
            { "Wave" };
            totalList = new List<object>
            { "Total" };
            highScoresStats = new Score[SCORES_TO_KEEP];

            //Read Score Info from txt file
            ReadScoreInfoFromTxt();

            for (int i = 0; i < SCORES_TO_KEEP; i++)
            {
                namesList.Add(highScoresStats[i].Name);
                killsList.Add(highScoresStats[i].Kills);
                wavesList.Add(highScoresStats[i].Wave);
                totalList.Add(highScoresStats[i].Total);
            }

            //Update Menu item Lists
            RebuildMenuItemLists();
            namesMenuList = BuildMenuList(screenManager, namesList, 0.1f);
            killsMenuList = BuildMenuList(screenManager, killsList, 0.3f);
            wavesMenuList = BuildMenuList(screenManager, wavesList, 0.5f);
            totalMenuList = BuildMenuList(screenManager, totalList, 0.7f);
            this.Components.Add(namesMenuList);
            this.Components.Add(killsMenuList);
            this.Components.Add(wavesMenuList);
            this.Components.Add(totalMenuList);
        }

        /// <summary>
        /// BuildMenuList is a method that creates a new MenuItemList instance based on the parameters and returns it.
        /// </summary>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        /// <param name="objectList">The List of objects for the new Item List.</param>
        /// <param name="xPercent">The X location on the screen in a percentage.</param>
        /// <returns>A new MenuItemList object based on the parameters.</returns>
        private MenuItemList BuildMenuList(ScreenManager screenManager, List<object> objectList, float xPercent)
        {
            MenuItemList newList = new MenuItemList(
                parent,
                objectList,
                game.PositionOnScreen(xPercent, 0.15f),
                screenManager.MenuFont,
                screenManager.HighLightMenuFont,
                SPACE_BETWEEN_MENUITEMS,
                PULSE_AMOUNT,
                PULSE_RATE);
            newList.UpdateSelectedIndex(0);
            newList.SetColor(Color.White, Color.DarkRed);
            return newList;
        }

        /// <summary>
        /// ReadScoreInfoFromTxt is a method that attempts to read high scores from a highscores text file.
        /// If not highscores file exists, it will create a new one.
        /// </summary>
        private void ReadScoreInfoFromTxt()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filepath);
                for (int i = 0; i < SCORES_TO_KEEP; i++)
                {
                    highScoresStats[i].Name = reader.ReadLine();
                    highScoresStats[i].Total = Convert.ToInt32(reader.ReadLine());
                    highScoresStats[i].Wave = Convert.ToInt32(reader.ReadLine());
                    highScoresStats[i].Kills = Convert.ToInt32(reader.ReadLine());
                }
                reader.Close();
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                Console.WriteLine("Valid High Score file not found!");
                for (int i = 0; i < SCORES_TO_KEEP; i++)
                {
                    highScoresStats[i].Name = "-----";
                    highScoresStats[i].Total = 0;
                    highScoresStats[i].Wave = 0;
                    highScoresStats[i].Kills = 0;
                }
                Console.WriteLine("Creating new highscore file...");
                SaveScoreInfoToTxt();
                Console.WriteLine("Created.");
            }
        }

        /// <summary>
        /// SaveScoreInfoToTxt is a method that saves the currently loaded High Score information to a 
        /// highscores text file, overwriting one if it exists already.
        /// </summary>
        private void SaveScoreInfoToTxt()
        {
            StreamWriter writer = new StreamWriter(filepath);
            for (int i = 0; i < SCORES_TO_KEEP; i++)
            {
                writer.WriteLine(highScoresStats[i].Name);
                writer.WriteLine(highScoresStats[i].Total);
                writer.WriteLine(highScoresStats[i].Wave);
                writer.WriteLine(highScoresStats[i].Kills);
            }
            writer.Close();
        }

        /// <summary>
        /// RebuildMenuItemLists is a method that rebuilds the menu item lists objects based on the 
        /// currently loaded high score data.
        /// </summary>
        private void RebuildMenuItemLists()
        {
            for (int i = 1; i < SCORES_TO_KEEP; i++)
            {
                namesList[i] = highScoresStats[i - 1].Name;
                killsList[i] = highScoresStats[i - 1].Kills;
                wavesList[i] = highScoresStats[i - 1].Wave;
                totalList[i] = highScoresStats[i - 1].Total;
            }
        }

        /// <summary>
        /// TryToAddHighScore is a method that attempts to add a new high score based a new score passed in as a parameter.
        /// </summary>
        /// <param name="newScore">The Score to be tested.</param>
        /// <returns>Returns true if a new high score has been added. Returns false if no new score has been added.</returns>
        public bool TryToAddHighScore(Score newScore)
        {
            Score bumpedScore = new Score();
            for (int i = 0; i < SCORES_TO_KEEP; i++)
            {
                if (highScoresStats[i].Total < newScore.Total)
                {
                    bumpedScore = highScoresStats[i];
                    highScoresStats[i] = newScore;
                    newScore = bumpedScore;
                }
            }
            if(bumpedScore.Name != null)
            {
                RebuildMenuItemLists();
                SaveScoreInfoToTxt();
                return true;
            }
            return false;
        }
    }
}
