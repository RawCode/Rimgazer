using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleApplication1
{

    public static class MethodUtil
    {

        public static void ReplaceMethod(MethodBase source, MethodBase dest)
        {
            IntPtr destAdr = GetMethodAddress(dest);
            IntPtr srcAdr = GetMethodAddress(source);
            unsafe
            {
                uint* d = (uint*)destAdr.ToPointer();
                *d = *((uint*)srcAdr.ToPointer());
            }
        }

        public static IntPtr GetMethodAddress(MethodBase method)
        {
            // Prepare the method so it gets jited
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            // If 3.5 sp1 or greater than we have a different layout in memory.
            unsafe
            {
                return new IntPtr(((int*)method.MethodHandle.Value.ToPointer() + 2));
            }

        }
    }

    public static class DefDatabase
    {
        public static void AX()
        {
            Console.WriteLine("AX");
        }

        public static void BX()
        {
            Console.WriteLine("BX");
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            MethodInfo A = typeof(DefDatabase).GetMethod("AX", BindingFlags.Static | BindingFlags.Public);
            MethodInfo B = typeof(DefDatabase).GetMethod("BX", BindingFlags.Static | BindingFlags.Public);

            MethodUtil.ReplaceMethod(A, B);

            DefDatabase.AX();
            DefDatabase.BX();


            //DefDatabase<Program>.Add(new Program());
            //Console.WriteLine("BREAK");
            //DefDatabase<ProgramZETA>.Add(new ProgramZETA());
            //DefDatabase<ProgramBETA>.Add(new ProgramBETA());

            //Console.WriteLine(typeof(DefDatabase<>).GetHashCode().ToString());
            //Console.WriteLine(typeof(DefDatabase<Program>).GetHashCode().ToString());
           // Console.WriteLine(typeof(DefDatabase<ProgramZETA>).GetHashCode().ToString());

            //Console.WriteLine(typeof(DefDatabase<>).ToString());
            //Console.WriteLine(typeof(DefDatabase<Program>).ToString());
            //Console.WriteLine(typeof(DefDatabase<ProgramZETA>).ToString());

            //foreach (Type t in typeof(DefDatabase<>).Assembly.GetTypes())
            //{
            //    Console.WriteLine(t.ToString());
            //}

            //Console.WriteLine(typeof(DefDatabase<ProgramZETA>).BaseType.ToString());

            Console.ReadKey();

        }
    }
}
