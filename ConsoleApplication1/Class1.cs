using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace byValByRefCSharp
{

    public class testClass
    {
        public int value = 1;
    }



    unsafe class Program
    {
        static public void MA()
        {
            int i = 10;
        }
        static public void MB()
        {
            int i = 10;
        }

        static public uint test()
        {
            return 0xDEADBEEF;
        }

        static void Main(string[] args)
        {

            //test();
            
            DynamicMethod dyn_m_1 = new DynamicMethod("T", typeof(int), new Type[] { typeof(uint) });

            ILGenerator il_1 = dyn_m_1.GetILGenerator();

            il_1.Emit(OpCodes.Break);
            il_1.Emit(OpCodes.Ldc_I4, 0xFFFFFFFF);
            //il_1.Emit (OpCodes.Ldc_I4,0xFFFFFFFF);
            //il_1.Emit (OpCodes.Ldc_I4,0xFFFFFFFF);
            //il_1.Emit (OpCodes.Ldc_I4,0xFFFFFFFF);
            il_1.Emit(OpCodes.Pop);
            il_1.Emit(OpCodes.Ldc_I4, 0xFFFFFFFF);
            il_1.Emit(OpCodes.Break);
            //il_1.Emit (OpCodes.Pop);
            //il_1.Emit (OpCodes.Pop);
            //il_2.Emit (OpCodes.Call, typeof(MainClass).GetMethod ("B", BindingFlags.Static | BindingFlags.Public));
            il_1.Emit(OpCodes.Ret);



            dyn_m_1.Invoke(null, new object[] { 0xFFFFFFFF });
            //DynamicMethod dm = new DynamicMethod("TEST", typeof(int), new Type[] { typeof(object) });
            //var generator = dm.GetILGenerator();

            //generator.Emit(OpCodes.Ldarg_0);
            //generator.Emit(OpCodes.Ldarg,0);
            //generator.Emit(OpCodes.Conv_I);
            //generator.Emit(OpCodes.Ldc_I4,8);
            //generator.Emit(OpCodes.Sub);
            //generator.Emit(OpCodes.Conv_I);
            //generator.Emit(OpCodes.Ret);

            //object c = new object();

            //int test = (int)dm.Invoke(null, new object[] { c });


            //**(int**)
            //Console.WriteLine(**(int**)test);

            //TypedReference tr = __makeref(c);
            //IntPtr intPtrAlternativeMethod = **(IntPtr**)(&tr);

            //Console.WriteLine ((int)&tr);

            Console.WriteLine(99999);



            /*
            testClass tc = new testClass();

            //Get the address.
            GCHandle objHandle = GCHandle.Alloc(tc, GCHandleType.WeakTrackResurrection);
            IntPtr intPtrAddressOfObjectPointer = GCHandle.ToIntPtr(objHandle);
            IntPtr intPtrObjectPtr = (IntPtr)Marshal.ReadInt32(intPtrAddressOfObjectPointer);

            //Alternative way of getting the same pointer to a pointer...
            TypedReference tr = __makeref(tc);
            IntPtr intPtrAlternativeMethod = **(IntPtr**)(&tr);

            Console.WriteLine("A) Address of object pointer: " + intPtrAddressOfObjectPointer);
            //Console.WriteLine("Bytes at address: " + getBytes(intPtrAddressOfObjectPointer));
            //Console.WriteLine("");
            Console.WriteLine("B) Address of actual object: " + intPtrObjectPtr);
            //Console.WriteLine("Ad(alt) of actual object: " + intPtrAlternativeMethod);

            testByVal(tc);
            testByRef(ref tc);

            MA();
            MB();
            */
            //Console.ReadKey();
        }

        public static string getBytes(IntPtr address)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
                bytes[i] = *((byte*)address + i);
            return String.Join(",", bytes);
        }

        public static void testByVal(testClass a)
        {
            Console.WriteLine("");
            Console.WriteLine("Inside test - pass by value.");
            GCHandle objHandle = GCHandle.Alloc(a, GCHandleType.WeakTrackResurrection);
            IntPtr intPtrAddressOfObjectPointer = GCHandle.ToIntPtr(objHandle);
            IntPtr intPtrObjectPtr = (IntPtr)Marshal.ReadInt32(intPtrAddressOfObjectPointer);

            TypedReference tr = __makeref(a);
            IntPtr intPtrAlternativeMethod = **(IntPtr**)(&tr);

            Console.WriteLine("C) Address of object pointer: " + intPtrAddressOfObjectPointer);
            //Console.WriteLine("Bytes at address: " + getBytes(intPtrAddressOfObjectPointer));
            //Console.WriteLine("");
            Console.WriteLine("D) Address of actual object: " + intPtrObjectPtr);
        }

        public static void testByRef(ref testClass a)
        {
            Console.WriteLine("");
            Console.WriteLine("Inside test - pass by reference.");
            GCHandle objHandle = GCHandle.Alloc(a, GCHandleType.WeakTrackResurrection);
            IntPtr intPtrAddressOfObjectPointer = GCHandle.ToIntPtr(objHandle);
            IntPtr intPtrObjectPtr = (IntPtr)Marshal.ReadInt32(intPtrAddressOfObjectPointer);

            Console.WriteLine("E) Address of object pointer: " + intPtrAddressOfObjectPointer + " << This should be same as A?");
            //Console.WriteLine("Bytes at address: " + getBytes(intPtrAddressOfObjectPointer));
            //Console.WriteLine("");
            Console.WriteLine("F) Address of actual object: " + intPtrObjectPtr);
        }
    }
}
