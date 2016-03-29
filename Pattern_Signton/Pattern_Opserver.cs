using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Modules
{
    /*
    public Form1()
        {
            InitializeComponent();

            CallBackTest cbt = new CallBackTest();
            cbt.onComplete(Image_File_Delete_Test);
        }
   

    public class CallBackTest
    {
        //콜백을 전달할 delegate 선언 (인수 전달/)
        public delegate void onCompleteDele(int i);

        public void onComplete(onCompleteDele _callCompleteMethod)
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.Start();
            int count = 0;

            t.Tick += delegate {
                if (count == 2)
                    //!!콜백발생
                    _callCompleteMethod(count);
                ++count;
            };
        }
    } 
    */

    class Pattern_Opserver
    {
        
    }
}
