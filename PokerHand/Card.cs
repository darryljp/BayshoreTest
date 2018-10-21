using System;
using System.Collections.Generic;
using System.Text;

namespace PokerHand
{
    public class Card
    {
        static private string[] Suits = { "c", "d", "h", "s" };
        static private string[] Cards = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        public string Value { get; set; }
        public string Suit { get; set; }

        public static Card GenerateCard()
        {
            var random = new Random();
            var cardNumber = random.Next(52);
            var card = new Card();
            card.Suit = Suits[cardNumber % 4];
            card.Value = Cards[cardNumber % 13];
            return card;
        }

        public string ToDisplayString()
        {
            return Value + Suit;
        }

        private Card()
        {

        }

        public Card(string card)
        {
            if (ValidateCard(card))
            {
                Value = card.Length == 3 ? card.Substring(0, 2) : card.Substring(0, 1);
                Suit = card.Substring(Value.Length, 1);
            } else
            {
                throw new Exception("Invalid Card");
            }
        }

        private bool ValidateCard(string card)
        {
            bool valid = false;
            if (card.Length == 2 || card.Length == 3)
            {
                var cardValue = card.Length == 3 ? card.Substring(0, 2) : card.Substring(0, 1);
                var suit = card.Substring(cardValue.Length, 1);
                if (InArray(Cards, cardValue) &&
                    InArray(Suits, suit))
                {
                    valid = true;
                }
            }
            return valid;
        }

        private bool InArray(string[] array, string value)
        {
            bool inArray = false;
            foreach (var item in array)
            {
                if (item.ToLower().Equals(value.ToLower()))
                {
                    inArray = true;
                    break;
                }
            }
            return inArray;
        }

        public int CompareCard(Card compareCard)
        {
            int result = 0;
            int thisCardIndex = -1;
            int compareCardIndex = -1;
            for (int index = 0;index < Cards.Length;index++)
            {
                if (this.Value.Equals(Cards[index]))
                {
                    thisCardIndex = index;
                }
                if (compareCard.Value.Equals(Cards[index]))
                {
                    compareCardIndex = index;
                }
            }
            if (thisCardIndex < compareCardIndex)
            {
                result = -1;
            }
            if (thisCardIndex > compareCardIndex)
            {
                result = 1;
            }
            // Else the default is that the cards are equal
            return result;
        }

        public bool IsSequential(Card cardB)
        {
            bool result = false;
            int thisCardIndex = -1;
            int compareCardIndex = -1;
            for (int index = 0; index < Cards.Length; index++)
            {
                if (this.Value.Equals(Cards[index]))
                {
                    thisCardIndex = index;
                }
                if (cardB.Value.Equals(Cards[index]))
                {
                    compareCardIndex = index;
                }
            }
            if (thisCardIndex == (compareCardIndex - 1))
            {
                result = true;
            }
            return result;
        }
    }
}
