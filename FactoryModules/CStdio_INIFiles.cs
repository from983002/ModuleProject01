using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace FactoryModules
{
    public class CStdio_INIFiles
    {
        [DllImport("kernel32")]

        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public string GetSet_FilePath { get; set; }
        public string GetSet_Section { get; set; }
        /// <summary>
        /// INI파일에서 섹션의 키 값을 읽어서 리턴한다.
        /// </summary>
        /// <param name="section">
        /// INI파일의 단원을 의미한다. 
        /// </param>
        /// <param name="key">
        /// 단원내의 항목이름을 의미한다.
        /// </param>
        /// <param name="filePath">
        /// INI파일의 위치(절대경로)를 의미한다.
        /// </param>
        /// <returns>
        /// INI파일에서 읽은 결과값을 리턴한다.
        /// </returns>
        public string GetIniValue(string section, string key, string filePath)
        {

            StringBuilder temp = new StringBuilder(255);

            GetPrivateProfileString(section, key, "", temp, 255, filePath);

            return temp.ToString();

        }

        /// <summary>
        /// INI파일에서 섹션의 키 값을 읽어서 리턴한다.
        /// </summary>
        /// <param name="section">
        /// INI파일의 단원을 의미한다. 
        /// </param>
        /// <param name="key">
        /// 단원내의 항목이름을 의미한다.
        /// </param>
        /// <param name="filePath">
        /// INI파일의 위치(절대경로)를 의미한다.
        /// </param>
        /// <returns>
        /// INI파일에서 읽은 결과값을 리턴한다.
        /// </returns>
        public void SetIniValue(string filePath, string section, string key, string value)
        {
            //현재의 실행파일명에서 exe를 ini로 변경하여 파일명으로 사용한다.
            //string sIniPath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToLower().Replace(".exe", ".ini");
            //WritePrivateProfileString(this.Name, textBox1.Name, textBox1.Text, sIniPath);
            WritePrivateProfileString(section, key, value, filePath);
        }


        public void SetIniValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, GetSet_FilePath);
        }

        public void SetIniValue(string key, string value)
        {
            WritePrivateProfileString(GetSet_Section, key, value, GetSet_FilePath);
        }
    }
}