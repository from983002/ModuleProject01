using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Modules
{
    public class Pattern_Signton
    {
        //C#에서는 이 1라인의 선언으로 멀티스레드에서 안전한 싱글톤을 완성한다.
        public static readonly Pattern_Signton Instance = new Pattern_Signton();

        //유니큐한 카운터를 생성한다.
        private static int count = 0;

        //생성한 카운터를 증가시키는 함수.
        public int NextValue()
        {
            return ++count;
        }
    }
}
