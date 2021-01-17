using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magma2
{
    public static class Magma
    {
        public static uint HexToUint(string hex)
        {
            return Convert.ToUInt32(hex, 16);
        }

        public static string UintToHex(uint a)
        {
            return a.ToString("X");
        }
        //4 лр Блок замены в шифре ГОСТ Р 34.12-2015 с длиной блока 64 бита (Магма)
        public static uint t(uint block)
        {
            uint[,] p =
            {
                {12, 4, 6, 2, 10, 5, 11, 9, 14, 8, 13, 7, 0, 3, 15, 1 },
                {6, 8, 2, 3, 9, 10, 5, 12, 1, 14, 4, 7, 11, 13, 0, 15 },
                {11, 3, 5, 8, 2, 15, 10, 13, 14, 1, 7, 4, 12, 9, 6, 0 },
                {12, 8, 2, 1, 13, 4, 15, 6, 7, 0, 10, 5, 3, 14, 9, 11 },
                {7, 15, 5, 10, 8, 1, 6, 13, 0, 9, 3, 14, 11, 4, 2, 12 },
                {5, 13, 15, 6, 9, 2, 12, 10, 11, 7, 8, 1, 4, 3, 14, 0 },
                {8, 14, 2, 5, 6, 9, 1, 12, 15, 4, 11, 0, 13, 10, 3, 7 },
                {1, 7, 14, 13, 0, 5, 8, 3, 4, 15, 10, 6, 9, 12, 11, 2 }
            };
            uint tmp;
            for (int i = 7; i >= 0; i--)
            {
                tmp = block >> 28;
                block = block & (uint)(Math.Pow(2, 32) -
                    Math.Pow(2, 31) - Math.Pow(2, 30) - Math.Pow(2, 29) - Math.Pow(2, 28) - 1); //-старшие 4 бита
                block = block << 4;
                block |= p[i, (int)tmp];
            }
            return block;
        }
    }
}