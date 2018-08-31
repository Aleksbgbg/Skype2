namespace Skype2.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    internal static class SecureStringExtensions
    {
        internal static bool IsEqualTo(this SecureString source, SecureString target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source.Length != target.Length)
            {
                return false;
            }

            IntPtr sourceBinary = IntPtr.Zero;
            IntPtr targetBinary = IntPtr.Zero;

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                sourceBinary = Marshal.SecureStringToBSTR(source);
                targetBinary = Marshal.SecureStringToBSTR(target);

                unsafe
                {
                    for (char* sourcePointer = (char*)sourceBinary.ToPointer(),
                               targetPointer = (char*)targetBinary.ToPointer();
                         *sourcePointer != 0 && *targetPointer != 0;
                         ++sourcePointer, ++targetPointer)
                    {
                        if (*sourcePointer != *targetPointer)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            finally
            {
                if (sourceBinary != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(sourceBinary);
                }

                if (targetBinary != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(targetBinary);
                }
            }
        }
    }
}