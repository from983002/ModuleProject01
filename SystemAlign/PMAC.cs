using System;
using System.Text;
using System.Runtime.InteropServices;

public class PMAC
{
    public delegate void DOWNLOADMSGPROC(String str, Int32 newline);

    public delegate void DOWNLOADGETPROC(Int32 nIndex, String lpszBuffer, Int32 nMaxLength);
    public delegate void DOWNLOADPROGRESS(Int32 nPercent);
    public delegate void DOWNLOADERRORPROC(String fname, Int32 err, Int32 line, String szLine);

    /*
    [DllImport("Pcomm32w.dll")]
    public static extern Int32 OpenPmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm32w.dll")]
    public static extern UInt32 PmacSelect(UInt32 dwDevice);
    [DllImport("Pcomm32w.dll")]
    public static extern UInt32 ClosePmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm32w.dll")]
    //public static extern Int32 PmacGetResponseA(UInt32 dwDevice, StringBuilder s, UInt32 maxchar, StringBuilder outstr);
    public static extern Int32 PmacGetResponseA(UInt32 dwDevice, Byte[] s, UInt32 maxchar, Byte[] outstr);

    [DllImport("Pcomm32w.dll")]
    public static extern Int32 PmacDownloadA(UInt32 dwDevice, DOWNLOADMSGPROC msgp, DOWNLOADGETPROC getp, DOWNLOADPROGRESS pprg, Byte[] filename, Int32 macro, Int32 map, Int32 log, Int32 dnld);

    [DllImport("Pcomm32w.dll")]
    public static extern IntPtr PmacDPRSetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);

    [DllImport("Pcomm32w.dll")]
    public static extern IntPtr PmacDPRGetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);
    */

    
    [DllImport("Pcomm64.dll")]
    public static extern Int32 OpenPmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm64.dll")]
    public static extern UInt32 PmacSelect(UInt32 dwDevice);
    [DllImport("Pcomm64.dll")]
    public static extern UInt32 ClosePmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm64.dll")]
    //public static extern Int32 PmacGetResponseA(UInt32 dwDevice, StringBuilder s, UInt32 maxchar, StringBuilder outstr);
    public static extern Int32 PmacGetResponseA(UInt32 dwDevice, Byte[] s, UInt32 maxchar, Byte[] outstr);

    [DllImport("Pcomm64.dll")]
    public static extern Int32 PmacDownloadA(UInt32 dwDevice, DOWNLOADMSGPROC msgp, DOWNLOADGETPROC getp, DOWNLOADPROGRESS pprg, Byte[] filename, Int32 macro, Int32 map, Int32 log, Int32 dnld);

    [DllImport("Pcomm64.dll")]
    public static extern IntPtr PmacDPRSetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);

    [DllImport("Pcomm64.dll")]
    public static extern IntPtr PmacDPRGetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);
    


   /*
    [DllImport("Pcomm32.dll")]
    public static extern Int32 OpenPmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm32.dll")]
    public static extern UInt32 PmacSelect(UInt32 dwDevice);
    [DllImport("Pcomm32.dll")]
    public static extern UInt32 ClosePmacDevice(UInt32 dwDevice);
    [DllImport("Pcomm32.dll")]
    //public static extern Int32 PmacGetResponseA(UInt32 dwDevice, StringBuilder s, UInt32 maxchar, StringBuilder outstr);
    public static extern Int32 PmacGetResponseA(UInt32 dwDevice, Byte[] s, UInt32 maxchar, Byte[] outstr);
   
    [DllImport("Pcomm32.dll")]
    public static extern Int32 PmacDownloadA(UInt32 dwDevice, DOWNLOADMSGPROC msgp, DOWNLOADGETPROC getp, DOWNLOADPROGRESS pprg, Byte[] filename, Int32 macro, Int32 map, Int32 log, Int32 dnld);

    [DllImport("Pcomm32.dll")]
    public static extern IntPtr PmacDPRSetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);

    [DllImport("Pcomm32.dll")]
    public static extern IntPtr PmacDPRGetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, IntPtr val);
    */

}
