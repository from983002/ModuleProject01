using System.Runtime.InteropServices;

public class EziMotionPlusR
{
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, DWORD dwBaud
    public static extern bool FAS_Connect(byte nCom, int uBaudRate);

    [DllImport("EziMOTIONPlusR.dll")]   //Byte nPortNo
    public static extern bool FAS_Close(byte nCom);

    //해당 드라이브의 존재 여부를 확인합니다. 존재:TRUE, 접속실패:FALSE
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_IsSlaveExist(byte nCom, byte iSlaveNo);

    //해당 드라이브의 존재 여부를 확인합니다. 존재:TRUE, 접속실패:FALSE
    //Return Value
    //FMM_OK : 명령이 정상적으로 수행되었습니다.
    //FMM_NOT_OPEN : 아직 Board를 연결하기 전 입니다.
    //FMM_INVALID_PORT_NUM : 연결한 Port 중에 nProt 는 존재하지 않습니다.
    //FMM_INVALID_SLAVE_NUM : 해당 Port에 iSlaveNo의 Slave는 존재하지 않습니다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, Byte &nType, char[256] IpBuffer
    public static extern int FAS_GetSlaveInfo(byte nCom, byte iSlaveNo,  byte nType, ref string IpBuffer, int nBuffSize);

    //모터를 감속하면서 정지시킨다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_MoveStop(byte nPortNo, byte iSlaveNo);

    //시스템의 원점(Origin)을 찾는다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_MoveOriginSingleAxis(byte nPortNo, byte iSlaveNo);

    //모터를 절대좌표 값으로 이동시킨다.FAS_MoveSingleAxisAbsPos(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity);
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, long lAbsPos:절대좌표값, DWORD LVelocity:속도값
    public static extern bool FAS_MoveSingleAxisAbsPos(byte nPortNo, byte iSlaveNo, int lAbsPos, int LVelocity);

     //모터를 절대좌표 값으로 이동시킨다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, long lAbsPos:절대좌표값, DWORD LVelocity:속도값
    public static extern bool FAS_MoveSingleAxisAbsPosEx(byte nPortNo, byte iSlaveNo, int lAbsPos, int LVelocity, ref short opt);    

    //모터를 상대좌표 값으로 이동시킨다
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, long lIncPos:상대좌표값, DWORD LVelocity:속도값
    public static extern bool FAS_MoveSingleAxisIncPos(byte nPortNo, byte iSlaveNo, int lIncPos, int LVelocity);
    //모터를 절대좌표 값으로 이동시킨다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, long lAbsPos:절대좌표값, DWORD LVelocity:속도값
    public static extern bool FAS_MoveSingleAxisIncPosEx(byte nPortNo, byte iSlaveNo, int lIncPos, int LVelocity, ref short opt);

    //지정한 드라이브의 Servo 상태를 ON/OFF 시킵니다.
    //Byte nPortNo, Byte iSlaveNo, BOOL bOnOff
    [DllImport("EziMOTIONPlusR.dll")]
    public static extern bool FAS_ServoEnable(byte nPortNo, byte iSlaveNo, bool bOnOff);

    //알람이 발생한 드라이브의 알람을 해제한다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_ServoAlarmReset(byte nPortNo, byte iSlaveNo);

    //알람이 발생한 드라이브의 알람을 해제한다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_GetAlarmType(byte nPortNo, byte iSlaveNo, ref byte iAlarmType);

    //모터를 급정지시킨다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo
    public static extern bool FAS_EmergencyStop(byte nPortNo, byte iSlaveNo);

    //보드의 특정 파라미터 값을 불러온다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lParamValue
    public static extern bool FAS_GetParameter(byte nPortNo, byte iSlaveNo, byte iParamNo, ref int lParamValue);

    //보드의 특정 파라미터 값을 불러온다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lParamValue
    public static extern bool FAS_Get(byte nPortNo, byte iSlaveNo, byte iParamNo, ref int lParamValue);

