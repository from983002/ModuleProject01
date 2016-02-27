using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SystemAlign
{
    public static class ImageUtil
    {
        // BitmapData객체를 보관하기 위한 Dictionary  
        public static Dictionary<Bitmap, BitmapData> BitmapDataDictionary = new Dictionary<Bitmap, BitmapData>();
        ////////////////////////////////////////////////////////////////////////////////////  
        // GetPointer로 취득한 메모리는 꼭 다시 ReleasePointer를 호출 해줘야합니다.  
        ////////////////////////////////////////////////////////////////////////////////////  
        // Image로부터 Read/Write가 가능한 Memory Pointer를 가져옵니다.  
        public static IntPtr LockMemory(this Bitmap Image)
        {
            return LockMemory(Image, new Rectangle(Point.Empty, Image.Size), ImageLockMode.ReadWrite);
        }

        // Image로부터 Mode에 맞는 작업을 수행 할 수 있는 Memory Pointer를 가져옵니다.  
        public static IntPtr LockMemory(this Bitmap Image, ImageLockMode Mode)
        {
            return LockMemory(Image, new Rectangle(Point.Empty, Image.Size), Mode);
        }

        // Image로부터 Rect영역을 Read/Write 할 수 있는 Memory Pointer를 가져옵니다.  
        public static IntPtr LockMemory(this Bitmap Image, Rectangle Rect)
        {
            return LockMemory(Image, Rect, ImageLockMode.ReadWrite);
        }

        // Image로부터 Rect영역을 Mode에 맞는 작업을 수행 할 수 있는 Memory Pointer를 가져옵니다.  
        public static IntPtr LockMemory(this Bitmap Image, Rectangle Rect, ImageLockMode Mode)
        {
            BitmapData ImageData = null;
            if (BitmapDataDictionary.ContainsKey(Image))
                ImageData = BitmapDataDictionary[Image];
            else
            {
                ImageData = Image.LockBits(Rect, Mode, Image.PixelFormat);
                BitmapDataDictionary.Add(Image, ImageData);
            }
            return ImageData.Scan0;
        }

        // LockMemory로 취득한 Memory Pointer를 반환 합니다.  
        public static void UnLockMemory(this Bitmap Image)
        {
            BitmapData ImageData = BitmapDataDictionary[Image];
            Image.UnlockBits(ImageData);
        }

        // Image의 Memory를 복사해옵니다.  
        public static byte[] CopyMemory(this Bitmap Image)
        {
            return CopyMemory(Image, new Rectangle(Point.Empty, Image.Size));
        }

        // Image로부터 Rect영역에 있는 Memroy를 가져옵니다.  
        public static byte[] CopyMemory(this Bitmap Image, Rectangle Rect)
        {
            byte[] ReturnMemory = new byte[Rect.Width*Rect.Height];
            IntPtr ImagePointer = Image.LockMemory(Rect, ImageLockMode.ReadOnly);
            Marshal.Copy(ImagePointer, ReturnMemory, 0, ReturnMemory.Length);
            Image.UnLockMemory();

            return ReturnMemory;
        }

    }
}