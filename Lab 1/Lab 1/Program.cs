using Shared_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly sampleAssembly;
            sampleAssembly = Assembly.LoadFrom("../../../ExternalLibrary/bin/debug/ExternalLibrary.dll");
            // Obtain a reference to a method known to exist in assembly.
            // MethodInfo method = sampleAssembly.GetTypes()[0].GetMethod("PrintEverything");
            object print = sampleAssembly.CreateInstance("ExternalLibrary.Person");
            //method.Invoke(print, null);
            IPrintable p = print as IPrintable;
            p.PrintEverything();

            Assembly secretAssembly;
            secretAssembly = Assembly.LoadFrom("Lab1SecretCode.Patched.dll");
            // Obtain a reference to a method known to exist in assembly.
            IEnumerable<Type> types = from type in secretAssembly.GetTypes()
                                      select type;
            Type[] secretTypesArray = types.ToArray();

            MethodInfo [] method = secretTypesArray[0].GetMethods();

            // foreach and console writeline every entry in secretTypesArray and print the FullName property

            //foreach (Type type1 in secretTypesArray)
            //{
            //    Console.WriteLine(type1);
            //    Console.WriteLine(type1.FullName);
            //}

            object printSecret = secretAssembly.CreateInstance("Lab1SecretCode.Lab1Secret");

            foreach (MethodInfo method1 in method)
            {
                Console.WriteLine(method1);
            }

            String message = "secret";


            Console.WriteLine(method[0].Invoke(printSecret, new object[] { message }));
            FieldInfo flagInfo = secretTypesArray[0].GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => x.Name == "flag");
            int flag = (int)flagInfo.GetValue(printSecret);
            flagInfo.SetValue(printSecret, 99);
            Console.WriteLine(method[1].Invoke(printSecret, null));


            Console.WriteLine(method[2].Invoke(printSecret, null));




            //sampleAssembly.GetExportedTypes();

            //method.Invoke(print, null);


            Console.ReadLine();


        }
    }
}
