using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Game
    {
        private const string BoardHeader = " ColumnNames   S[T]ack        [1] [2] [3] [4] [5] [6] [7] [D] [H] [C] [S]";
        private const string BoardBreaker = "-------------------------------------------------------------------------";

        public Game()
        {
            Reset();
        }

        public Card Stack { get; private set; }

        public Dictionary<int, List<Card>> Columns;  

        public void Reset()
        {

            // Make it look exactly like the example
            Stack = new Card(Suits.Diamond, Ordinals.Two, isFaceDown: false);
            
            Columns = new Dictionary<int, List<Card>>();

            /* HA  **  **  **  **  **  **                 
                                  sK  **  **  **  **  **                 
                                      D8  **  **  **  **                 
                                          c3  **  **  **                 
                                              D6  **  **                 
                                                  cJ  **                 
                                                      cQ    */

           

            for (int i = 1; i <= 7; i++)
            {
                Columns[i] = new List<Card>();

                var cardsFaceDown = Enumerable.Range(0, i - 1).Select(x => new Card());
                Columns[i].AddRange(cardsFaceDown);


            }
        }

        public string Render()
        {
            var builder = new StringBuilder();

            // Builder header
            builder.AppendLine(BoardHeader);
            builder.AppendLine(BoardBreaker);

            var highestRow = 7; // Hard coded to 7 for now, can be larger.

            // Render rows
            for (var rowIndex = 1; rowIndex <= highestRow; rowIndex++)
            {
                // Print blanks at start
                builder.Append(new string(' ', 19));

                // On the first row, render the stack
                if (rowIndex == 1)
                {
                    builder.Append(Stack.Render());
                }
                else
                {
                    builder.Append("  ");
                }

                builder.Append(new string(' ', 9));

                for (var columnIndex = 1; columnIndex < 7; columnIndex++)
                {
                    builder.Append(RenderColum(columnIndex, rowIndex));
                    builder.Append(' ');
                }

               
                builder.Append(RenderComplete(Suits.Diamond, rowIndex));
                builder.Append(' ');

                builder.Append(RenderComplete(Suits.Heart, rowIndex));
                builder.Append(' ');

                builder.Append(RenderComplete(Suits.Club, rowIndex));
                builder.Append(' ');

                builder.Append(RenderComplete(Suits.Spade, rowIndex));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string RenderComplete(string suit, int rowIndex)
        {
            return "  ";
        }

        private string RenderColum(int columnIndex, int rowIndex)
        {
            if(columnIndex <1 || columnIndex > 7)
                throw new ArgumentException("columnIndex must be in range 1-7", "columnIndex");

            var cards = Columns[columnIndex];

            var card = cards.Skip(rowIndex).FirstOrDefault();
            if (card != null)
            {
                return card.Render();
            }

            return "  ";
        }
    }
}
