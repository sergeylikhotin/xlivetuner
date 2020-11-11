using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using J2534DotNet;

namespace XLiveTuner
{
    class Program
    {
        static void Main(string[] args)
        {
            J2534Device j2534Device = new J2534Device { FunctionLibrary = "op20pt32.dll" };
            J2534Extended j2534 = new J2534Extended();
            if(!j2534.LoadLibrary(j2534Device))
            {
                Console.WriteLine("Ошибка при загрузке драйвера!");
                Console.ReadLine();
                return;
            }

            //int deviceId = 0;
            //J2534Err openErr = j2534.PassThruOpen(IntPtr.Zero, ref deviceId);



            Console.ReadLine();
        }
    }
}
