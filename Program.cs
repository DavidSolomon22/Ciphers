using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Ciphers
{
    internal class Program
    {
        private enum Operation
        {
            Encryption,
            Decryption
        };

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
                Console.WriteLine("\n\n--------------- Choose which cipher you want to use ---------------\n");
                Console.WriteLine("1. Playfair");
                Console.Write("> ");
                string cipher = Console.ReadLine()?.Replace(" ", "").ToUpper();

                switch (cipher)
                {
                    case "1":
                    case "PLAYFAIR":
                        
                        try
                        {
                            var operation = OperationType();
                            var key = SelectKey();
                            var input = InputType();
                            Playfair(operation, input, key);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    default:
                        Console.WriteLine("\nERROR: INVALID SELECTION. PLEASE SELECT NUMBER OR NAME OF CIPHER.");
                        break;
                }
            }
        }

        private static Operation OperationType()
        {
            Console.WriteLine("\nChoose type of operation:\n");
            Console.WriteLine("1. Encryption");
            Console.WriteLine("2. Decryption");
            Console.Write("> ");
            string operation = Console.ReadLine()?.Replace(" ", "").ToUpper();

            switch (operation)
            {
                case "1":
                case "ENCRYPTION":
                    return Operation.Encryption;
                case "2":
                case "DECRYPTION":
                    return Operation.Decryption;
                default:
                    throw new Exception("\nERROR: INVALID SELECTION. PLEASE SELECT NUMBER OR NAME OF OPERATION.");
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

        private static string SelectKey()
        {
            Console.WriteLine("\nType in key: ");
            Console.Write("> ");
            var key = Console.ReadLine();
            if (key != null && key.Length != 5)
            {
                throw new Exception("\nERROR: KEY MUST HAVE 5 LETTERS.");
            }

            return key;
        }

        private static void Playfair(Operation operation, Input input, string key)
        {
            var playfair = new Playfair();

            switch (input)
            {
                case Input.Keyboard:
                {
                    switch (operation)
                    {
                        case Operation.Encryption:
                        {
                            Console.WriteLine("\nType in plaintext: ");
                            Console.Write("> ");
                            var plaintext = Console.ReadLine();
                            var ciphertext = playfair.Encrypt(plaintext, key);
                            Console.WriteLine("\nCiphertext: " + ciphertext);
                            break;
                        }
                        case Operation.Decryption:
                        {
                            Console.WriteLine("\nType in ciphertext: ");
                            Console.Write("> ");
                            var ciphertext = Console.ReadLine();
                            var plaintext = playfair.Decrypt(ciphertext, key);
                            Console.WriteLine("\nPlaintext: " + plaintext);
                            break;
                        }
                    }
                    break;
                }
                case Input.File:
                {
                    switch (operation)
                    {
                        case Operation.Encryption:
                        {
                            Console.WriteLine("\nType in file name: ");
                            Console.Write("> ");
                            var fileName = Console.ReadLine();
                            try
                            {
                                var text = ReadFile(fileName);
                                text = Regex.Replace(text, @"\t|\n|\r", " ");
                                Console.WriteLine("\nPlaintext: " + text);
                                var ciphertext = playfair.Encrypt(text, key);
                                Console.WriteLine("\nCiphertext: " + ciphertext);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("\nSuch file does not exist in project folder /Ciphers/bin/Debug");
                            }
                            break;
                        }
                        case Operation.Decryption:
                        {
                            Console.WriteLine("\nType in file name: ");
                            Console.Write("> ");
                            var fileName = Console.ReadLine();
                            try
                            {
                                var text = ReadFile(fileName);
                                Console.WriteLine("\nCiphertext: " + text);
                                var plaintext = playfair.Decrypt(text, key);
                                Console.WriteLine("\nPlaintext: " + plaintext);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("\nSuch file does not exist in project folder /Ciphers/bin/Debug");
                            }
                            break;
                        }
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
