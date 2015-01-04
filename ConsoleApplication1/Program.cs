using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApplication1
{

    public struct STORESTRUCT
    {
        public int A;
        public int B;
    }

    public struct STORESTRUCTCOPY
    {
        public int A;
        public int B;
    }


    public class STORECLASS
    {
        public int A;
        public int B;
    }



    public class IWR
    {
        public int TEST = 0xFFDDFF;
    }

    public class TEST
    {

        public static STORECLASS  SC;
        public static STORESTRUCT SS;


        static unsafe void Main(string[] args)
        {
            /*
                byte[] bytes = new byte[100];
     bytes[0] = 1;
     bytes[1] = 2;
    bytes[2] = 3;
    bytes[3] = 4;
 
     Type arrayType = null;
     fixed (byte* pMem = &bytes[0])
    {
        Console.WriteLine("{0:x16}", (long)pMem);
        int* pArrayBase = (int*) pMem;
       Console.WriteLine("{0:x8}", *pArrayBase);
       pArrayBase--;
        Console.WriteLine("{0:x8}", *pArrayBase);
        pArrayBase--;
        Console.WriteLine("{0:x8}", *pArrayBase);
         pArrayBase--;
        Console.WriteLine("{0:x8}", *pArrayBase);
         pArrayBase--;
         Console.WriteLine("{0:x8}", *pArrayBase);
        long rtth = *(long*) pArrayBase;
       // RuntimeTypeHandle handle;
         // RTTH is a value-type whose only member is an IntPtr; can be set as a long on x64
         //RuntimeTypeHandle* pH = &handle;
        // *((long*) pH) = rtth;
        //arrayType = Type.GetTypeFromHandle(handle);
     }
 
     if (arrayType != null)
    {
         Console.WriteLine(arrayType.Name);
      }
   
     Console.WriteLine("byte[] RTTH: {0:x16}", typeof (byte[]).TypeHandle.Value.ToInt64());
    int a = 1;
     int b = 2;
   int* pA = &a;
    int* pB = &b;
    Console.WriteLine(*pB);
     Console.WriteLine(*(pB - 1));



     Console.WriteLine(typeof(STORESTRUCT).TypeHandle.Value);
     Console.WriteLine(typeof(STORESTRUCTCOPY).TypeHandle.Value);


Console.ReadLine();
            */


            STORESTRUCT test1 = new STORESTRUCT();
            STORESTRUCTCOPY test2 = new STORESTRUCTCOPY();
            STORESTRUCT test3 = new STORESTRUCT();
            STORESTRUCTCOPY test4 = new STORESTRUCTCOPY();

            test1.A = 0xffff;
            test1.B = 0xaaaa;
            test2.A = 0xadad;
            test2.B = 0xadad;
            test3.A = 0xffff;
            test3.B = 0xaaaa;
            test4.A = 0xadad;
            test4.B = 0xadad;
            int* pointer1 = (int*)&test1;
            STORESTRUCT zz = *((STORESTRUCT*)pointer1);

            IWR TEST = new IWR();
            TypedReference REFERENCE = __makeref(TEST);

            int* pxx = (int*)&REFERENCE;

            IntPtr* ff = (IntPtr*)pxx;

            IntPtr ffx = *ff;


            System.Diagnostics.Debug.WriteLine("{0:x16}", ffx.ToInt32());
            System.Diagnostics.Debug.WriteLine("{0:x16}", *pxx);




            //this method is fully functional

            IWR o = TEST;
            TypedReference tr = __makeref(o);
            IntPtr ptr = **(IntPtr**)(&tr);



            fixed( int* tt = &o.TEST)
            {
                System.Diagnostics.Debug.WriteLine("{0:x16}", (int)tt);
            }

            System.Diagnostics.Debug.WriteLine("{0:x16}", ptr.ToInt32());


            IntPtr ffxc = *ff;

            //int* pointer4 = (int*)&test4;
            //System.Diagnostics.Debug.WriteLine("{0:x16}", (int)pointer4);
            //Console.WriteLine((int)pointer2);
            //Console.WriteLine((int)pointer3);

            //IWR nez = new IWR();

            //TypedReference z = __makeref(nez);

            //int* value = (int*)&z;
            //IntPtr zz = (IntPtr)*value;

            //System.Diagnostics.Debug.WriteLine("{0:x16}", (int)value);

            //int** twodeep = &pointer1;

            //STORESTRUCT* tzz = &test;

            //Console.WriteLine(tzz->A);

            var tz = new STORESTRUCTCOPY();
            for (int i = -8; i < 20; i++)
            {
                //lol = *((int*)&tz + i);
                //Console.WriteLine("OFFSET " + i + " value " + lol);
            }

            //Console.WriteLine("{0:x16}", typeof(STORESTRUCT).TypeHandle.Value.ToInt64());
            //Console.WriteLine("{0:x16}", typeof(STORESTRUCTCOPY).TypeHandle.Value.ToInt64());


            Console.ReadKey();

        }
    }
}
