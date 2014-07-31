using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTFS
{
    /// <summary>
    /// Contains common extensions for this GTFS library.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses an integer number from a string at the given index and with the given length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="idx"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int FastParse(this string value, int idx, int count)
        {
            int result = 0;
            int relIdx = count;
            for (int c = idx; c < idx + count; c++)
            {
                relIdx--;
                if (relIdx == 0)
                {
                    result = result + value[c].FastParse();
                    return result;
                }
                else if (relIdx == 1)
                {
                    result = result +
                        value[c].FastParse() * 10;
                }
                else if (relIdx == 2)
                {
                    result = result +
                        value[c].FastParse() * 100;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "Value string too long, can be max 3 chars long.");
                }
            }
            return result;
        }

        /// <summary>
        /// Parses a number from a char value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FastParse(this char value)
        {
            switch(value)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
            }
            throw new ArgumentOutOfRangeException("value", value.ToString());
        }
    }
}
