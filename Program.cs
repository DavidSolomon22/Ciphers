using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Ciphers
{
    internal class Program
    {

        private enum Input
        {
            File,
            Keyboard
        }

        private static void Main()
        {
            Console.WriteLine("----------------------------- CIPHERS -----------------------------");

            while (true)
            {
                var input = InputType();
                Playfair(input);
            }
        }

        private static Input InputType()
        {
            Console.WriteLine("\nChoose type of input:\n");
            Console.WriteLine("1. Keyboard");
            Console.WriteLine("2. File");
            Console.Write("> ");
            string input = Console.ReadLine()?.Replace(" ", "").ToUpper();

            switch (input)
            {
                case "1":
                case "KEYBOARD":
                    return Input.Keyboard;
                case "2":
                case "FILE":
                    return Input.File;
                default:
                    throw new Exception("\nERROR: INVALID SELECTION. PLEASE SELECT NUMBER OR NAME OF INPUT.");
            }
        }

        private static void Playfair(Input input)
        {
            var playfair = new Playfair();

            switch (input)
            {
                case Input.Keyboard:
                {
                    Console.WriteLine("\nType in plaintext: ");
                    Console.Write("> "); var plaintext = Console.ReadLine();
                    var ciphertext = playfair.Encrypt(plaintext, "SZYFR");
                    Console.WriteLine("\nCiphertext: " + ciphertext);
                    break;
                }
                case Input.File:
                {
                    Console.WriteLine("\nType in file name: ");
                    Console.Write("> ");
                    var fileName = Console.ReadLine();
                    try
                    {
                        var text = ReadFile(fileName);
                        text = Regex.Replace(text, @"\t|\n|\r", " ");
                        Console.WriteLine("\nPlaintext: " + text);
                        var ciphertext = playfair.Encrypt(text, "SZYFR");
                        Console.WriteLine("\nCiphertext: " + ciphertext);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nSuch file does not exist in project folder /Ciphers/bin/Debug");
                    }
                    break;
                }
            }
        }

        private static string ReadFile(string path)
        {
            if (!path.Contains(".txt"))
            {
                path += ".txt";
            }
            
            using (var streamReader = new StreamReader(path))
            {
                var text = streamReader.ReadToEnd();
                return text;
            }
        }
    }
}
