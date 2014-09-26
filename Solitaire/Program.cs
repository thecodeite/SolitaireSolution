using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solitaire.Models;

namespace Solitaire
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.Deal();

            while (game.Quit == false)
            {
                Console.Clear();
                Console.WriteLine(game.Render());
                Console.Write(Game.Prompt);

                var userCommand = Console.ReadLine();
                game.ParseInput(userCommand);
            }

        }
    }
}
