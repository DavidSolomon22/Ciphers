using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    class Program
    {
        private enum Operation
        {
            Encryption,
            Decryption,
            Error
        };

        static void Main(string[] args)
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
                        Playfair(OperationType());
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
                    Console.WriteLine("\nERROR: INVALID SELECTION. PLEASE SELECT NUMBER OR NAME OF OPERATION.");
                    break;
            }

            return Operation.Error;
        }

        private static void Playfair(Operation operation)
        {
            var playfair = new Playfair();

            switch (operation)
            {
                case Operation.Encryption:
                {
                    Console.WriteLine("\nType in plaintext: ");
                    Console.Write("> ");
                    var plaintext = Console.ReadLine();
                    //var text = playfair.ReadFile("text.txt");
                    //var ciphertext = playfair.Encrypt(text);
                    var ciphertext = playfair.Encrypt(plaintext);
                    Console.WriteLine("\nCiphertext: " + ciphertext);
                    break;
                }
                case Operation.Decryption:
                {
                    Console.WriteLine("\nType in ciphertext: ");
                    Console.Write("> ");
                    var ciphertext = Console.ReadLine();
                    var plaintext = playfair.Decrypt(ciphertext);
                    Console.WriteLine("\nPlaintext: " + plaintext);
                    break;
                }
            }
        }
    }
}
