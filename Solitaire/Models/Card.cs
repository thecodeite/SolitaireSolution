using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Card
    {
        public Card()
        {
            IsFaceDown = true;
        }

        public bool IsFaceDown { get; set; }

        public string Render()
        {
            return "**";
        }
    }
}
