namespace Butterfly.Util
{
    using System;
    using System.Text;

    internal static class OldEncoding
    {
        internal static int decodeVL64(string data)
        {
            return decodeVL64(data.ToCharArray());
        }

        internal static int decodeVL64(char[] raw)
        {
            try
            {
                int index = 0;
                int num2 = 0;
                bool flag = (raw[index] & '\x0004') == 4;
                int num3 = (raw[index] >> 3) & '\a';
                num2 = raw[index] & '\x0003';
                index++;
                int num4 = 2;
                for (int i = 1; i < num3; i++)
                {
                    num2 |= (raw[index] & 0x3f) << num4;
                    num4 = 2 + (6 * i);
                    index++;
                }
                if (flag)
                {
                    num2 *= -1;
                }
                return num2;
            }
            catch
            {
                return 0;
            }
        }

        internal static string encodeVL64(int i)
        {
            byte[] bytes = new byte[6];
            int index = 0;
            int num2 = index;
            int num3 = 1;
            int num4 = (i >= 0) ? 0 : 4;
            i = Math.Abs(i);
            index++;
            bytes[index] = (byte) (0x40 + (i & 3));
            i = i >> 2;
            while (i != 0)
            {
                num3++;
                index++;
                bytes[index] = (byte) (0x40 + (i & 0x3f));
                i = i >> 6;
            }
            bytes[num2] = (byte) ((bytes[num2] | (num3 << 3)) | num4);
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(bytes).Replace("\0", "");
        }
    }
}

