using System;

namespace TowerOffense
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TOGame game = new TOGame())
            {
                game.Run();
            }
        }
    }
#endif
}

