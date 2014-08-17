using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApplication1
{
    class TEST : Attribute
    {
        public TEST(string s,string x,string y){}
    }

    [TEST("BLABLA","HAHA","TEST")]
    class Program
    {

        static void Register(MethodInfo METHOD)
        {

        }

        public void PUBLIC() { Console.WriteLine("TESTE METHOD INVOCATION"); }
        public static void TTTTTT() {} 

        //[TEST("BLABLA")]
        public void TestMethod(string EVENT)
        {
            Console.WriteLine("TESTE METHOD INVOCATION");
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("TEST");

            //MethodInfo entry = typeof(Program).GetMethod("TestMethod", new Type[]{typeof(string)});
            //MethodInfo[] entry = typeof(Program).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            //foreach (MethodInfo mm in entry)
                //Console.WriteLine(mm.ToString());

            //entry[0].Invoke(new Program(), null);

            //Console.WriteLine((entry != null).ToString());

            //Console.WriteLine((BindingFlags)17);

            Type s = typeof(Program);


            //Object[] data = s.GetCustomAttributes(typeof(TEST),false);

            //MethodInfo ss = typeof(Program).Module;

            Type ss = typeof(Program);
                
                
                //get   ("TestMethod",BindingFlags.Public | BindingFlags.Instance);

            CustomAttributeData dd = CustomAttributeData.GetCustomAttributes(ss)[0];
           // dd.

            //Console.WriteLine(dd[0].ToString());
            //Console.WriteLine(dd[0].GetType().ToString());

            foreach (CustomAttributeTypedArgument x in dd.ConstructorArguments)
                Console.WriteLine(x.Value);
            
            //CustomAttributeData data = ll.First < CustomAttributeData >();
            //Console.WriteLine(ll.GetType().ToString());

            //Console.WriteLine(data.ConstructorArguments.First().Value);
            //Console.WriteLine(data.ToString());
            Console.ReadKey();

        }
    }
}
