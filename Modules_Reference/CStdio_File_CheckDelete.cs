using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*


업무를 하다보면 로그파일들을 최근 몇일동안만 남기고 지워야 하는 경우가 있다. 이를 위한 C# 코드..

 

string[] files = Directory.GetFiles(@"로그가 담긴 폴더명");

DateTime 확인대상 = DateTime.now;

 

foreach (string 파일 in files)
{

  확인대상 = File.GetCreationTime(파일);

     

  // 여기서는 하루 전날 이하인 파일을 삭제하므로 -1

  if (DateTime.Compare((DateTime.Now.AddDays(-1)), 확인대상) > 0)
  {
      File.Delete(파일);
  }

}

 

 

Compare 함수는 첫번째 인자가 A, 두번째 인자가 B일때

 

- A > B = 양수

- A < B = 음수

- A == B = 0

 

을 반환함.

[출처] 특정시간 전 데이터 파일 삭제방법|작성자 심시티사람들

    DateTime t1 = DateTime.Parse("2004-03-17");
	    DateTime t2 = DateTime.Parse("2004-03-18");

[출처] c# 날짜변경(변경,변환,비교)|작성자 심시티사람들

*/
namespace Modules_Reference
{
    class CStdio_File_CheckDelete
    {
        public string GetSet_FolderPath { get; set; }
        public int GetSet_CompDate { get; set; }
        

        public void Image_File_Delete()
        {
            string[] files = Directory.GetFiles(GetSet_FolderPath);

            foreach (string checkFile in files)
            {

                DateTime FileCreateTime = File.GetCreationTime(checkFile);

                // A : 설정 일자, B : 파일의 생성날자
                if (DateTime.Compare((DateTime.Now.AddDays(GetSet_CompDate)), FileCreateTime) > 0)
                {
                    File.Delete(checkFile);
                }

            }
        }        
    }
}
