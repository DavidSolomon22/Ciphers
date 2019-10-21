using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ciphers.Extensions;

namespace Ciphers
{
    internal class Playfair
    {
        private static char[,] _bigMatrix = new char[5, 5];
        private static char[,] _smallMatrix = new char[5, 5];
        private static char[,] _reversedBigMatrix = new char[5, 5];
        private static char[,] _reversedSmallMatrix = new char[5, 5];

        public string Encrypt(string input, string key)
        {
            CreateLetterMatrix(key);
            var otherChars = OtherCharsDictionary(ref input);
            FormatPlaintext(ref input);
            var letterPairs = CreateLetterPairs(ref input);
            SwitchLetterPairs(ref letterPairs, ref _smallMatrix, ref _bigMatrix, false);
            var concatLetterPairs = AggregateLetterPairs(ref letterPairs);

            return otherChars == null ? concatLetterPairs : InsertOtherChars(ref concatLetterPairs, ref otherChars);
        }

        public string Decrypt(string input, string key)
        {
            CreateLetterMatrix(key);
            var otherChars = OtherCharsDictionary(ref input);
            var letterPairs = CreateLetterPairs(ref input);
            ReverseLetterMatrix();
            SwitchLetterPairs(ref letterPairs, ref _reversedSmallMatrix, ref _reversedBigMatrix, true);
            var formattedText = AggregateLetterPairs(ref letterPairs);
            var unformattedText = formattedText.Replace("X", "");

            return otherChars == null ? unformattedText : InsertOtherChars(ref unformattedText, ref otherChars);
        }

        private static void CreateLetterMatrix(string key)
        {
            var smallKey = key.ToLower();
            var bigKey = key.ToUpper();

            var bigAlphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            var smallAlphabet = bigAlphabet.ToLower();

            for (var i = 0; i < 5; i++)
            {
                smallAlphabet = smallAlphabet.Replace(smallKey[i].ToString(), "");
                bigAlphabet = bigAlphabet.Replace(bigKey[i].ToString(), "");
                _smallMatrix[0, i] = smallKey[i];
                _bigMatrix[0, i] = bigKey[i];
            }

            for (var i = 1; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    _smallMatrix[i, j] = smallAlphabet.First();
                    _bigMatrix[i, j] = bigAlphabet.First();
                    smallAlphabet = smallAlphabet.Remove(0, 1);
                    bigAlphabet = bigAlphabet.Remove(0, 1);
                }
            }
        }

        private static void ReverseLetterMatrix()
        {
            Stack stack = new Stack();
            foreach (var element in _smallMatrix)
            {
                stack.Push(element);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _reversedSmallMatrix[i, j] = (char)stack.Pop();
                }
            }
            stack.Clear();

