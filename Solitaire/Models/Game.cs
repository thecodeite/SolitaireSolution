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
        public  const string Prompt = "Enter a move: ";

        public Game()
        {
            Clear();
        }

        public bool Quit { get; private set; }

        public List<Card> Stack { get; private set; }
        public List<Card> WastePile { get; private set; }

        public Dictionary<string, List<Card>> DiscardPiles { get; private set; }
        public Dictionary<int, List<Card>> Columns { get; private set; }

        public void Clear()
        {
            Stack = new List<Card>();
            WastePile = new List<Card>();
            Columns = Enumerable.Range(1, 7).ToDictionary(x => x, x => new List<Card>());
            DiscardPiles = Suits.AllSuits.ToDictionary(x => x, x => new List<Card>());
        }

        public void Deal(Deck deck = null)
        {
            // If no deck is provided, get a random, shuffled deck
            deck = deck ?? Deck.MakeShuffledDeck();

            // Init data strcutures
            Clear();
            
            // Deal the columns, one less each time and the first card face up.
            for (int rowIndex = 0; rowIndex < 7; rowIndex++)
            {
                for (int columnIndex = rowIndex+1; columnIndex <= 7; columnIndex++)
                {
                    var card = deck.TakeCard();
                    var isFirstCardOfRow = (columnIndex == rowIndex + 1);
                    card.IsFaceDown = !isFirstCardOfRow;
                        
                    Columns[columnIndex].Add(card);
                }
            }

            // Flip the rest of the cards over and make them into the stack
            deck.Flip();
            Stack.AddRange(deck.Cards);            
        }

        public void Cheat()
       { 
            Clear();
            foreach (var suit in Suits.AllSuits)
            {
                foreach (var ordinal in Ordinals.AllOrdinals)
                {
                    DiscardPiles[suit].Add(new Card(suit, ordinal, isFaceDown: false));
                }
            }
        }

        public string Render()
        {
            var builder = new StringBuilder();

            // Builder header
            builder.AppendLine(BoardHeader);
            builder.AppendLine(BoardBreaker);

            var highestRow =  Columns.Select(x => x.Value.Count)
                .Concat(DiscardPiles.Select(x => x.Value.Count))
                .Max();

            // Render rows
            for (var rowIndex = 1; rowIndex <= highestRow; rowIndex++)
            {
                // Print blanks at start
                builder.Append(new string(' ', 19));

                // On the first row, render the top card of the stack (if any)
                if (rowIndex == 1 && Stack.Any())
                {
                    builder.Append(Stack.First().Render());
                }
                else
                {
                    builder.Append("  ");
                }

                builder.Append(new string(' ', 8));

                for (var columnIndex = 1; columnIndex <= 7; columnIndex++)
                {
                    builder.Append(' ');
                    builder.Append(RenderColum(columnIndex, rowIndex));
                    builder.Append(' ');
                }

                new []{Suits.Diamond, Suits.Heart, Suits.Club, Suits.Spade}
                    .ToList().ForEach(suit =>
                    {
                        builder.Append(' ');
                        builder.Append(RenderDiscardPile(suit, rowIndex));
                        builder.Append(' ');
                    });

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string RenderDiscardPile(string pileSuit, int rowIndex)
        {
            var pile = DiscardPiles[pileSuit];
            var card = pile.Skip(rowIndex-1).FirstOrDefault();

            return card == null ? "  " : card.Render();
        }

        private string RenderColum(int columnIndex, int rowIndex)
        {
            if(columnIndex <1 || columnIndex > 7)
                throw new ArgumentException("columnIndex must be in range 1-7", "columnIndex");

            var cards = Columns[columnIndex];

            var card = cards.Skip(rowIndex-1).FirstOrDefault();
            if (card != null)
            {
                return card.Render();
            }

            return "  ";
        }

        private void MoveCardsOffSTack()
        {
            WastePile.AddRange(Stack.Take(3));
            Stack = Stack.Skip(3).ToList();
        }

        public void ParseInput(string userCommand)
        {
            // Not in spec, will check wih client.
            userCommand = userCommand.ToUpperInvariant();

            if (userCommand == "Q")
            {
                Quit = true;
                return;
            }

            if (userCommand == "N")
            {
                Clear();
                Deal();
                return;
            }

            if (userCommand == "T")
            {
                MoveCardsOffSTack();
                return;
            }
        }
    }
}
