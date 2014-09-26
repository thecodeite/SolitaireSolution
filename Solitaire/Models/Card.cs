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

        public bool IsFaceDown { get; private set; }

        public string Render()
        {
            if (IsFaceDown)
            {
                return "**";
            }
            else
            {
                return "??";
            }
        }

        public void Flip()
        {
            IsFaceDown = !IsFaceDown;
        }
    }
}
