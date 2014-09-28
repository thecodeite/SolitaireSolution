using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Solitaire.Models
{
    public class Pile : List<Card>
    {
        private readonly char _name;

        public Pile(char name)
        {
            _name = name;
        }

        public void MoveTo(Pile targetPile, IEnumerable<Card> cards)
        {
            foreach (var card in cards.ToList())
            {
                Remove(card);
                targetPile.Add(card);
            }
        }

        public bool IsColumn
        {
            get { return char.IsDigit(_name); }
        }

        public decimal Name
        {
            get { return _name; }
        }

        public int? Column
        {
            get
            {
                if (IsColumn)
                {
                    return _name - '0';
                }

                return null;
            }
        }

        public bool IsDiscard
        {
            get
            {
                return _name == 'H' ||
                       _name == 'D' ||
                       _name == 'S' ||
                       _name == 'C';
            }
        }
    }
}