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
            APIInfo j2534Driver = APIFactory.GetAPIinfo().First();
            API j2534Api = APIFactory.GetAPI(j2534Driver.Filename);
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

            uint address = 0x060fb2;
            uint length = 4;
            
            j2534Channel.SendMessage(new Message(
                new byte[] {
                    0x00, 0x00,
                    0x07, 0xE0,
                    0x23,
                    (byte)(address >> 16),
                    (byte)(address - (address >> 16 << 16) >> 8),
                    (byte)(address - (address >> 8 << 8)),
                    (byte)length
                },
                (TxFlag)64
            ));
            GetMessageResults Response = j2534Channel.GetMessages(2, 1000*10);
            /*
             * Data format:
             * byte[] {
             * 0x00, 0x00,
             * 0x07, 0xE8,
             * 0x63 - UDS Read Address
             * 0x00, 0x00, - First 2 bytes of data
             * 0x00, 0x20  - Second 2 bytes of data
             */
            Console.ReadLine();
        }
    }
}