namespace SystemAlign
{
    class CNIAMMachineRearUse
    {
        AEquipmentFactorys mRearCarrier, mRearCarrierFeeding, mRearCarrierInput, mRearCarrierOutput, mRearHardening, mRearReject, mRearReverseSub, mRearReverse;
        AEquipments rearCarrierFeeding, rearCarrierInput, rearCarrierOutput, rearHeardening, rearReject, rearReverseSub, rearReverse;
        AEquipments[] equipmentAssembly = new AEquipments[8];
        public CNIAMMachineRearUse()
        {
            this.ObjectCreateRear();
        }

        public AEquipments[] EquipmentAssembly
        {
            get { return this.equipmentAssembly; }
            set { this.equipmentAssembly = value; }
        }

        private void ObjectCreateRear()
        {           
            mRearCarrierFeeding = new CEquipmentFactory_CarrierFeeding();
            equipmentAssembly[0] = mRearCarrierFeeding.OrderEquipment("Rear");

            mRearCarrierInput = new CEquipmentFactory_CarrierInput();
            equipmentAssembly[1] = mRearCarrierInput.OrderEquipment("Rear");

            mRearCarrierOutput = new CEquipmentFactory_CarrierOutput();
            equipmentAssembly[2] = mRearCarrierOutput.OrderEquipment("Rear");

            mRearHardening = new CEquipmentFactory_Hardening();
            equipmentAssembly[3] = mRearHardening.OrderEquipment("Rear");

            mRearReject = new CEquipmentFactory_Reject();
            equipmentAssembly[4] = mRearReject.OrderEquipment("Rear");

            mRearReverseSub = new CEquipmentFactory_ReverseSub();
            equipmentAssembly[5] = mRearReverseSub.OrderEquipment("Rear");

            mRearReverse = new CEquipmentFactory_Reverse();
            equipmentAssembly[6] = mRearReverse.OrderEquipment("Rear");

            mRearCarrier = new CEquipmentFactory_Carrier();
            equipmentAssembly[7] = mRearCarrier.OrderEquipment("Rear");
        }
       
        IServoMotors[]  rCarrierFeedingServoMotors;
        ISensors[]      rCarrierFeedingSensors;
        ICylinders[]    rCarrierFeedingCylinders;

        IServoMotors Axis0, Axis1, Axis2, Axis3, Axis4, Axis5, Axis6, Axis7, Axis8;

        ICylinders  Y000, Y001,                                                             Y00C, Y00D, Y00E, Y00F,
                    Y010, Y011, Y012, Y013, Y014, Y015, Y016, Y017, Y018, Y019, Y01A, Y01B, Y01C, Y01D, Y01E, Y01F, 
                    Y020, Y021, Y022, Y023, Y024, Y025,                                                       Y02F;

        ISensors    X000, X001, X002, X003, X004, X005, X006, X007, X008, X009, X00A, X00B, X00C, X00D, X00E, X00F,
                    X010, X011, X012, X013, X014, X015, X016, X017, X018, X019, X01A, X01B, X01C, X01D, X01E, X01F,
                    X020, X021, X022, X023, X024, X025, X026, X027, X028, X029, X02A, X02B, X02C, X02D, X02E, X02F,
                    X030, X031, X032, X033, X034, X035, X036, X037, X038, X039, X03A, X03B, X03C, X03D, X03E, X03F,
                    X040, X041, X042, X043, X044, X045, X046, X047, X048, X049, X04A, X04B, X04C;//, X04D, X04E, X04F;

