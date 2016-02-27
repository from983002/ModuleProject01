using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;
using System.Drawing;

namespace SystemAlign
{
    public class Control_Metrox
    {
        public void Grab_Image()
        {
            //변수 선언
            MIL_INT Image_Size = 450;
            MIL_INT Buf_Define = 25;
            MIL_ID MilApplication = MIL.M_NULL;
            MIL_ID MilSystem = MIL.M_NULL;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID MilDigiti = MIL.M_NULL;
            MIL_ID[] Milimage = new MIL_ID[25];
            string[] FileName = new string[25];
            String[] strPathFileName = new String[25];
            Image[] Img = new Image[25];
            String File_temp = string.Empty;

            //ALLOC 설정
            MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication);
            MIL.MsysAlloc(MIL.M_SYSTEM_SOLIOS, MIL.M_DEV0, MIL.M_DEFAULT, ref MilSystem);

            //DCF상에서 450 * 25 사이즈에 대하여 영상획득하게 설정되어 있습니다. 바꾸시길 원하시면 DCF상에서 바꾸시고 소스상에서
            //MIL_INT Image_Size = 450; MIL_INT Buf_Define = 25; 값들을 변경하여 주시면 됩니다.
            //제가 원한사이즈는 4096 * 11250 ( 450 사이즈에 대하여 25번 GRAB하였습니다.)

            for (int i = 0; i < Buf_Define; i++)
            {
                MIL.MbufAllocColor(MilSystem, 3, 4096, Image_Size, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref Milimage[i]);
            }

            MIL.MdispAlloc(MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, ref MilDisplay);
            MIL.MdigAlloc(MilSystem, MIL.M_DEFAULT, "scan.dcf", MIL.M_DEFAULT, ref MilDigiti);
            MIL.MdigControl(MilDigiti, MIL.M_CAMERALINK_CC3_SOURCE, MIL.M_GRAB_EXPOSURE);
            SaveFileDialog SaveFile = new SaveFileDialog();

            //파일저장 경로 설정 -> 윈도우폼으로 저장경로 설정
            SaveFile.DefaultExt = "jpg";
            SaveFile.Filter = "*.jpg|*.jpg| *.mim|*.mim| *.tif|*.tif";
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                File_temp = SaveFile.FileName;
            }

            //위에 경로에 대하여 이미지 저장
            for (int i = 0; i < Buf_Define; i++)
            {
                MIL.MbufClear(Milimage[i], 0);
                MIL.MdispSelect(MilDisplay, Milimage[i]);
                MIL.MdigGrab(MilDigiti, Milimage[i]);
            }

            //Filename changed.
            //File_temp
            for (int j = 0; j < Buf_Define; j++)
            {
                FileName[j] = File_temp.Replace(".jpg", "_" + j + ".jpg");
                MIL.MbufExport(FileName[j], MIL.M_JPEG_LOSSY, Milimage[j]);
            }

            //ALLOC 해제
            for (int i = 0; i < Buf_Define; i++)
            {
                MIL.MbufFree(Milimage[i]);
            }

            MIL.MdigFree(MilDigiti);
            MIL.MdispFree(MilDisplay);
            MIL.MsysFree(MilSystem);
        }
    }
}