            foreach (var element in _bigMatrix)
            {
                stack.Push(element);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _reversedBigMatrix[i, j] = (char)stack.Pop();
                }
            }
        }

        private static Dictionary<char, ArrayList> OtherCharsDictionary(ref string input)
        {
            var dictionary = new Dictionary<char, ArrayList>();
            var array = new ArrayList();
            if (!input.Contains(" ")) return dictionary;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    array.Add(i);
                }
            }
            dictionary.Add(' ', array);

            return dictionary;
        }

        private static void FormatPlaintext(ref string input)
        {
            var previousLetter = input.First();
            input = input.Replace(" ", string.Empty);
            var length = input.Length;

            for (int i = 1; i < length; i++)
            {
                if (previousLetter == input[i])
                {
                    previousLetter = 'X';
                    input = input.Insert(i, "X");
                    length++;
                }
                else
                {
                    previousLetter = input[i];
                }
            }

            if (length % 2 != 0)
            {
                input = string.Concat(input, "X");
            }
        }

        private static string[] CreateLetterPairs(ref string input)
        {
            input = input.Replace(" ", string.Empty);
            var numberOfPairs = input.Length / 2;
            var letterPairs = new string[numberOfPairs];
            var j = 0;

            for (int i = 0; i < numberOfPairs; i++)
            {
                letterPairs[i] = input.Substring(j, 2);
                j += 2;
            }

            return letterPairs;
        }

        private static void SwitchLetterPairs(ref string[] letterPairs, ref char[,] smallMatrix, ref char[,] bigMatrix, bool isReversed)
        {
            for (var i = 0; i < letterPairs.Length; i++)
            {
                var firstLetterPos = GetElementPosition(letterPairs[i][0], isReversed);
                var secondLetterPos = GetElementPosition(letterPairs[i][1], isReversed);

                var firstLetterSize = char.IsUpper(letterPairs[i][0]);
                var secondLetterSize = char.IsUpper(letterPairs[i][1]);

                if (CheckSameRow(firstLetterPos.Y, secondLetterPos.Y))
                {
                    var firstOffset = firstLetterPos.X + 1;
                    var secondOffset = secondLetterPos.X + 1;
                    if (firstLetterPos.X == 4)
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.X == 4)
                    {
                        secondOffset = 0;
                    }

                    if (firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstLetterPos.Y, firstOffset].ToString(), bigMatrix[secondLetterPos.Y, secondOffset].ToString());
                    }
                    else if (firstLetterSize && !secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstLetterPos.Y, firstOffset].ToString(), smallMatrix[secondLetterPos.Y, secondOffset].ToString());
                    }
                    else if (!firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstLetterPos.Y, firstOffset].ToString(), bigMatrix[secondLetterPos.Y, secondOffset].ToString());
                    }
                    else
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstLetterPos.Y, firstOffset].ToString(), smallMatrix[secondLetterPos.Y, secondOffset].ToString());
                    }
                }
                else if (CheckSameColumn(firstLetterPos.X, secondLetterPos.X))
                {
                    var firstOffset = firstLetterPos.Y + 1;
                    var secondOffset = secondLetterPos.Y + 1;
                    if (firstLetterPos.Y == 4)
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.Y == 4)
                    {
                        secondOffset = 0;
                    }

                    if (firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstOffset, firstLetterPos.X].ToString(), bigMatrix[secondOffset, secondLetterPos.X].ToString());
                    }
                    else if (firstLetterSize && !secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstOffset, firstLetterPos.X].ToString(), smallMatrix[secondOffset, secondLetterPos.X].ToString());
                    }
                    else if (!firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstOffset, firstLetterPos.X].ToString(), bigMatrix[secondOffset, secondLetterPos.X].ToString());
                    }
                    else
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstOffset, firstLetterPos.X].ToString(), smallMatrix[secondOffset, secondLetterPos.X].ToString());
                    }
                }
                else
                {
                    if (firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstLetterPos.Y, secondLetterPos.X].ToString(), bigMatrix[secondLetterPos.Y, firstLetterPos.X].ToString());
                    }
                    else if (firstLetterSize && !secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(bigMatrix[firstLetterPos.Y, secondLetterPos.X].ToString(), smallMatrix[secondLetterPos.Y, firstLetterPos.X].ToString());
                    }
                    else if (!firstLetterSize && secondLetterSize)
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstLetterPos.Y, secondLetterPos.X].ToString(), bigMatrix[secondLetterPos.Y, firstLetterPos.X].ToString());
                    }
                    else
                    {
                        letterPairs[i] = string.Concat(smallMatrix[firstLetterPos.Y, secondLetterPos.X].ToString(), smallMatrix[secondLetterPos.Y, firstLetterPos.X].ToString());
                    }
                }
            }
        }

        private static string AggregateLetterPairs(ref string[] letterPairs)
        {
            return letterPairs.Aggregate("", string.Concat);
        }

        private static Position GetElementPosition(char letter, bool isReversed)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (letter == 'j')
                    {
                        letter = 'i';
                    }
                    else if (letter == 'J')
                    {
                        letter = 'I';
                    }

                    if (isReversed)
                    {
                        if (letter == _reversedSmallMatrix[i, j] || letter == _reversedBigMatrix[i, j])
                        {
                            return new Position(i, j);
                        }
                    }
                    else
                    {
                        if (letter == _smallMatrix[i, j] || letter == _bigMatrix[i, j])
                        {
                            return new Position(i, j);
                        }
                    }
                }
            }

            return null;
        }

        private static bool CheckSameColumn(int x1, int x2)
        {
            return x1 == x2;
        }

        private static bool CheckSameRow(int y1, int y2)
        {
            return y1 == y2;
        }

        private static string InsertOtherChars(ref string input, ref Dictionary<char, ArrayList> otherChars)
        {
            foreach (var pair in otherChars)
            {
                foreach (int index in pair.Value)
                {
                    input = input.Insert(index, pair.Key.ToString());
                }
            }
            return input;
        }
    }
}
