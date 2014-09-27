using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        public string Message { get; private set; }

        public Dictionary<string, List<Card>> Locations { get; private set; }

        public List<Card> Stack 
        {
            get { return Locations["T"]; }
            set { Locations["T"] = value; }
        }

        public List<Card> WastePile
        {
            get { return Locations["W"]; }
            set { Locations["W"] = value; }
        }

        public void Clear()
        {
            var columnNames = Suits.AllSuitsUpperCase
                .Concat(Enumerable.Range(1, 7).Select(x => x.ToString()))
                .Concat(new[] {"W", "T"})
                .ToList();

            Locations = columnNames.ToDictionary(x => x, x => new List<Card>());
        }

        public void Deal(Deck deck = null)
        {
            // If no deck is provided, get a random, shuffled deck
            deck = deck ?? Deck.MakeTestDeck();

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
                        
                    Locations[columnIndex.ToString()].Add(card);
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
                    Locations[suit.ToUpperInvariant()].Add(new Card(suit, ordinal, isFaceDown: false));
                }
            }
        }

        public string Render()
        {
            var builder = new StringBuilder();

            // Builder header
            builder.AppendLine(BoardHeader);
            builder.AppendLine(BoardBreaker);

            var highestRow = Locations
                .Where(x => x.Key != "W" && x.Key != "T")
                .Select(x => x.Value.Count)
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

                Suits.AllSuitsUpperCase
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
            var pile = Locations[pileSuit];
            var card = pile.Skip(rowIndex-1).FirstOrDefault();

            return card == null ? "  " : card.Render();
        }

        private string RenderColum(int columnIndex, int rowIndex)
        {
            if(columnIndex <1 || columnIndex > 7)
                throw new ArgumentException("columnIndex must be in range 1-7", "columnIndex");

            var cards = Locations[columnIndex.ToString()];

            var card = cards.Skip(rowIndex-1).FirstOrDefault();
            if (card != null)
            {
                return card.Render();
            }

            return "  ";
        }

        private void MoveCardsOffStack()
        {
            if (Stack.Count <= 3)
            {
                // Add the remaining cards to the waste pile
                WastePile.AddRange(Stack);
                Stack.Clear();

                // move all cards from the waste pile back into the stack
                Stack.AddRange(WastePile);
                WastePile.Clear();
            }
            else
            {
                // Add top three cards from stack to the waste
                WastePile.AddRange(Stack.Take(3));

                // remove those three cards from the stack
                Stack = Stack.Skip(3).ToList();
            }
        }

        public void ParseInput(string userCommand)
        {
            // Not in spec, will check wih client.
            userCommand = userCommand.ToUpperInvariant();
            Message = string.Empty;

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
                MoveCardsOffStack();
                return;
            }

            var validCardColumnCommand = new Regex("^(?<card>[H,D,C,S][A,2-9,T,J,Q,K]) (?<dest>[1-7,D,H,C,S])$");
            var match = validCardColumnCommand.Match(userCommand);

            if (match.Success)
            {
                var card = match.Groups["card"].Value;
                var dest = match.Groups["dest"].Value;
                Message = String.Format("Moving {0} to {1}", card, dest);
                Move(card, dest);
                return;
            }

            Message = "Command is invalid: " + userCommand;
        }

        private void Move(string cardShortHand, string dest)
        {
            // Apply no rules
            
            // Find where the card is currently
            var currentLocation = Locations
                .FirstOrDefault(x => x.Value.Any(y => string.Equals(y.ToShortHand(), cardShortHand, StringComparison.InvariantCultureIgnoreCase)));

            if (currentLocation.Key == null)
            {
                Message = "Could not find card: " + cardShortHand;
                return;
            }

            var card = currentLocation.Value
                .First(x => string.Equals( x.ToShortHand(), cardShortHand, StringComparison.InvariantCultureIgnoreCase));

            var index = currentLocation.Value.IndexOf(card);
            var count = currentLocation.Value.Count - index;
            
            // Do not move child cards if the location is the stack
            if (currentLocation.Key == "T")
            {
                count = 1;
            }

            var cards = currentLocation.Value.GetRange(index, count);

            currentLocation.Value.RemoveRange(index, count);

            var tip = currentLocation.Value.LastOrDefault();
            if (tip != null && tip.IsFaceDown)
            {
                tip.Flip();
            }

            var destination = Locations[dest];
            destination.AddRange(cards);
        }
    }
}
