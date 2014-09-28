using System.Collections.Generic;
using System.Linq;

namespace Solitaire.Models
{
    public class Pile : List<Card>
    {
        public void MoveTo(Pile targetPile, IEnumerable<Card> cards)
        {
            foreach (var card in cards.ToList())
            {
                Remove(card);
                targetPile.Add(card);
            }
        }
    }
}