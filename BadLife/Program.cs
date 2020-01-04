using System;

namespace BadLife
{
    /// <summary>
    /// Implements Conway's Game Of Life badly
    /// https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life on a torus
    /// </summary>
    internal static class Program
    {
        private static int Main()
        {
            try
            {
                World world = World.LoadFromTextFile(@"sample_input.txt");

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine();
                    world.Evolve();
                    world.PrintToConsole();
                    Console.WriteLine();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
                return -1;
            }

            return 0;
        }
    }
}
