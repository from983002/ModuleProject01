using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;

namespace FactoryModules
{
    public class CStdio_ControlListing
    {
        //public string GetSet_ContainerName { get; set; }

        public List<string> ControlData = new List<string>();

        // 모든 텍스트 상자의 이름 가져오기!!
        public List<string> getControlNames(Control control)
        {
            ControlData.Clear();

            // 대상 control 의 하위에 있는 모든 컨트롤을 대상으로 스캔한다.
            foreach (Control tmpCtrl in control.Controls)
            {
                // 재귀호출 : 컨트롤이 또 다른 컨트롤을 품고 있으면 하위의 끝까지 스캔한다.
                if (tmpCtrl.Controls.Count > 0)
                    getControlNames(tmpCtrl);                

                // 현재 스캔중인 컨트롤이 TextBox 형식이면...
                if (tmpCtrl is UltraTextEditor)
                {
                    //TextBox textBox = (TextBox)tmpCtrl; // TextBox 으로 형 변환 한다.
                    //txtLog.AppendText(textBox.Name.ToString() + "\n"); // 현재 컨트롤 이름 Log 추가
                    //ControlData.Add(((TextBox)tmpCtrl).Name.ToString());
                    ControlData.Add(tmpCtrl.Name.ToString());
                    ControlData.Add(((UltraTextEditor)tmpCtrl).Text);
                }
                else if (tmpCtrl is UltraCheckEditor)
                {
                    ControlData.Add(tmpCtrl.Name.ToString());
                    ControlData.Add(((UltraCheckEditor)tmpCtrl).Checked.ToString());
                    //checkBox1.Checked = GetIniValue(this.Name, checkBox1.Name, sIniPath).ToLower() == "true" ? true : false;
                }

                else if (tmpCtrl is UltraComboEditor)
                {
                    ControlData.Add(tmpCtrl.Name.ToString());
                    ControlData.Add(((UltraComboEditor)tmpCtrl).SelectedText);
                }
            }

            return ControlData;
        }
    }
}