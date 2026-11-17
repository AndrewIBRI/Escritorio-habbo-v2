namespace Butterfly
{
    using System;
    using System.Runtime.InteropServices;

    internal class TextHandling
    {
        internal static int BooleanToInt(bool k)
        {
            return (k ? 1 : 0);
        }

        internal static double Combine(int a, int b)
        {
            return (double) (a + (((float) b) / 100f));
        }

        internal static int GetFirstSiffer(double k)
        {
            return (int) Math.Round((double) ((k % 1.0) * 100.0));
        }

        internal static string GetString(double k)
        {
            return k.ToString(ButterflyEnvironment.cultureInfo);
        }

        internal static int Parse(string a)
        {
            int num = 0;
            int num2 = 0;
            int length = a.Length;
            if (length == 0)
            {
                return 0;
            }
            do
            {
                num2++;
                int num4 = a[num2];
                if ((num4 < 0x30) || (num4 > 0x3b))
                {
                    return 0;
                }
                num = ((10 * num) + num4) - 0x30;
            }
            while (num2 < length);
            return num;
        }

        internal static int Parse2(string a) // Come from R63A BF
        {
            int w = 0, i = 0, length = a.Length, k;

            if (length == 0)
                return 0;

            do
            {
                k = a[i++];
                if (k < 48 || k > 59)
                    return 0;
                w = 10 * w + k - 48;
            }
            while (i < length);

            return w;
        }

        internal static void Split(double k, out int a, out int b)
        {
            b = (int) Math.Round((double) ((k % 1.0) * 100.0));
            a = (((int) Math.Round((double) (k * 100.0))) - b) / 100;
        }
    }
}