        private void RearNamingCrrierFeeding()
        {
            this.rCarrierFeedingServoMotors = rearCarrierFeeding.ServoMoters;
            this.rCarrierFeedingSensors = rearCarrierFeeding.Sensors;
            this.rCarrierFeedingCylinders = rearCarrierFeeding.Cylinders;

            //Carrier_X_Axis
            this.Axis0 = rCarrierFeedingServoMotors[0];
            //Buffer_Z_Axis
            this.Axis3 = rCarrierFeedingServoMotors[1];

            //Carrier Stopper Cylinder
            Y010 = rCarrierFeedingCylinders[0];
            //Carrier UpDown Cylinder
            Y011 = rCarrierFeedingCylinders[1];

            //Carrier 자재 유무 감지
            X010 = rCarrierFeedingSensors[0];
            //Carrier_Stopper_Cylinder Up
            X011 = rCarrierFeedingSensors[1];
            //Carrier_Stopper_Cylinder Down
            X012 = rCarrierFeedingSensors[2];
            //Carrier Arriver 감지
            X013 = rCarrierFeedingSensors[3];
            //Carrier 대기 자재 공급 감지
            X014 = rCarrierFeedingSensors[4];
            //Miss Orientation Sensor 1
            X015 = rCarrierFeedingSensors[5];
            //Miss Orientation Sensor 2
            X016 = rCarrierFeedingSensors[6];
            //Miss Orientation Sensor 3
            X017 = rCarrierFeedingSensors[7];
            //Miss Orientation Sensor 4
            X018 = rCarrierFeedingSensors[8];
            //Miss Orientation Sensor 5
            X019 = rCarrierFeedingSensors[9];
            //Miss Orientation Sensor 6
            X01A = rCarrierFeedingSensors[10];
            //Miss Orientation Sensor 7
            X01B = rCarrierFeedingSensors[11];
            //Miss Orientation Sensor 8
            X01C = rCarrierFeedingSensors[12];
            //Output Insert 작동 감지
            X01D = rCarrierFeedingSensors[13];
            //Output Pusher 작동 감지
            X01E = rCarrierFeedingSensors[14];
            //매거진 유무 감지
            X01F = rCarrierFeedingSensors[15];
            //Carrier UpDown Cylinder Up Sensor
            X020 = rCarrierFeedingSensors[16];
            //Carrier UpDown Cylinder Down Sensor
            X021 = rCarrierFeedingSensors[17];
        }

        IServoMotors[]  rCarrierInputServoMotors;
        ISensors[]      rCarrierInputSensors;
        ICylinders[]    rCarrierInputCylinders;
        private void RearNamingCarrierInput()
        {
            this.rCarrierInputServoMotors = rearCarrierInput.ServoMoters;
            this.rCarrierInputSensors = rearCarrierInput.Sensors;
            this.rCarrierInputCylinders = rearCarrierInput.Cylinders;

            //Input Feeding X Axis HF-KP13 100w
            Axis1 = rCarrierInputServoMotors[0];

            //Carrier Input Cylinder
            Y012 = rCarrierInputCylinders[0];

            //Carrier Orientation Sensor
            X023 = rCarrierInputSensors[0];
            //Carrier Miss Feeding Sensor
            X024 = rCarrierInputSensors[1];
            //Carrier Input Cylinder Sensor Up 
            X025 = rCarrierInputSensors[2];
            //Carrier Input Cylinder Sensor Down 
            X026 = rCarrierInputSensors[3];
        }

        IServoMotors[]  rCarrierOutputServoMotors;
        ISensors[]      rCarrierOutputSensors;
        ICylinders[]    rCarrierOutputCylinders;
        private void RearNamingCarrierOutput()
        {
            this.rCarrierOutputServoMotors = this.rearCarrierOutput.ServoMoters;
            this.rCarrierOutputSensors = this.rearCarrierOutput.Sensors;
            this.rCarrierOutputCylinders = this.rearCarrierOutput.Cylinders;

            //Output Feeding X Axis HF-KP13 100w
            Axis2 = rCarrierOutputServoMotors[0];

            //Carrier Input Cylinder
            Y013 = rCarrierOutputCylinders[0];

            //Carrier Orientation Sensor
            X027 = rCarrierOutputSensors[0];
            //Carrier Miss Feeding Sensor
            X028 = rCarrierOutputSensors[1];
            //Carrier Input Cylinder Sensor Up 
            X029 = rCarrierOutputSensors[2];
            //Carrier Input Cylinder Sensor Down 
            X02A = rCarrierOutputSensors[3];
        }

