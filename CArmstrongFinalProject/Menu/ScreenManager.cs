/* AboutScreen.cs
 * Description: ScreenManager is a class that inherits from the DrawableGameComponent class. 
 * It contains and controls GameScreens, and shows the currently active GameScreen.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.03: Created
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// ScreenManager: A class that inherits from the DrawableGameComponent class. 
    /// It contains and controls GameScreens, and shows the currently active GameScreen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        private Game parent;

        private GameScreen currentScreen;

        private MainMenuScreen mainMenuScreen;
        private PlayScreen playScreen;
        private HelpScreen helpScreen;
        private SettingsScreen settingsScreen;
        private LoadingScreen loadingScreen;
        private HighScoreScreen highScoreScreen;
        private AboutScreen aboutScreen;
        private GameOverScreen scoreScreen;

        private MenuBackGround menuBackGround;

        private SpriteFont menuFont;
        /// <summary>
        /// Property for the menu SpriteFont.
        /// </summary>
        public SpriteFont MenuFont { get => menuFont; }

        private SpriteFont highLightMenuFont;
        /// <summary>
        /// Property for the highlightMenu SpriteFont.
        /// </summary>
        public SpriteFont HighLightMenuFont { get => highLightMenuFont; }

        private SpriteFont menuDetailFont;
        /// <summary>
        /// Property for the menuDetail SpriteFont.
        /// </summary>
        public SpriteFont MenuDetailFont { get => menuDetailFont; }

        /// <summary>
        /// The Primary constructor for the ScreenManager class.
        /// </summary>
        /// <param name="parent">A reference to the Game class that is the parent of this class.</param>
        public ScreenManager(Game parent): base(parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// LoadContent is an overriden method used to load game assets.
        /// </summary>
        protected override void LoadContent()
        {
            menuBackGround = new MenuBackGround(parent);
            parent.Components.Add(menuBackGround);

            menuFont = parent.Content.Load<SpriteFont>("Fonts/menuRegularFont");
            highLightMenuFont = parent.Content.Load<SpriteFont>("Fonts/menuHilightFont");
            menuDetailFont = parent.Content.Load<SpriteFont>("Fonts/menuDetailFont");

            helpScreen = new HelpScreen(parent, this);
            parent.Components.Add(helpScreen);

            loadingScreen = new LoadingScreen(parent, this);
            parent.Components.Add(loadingScreen);

            settingsScreen = new SettingsScreen(parent, this);
            parent.Components.Add(settingsScreen);

            highScoreScreen = new HighScoreScreen(parent, this);
            parent.Components.Add(highScoreScreen);

            aboutScreen = new AboutScreen(parent, this);
            parent.Components.Add(aboutScreen);

            mainMenuScreen = new MainMenuScreen(parent, this);
            parent.Components.Add(mainMenuScreen);
            currentScreen = mainMenuScreen;
            currentScreen.Show();
            menuBackGround.Show();
            base.LoadContent();
        }

        /// <summary>
        /// ChangeScreen is a method that changes the currently active screen based on parameters.
        /// </summary>
        /// <param name="sender">The GameScreen that is calling this method.</param>
        /// <param name="action">The name of the action to be executed.</param>
        internal void ChangeScreen(GameScreen sender, string action)
        {
            currentScreen.Hide();
            switch (action)
            {
                case "Menu":
                    if (sender is PlayScreen)
                    {
                        ClearPlayScreen(action);
                    }
                    currentScreen = mainMenuScreen;
                    break;
                case "GameOver":
                    ClearPlayScreen(action);
                    scoreScreen = new GameOverScreen(parent, this);
                    parent.Components.Add(scoreScreen);
                    scoreScreen.SetScore((PlayScreen)sender, highScoreScreen);
                    currentScreen = scoreScreen;
                    break;
                case "Start Game":
                    loadingScreen.LoadScreen(action);
                    menuBackGround.Hide();
                    currentScreen = loadingScreen;
                    if (sender is LoadingScreen)
                    {
                        playScreen = new PlayScreen(parent, this);
                        parent.Components.Add(playScreen);
                        currentScreen = playScreen;
                    }
                    break;
                case "Help":
                    currentScreen = helpScreen;
                    break;
                case "High Score":
                    currentScreen = highScoreScreen;
                    break;
                case "Settings":
                    currentScreen = settingsScreen;
                    break;
                case "About":
                    currentScreen = aboutScreen;
                    break;
                case "Quit":
                    parent.Exit();
                    break;
            }
            currentScreen.Show();
        }

        /// <summary>
        /// ClearPlayScreen is a method that resets the play screen and shows the menu background again.
        /// </summary>
        /// <param name="action"></param>
        private void ClearPlayScreen(string action)
        {
            loadingScreen.LoadScreen(action);
            menuBackGround.Show();
            currentScreen = loadingScreen;
            playScreen = null;
        }
    }
}
