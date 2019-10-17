using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Ciphers.Extensions;

namespace Ciphers
{
    class Playfair
    {
        private static char[,] matrix = new char[5, 5] {
            { 'S', 'Z', 'Y', 'F', 'R' } ,
            { 'A', 'B', 'C', 'D', 'E' } ,
            { 'G', 'H', 'I', 'K', 'L' } ,
            { 'M', 'N', 'O', 'P', 'Q' } ,
            { 'T', 'U', 'V', 'W', 'X' } };

        private static char[,] smallMatrix = new char[5, 5] {
            { 's', 'z', 'y', 'f', 'r' } ,
            { 'a', 'b', 'c', 'd', 'e' } ,
            { 'g', 'h', 'i', 'k', 'l' } ,
            { 'm', 'n', 'o', 'p', 'q' } ,
            { 't', 'u', 'v', 'w', 'x' } };

        //private static char[,] bigMatrix = new char[5, 5];
        //private static char[,] smallMatrix = new char[5, 5];

        public string Encrypt(string input)
        {
            //CreateLetterMatrix2("SZYFR");
            var otherChars = OtherCharsDictionary(ref input);
            FormatPlaintext(ref input);
            var letterPairs = CreateLetterPairs(ref input);
            var matrix = CreateLetterMatrix();
            SwitchLetterPairs(ref letterPairs, ref matrix);
            var concatLetterPairs = ConcatLetterPairs(ref letterPairs);

            return otherChars == null ? concatLetterPairs : InsertOtherChars(ref concatLetterPairs, ref otherChars);
        }

        public string Decrypt(string input)
        {
            var otherChars = OtherCharsDictionary(ref input);
            var letterPairs = CreateLetterPairs(ref input);
            var matrix = ReverseLetterMatrix(CreateLetterMatrix());
            SwitchLetterPairs(ref letterPairs, ref matrix);
            var formattedText = ConcatLetterPairs(ref letterPairs);
            var unformattedText = formattedText.Replace("X", "");

            return otherChars == null ? unformattedText : InsertOtherChars(ref unformattedText, ref otherChars);
        }

        public string ReadFile(string path)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    String text = streamReader.ReadToEnd();
                    Console.WriteLine(text);
                    return text;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("The file could not be read");
                return ex.Message;
            }

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

        private static char[,] CreateLetterMatrix()
        {
            //char[,] matrix = new char[5, 5] {
            //    { 'K', 'A', 'R', 'O', 'L' } ,
            //    { 'M', 'N', 'P', 'Q', 'S' } ,
            //    { 'T', 'U', 'V', 'W', 'X' } ,
            //    { 'Y', 'Z', 'B', 'C', 'D' } ,
            //    { 'E', 'F', 'G', 'H', 'I' } };

            char[,] matrix = new char[5, 5] {
                { 'S', 'Z', 'Y', 'F', 'R' } ,
                { 'A', 'B', 'C', 'D', 'E' } ,
                { 'G', 'H', 'I', 'K', 'L' } ,
                { 'M', 'N', 'O', 'P', 'Q' } ,
                { 'T', 'U', 'V', 'W', 'X' } };

            return matrix;
        }

        //private static void CreateLetterMatrix2(string key)
        //{
        //    //if (key.Length != 5)
        //    //{
        //    //    return null;
        //    //}

        //    var smallKey = key.ToLower();
        //    var bigKey = key.ToUpper();

        //    var bigAlphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        //    var smallAlphabet = bigAlphabet.ToLower();

        //    for (var i = 0; i < 5; i++)
        //    {
        //        smallAlphabet = smallAlphabet.Replace(key[i].ToString(), "");
        //        bigAlphabet = bigAlphabet.Replace(key[i].ToString(), "");
        //        smallMatrix[0, i] = smallKey[i];
        //        bigMatrix[0, i] = bigKey[i];
        //    }

        //    for (var i = 1; i < 5; i++)
        //    {
        //        for (var j = 0; j < 5; j++)
        //        {
        //            smallMatrix[i, j] = smallAlphabet.First();
        //            bigMatrix[i, j] = bigAlphabet.First();
        //            smallAlphabet = smallAlphabet.Remove(0, 1);
        //            bigAlphabet = bigAlphabet.Remove(0, 1);
        //        }
        //    }
        //}

        private static char[,] ReverseLetterMatrix(char[,] matrix)
        {
            Stack stack = new Stack();
            foreach (var element in matrix)
            {
                stack.Push(element);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = (char)stack.Pop();
                }
            }