        ISensors[]          rHardeningSensors;
        ICylinders[]        rHardeningCylinders;
        IBarcodeReaders[]   rHardeningBarcodeReaders;
        IBarcodeReaders Barcode;
        private void RearNamingHardening()
        {
            this.rHardeningSensors = this.rearHeardening.Sensors;
            this.rHardeningCylinders = this.rearHeardening.Cylinders;
            this.rHardeningBarcodeReaders = this.rearHeardening.BarcodeReaders;

            //Barcode Reader
            Barcode = rHardeningBarcodeReaders[0];

            //Carrier Shuttle Cylinder Front
            Y014 = rHardeningCylinders[0];
            //Carrier Shuttle Cylinder Rear
            Y015 = rHardeningCylinders[1];

            //Carrier Shuttle Cylinder Front Sensor Up
            X02B = rHardeningSensors[0];
            //Carrier Shuttle Cylinder Front Sensor Down
            X02C = rHardeningSensors[1];
            //Carrier Shuttle Cylinder Rear Sensor Up
            X02D = rHardeningSensors[2];
            //Carrier Shuttle Cylinder Rear Sensor Down
            X02E = rHardeningSensors[3];
            //UV Sensor On/Off 
            X02F = rHardeningSensors[4];
        }

        IServoMotors[]  rRejectServoMotors;
        ISensors[]      rRejectSensors;
        ICylinders[]    rRejectCylinders;
        ICameras[]      rRejectCameras;
        IFlashLights[]  rRejectFlashLights;

        ICameras rRejectCamera, rReverseCamera;
        IFlashLights RejectFlashLight, ReverseFlashLight;

        private void RearNamingRejct()
        {
            this.rRejectServoMotors = this.rearReject.ServoMoters;
            this.rRejectSensors = this.rearReject.Sensors;
            this.rRejectCylinders = this.rearReject.Cylinders;
            this.rRejectCameras = this.rearReject.Cameras;
            this.rRejectFlashLights = this.rearReject.FlashLights;

            //STC-CLC500A, 4096/2048, 3.45um, 8bit bayer
            //Hardening Check
            rRejectCamera = rRejectCameras[0];

            //카메라 촬영 시 사용되는 플레쉬 조명
            //JFLLS-100, 포트:1번
            RejectFlashLight = rRejectFlashLights[0];

            //Reject Y Axis HF-KP13 100w
            Axis4 = rRejectServoMotors[0];
            //Reject X Axis HF-KP13 100w
            Axis5 = rRejectServoMotors[1];

            //Cylinder : Z Axis 
            Y01A = rRejectCylinders[0];
            //Cylinder : Gripper
            Y01B = rRejectCylinders[1];

            //Cylinder : Z Axis : Sensor : Up
            X038 = rRejectSensors[0];
            //Cylinder : Z Axis : Sensor : Down
            X039 = rRejectSensors[1];
            //Cylinder : Gripper : Sensor Up 
            X03A = rRejectSensors[2];
            //Cylinder : Gripper : Sensor Down 
            X03B = rRejectSensors[3];
        }

        ISensors[]          rReverseSubSensors;
        IStepMotors[]       rReverseSubStepMotors;
        ICylinders[]        rReverseSubCylinders;
       
        ISensors SensorReverseSubHome, SensorReverseHome;
        private void RearNamingReverseSub()
        {
            this.rReverseSubSensors = this.rearReverseSub.Sensors;
            this.rReverseSubStepMotors = this.rearReverseSub.StepMoters;
            this.rReverseSubCylinders = this.rearReverseSub.Cylinders;

            //제품 180도 반전 구동용 모터 : 페스트텍 제품
            //RK545AMA Fastech 이지 서보 이용해서 제어
            rReverseSubStepMotor = rReverseSubStepMotors[0];

            //Cylinder : Left Z Axis Sol Up
            Y016 = rReverseSubCylinders[0];
            //Cylinder : Left Z Axis Sol Down
            Y017 = rReverseSubCylinders[1];
            //Cylinder : Right Z Axis Sol Up
            Y018 = rReverseSubCylinders[2];
            //Cylinder : Right Z Axis Sol Down
            Y019 = rReverseSubCylinders[3];

            //Cylinder : Left  Gripper Sol Open
            Y01C = rReverseSubCylinders[4];
            //Cylinder : Left  Gripper Sol Close
            Y01D = rReverseSubCylinders[5];
            //Cylinder : Right  Gripper Sol Open
            Y01E = rReverseSubCylinders[6];
            //Cylinder : Right  Gripper Sol Close
            Y01F = rReverseSubCylinders[7];

            //Cylinder : Left Z Axis Sol : Sensor : Up
            X030 = rReverseSubSensors[0];
            //Cylinder : Left Z Axis Sol : Sensor : Down
            X031 = rReverseSubSensors[1];
            //Cylinder : Right Z Axis Sol : Sensor : Up
            X032 = rReverseSubSensors[2];
            //Cylinder : Right Z Axis Sol : Sensor : Down
            X033 = rReverseSubSensors[3];

            //Cylinder : Left Girpper : Sensor : Open
            X034 = rReverseSubSensors[4];
            //Cylinder : Left Girpper : Sensor : Close
            X035 = rReverseSubSensors[5];
            //Cylinder : Right Girpper : Sensor : Open
            X036 = rReverseSubSensors[6];
            //Cylinder : Right Girpper : Sensor : Close
            X037 = rReverseSubSensors[7];
            //Reverse Home Sensor
            SensorReverseSubHome = rReverseSubSensors[8];
        }

