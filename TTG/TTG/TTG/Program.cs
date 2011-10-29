using System;
using System.Diagnostics;

namespace TTG
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // delete when done
            //PuzzleGrid _grid = new PuzzleGrid(0, 0);
            //Debug.Write(_grid.ToString());
            //

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