    //보드의 특정 파라미터 값을 설정한다.
    [DllImport("EziMOTIONPlusR.dll")]   //BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lParamValue
    public static extern bool FAS_SetParameter(byte nPortNo, byte iSlaveNo, byte iParamNo, int lParamValue);

    //현재 상태를 모두 포함하여 한꺼번에 읽어 들입니다.:GetIOStatus와 GetMotionStatus를 합해 놓은것
    [DllImport("EziMOTIONPlusR.dll")]   
    //byte nPortNo, byte iSlaveNo, ULONGLONG* ulnStatus, DWORD* dwOutStatus, 
    //DWORD* dwAxisStatus, long* lCmdPos, long* lAotPos, long* lPosErr, long* lAotVel, WORD* wPosItemNo
    //FAS_GetAllStatus(m_nPortNo, m_iSlaveNo, &dwInStatus, &dwOutStatus, &dwAxisStatus, &lCmdPos, &lActPos, &lPosErr, &lActVel, &wPosItemNo);
    public static extern bool FAS_GetAllStatus
        (byte nPortNo, byte iSlaveNo, ref uint Status, ref uint OutStatus,
        ref uint AxisStatus, ref uint CmdPos, ref uint AotPos, ref uint PosErr, ref uint AotVel, ref ushort PosItemNo);

    //동일 포트에 연결된 모든 운전중인 모터를 감속하면서 정지시킵니다.
    [DllImport("EziMOTIONPlusR.dll")]
    public static extern bool FAS_AllMoveStop(byte nPortNo);

    //동일 포트에 연결된 모든 운전중인 모터를 급정지시킵니다.
    [DllImport("EziMOTIONPlusR.dll")]
    public static extern bool FAS_AllEmergencyStop(byte nPortNo);

    ///동일 포트에 연결된 모든 모터의 원점을 찾는다.
    [DllImport("EziMOTIONPlusR.dll")]
    public static extern bool FAS_AllMoveOriginSingleAxis(byte nPortNo);

    //모터를 해당 방향, 해당 속도로 이동시킨다. 조그 운전시 사용된다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD iVelocity, int iVelDir
    public static extern bool FAS_MoveVelocity(byte nPortNo, byte iSlaveNo, int iVelocity, int iVelDir);

    // I/O Input 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwInput
    public static extern int FAS_GetIOInput(byte nPortNo, byte iSlaveNo, ref uint dwInput);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetIOOutput(byte nPortNo, byte iSlaveNo, ref uint dwOutput);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetIOAssignMap(byte nPortNo, byte iSlaveNo, int i, ref int LogicMask, ref bool Level);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_SetIOAssignMap(byte nPortNo, byte iSlaveNo, int i, ref int LogicMask, ref bool Level);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetOutputAssignMap(byte nPortNo, byte iSlaveNo, int i, ref int LogicMask, ref bool Level);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetActualPos(byte nPortNo, byte iSlaveNo, ref uint ActPos);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetCommandPos(byte nPortNo, byte iSlaveNo, ref uint CmdPos);

    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //byte nPortNo, byte iSlaveNo, DWORD dwOutput
    public static extern int FAS_GetAxisStatus(byte nPortNo, byte iSlaveNo, ref int Status);
     
    [DllImport("EziMOTIONPlusR.dll")] //FAS_GetMotionStatus(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos, long* lActPos, long* lPosErr, long* lActVel, WORD* wPosItemNo);
    public static extern int FAS_GetMotionStatus(byte nPortNo, byte iSlaveNo, ref int lCmdPos, ref int ActPos, ref int PosErr, ref int ActVel, ref short PosItemNo);
    
    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //FAS_GetActualVel(BYTE nPortNo, BYTE iSlaveNo, long* lActVel);
    public static extern int FAS_GetActualVel(byte nPortNo, byte iSlaveNo, ref uint ActVel);
    
    // I/O Output 을 확인합니다.
    [DllImport("EziMOTIONPlusR.dll")]   //FAS_ClearPosition(BYTE nPortNo, BYTE iSlaveNo);
    public static extern int FAS_ClearPosition(byte nPortNo, byte iSlaveNo);
}