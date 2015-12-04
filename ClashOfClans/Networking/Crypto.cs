using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    public class Crypto
    {
        private const string INITIAL_KEY = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";
        private const string INITIAL_NONCE = "nonce";

        private RC4 Encryptor { get; set; }
        private RC4 Decryptor { get; set; }
        public Crypto()
        {
            InitializeCiphers(INITIAL_KEY + INITIAL_NONCE);
        }

        public Crypto(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(nameof(key));
            }

            InitializeCiphers(key);
        }

        public void Encrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentException(nameof(data));

            for(int i = 0; i < data.Length; i++)
            {
                data[i] ^= Encryptor.PRGA();
            }
        }

        public void Decrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentException(nameof(data));

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= Decryptor.PRGA();
            }
        }

        public void UpdateCiphers(ulong clientSeed, byte[] serverNonce)
        {
            if(serverNonce == null)
                throw new ArgumentException(nameof(serverNonce));

            var newNonce = ScrambleNonce(clientSeed, serverNonce);

            var key = INITIAL_KEY + newNonce;

            InitializeCiphers(key);
        }

        public static byte[] CreateRandomByteArray()
        {
            Random random = new Random();

            byte[] buffer = new byte[random.Next(20)];
            random.NextBytes(buffer);
            return buffer;
        }

        private void InitializeCiphers(string key)
        {
            Encryptor = new RC4(key);
            Decryptor = new RC4(key);

            for (int i = 0; i < key.Length; i++) //skip key bytes
            {
                Encryptor.PRGA();
                Decryptor.PRGA();
            }
        }
        private static string ScrambleNonce(ulong clientSeed, byte[] serverNonce)
        {
            var scrambler = new Scrambler(clientSeed);
            var byte100 = 0;
            for (int i = 0; i < 100; i++)
                byte100 = scrambler.GetByte();
            var scrambled = string.Empty;
            for (int i = 0; i < serverNonce.Length; i++)
                scrambled += (char)(serverNonce[i] ^ (scrambler.GetByte() & byte100));
            return scrambled;
        }
        private class RC4
        {
            public RC4(byte[] key)
            {
                Key = KSA(key);
            }

            public RC4(string key)
            {
                Key = KSA(StringToByteArray(key));
            }

            public byte[] Key { get; set; } // "S"

            private byte i { get; set; }
            private byte j { get; set; }

            public byte PRGA()
            {
                /* Pseudo-Random Generation Algorithm
                 * 
                 * The returned value should be XORed with
                 * the data to encrypt or decrypt it.
                 */

                i = (byte)((i + 1) % 256);
                j = (byte)((j + Key[i]) % 256);

                // swap S[i] and S[j];           
                var temp = Key[i];
                Key[i] = Key[j];
                Key[j] = temp;

                return Key[(Key[i] + Key[j]) % 256]; // value to XOR with data
            }

            private static byte[] KSA(byte[] key)
            {
                /* Key-Scheduling Algorithm
                 * 
                 * Used to intialize key array.
                 */

                var keyLength = key.Length;
                var S = new byte[256];

                for (int i = 0; i != 256; i++) S[i] = (byte)i;

                var j = (byte)0;

                for (int i = 0; i != 256; i++)
                {
                    j = (byte)((j + S[i] + key[i % keyLength]) % 256); // meth is working

                    // swap S[i] and S[j];
                    var temp = S[i];
                    S[i] = S[j];
                    S[j] = temp;
                }
                return S;
            }

            private static byte[] StringToByteArray(string str)
            {
                var bytes = new byte[str.Length];
                for (int i = 0; i < str.Length; i++) bytes[i] = (byte)str[i];
                return bytes;
            }
        }

        private class Scrambler
        {
            public Scrambler(ulong seed)
            {
                IX = 0;
                Buffer = SeedBuffer(seed);
            }

            private int IX { get; set; }
            private ulong[] Buffer { get; }

            private static ulong[] SeedBuffer(ulong seed)
            {
                var buffer = new ulong[624];
                for (int i = 0; i < 624; i++)
                {
                    buffer[i] = seed;
                    seed = (1812433253 * ((seed ^ RShift(seed, 30)) + 1)) & 0xFFFFFFFF;
                }
                return buffer;
            }

            public int GetByte()
            {
                var x = (ulong)GetInt();
                if (IsNeg(x)) x = Negate(x);
                return (int)(x % 256);
            }

            private int GetInt()
            {
                if (IX == 0) MixBuffer();
                var val = Buffer[IX];

                IX = (IX + 1) % 624;
                val ^= RShift(val, 11) ^ LShift((val ^ RShift(val, 11)), 7) & 0x9D2C5680;
                return (int)(RShift((val ^ LShift(val, 15L) & 0xEFC60000), 18L) ^ val ^ LShift(val, 15L) & 0xEFC60000);
            }

            private void MixBuffer()
            {
                var i = 0;
                var j = 0;
                while (i < 624)
                {
                    i += 1;
                    var v4 = (Buffer[i % 624] & 0x7FFFFFFF) + (Buffer[j] & 0x80000000);
                    var v6 = RShift(v4, 1) ^ Buffer[(i + 396) % 624];
                    if ((v4 & 1) != 0) v6 ^= 0x9908B0DF;
                    Buffer[j] = v6;
                    j += 1;
                }
            }

            private static ulong RShift(ulong num, ulong n)
            {
                var highbits = (ulong)0;
                if ((num & Pow(2, 31)) != 0) highbits = (Pow(2, n) - 1) * Pow(2, 32 - n);
                return (num / Pow(2, n)) | highbits;
            }

            private static ulong LShift(ulong num, ulong n)
            {
                return (num * Pow(2, n)) % Pow(2, 32);
            }

            private static bool IsNeg(ulong num)
            {
                return (num & (ulong)Math.Pow(2, 31)) != 0;
            }

            private static ulong Negate(ulong num)
            {
                return (~num) + 1;
            }

            private static ulong Pow(ulong x, ulong y)
            {
                return (ulong)Math.Pow(x, y);
            }
        }
    }
}
