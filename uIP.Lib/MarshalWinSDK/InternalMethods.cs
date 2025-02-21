using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace uIP.Lib.MarshalWinSDK
{
    internal static class InternalMethods
    {
        internal static bool LittleEndian = false;
        static InternalMethods()
        {
            unsafe
            {
                UInt32 v = 1;
                UInt32* ptr32 = &v;
                byte* ptr8 = ( byte* )ptr32;
                LittleEndian = ptr8[ 0 ] != 0;
            }
        }

        private delegate IntPtr fpMemAlloc(int cb);
        private static IntPtr StringToIntptr<T>(string str, Encoding enc, fpMemAlloc fpAllocMem, out int lenInByte)
        {
            lenInByte = 0;
            if (typeof(T) != typeof(byte) && typeof(T) != typeof(Int16) && typeof(T) != typeof(char))
                return IntPtr.Zero;
            if (String.IsNullOrEmpty(str) || enc == null)
                return IntPtr.Zero;

            int unit = typeof(T) == typeof(char) ? sizeof(char) : Marshal.SizeOf(typeof(T));

            byte[] bytes = enc.GetBytes(str);
            lenInByte = bytes.Length + 1 * unit;
            IntPtr pMem = fpAllocMem(lenInByte);
            Marshal.Copy(bytes, 0, pMem, bytes.Length);

            unsafe
            {
                if (unit == 1)
                {
                    byte* p8 = (byte*)pMem.ToPointer();
                    p8[bytes.Length] = 0;
                }
                else
                {
                    Int16* p16 = (Int16*)pMem.ToPointer();
                    p16[bytes.Length / unit] = 0;
                }
            }
            return pMem;
        }

        internal static IntPtr StringToIntptrHGlobal<T>(string str, Encoding enc, out int lenInByte)
        {
            return StringToIntptr<T>(str, enc, Marshal.AllocHGlobal, out lenInByte);
        }
    }
}
