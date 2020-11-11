using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SAE.J2534;

namespace XLiveTuner
{
    class Program
    {
        static void Main(string[] args)
        {
            API j2534Api = APIFactory.GetAPI("op20pt32.dll");
            Device j2534Device = j2534Api.GetDevice();
            Channel j2534Channel = j2534Device.GetChannel(Protocol.ISO15765, Baud.ISO15765, ConnectFlag.NONE);

            j2534Channel.SetConfig(new[] {new SConfig(Parameter.ISO15765_BS, 32), new SConfig(Parameter.ISO15765_STMIN, 0)});

            MessageFilter filter = new MessageFilter()
            {
                FilterType = Filter.FLOW_CONTROL_FILTER,
                Mask = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF },
                Pattern = new byte[] { 0x00, 0x00, 0x07, 0xE8 },
                FlowControl = new byte[] { 0x00, 0x00, 0x07, 0xE0 }
            };

            j2534Channel.StartMsgFilter(filter);
            j2534Channel.SendMessage(new byte[] { 0x00, 0x00, 0x07, 0xE0, 0x01, 0x00 });
            GetMessageResults Response = j2534Channel.GetMessage();

            Console.ReadLine();
        }
    }
}