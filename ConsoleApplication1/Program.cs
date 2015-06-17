using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApplication1
{

    public struct valid
    {
        public int a;
        public int b;
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct SafePtr
    {
        [FieldOffset(0)]
        public int ancha;

        [FieldOffset(8)]
        public object refkey;

        [FieldOffset(16)]
        public int anchb;

        unsafe public static void Main8(string[] dfs)
        {



            SafePtr safeptr = new SafePtr();
            SafePtr safeptrb = new SafePtr();

            object b = new SafePtr();

            safeptr.ancha = 11011111;
            safeptr.anchb = 22202222;

            //!!for STRANGE TWIST OF FATE NO CHANGES PASSED AFTER THIS LINE!!

            //probably my method of fetching object id is invalid or some kind of optimization cause issues

            //i will double check with verify method

            safeptr.refkey = safeptrb;
            TypedReference tra = __makeref(safeptr.refkey);
            IntPtr ptra = **(IntPtr**)(&tra);
            Console.WriteLine("Address of test is: {0}", ptra.ToInt32());
            Console.WriteLine("Address of test is: {0}", safeptr.refkey.GetHashCode());

            safeptr.refkey = safeptrb;
            TypedReference trb = __makeref(safeptr.refkey);
            IntPtr ptrb = **(IntPtr**)(&tra);
            Console.WriteLine("Address of test is: {0}", ptrb.ToInt32());
            Console.WriteLine("Address of test is: {0}", safeptr.refkey.GetHashCode());



            //Console.WriteLine("T1");

            //same return section
            //safeptr.ancha = 34424234;
            //Console.WriteLine("Address of 0 is: {0}", *(int*)(*(&safeptr.ancha + 2)));
            //Console.WriteLine("Address of test is: {0}", *(int*)(ptr.ToInt32()+8));

            //same invalid behaivour with different type of pointer fetch
            //probably structs work in other manner, must veryfy everything again


            //Console.WriteLine("Address of 1 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 1));
            //Console.WriteLine("Address of 2 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 2));
            //Console.WriteLine("Address of 3 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 3));
            //Console.WriteLine("Address of 4 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 4));
            //Console.WriteLine("Address of 5 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 5));
            //Console.WriteLine("Address of 6 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 6));
            //Console.WriteLine("Address of 7 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 7));
            //Console.WriteLine("Address of 8 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 8));


            //safeptr.refkey = safeptr;
            //safeptr.refkey = safeptr;



            /*
            Console.WriteLine("T2");

            Console.WriteLine("Address of 1 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 1));
            Console.WriteLine("Address of 2 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 2));
            Console.WriteLine("Address of 3 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 3));
            Console.WriteLine("Address of 4 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 4));
            Console.WriteLine("Address of 5 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 5));
            Console.WriteLine("Address of 6 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 6));
            Console.WriteLine("Address of 7 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 7));
            Console.WriteLine("Address of 8 is: {0}", *((int*)(*(&safeptr.ancha + 2)) + 8));
            */
            /*
            //safeptr.refkey = new object();
            Console.WriteLine("T2");
            Console.WriteLine("Address of -7 is: {0}", *(rca - 7));
            Console.WriteLine("Address of ra is: {0}", *(rca - 6));
            Console.WriteLine("Address of ra is: {0}", *(rca - 5));
            Console.WriteLine("Address of ra is: {0}", *(rca - 4));
            Console.WriteLine("Address of ra is: {0}", *(rca - 3));
            Console.WriteLine("Address of -2 is: {0}", *(rca - 2));
            Console.WriteLine("Address of -1 is: {0}", *(rca - 1));
            Console.WriteLine("Address of 0 is: {0}", *(rca + 0));
            Console.WriteLine("Address of 1 is: {0}", *(rca + 1));
            Console.WriteLine("Address of 2 is: {0}", *(rca + 2));
            Console.WriteLine("Address of 3 is: {0}", *(rca + 3));
            Console.WriteLine("Address of 4 is: {0}", *(rca + 4));
            Console.WriteLine("Address of 5 is: {0}", *(rca + 5));
            Console.WriteLine("Address of 6 is: {0}", *(rca + 6));
            Console.WriteLine("Address of 7 is: {0}", *(rca + 7));
            Console.WriteLine("Address of 8 is: {0}", *(rca + 8));
            Console.WriteLine("Address of 9 is: {0}", *(rca + 9));
            Console.WriteLine("Address of ra is: {0}", *(rca + 10));
            Console.WriteLine("Address of ra is: {0}", *(rca + 11));
            Console.WriteLine("Address of ra is: {0}", *(rca + 12));
            */

            Console.ReadKey();
        }
    }
}