            return matrix;
        }

        private static void SwitchLetterPairs(ref string[] letterPairs, ref char[,] matrix)
        {
            for (int i = 0; i < letterPairs.Length; i++)
            {
                var firstLetterPos = GetElementPosition(letterPairs[i][0], ref matrix);
                var secondLetterPos = GetElementPosition(letterPairs[i][1], ref matrix);

                if (CheckSameRow(firstLetterPos.y, secondLetterPos.y))
                {
                    var firstOffset = firstLetterPos.x + 1;
                    var secondOffset = secondLetterPos.x + 1;
                    if (firstLetterPos.x == (matrix.GetLength(1) - 1))
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.x == (matrix.GetLength(1) - 1))
                    {
                        secondOffset = 0;
                    }
                    letterPairs[i] = string.Concat(
                        matrix[firstLetterPos.y, firstOffset].ToString(),
                        matrix[secondLetterPos.y, secondOffset].ToString());
                }
                else if (CheckSameColumn(firstLetterPos.x, secondLetterPos.x))
                {
                    var firstOffset = firstLetterPos.y + 1;
                    var secondOffset = secondLetterPos.y + 1;
                    if (firstLetterPos.y == (matrix.GetLength(1) - 1))
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.y == (matrix.GetLength(1) - 1))
                    {
                        secondOffset = 0;
                    }
                    letterPairs[i] = string.Concat(
                        matrix[firstOffset, firstLetterPos.x].ToString(),
                        matrix[secondOffset, secondLetterPos.x].ToString());
                }
                else
                {
                    letterPairs[i] = string.Concat(
                        matrix[firstLetterPos.y, secondLetterPos.x].ToString(),
                        matrix[secondLetterPos.y, firstLetterPos.x].ToString());
                }
            }
        }

        private static void SwitchLetterPairs2(ref string[] letterPairs)
        {
            for (int i = 0; i < letterPairs.Length; i++)
            {
                var firstLetterPos = GetElementPosition(letterPairs[i][0], ref matrix);
                var secondLetterPos = GetElementPosition(letterPairs[i][1], ref matrix);

                if (CheckSameRow(firstLetterPos.y, secondLetterPos.y))
                {
                    var firstOffset = firstLetterPos.x + 1;
                    var secondOffset = secondLetterPos.x + 1;
                    if (firstLetterPos.x == (smallMatrix.GetLength(1) - 1))
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.x == (smallMatrix.GetLength(1) - 1))
                    {
                        secondOffset = 0;
                    }
                    
                    letterPairs[i] = string.Concat(
                        matrix[firstLetterPos.y, firstOffset].ToString(),
                        matrix[secondLetterPos.y, secondOffset].ToString());
                }
                else if (CheckSameColumn(firstLetterPos.x, secondLetterPos.x))
                {
                    var firstOffset = firstLetterPos.y + 1;
                    var secondOffset = secondLetterPos.y + 1;
                    if (firstLetterPos.y == (matrix.GetLength(1) - 1))
                    {
                        firstOffset = 0;
                    }
                    else if (secondLetterPos.y == (matrix.GetLength(1) - 1))
                    {
                        secondOffset = 0;
                    }
                    letterPairs[i] = string.Concat(
                        matrix[firstOffset, firstLetterPos.x].ToString(),
                        matrix[secondOffset, secondLetterPos.x].ToString());
                }
                else
                {
                    letterPairs[i] = string.Concat(
                        matrix[firstLetterPos.y, secondLetterPos.x].ToString(),
                        matrix[secondLetterPos.y, firstLetterPos.x].ToString());
                }
            }
        }

        private static Position GetElementPosition(char letter, ref char[,] matrix)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //if (letter == 'j')
                    //{
                    //    letter = 'i';
                    //}

                    //if (letter == 'J')
                    //{
                    //    letter = 'I';
                    //}
                    //if (letter == smallMatrix[i, j] || letter == bigMatrix[i, j])
                    //{
                    //    return new Position(i, j);
                    //}
                    if (letter == matrix[i, j])
                    {
                        return new Position(i, j);
                    }
                }
            }

            return null;
        }

        //private static Position GetElementPosition2(char letter)
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            if (letter == 'j')
        //            {
        //                letter = 'i';
        //            }

        //            if (letter == 'J')
        //            {
        //                letter = 'I';
        //            }
        //            if (letter == smallMatrix[i, j] || letter == bigMatrix[i, j])
        //            {
        //                return new Position(i, j);
        //            }
        //        }
        //    }

        //    return null;
        //}

        private static bool CheckSameColumn(int x1, int x2)
        {
            return x1 == x2;
        }

        private static bool CheckSameRow(int y1, int y2)
        {
            return y1 == y2;
        }

        private Dictionary<char, ArrayList> OtherCharsDictionary(ref string input)
        {
            var dictionary = new Dictionary<char, ArrayList>();
            var array = new ArrayList();
            if (!input.Contains(" ")) return null;

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

        private static string ConcatLetterPairs(ref string[] letterPairs)
        {
            return letterPairs.Aggregate("", string.Concat);
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
