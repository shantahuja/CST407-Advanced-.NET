using Shared_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLibrary
{
    public class Person : IPrintable
    {
        public void PrintEverything()
        {
            Console.WriteLine("Hello John Smith");
        }
    }
}
