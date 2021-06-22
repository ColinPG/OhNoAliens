/* ScoreTracker.cs
 * Description: ScoreTracker.cs is a class file that holds the Score struct and the ScoreTracker class.
 * Score is a struct that contains all values of a single player's score.
 * ScoreTracker is a class that helps track a player's current score 
 * for a game instance.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
namespace CArmstrongFinalProject
{
    /// <summary>
    /// Score is a struct that contains all values of a single player's score.
    /// </summary>
    public struct Score
    {
        /// <summary>
        /// The name of the Player, max 5 characters.
        /// </summary>
        public string Name;
        /// <summary>
        /// The number of waves a player survived.
        /// </summary>
        public int Wave;
        /// <summary>
        /// The number of enemies killed by a player.
        /// </summary>
        public int Kills;
        /// <summary>
        /// The total calculated score.
        /// </summary>
        public int Total;
    }
    /// <summary>
    /// ScoreTracker: A class that helps track a player's current score 
    /// for a game instance.
    /// </summary>
    public class ScoreTracker
    {
        private int enemiesKilled;
        /// <summary>
        /// property for the number of enemies killed by the player in one game instance.
        /// </summary>
        public int EnemiesKilled { get => enemiesKilled; }

        private int waveNumber;
        /// <summary>
        /// Property for the number of waves survived by the player in one game instance..
        /// </summary>
        public int WavesSurvived { get => waveNumber; }

        /// <summary>
        /// Primary constructor for the ScoreTracker class.
        /// </summary>
        public ScoreTracker()
        {
            ResetScore();
        }

        /// <summary>
        /// GetScore is a method that creates and returns a new Score object based on the current 
        /// ScoreTracker's values.
        /// </summary>
        /// <returns>Returns a new Score object based on the current ScoreTracker's values.</returns>
        public Score GetScore()
        {
            return new Score
            {
                Name = "",
                Wave = waveNumber,
                Kills = enemiesKilled,
                Total = CalculateScoreTotal()
            };
        }

        /// <summary>
        /// ResetScore is a method that resets the current number of enemies killed and the 
        /// wave number back to 0.
        /// </summary>
        internal void ResetScore()
        {
            enemiesKilled = 0;
            waveNumber = 0;
        }

        /// <summary>
        /// EnemyKilled is a method that increases the killed enemies counter by one.
        /// </summary>
        internal void EnemyKilled() //In future, it would be possible to add parameter enemytype, to track for different types.
        {
            enemiesKilled++;
        }

        /// <summary>
        /// WaveSurvived is a method that increases the waves survived counter by one.
        /// </summary>
        internal void WaveSurvived()
        {
            waveNumber++;
        }

        /// <summary>
        /// CalculateScoreTotal is a method that calculates a total score based off the 
        /// current number of enemies killed and waves survived.
        /// </summary>
        /// <returns>Returns an integer of a final score value based on a players kills and waves survived.</returns>
        internal int CalculateScoreTotal()
        {
            return (EnemiesKilled * 10) + (WavesSurvived * 100);
        }
    }
}