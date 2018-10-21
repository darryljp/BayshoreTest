using System;
using System.Collections.Generic;

namespace PokerHand
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter 5 cards for a poker hand, separated by spaces -  in the format (Card)(Suit)");
            Console.WriteLine("ex. Ac Kd Qh Js 10c");
            Console.WriteLine("Alternatively - Hit <Enter> to generate a hand and evaluate it.  Hit <X><Enter> to quit.");
            while (true)
            {
                Console.WriteLine("Enter a poker hand");
                var inputString = Console.ReadLine();
                if (inputString.ToLower() == "x")
                {
                    Console.WriteLine("Exiting");
                    break;
                } else 
                if (inputString == "")
                {
                    inputString = GenerateHand();
                }
                Console.WriteLine("Entered hand: " + inputString);
                var tempHand = inputString.Split(' ');
                var pokerHand = new List<Card>();
                var validHand = tempHand.Length == 5;
                if (!validHand)
                {
                    Console.WriteLine("Invalid poker hand - card count = " + tempHand.Length);
                }
                else
                {
                    try
                    {
                        foreach (var item in tempHand)
                        {
                            SortedAdd(item, pokerHand);
                        }
                        if (validHand)
                        {
                            var handRank = EvaluateHand(pokerHand);
                            Console.WriteLine(handRank);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            } 
        }

        private static string GenerateHand()
        {
            string result = string.Empty;
            for (int index = 0;index < 5;index++)
            {
                var newCard = Card.GenerateCard();
                result += newCard.ToDisplayString() + " ";
            }
            return result.Trim();
        }

        private static void SortedAdd(string item, List<Card> pokerHand)
        {
            var card = new Card(item);
            if (pokerHand.Count == 0)
            {
                pokerHand.Add(card);
            }
            else
            {
                var insertPosition = -1;
                for (int index = 0; index < pokerHand.Count; index++)
                {
                    var compareCard = pokerHand[index];
                    if (card.CompareCard(compareCard) < 0)
                    {
                        insertPosition = index;
                        break;
                    }
                }
                if (insertPosition == -1)
                {
                    pokerHand.Add(card);
                }
                else
                {
                    pokerHand.Insert(insertPosition, card);
                }
            }
        }

        static private string EvaluateHand(List<Card> pokerHand)
        {
            string returnRank = string.Empty;
            var valueCheck = new Dictionary<string, int>();
            var suitCheck = new Dictionary<string, int>();
            foreach(var card in pokerHand)
            {
                if (!valueCheck.ContainsKey(card.Value))
                {
                    valueCheck.Add(card.Value, 1);
                }
                else
                {
                    valueCheck[card.Value]++;
                }
                if (!suitCheck.ContainsKey(card.Suit))
                {
                    suitCheck.Add(card.Suit, 1);
                }
                else
                {
                    suitCheck[card.Suit]++;
                }
            }
            returnRank = GetBestHand(pokerHand, valueCheck, suitCheck);
            return returnRank;
        }

        static private string GetBestHand(List<Card> pokerHand, Dictionary<string, int> valueCheck, Dictionary<string, int> suitCheck)
        {
            var rank = string.Empty;
            string[] ranks = {
                "Five of a kind",
                "Straight flush",
                "Four of a kind",
                "Full house",
                "Flush",
                "Straight",
                "Three of a kind",
                "Two Pair",
                "Pair",
                "High card"
            };
            if (CheckValueCount(valueCheck, 5))
            {
                rank = ranks[0];
            } else
            if (CheckSuitCount(suitCheck, 5) && CheckSequential(pokerHand))
            {
                rank = ranks[1];
            }
            else
            if (CheckValueCount(valueCheck, 4))
            {
                rank = ranks[2];
            }
            else
            if (CheckValueCount(valueCheck, 3) && CheckValueCount(valueCheck, 2))
            {
                rank = ranks[3];
            }
            else
            if (CheckSuitCount(valueCheck, 5))
            {
                rank = ranks[4];
            }
            else
            if (CheckSequential(pokerHand))
            {
                rank = ranks[5];
            }
            else
            if (CheckValueCount(valueCheck, 3))
            {
                rank = ranks[6];
            }
            else
            if (CheckTwoPair(valueCheck))
            {
                rank = ranks[7];
            }
            else
            if (CheckValueCount(valueCheck, 2))
            {
                rank = ranks[8];
            }
            else
            {
                rank = ranks[9];
            }
            return rank;
        }

        static private bool CheckTwoPair(Dictionary<string, int> valueCheck)
        {
            int pairCount = 0;
            foreach (var item in valueCheck)
            {
                if (item.Value == 2)
                {
                    pairCount++;
                }
            }
            return pairCount == 2;
        }

        static private bool CheckSequential(List<Card> pokerHand)
        {
            // Assume success
            bool result = true;
            for (int index = 0;index < pokerHand.Count - 1;index++)
            {
                var cardA = pokerHand[index];
                var cardB = pokerHand[index + 1];
                if (!cardA.IsSequential(cardB))
                {
                    // One exception breaks assumption, result is false
                    result = false;
                    break;
                }
            }
            return result;
        }

        static private bool CheckValueCount(Dictionary<string, int> valueCheck, int checkCount)
        {
            bool result = false;
            foreach (var item in valueCheck)
            {
                if (item.Value == checkCount)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        static private bool CheckSuitCount(Dictionary<string, int> suitCheck, int checkCount)
        {
            bool result = false;
            foreach (var item in suitCheck)
            {
                if (item.Value == checkCount)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
