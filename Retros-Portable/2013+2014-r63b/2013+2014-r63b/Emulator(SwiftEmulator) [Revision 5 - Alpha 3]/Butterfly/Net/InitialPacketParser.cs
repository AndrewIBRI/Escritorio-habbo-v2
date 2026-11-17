namespace Butterfly.Net
{
    using SharedPacketLib;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class InitialPacketParser : IDataParser, IDisposable, ICloneable
    {
        public byte[] currentData;

        public event NoParamDelegate PolicyRequest;

        public event NoParamDelegate SwitchParserRequest;

        public object Clone()
        {
            return new InitialPacketParser();
        }

        public void Dispose()
        {
            this.PolicyRequest = null;
            this.SwitchParserRequest = null;
        }

        public void handlePacketData(byte[] packet)
        {
            if ((packet[0] == 60) && (this.PolicyRequest != null))
            {
                this.PolicyRequest();
            }
            else if ((packet[0] != 0x43) && (this.SwitchParserRequest != null))
            {
                this.currentData = packet;
                this.SwitchParserRequest();
            }
        }

        public delegate void NoParamDelegate();
    }
}

