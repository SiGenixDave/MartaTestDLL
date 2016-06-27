using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MartaTestEquipment;

namespace DllTest
{
    class Program
    {
        static int retVal;
        static short x;
        
        static void Main(string[] args)
        {
            Console.WriteLine("DLL TEST...");
            Console.WriteLine("");
            retVal = PtuApi.InitCommunication(args[0], "19200");
            Console.WriteLine("Init Communication return value = " + retVal.ToString());
            ComcWrite();
            ComcRead();
        }

        static void ComcWrite()
        {
            ushort[] requestBuffer = new ushort[17];
            ushort[] resultBuffer = new ushort[17];

            requestBuffer[0] = 0x00bb;
            requestBuffer[2] = 0xa5c1;

            retVal = PtuApi.PTU_MVB_Interface(1, requestBuffer);

            Console.WriteLine("ComC Write occurred... Error code = " + retVal.ToString());

            PrintResults(resultBuffer);

        }

        static void ComcRead()
        {
            ushort[] requestBuffer = new ushort[17];
            ushort[] resultBuffer = new ushort[17];

            requestBuffer[0] = 0x00b6;
            requestBuffer[2] = 0xa5c1;

            retVal = PtuApi.PTU_MVB_Interface(0, requestBuffer);

            Console.WriteLine("ComC Read occurred... Error code = " + retVal.ToString());

            for (ushort i = 0; i < 17; i++)
            {
                PtuApi.ReadResultBuffer(ref resultBuffer[i], i);
            }
            PrintResults(resultBuffer);

        }

        static void PrintResults(ushort[] resultsBuffer)
        {
            uint i = 0;
            foreach (ushort res in resultsBuffer)
            {
                Console.WriteLine(res.ToString("X4") + "[" + i.ToString() + "]");
                i++;
            }
        }

    
    }
}
