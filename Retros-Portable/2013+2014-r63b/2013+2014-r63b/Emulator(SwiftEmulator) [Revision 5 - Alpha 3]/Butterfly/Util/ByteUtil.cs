namespace Butterfly.Util
{
    using System;

    internal static class ByteUtil
    {
        internal static byte[] ChompBytes(byte[] bzBytes, int Offset, int numBytes)
        {
            int length = Offset + numBytes;
            if (length > bzBytes.Length)
            {
                length = bzBytes.Length;
            }
            int num2 = length - numBytes;
            if (numBytes > bzBytes.Length)
            {
                numBytes = bzBytes.Length;
            }
            if (numBytes < 0)
            {
                numBytes = 0;
            }
            byte[] buffer = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                Offset++;
                buffer[i] = bzBytes[Offset];
            }
            return buffer;
        }
    }
}

