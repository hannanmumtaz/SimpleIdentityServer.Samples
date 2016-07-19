using System;

namespace RfidReader
{
    class Program
    {

        static void Main(string[] args)
        {
            RfidWatcher.Start();
            while(true)
            {
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }
}
