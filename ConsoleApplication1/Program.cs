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

    enum TZ
    {
        ONE,
        TWO,
        FOR,
        THIS
    }

    class ZXX
    {
        public TZ priority;
        public ZXX(TZ check)
        {
            this.priority = check;
        }

        public override string ToString()
        {
            return priority.ToString();
        }
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
            List<ZXX> test = new List<ZXX>();
            Random rz = new Random();

            for (int i = 0; i < 20; i++)
            {
                test.Add(new ZXX((TZ)rz.Next(4)));
            }

            test.Add(new ZXX(TZ.FOR));
            test.Add(new ZXX(TZ.TWO));
            test.Add(new ZXX(TZ.THIS));
            test.Add(new ZXX(TZ.ONE));

            foreach (ZXX cc in test)
                Console.WriteLine(cc);

            int dimension = Enum.GetNames(typeof(TZ)).Length;
            int size = test.Count;
            int[] offsets = new int[dimension];

            ZXX[] sorted = new ZXX[dimension*size];

            foreach (ZXX cc in test)
            {
                int targetindex = ((int)(cc.priority) * size) + offsets[(int)cc.priority];
                offsets[(int)cc.priority]++;
                sorted[targetindex] = cc;
            }
            //int a = 0;
            /*
            foreach(ZXX cv in sorted)
            {
                if (cv == null)
                    continue;
                test[a] = cv;
                a++;
            }
            */
          //  foreach (int f in offsets)
           // {
                //Console.WriteLine(f);
          //  }

           // int xx = 0;
            //foreach (ZXX x in sorted)
            //{
                //Console.WriteLine(x +"%%" + xx);
            //    xx++;
            // }

            //Console.ReadKey();
            //if (true) return;


            int index = 0;
            int step = 0;
            test.Clear();

            while(true)
            {
                if (index == offsets[step] + step * size)
                {
                    if (step == dimension-1)
                        break;
                    step++;
                    index = step * size;
                    continue;
                }
                test.Add(sorted[index]);
                index++;
            }


            //sxxx.
            foreach (ZXX cc in test)
                Console.WriteLine(cc);
            Console.WriteLine(test.Count);

            //experemental sorting


            //Console.WriteLine("TEST");

            //MethodInfo entry = typeof(Program).GetMethod("TestMethod", new Type[]{typeof(string)});
            //MethodInfo[] entry = typeof(Program).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            //foreach (MethodInfo mm in entry)
                //Console.WriteLine(mm.ToString());

            //entry[0].Invoke(new Program(), null);

            //Console.WriteLine((entry != null).ToString());

            //Console.WriteLine((BindingFlags)17);

            //Type s = typeof(Program);


            //Object[] data = s.GetCustomAttributes(typeof(TEST),false);

            //MethodInfo ss = typeof(Program).Module;

            //Type ss = typeof(Program);
                
                
                //get   ("TestMethod",BindingFlags.Public | BindingFlags.Instance);

            //CustomAttributeData dd = CustomAttributeData.GetCustomAttributes(ss)[0];
           // dd.

            //Console.WriteLine(dd[0].ToString());
            //Console.WriteLine(dd[0].GetType().ToString());

            //foreach (CustomAttributeTypedArgument x in dd.ConstructorArguments)
               // Console.WriteLine(x.Value);
            
            //CustomAttributeData data = ll.First < CustomAttributeData >();
            //Console.WriteLine(ll.GetType().ToString());

            //Console.WriteLine(data.ConstructorArguments.First().Value);
            //Console.WriteLine(data.ToString());
            Console.ReadKey();

        }
    }
}
