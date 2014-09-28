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

        public List<Pile> Locations { get; private set; }

        public Pile Stack 
        {
            get { return Locations.First(x => x.Name == 'T'); }
        }

        public Pile WastePile
        {
            get { return Locations.First(x => x.Name == 'W'); }
        }

        public void Clear()
        {
            var columnNames = Suits.AllSuitsUpperCase.Select(x=>x[0])
                .Concat(Enumerable.Range(1, 7).Select(x => (char)('0' + x)))
                .Concat(new[] {'W', 'T'})
                .ToList();

            Locations = columnNames.Select(x => new Pile(x)).ToList();
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
                        
                    GetLocation(columnIndex).Add(card);
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
                    GetLocation(suit.ToUpperInvariant()).Add(new Card(suit, ordinal, isFaceDown: false));
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
                .Where(x => x.IsColumn || x.IsDiscard)
                .Select(x => x.Count)
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
            var pile = GetLocation(pileSuit);
            var card = pile.Skip(rowIndex-1).FirstOrDefault();

            return card == null ? "  " : card.Render();
        }

        private string RenderColum(int columnIndex, int rowIndex)
        {
            if(columnIndex <1 || columnIndex > 7)
                throw new ArgumentException("columnIndex must be in range 1-7", "columnIndex");

            var cards = GetLocation(columnIndex);

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
                Stack.MoveTo(WastePile, Stack);

                // move all cards from the waste pile back into the stack
                WastePile.MoveTo(Stack, WastePile);
            }
            else
            {
                // Move top three cards to waste pile
                Stack.MoveTo(WastePile, Stack.Take(3));
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
            
            // Find while pile the card is currently in
            var currentLocation = Locations
                .FirstOrDefault(x => x.Any(y => string.Equals(y.ToShortHand(), cardShortHand, StringComparison.InvariantCultureIgnoreCase)));

            if (currentLocation == null)
            {
                Message = "Could not find card: " + cardShortHand;
                return;
            }

            var card = currentLocation
                .First(x => string.Equals( x.ToShortHand(), cardShortHand, StringComparison.InvariantCultureIgnoreCase));

            var index = currentLocation.IndexOf(card);
            var count = currentLocation.Count - index;

            // Do not move child cards unless moving from column to column
            if (!currentLocation.IsColumn)
            {
                count = 1;
            }

            var cards = currentLocation.GetRange(index, count);

            currentLocation.RemoveRange(index, count);

            var tip = currentLocation.LastOrDefault();
            if (tip != null && tip.IsFaceDown)
            {
                tip.Flip();
            }

            var destination = GetLocation(dest);
            destination.AddRange(cards);
        }

        public Pile GetLocation(char pileName)
        {
            return Locations.First(x => x.Name == pileName);
        }

        public Pile GetLocation(string pileName)
        {
            return Locations.First(x => x.Name == pileName[0]);
        }

        public Pile GetLocation(int columnName)
        {
            var pileName = (char)('0' + columnName);
            return Locations.First(x => x.Name == pileName);
        }
    }
}
