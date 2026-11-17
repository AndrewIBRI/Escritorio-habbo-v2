namespace Butterfly.Util
{
    using System;
    using System.Runtime.InteropServices;

    internal static class WireEncoding
    {
        internal const int MAX_INTEGER_BYTE_AMOUNT = 6;
        internal const byte NEGATIVE = 0x48;
        internal const byte POSITIVE = 0x49;

        internal static int DecodeInt32(byte[] bzData, out int totalBytes)
        {
            int index = 1;
            int num2 = bzData[0] & 3;
            int num3 = 2;
            totalBytes = (bzData[0] >> 3) & 7;
            while (index < totalBytes)
            {
                num2 |= (bzData[index] & 0x3f) << num3;
                index++;
                num3 = 2 + (6 * index);
            }
            if ((bzData[0] & 4) == 4)
            {
                num2 *= -1;
            }
            return num2;
        }

        internal static uint DecodeUInt32(byte[] bzData, out int totalBytes)
        {
            int index = 1;
            int num2 = bzData[0] & 3;
            int num3 = 2;
            totalBytes = (bzData[0] >> 3) & 7;
            while (index < totalBytes)
            {
                num2 |= (bzData[index] & 0x3f) << num3;
                index++;
                num3 = 2 + (6 * index);
            }
            return (uint) num2;
        }

        internal static byte[] EncodeInt32(int i)
        {
            int index = 1;
            int num2 = 1;
            int num3 = (i >= 0) ? 0 : 4;
            if (i < 0)
            {
                i *= -1;
            }
            byte[] buffer4 = new byte[6];
            buffer4[0] = (byte) (0x40 + (i & 3));
            byte[] buffer = buffer4;
            i = i >> 2;
            while (i > 0)
            {
                index++;
                buffer[index] = (byte) (0x40 + (i & 0x3f));
                i = i >> 6;
                num2++;
            }
            buffer[0] = (byte) ((buffer[0] | (num2 << 3)) | num3);
            byte[] buffer2 = new byte[num2];
            for (int j = 0; j < num2; j++)
            {
                buffer2[j] = buffer[j];
            }
            return buffer2;
        }
    }
}

