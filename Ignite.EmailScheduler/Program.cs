using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.EmailScheduler
{
    class Program
    {
        private static readonly Engine engine = new Engine();

        static void Main(string[] args)
        {
            engine.Start();
            Console.ReadLine();
            engine.Stop();
        }
    }
}