        IServoMotors[]      rReverseServoMoters;
        ISensors[]          rReverseSensors;
        IStepMotors[]       rReverseStepMotors;
        ICameras[]          rReverseCameras;
        IFlashLights[]      rReverseFlashLights;
        ICylinders[]        rReverseCylinders;
        
        IStepMotors rReverseStepMotor,rReverseSubStepMotor;
        private void RearNamingReverse()
        {
            this.rReverseServoMoters = this.rearReverse.ServoMoters;
            this.rReverseSensors = this.rearReverse.Sensors;
            this.rReverseStepMotors = this.rearReverse.StepMoters;
            this.rReverseFlashLights = this.rearReverse.FlashLights;
            this.rReverseCylinders = this.rearReverse.Cylinders;

            //Reverse X-Axis HF-K23(+Limit, Home, -Limit) 
            Axis6 = rReverseServoMoters[0];
            //Reverse Y-Axis HF-K13(+Limit, Home, -Limit) 
            Axis7 = rReverseServoMoters[1];
            //Reverse Z-Axis HF-K23B(+Limit, Home, -Limit) 
            Axis8 = rReverseServoMoters[2];

            //얼라인 구동용 모터 : 페스트텍 제품
            //Reverse R Axis CFK513AP2
            rReverseStepMotor = rReverseStepMotors[0];

            //Aline 작업용 카메라 반전검사 광학계
            //STC-CLC500A, 4096/2048, 3.45um, 8bit bayer
            //SOLIOS 보드:0, 디지타이저:1
            rReverseCamera = rReverseCameras[0];

            //카메라 촬영시에 사용되는 플레쉬 조명
            //JFLLS-100, 포트:2번
            ReverseFlashLight = rReverseFlashLights[0];

            //Pick Up : Left Sol Vacuum
            Y020 = rReverseCylinders[0];
            //Pick Up : Left Sol Blower
            Y021 = rReverseCylinders[1];
            //Pick Up : Right Sol Vacuum
            Y022 = rReverseCylinders[2];
            //Pick Up : Right Sol Blower
            Y023 = rReverseCylinders[3];
            //Cylinder : Left  Z Axis Sol
            Y024 = rReverseCylinders[4];
            //Cylinder : Right  Z Axis Sol
            Y025 = rReverseCylinders[5];

            //Sensor : 진공확인 : Pick Up : Left Sol
            //Vacuum을 활성화 시킨 후 이 센싱이 된후에 다음도작을 진행한다.
            X04B = rReverseSensors[0];
            //Sensor : 진공확인 : Pick Up : Right Sol
            X04C = rReverseSensors[1];

            //Sensor : Up   Cylinder : Left  Z Axis Sol
            X047 = rReverseSensors[2];
            //Sensor : Down Cylinder : Left  Z Axis Sol
            X048 = rReverseSensors[3];
            //Sensor : Up   Cylinder : Right  Z Axis Sol
            X049 = rReverseSensors[4];
            //Sensor : Down Cylinder : Right  Z Axis Sol
            X04A = rReverseSensors[5];
            //Sensor : Reverse R-Axis Home Sensor
            SensorReverseHome = rReverseSensors[6];
        }
    }
}
