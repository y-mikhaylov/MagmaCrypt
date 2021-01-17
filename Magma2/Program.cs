using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

/*5 лр Процедуры разворачивания ключа, 
зашифрования и расшифрования блока в шифре 
ГОСТ Р 34.12-2015 с длиной блока 64 бита (Магма).*/
namespace Magma2
{
    class Program
    {
        static uint CycleShiftLeft(uint n, int step)
        {
            uint n1, n2;
            uint mask = (uint)(Math.Pow(2, 32) - Math.Pow(2, 31) - 1);
            for (int i = 0; i < step; i++)
            {
                n1 = (n & mask) << 1;
                n2 = n >> 32 - 1;
                n = n1 | n2;
            }
            return n;
        }
        static uint g(uint k, uint a0)
        {
            UInt32 result = a0 + k;
            result = Magma.t(result);
            result = CycleShiftLeft(result, 11);

            return result;
        }

        static uint[] Crypt(uint a0, uint a1, uint[] _8bitkeys)
        {
            uint tmp;
            uint[] _8bitkeysrev = _8bitkeys.Reverse().ToArray();

            for (int i = 0; i < 24; i++)
            {
                tmp = a0;
                a0 = g(_8bitkeys[i % 8], a0);
                a0 = a0 ^ a1;
                a1 = tmp;
            }
            for (int i = 24; i < 32; i++)
            {
                tmp = a0;
                a0 = g(_8bitkeysrev[i % 8], a0);
                a0 = a0 ^ a1;
                a1 = tmp;
            }
            return new uint[] { a1, a0 };
        }


        static uint[] Decrypt(uint a0, uint a1, uint[] _8bitkeys)
        {
            uint tmp;
            uint[] _8bitkeysrev = _8bitkeys.Reverse().ToArray();

            for (int i = 0; i < 8; i++)
            {
                tmp = a0;
                a0 = g(_8bitkeys[i % 8], a0);
                a0 = a0 ^ a1;
                a1 = tmp;
            }
            for (int i = 8; i < 32; i++)
            {
                tmp = a0;
                a0 = g(_8bitkeysrev[i % 8], a0);
                a0 = a0 ^ a1;
                a1 = tmp;
            }
            return new uint[] { a1, a0 };
        }

        static void Main(string[] args)
        {
            Console.Write("Введите ключ в 16чной системе счисления (256 бит): ");
            string strkey = Console.ReadLine();
            BigInteger key = BigInteger.Parse(strkey, System.Globalization.NumberStyles.HexNumber);

            uint[] _8bitkeys = new uint[8];

            for (int i = 0; i < _8bitkeys.Length; i++)
            {
                _8bitkeys[7 - i] = (uint)(key & 0xffffffff); //последние 32 бита
                key = key >> 32;
            }
            Console.Write("Введите сообщение в 16чной системе счисления (64 бита): ");
            string stra = Console.ReadLine();
            BigInteger a = BigInteger.Parse(stra, System.Globalization.NumberStyles.HexNumber);

            uint a0 = (uint)(a & 0xffffffff);
            a = a >> 32;
            uint a1 = (uint)(a & 0xffffffff);

            uint[] cres = Crypt(a0, a1, _8bitkeys);
            a0 = cres[0];
            a1 = cres[1];

            Console.WriteLine("Результат шифрования: " + a1.ToString("X") + " " + a0.ToString("X"));

            uint[] dres = Decrypt(a0, a1, _8bitkeys);
            a0 = dres[0];
            a1 = dres[1];

            Console.WriteLine("Результат дешифрования: " + a1.ToString("X") + " " + a0.ToString("X"));

            Console.ReadKey();
        }
    }
}
