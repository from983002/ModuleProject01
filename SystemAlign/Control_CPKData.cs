using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemAlign
{
    public class Control_CPKData
    {
        //calculate the standard deviation
        public static float StDev(float[] arrData)
        {
            float xSum = 0f;
            float xAvg = 0f;
            float sSum = 0f;
            float tmpStDev = 0f;
            int arrNum = arrData.Length;
            for (int i = 0; i < arrNum; i++)
            {
                xSum += arrData[i];
            }
            xAvg = xSum/arrNum;
            for (int j = 0; j < arrNum; j++)
            {
                sSum += ((arrData[j] - xAvg)*(arrData[j] - xAvg));
            }
            tmpStDev = Convert.ToSingle(Math.Sqrt((sSum/(arrNum - 1))).ToString());
            return tmpStDev;
        }

        public static double StDev(int cnt, double sum, double sqSum)
        {
            double iavg = (double)sum / cnt;
            double iavg2 = iavg * iavg;

            double qavg = (double)(sqSum / cnt);
            double tmp1 = Math.Abs(iavg2 - qavg);
            double tmp2 = Math.Sqrt(tmp1);

            return tmp2;
        }

        public static float Cp(float UpperLimit, float LowerLimit, float stDev)
        {
            float tmpV = 0f;
            tmpV = UpperLimit - LowerLimit;
            return Math.Abs(tmpV/(6*stDev));
        }

        public static float Avage(float[] arrData)
        {
            float tmpSum = 0f;
            for (int i = 0; i < arrData.Length; i++)
            {
                tmpSum += arrData[i];
            }
            return tmpSum/arrData.Length;
        }

        public static float Max(float[] arrData)
        {
            float tmpMax = 0f;
            for (int i = 0; i < arrData.Length; i++)
            {
                if (tmpMax < arrData[i])
                {
                    tmpMax = arrData[i];
                }
            }
            return tmpMax;
        }

        public static float Min(float[] arrData)
        {
            float tmpMin = 1000f;
            for (int i = 0; i < arrData.Length; i++)
            {
                if (tmpMin > arrData[i] && arrData[i] != 0f)
                {
                    tmpMin = arrData[i];
                }
            }
            return tmpMin;
        }

        public static float Min(float NewData, float OldData)
        {
            float tmpMin = 0f;

            if (OldData == 0f) tmpMin = NewData;
            else if (OldData < NewData) tmpMin = OldData;
            else if (OldData > NewData) tmpMin = NewData;
            else if (OldData == NewData) tmpMin = NewData;

            return tmpMin;
        }

        public static float Max(float NewData, float OldData)
        {
            float tmpMax = 0f;

            if (OldData == 0f) tmpMax = NewData;
            else if (OldData < NewData) tmpMax = NewData;
            else if (OldData > NewData) tmpMax = OldData;
            else if (OldData == NewData) tmpMax = NewData;

            return tmpMax;
        }

        public static float CpkU(float UpperLimit, float Avage, float StDev)
        {
            float tmpV = 0f;
            tmpV = UpperLimit - Avage;
            return tmpV / (3 * StDev);
        }

        public static float CpkL(float LowerLimit, float Avage, float StDev)
        {
            float tmpV = 0f;
            tmpV = Avage - LowerLimit;
            return tmpV / (3 * StDev);
        }

        public static float Cpk(float Cpku, float Cpkl)
        {
            return Math.Abs(Math.Min(Cpku, Cpkl));
        }
    }
}
