using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    class RSA
    {
        private BigInteger _p;
        private BigInteger _q;
        private BigInteger _n;
        private BigInteger _phi;
        private BigInteger _e;
        private BigInteger _d;
        private string _m;
        private string _c;

        public RSA()
        {
            GenerateKeys();
        }

        private void GenerateKeys()
        {
            var random = new Random();
            _p = BigInteger.genPseudoPrime(8, 100, random);
            _q = BigInteger.genPseudoPrime(8, 100, random);
            _n = _p * _q;
            _phi = (_p - 1) * (_q - 1);
            _e = GenerateCoprimeBigInteger();
            _d = ModInverse(_e, _phi);
        }

        public string Encrypt(string input)
        {
            var output = new char[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                output[i] = EncryptChar(input[i]);
            }

            return new string(output);
        }

        private char EncryptChar(char input)
        {
            var m = new BigInteger(input);
            var c = m.modPow(_e, _n);

            return (char)c.IntValue();
        }

        public string Decrypt(string input)
        {
            var output = new char[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                output[i] = DecryptChar(input[i]);
            }

            return new string(output);
        }

        private char DecryptChar(char input)
        {
            var c = new BigInteger(input);
            var m = c.modPow(_d, _n);

            return (char)m.IntValue();
        }

        private BigInteger GenerateCoprimeBigInteger()
        {
            while (true)
            {
                var random = new Random();
                var primeNumber = BigInteger.genPseudoPrime(16, 100, random);
                if (_phi.gcd(primeNumber) != 1) continue;

                return primeNumber;
            }
        }

        private static BigInteger ModInverse(BigInteger e, BigInteger phi)
        {
            e %= phi;
            for (var i = 1; i < phi; i++)
            {
                if ((e * i) % phi == 1) return i;
            }

            return 1;
        }

    }
}
