using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemAlign
{
    //Reverse Assembly elements List
    //1.Servo Motor :       3EA :   HF-KP23, HF-KP13, HF-KP23B   
    //1.Step Motor  :       1EA :   CFK513AP2
    //2.Cyliender :         6EA :   MXH6_10_M9BL
    //2.SolValve    :       6EA :   SY3120_5LZ_C4
    //3.PhotoSensor :       6EA :   EE_SX671
    //4.BeamSensor  :       1EA :   EX_14A
    //7.Camera      :       1EA :   STC-CLC500A 4096/2048, 3.45um, 8bitbayer(bmp)
    //8.Light       :       1EA :   JFLLS-100 Port2번
    public class PAFCAssemblyReverse : PAFIAssemblyIngredientFactory
    {
        public PAFCAssemblyReverse() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { new PAFCOtp_ServoMotor(), new PAFCOtp_ServoMotor(), new PAFCOtp_ServoMotor() };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { new PAFCOtp_StepMotor()};
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(),
                                              new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder()};
            return elementCylinders;
        }


        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = {   new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), 
                                            new PAFCInp_Sensor(), new PAFCInp_StepMotor_Home() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders = {  };
            return elementBarcodeReaders;
        }

        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { new PAFCInp_Camera() };
            return elementCameras;
        }

        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { new PAFCInp_FlashLight() };
            return elementFlashLights;
        }
    }

    //ReverseSub Assembly elements List
    //1.Servo Motor :       0EA :   HF-KP13   
    //1.Step Motor  :       1EA :   RK545AMA
    //2.Cyliender :         0EA :   MXH6_10_M9BL
    //2.SolValve    :       8EA :   SY3120_5LZ_C4
    //3.PhotoSensor :       8EA :   EE_SX671
    //4.BeamSensor  :       1EA :   EX_14A
    //5.UVSensor    :       0EA :   
    //6.Barcode     :       0EA :   
    //7.Camera      :       0EA :   XCL-5005
    //8.Light       :       0EA :   RLW-50-28
    public class PAFCAssemblyReverseSub : PAFIAssemblyIngredientFactory
    {
        public PAFCAssemblyReverseSub() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { new PAFCOtp_StepMotor() };
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = {   new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(),
                                                new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder(),};
            return elementCylinders;
        }

        

        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = { new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), 
                                                    new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_StepMotor_Home() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }

        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { };
            return elementCameras;
        }

        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }
    }

    //Reject Assembly elements List
    //1.Servo Motor :       2EA :   HF-KP13    
    //2.Cyliender :         2EA :   MXH6_10_M9BL
    //3.PhotoSensor :       2EA :   EE_SX671
    //4.BeamSensor  :       2EA :   EX_14A
    //5.UVSensor    :       0EA :   
    //6.Barcode     :       0EA :   
    //7.Camera      :       1EA :   STC-CLC500A 4096/2048, 3.45um, 8bitbayer(bmp)
    //8.Light       :       1EA :   JFLLS-100 Port1번
    public class PAFCAssemblyReject : PAFIAssemblyIngredientFactory
    {
        public PAFCAssemblyReject() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { new PAFCOtp_ServoMotor(), new PAFCOtp_ServoMotor() };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder() };
            return elementCylinders;
        }

        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = { new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders= { };
            return elementBarcodeReaders;
        }

        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { new PAFCInp_Camera()};
            return elementCameras;
        }
        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { new PAFCInp_FlashLight()};
            return elementFlashLights;
        }
    }


    //Hardening Assembly elements List
    //1.Servo Motor :       0EA :      
    //2.Cyliender :         2EA :   MXH6_10_M9BL
    //3.PhotoSensor :       0EA :      
    //4.BeamSensor  :       4EA :   EX_14A
    //5.UVSensor    :       1EA :   UV-On/Off Sensor
    //6.Barcode     :       1EA :   Barcode Reader
    public class PAFAssemblyHardening : PAFIAssemblyIngredientFactory
    {
        public PAFAssemblyHardening() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder() };
            return elementCylinders;
        }
        
        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = { new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders= {new PAFCInp_BarcodeReading()};
            return elementBarcodeReaders;
        }

        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { };
            return elementCameras;
        }
        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }
    }



    //Carrier Input Assembly elements List
    //1.Servo Motor :       1EA :      HF-KP13
    //2.Cyliender :           1EA :      MXH6_10_M9BL
    //3.PhotoSensor :       2EA :      EE_SX671
    //4.BeamSensor :        2EA :      EX_14A
    public class PAFCAssemblyCarrierInput : PAFIAssemblyIngredientFactory
    {
        public PAFCAssemblyCarrierInput() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { new PAFCOtp_ServoMotor() };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder() };
            return elementCylinders;
        }

       

        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = { new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders= {};
            return elementBarcodeReaders;
        }
        public PAFISensorUVs[] CreateSensorUVs()
        {
            PAFISensorUVs[] elementSensorUVs = {};
            return elementSensorUVs;
        }
        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { };
            return elementCameras;
        }
        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }
    }

    //Carrier Output Assembly elements List
    //1.Servo Motor :       1EA :      HF-KP13
    //2.Cyliender :           1EA :      MXH6_10_M9BL
    //3.PhotoSensor :       2EA :      EE_SX671
    //4.BeamSensor :        2EA :      EX_14A
    public class PAFAssemblyCarrierOutput : PAFIAssemblyIngredientFactory
    {
        public PAFAssemblyCarrierOutput() { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { new PAFCOtp_ServoMotor() };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder() };
            return elementCylinders;
        }

        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = { new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor() };
            return elementSensors;
        }

        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders= {};
            return elementBarcodeReaders;
        }
        public PAFISensorUVs[] CreateSensorUVs()
        {
            PAFISensorUVs[] elementSensorUVs = {};
            return elementSensorUVs;
        }
        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { };
            return elementCameras;
        }
        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }
    }

 
    //Carrier Feeding Assembly Element List
    //Sensor List
    //01.X010;1EA:Carrier 자재 유무감지
    //02.X011:1EA:Cylinder Sensor Up
    //03.X012:1EA:Cylinder Sensor Down
    //04.X013:1EA:Carrier Arriver 감지
    //05.X014;1EA:Carrier 자재 유무감지
    //06.X015;1EA:Carrier Miss Operation Sensor
    //07.X016;1EA:Carrier Miss Operation Sensor
    //08.X017;1EA:Carrier Miss Operation Sensor
    //09.X018;1EA:Carrier Miss Operation Sensor
    //10.X019;1EA:Carrier Miss Operation Sensor
    //11.X01A;1EA:Carrier Miss Operation Sensor
    //12.X01B;1EA:Carrier Miss Operation Sensor
    //13.X01C;1EA:Carrier Miss Operation Sensor
    //14.X01D:1EA:Carrier Output Insert 작동 감지
    //15.X01E:1EA:Carrier Output Pusher 작동 감지
    //16.X01F:1EA:매거진 유무 감지
    //17.X020:1EA:Cylinder Sensor Up
    //18.X021:1EA:Cylinder Sensor Down
    //Cylinder List
    //01.Y010:1EA:Carrie Stopper Cylinder
    //02.Y011:2EA:Up/Down Cylinder
    //Axis List
    //01.Axis0:1-1-1:Carrier Feeding X-Axis HF-KP13 100W
    //02.Axis3:1-2-4:Buffer Feeding Z-Axis HF-KP13B 100W
    public class PAFCAssemblyCarrierFeeding : PAFIAssemblyIngredientFactory
    {
        public PAFCAssemblyCarrierFeeding()        { }

        public PAFIServoMotors[] CreateServoMotors()
        {
            PAFIServoMotors[] elementServoMotors = { new PAFCOtp_ServoMotor(), new PAFCOtp_ServoMotor() };
            return elementServoMotors;
        }

        public PAFIStepMotors[] CreateStepMotors()
        {
            PAFIStepMotors[] elementStopMotors = {};               return elementStopMotors;
        }

        public PAFICylinders[] CreateCylinders()
        {
            PAFICylinders[] elementCylinders = { new PAFCOtp_Cylinder(), new PAFCOtp_Cylinder() };
            return elementCylinders;
        }


        public PAFIBarcodeReaders[] CreateBarcodeReaders()
        {
            PAFIBarcodeReaders[] elementBarcodeReaders= {};        return elementBarcodeReaders;
        }

        public PAFISensorUVs[] CreateSensorUVs()
        {
            PAFISensorUVs[] elementSensorUVs = {};                 return elementSensorUVs;
        }

        public PAFISensors[] CreateSensors()
        {
            PAFISensors[] elementSensors = {new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(),
                                              new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(),
                                              new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor(), new PAFCInp_Sensor()};
            return elementSensors;
        }

        public PAFICameras[] CreateCameras()
        {
            PAFICameras[] elementCameras = { };                    return elementCameras;
        }

        public PAFIFlashLights[] CreateFlashLights()
        {
            PAFIFlashLights[] elementFlashLights = { };            return elementFlashLights;
        }
    }

    public interface PAFIAssemblyIngredientFactory
	{
        PAFIServoMotors[] CreateServoMotors();
        PAFIStepMotors[] CreateStepMotors();
        PAFICylinders[] CreateCylinders(); 
        PAFIBarcodeReaders[] CreateBarcodeReaders();
        PAFICameras[] CreateCameras();
        PAFIFlashLights[] CreateFlashLights();
        PAFISensors[] CreateSensors();
	}


    public abstract class PAFAEquipmentFactorys
    {
        public PAFAEquipmentFactorys()        { }
        public PAFAEquipments OrderEquipment(string type)
        {
            PAFAEquipments equipment;
            equipment = CreateEquipment(type);
            return equipment;
        }
        protected abstract PAFAEquipments CreateEquipment(string type);
    }

    public class PAFCEquipmentFactory_Reverse : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_Reverse() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFCAssemblyReverse();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Reverse\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Reverse\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class PAFCEquipmentFactory_ReverseSub : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_ReverseSub() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFCAssemblyReverseSub();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : ReverseSub\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : ReverseSub\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class PAFCEquipmentFactory_Reject : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_Reject() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFCAssemblyReject();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Reject\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Reject\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class PAFCEquipmentFactory_Hardening : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_Hardening() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFAssemblyHardening();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Hardening\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Hardening\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class PAFCEquipmentFactory_CarrierOutput : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_CarrierOutput() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFAssemblyCarrierOutput();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierOutput\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierOutput\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class PAFCEquipmentFactory_CarrierInput : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_CarrierInput() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFCAssemblyCarrierInput();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierInput\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierInput\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class PAFCEquipmentFactory_CarrierFeeding : PAFAEquipmentFactorys
    {
        public PAFCEquipmentFactory_CarrierFeeding() { }

        protected override PAFAEquipments CreateEquipment(string type)
        {
            PAFAEquipments equipment = null;
            PAFIAssemblyIngredientFactory ingredientFactory = new PAFCAssemblyCarrierFeeding();

            switch (type)
            {
                case "Front":
                    equipment = new PAFCEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierFeeding\r\n";
                    break;
                case "Rear":
                    equipment = new PAFCEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierFeeding\r\n";
                    break;
            }
            return equipment;
        }
    }

    
    

    public abstract class PAFAEquipments
    {
        protected PAFIStepMotors[] motorSteps;
        protected PAFIServoMotors[] motorServos;
        
        protected PAFICylinders[] cylinders;   
        protected PAFIBarcodeReaders[] barcodeReaders;
        protected PAFICameras[] cameras;
        protected PAFIFlashLights[] flashLights;

        protected PAFISensors[] sensors;

        public PAFIStepMotors[] StepMoters
        {
            get { return this.motorSteps; }
            set { motorSteps = value; }
        }

        public PAFIServoMotors[] ServoMoters
        {
            get { return this.motorServos; }
            set { motorServos = value; }
        }

        public PAFICylinders[] Cylinders
        {
            get { return this.cylinders; }
            set { cylinders = value; }
        }

        public PAFIBarcodeReaders[] BarcodeReaders
        {
            get { return this.barcodeReaders; }
            set { barcodeReaders = value; }
        }

        public PAFICameras[] Cameras
        {
            get { return this.cameras; }
            set { cameras = value; }
        }

        public PAFIFlashLights[] FlashLights
        {
            get { return this.flashLights; }
            set { flashLights = value; }
        }

        public PAFISensors[] Sensors
        {
            get { return this.sensors; }
            set { sensors = value; }
        }

        public PAFISensors sensor;
        public PAFISensors Sensor
        {
            get {
                this.sensor = this.Sensors[0];
                return this.sensor; }
            set { sensor = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public PAFAEquipments()        { }
        public abstract string Prepare();
    }

    public class PAFCEquipmentRears : PAFAEquipments
    {
        PAFIAssemblyIngredientFactory assemblyIngredientFactory;

        public PAFCEquipmentRears(PAFIAssemblyIngredientFactory assemblyIngredientFactory)
        {
            this.assemblyIngredientFactory = assemblyIngredientFactory;
        }

        

        int elementCount = 1;
        public override string Prepare()
        {
            motorSteps = assemblyIngredientFactory.CreateStepMotors();
            motorServos = assemblyIngredientFactory.CreateServoMotors();            
            cylinders = assemblyIngredientFactory.CreateCylinders();
            barcodeReaders = assemblyIngredientFactory.CreateBarcodeReaders();            
            cameras = assemblyIngredientFactory.CreateCameras();
            flashLights = assemblyIngredientFactory.CreateFlashLights();
            sensors = assemblyIngredientFactory.CreateSensors();
                        
			StringBuilder sb = new StringBuilder();
            sb.Append("Preparing " + Name + "\n");

            for (int i = 0; i < motorSteps.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Motor Step      : " + motorSteps[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < motorServos.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Motor Servo      : " + motorServos[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < cylinders.Length; i++)
            {
                sb.Append(elementCount.ToString("00") +"  "+ (i + 1).ToString("00") + "Cylinder : " + cylinders[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < barcodeReaders.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "BarcodeReaders : " + barcodeReaders[i].toString() + "\r\n");
                elementCount++;
            }
            for (int i = 0; i < cameras.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Cameras : " + cameras[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < flashLights.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "FlashLights : " + flashLights[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < sensors.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Sensors : " + sensors[i].toString() + "\r\n");
                elementCount++;
            }
           
			return sb.ToString();            
        }
    }

    public class PAFCEquipmentFronts : PAFAEquipments
    {
        PAFIAssemblyIngredientFactory assemblyIngredientFactory;

        public PAFCEquipmentFronts(PAFIAssemblyIngredientFactory assemblyIngredientFactory)
        {
            this.assemblyIngredientFactory = assemblyIngredientFactory;
        }

        int elementCount = 1;
        public override string Prepare()
        {
            motorSteps = assemblyIngredientFactory.CreateStepMotors();
            motorServos = assemblyIngredientFactory.CreateServoMotors();

            cylinders = assemblyIngredientFactory.CreateCylinders();
            barcodeReaders = assemblyIngredientFactory.CreateBarcodeReaders();

            cameras = assemblyIngredientFactory.CreateCameras();
            flashLights = assemblyIngredientFactory.CreateFlashLights();

            sensors = assemblyIngredientFactory.CreateSensors();

            StringBuilder sb = new StringBuilder();
            sb.Append("Preparing " + Name + "\n");

            for (int i = 0; i < motorSteps.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Motor Step      : " + motorSteps[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < motorServos.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Motor Servo      : " + motorServos[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < cylinders.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Cylinder : " + cylinders[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < barcodeReaders.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "BarcodeReaders : " + barcodeReaders[i].toString() + "\r\n");
                elementCount++;
            }
            for (int i = 0; i < cameras.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Cameras : " + cameras[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < flashLights.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "FlashLights : " + flashLights[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < sensors.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Sensors : " + sensors[i].toString() + "\r\n");
                elementCount++;
            }
            return sb.ToString();
        }
    }

    

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFISensors
    {
        bool Status { get; set; }
        string toString();
    }
    public class PAFCInp_Sensor : PAFISensors
    {
        bool statusData = false;

        public bool Status
        {
            get { return this.statusData; }
            set { this.statusData = value; }
        }

        public string toString()
        {
            return "Inp_Sensor";
        }
    }

    //이 센서는 페스트텍의 디바이스를 통해서 입력되는 상태 신호이다.
    public class PAFCInp_StepMotor_Home : PAFISensors
    {
        bool statusData = false;

        public bool Status
        {
            get { return this.statusData; }
            set { this.statusData = value; }
        }

        public string toString()
        {
            return "Inp_StepMotor_Home";
        }
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFISensorsFastech
    {
        bool Status { get; set; }
        string toString();
    }

    public class PAFCInp_Fastech_Home : PAFISensorsFastech
    {
        bool statusData = false;

        public bool Status
        {
            get { return this.statusData; }
            set { this.statusData = value; }
        }

        public string toString()
        {
            return "Inp_Fastech_Home";
        }
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFISensorPhotos
    {
        bool Status{get;set;}
        string toString();        
    }

    

    public class PAFCInp_EE_SX913_R : PAFISensorPhotos
    {
        bool statusData;

        public bool Status
        {
            get{return this.statusData;} 
            set{this.statusData = value; }
        }

        public string toString()
        {
            return "EE_SX913_R";
        }
    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    public interface PAFIBarcodeReaders
    {
        bool Status{get;set;}
        string toString();           
        string ReadedData{get;}
    }

    public class PAFCInp_BarcodeReading : PAFIBarcodeReaders
    {
        bool bStatusData;
        string strReadedData;
        public bool Status
        {
            get { return this.bStatusData; }
            set { this.bStatusData = value; }
        }

        public string ReadedData
        {
            get{return this.strReadedData;} 
        }

        public string toString()
        {
            return "BarcodeReader";
        }        
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    public interface PAFISensorUVs
    {
        bool Status{get;set;}
        string toString();           
    }

    public class PAFCInp_UV_OnOff_Sensor : PAFISensorUVs
    {
        bool statusData;

        public bool Status
        {
            get{return this.statusData;} 
            set{this.statusData = value; }
        }

        public string toString()
        {
            return "UV_OnOff_Sensor";
        }
    }
   

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
     //스텝 모터 인터페이스
    public interface PAFIStepMotors
    {        
        int PortNo { get; set; }
        int Baudrate { get; set; }
        float Status{get; set;}
        string toString();
        string MoveInc(float movePoint);
        string MoveAbs(float movePoint);
    }

    public class PAFCOtp_StepMotor : PAFIStepMotors
    {
        int m_iPortNo;
        int m_iBaudrate;
        public int PortNo
        {
            get { return this.m_iPortNo; }
            set { this.m_iPortNo = value; }
        }
        public int Baudrate
        {
            get { return this.m_iBaudrate; }
            set { this.m_iBaudrate = value; }
        }

        float locationData;
        public float Status
        {
            get { return this.locationData; }
            set { this.locationData = value; }
        }

        public string toString()
        {
            return "Otp_StepMotor";
        }

        public string MoveInc(float movePoint)
        {
            string moveIncStr = "상대 좌료 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }

        public string MoveAbs(float movePoint)
        {
            string moveIncStr = "절대 좌표 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }
    }

    //Carrier Feeding Assembly : Servo Motor : Carrier Feeding X Axis HF-KP13
    public class PAFCOtp_CFK513AP2 : PAFIStepMotors
    {
        int m_iPortNo;
        int m_iBaudrate;
        public int PortNo
        {
            get { return this.m_iPortNo; }
            set { this.m_iPortNo = value; }
        }
        public int Baudrate
        {
            get { return this.m_iBaudrate; }
            set { this.m_iBaudrate = value; }
        }

        float locationData;
        public float Status
        {
            get { return this.locationData; }
            set { this.locationData = value; }
        }

        public string toString()
        {
            return "RK545AMA";
        }

        public string MoveInc(float movePoint)
        {
            string moveIncStr = "상대 좌료 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }

        public string MoveAbs(float movePoint)
        {
            string moveIncStr = "절대 좌표 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }
    }

    //Carrier Feeding Assembly : Servo Motor : Carrier Feeding X Axis HF-KP13
    public class PAFCOtp_RK545AMA : PAFIStepMotors
    {
        int m_iPortNo;
        int m_iBaudrate;
        public int PortNo
        {
            get { return this.m_iPortNo; }
            set { this.m_iPortNo = value; }
        }
        public int Baudrate
        {
            get { return this.m_iBaudrate; }
            set { this.m_iBaudrate = value; }
        }

        float locationData;
        public float Status
        {
            get { return this.locationData; }
            set { this.locationData = value; }
        }

        public string toString()
        {
            return "RK545AMA";
        }

        public string MoveInc(float movePoint)
        {
            string moveIncStr = "상대 좌료 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }

        public string MoveAbs(float movePoint)
        {
            string moveIncStr = "절대 좌표 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    //서보 모터 인터페이스
    public interface PAFIServoMotors
    {
        float Status { get; set; }
        float fPLimit { get; set; }
        float fNLimit { get; set; }
        string toString();
        string MoveInc(float movePoint);
        string MoveAbs(float movePoint);
        string SearchHome(bool bSearching); 
    }

    public class PAFCOtp_ServoMotor : PAFIServoMotors
    {
        float locationData;

        public float Status
        {
            get { return this.locationData; }
            set { this.locationData = value; }
        }

        float fPlimitData, fNlimitData;

        public float fPLimit
        {
            get { return this.fPlimitData; }
            set { this.fPlimitData = value; }
        }
        public float fNLimit
        {
            get { return this.fNlimitData; }
            set { this.fNlimitData = value; }
        }

        public string SearchHome(bool bSearching)
        {
            string strSearchingData = "SearchHome Function!";
            return strSearchingData;
        }

        public string toString()
        {
            return "Otp_ServoMotor";
        }

        public string MoveInc(float movePoint)
        {
            string moveIncStr = "상대 좌료 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }

        public string MoveAbs(float movePoint)
        {
            string moveIncStr = "절대 좌표 X축:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }
    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFICameras
    {
        System.Drawing.Size CamSize { get; set; }
        System.Drawing.Size CamResolution { get; set; }
        bool Status { get; set; }
        string toString();
        string Graping(string strSaveName);
    }

    public class PAFCInp_Camera : PAFICameras
    {
        System.Drawing.Size m_szCamSize;
        System.Drawing.Size m_szCamResolution;

        public System.Drawing.Size CamSize
        {
            get { return this.m_szCamSize; }
            set { this.m_szCamSize = value; }
        }
        public System.Drawing.Size CamResolution
        {
            get { return this.m_szCamResolution; }
            set { this.m_szCamResolution = value; }
        }

        bool bStatusData;
        bool bReadyGrap = true;

        public bool Status
        {
            get { return this.bStatusData; }
            set { this.bStatusData = value; }
        }

        public string toString()
        {
            return "Inp_Camera";
        }

        public string Graping(string strSaveName)
        {
            if (bReadyGrap == true) strSaveName = "이미지를 확보합니다.\r\n";
            else strSaveName = "이미지를 확보안합니다.\r\n";

            return strSaveName;
        }
    }

    //Carrier Feeding Assembly : Cylinder : Carrier Stopper Cylinder 1EA : Y010
    public class PAFCInp_XCL_5005 : PAFICameras
    {
        System.Drawing.Size m_szCamSize;
        System.Drawing.Size m_szCamResolution;

        public System.Drawing.Size CamSize
        {
            get { return this.m_szCamSize; }
            set { this.m_szCamSize = value; }
        }
        public System.Drawing.Size CamResolution
        {
            get { return this.m_szCamResolution; }
            set { this.m_szCamResolution = value; }
        }

        bool bStatusData;
        bool bReadyGrap = true;

        public bool Status
        {
            get { return this.bStatusData; }
            set { this.bStatusData = value; }
        }

        public string toString()
        {
            return "XCL_5005";
        }

        public string Graping(string strSaveName)
        {
            if (bReadyGrap == true) strSaveName = "이미지를 확보합니다.\r\n";
            else strSaveName = "이미지를 확보안합니다.\r\n";

            return strSaveName;
        }
    }

    //Carrier Feeding Assembly : Cylinder : Carrier Stopper Cylinder 1EA : Y010
    public class PAFCInp_STC_CLC500A : PAFICameras
    {
        System.Drawing.Size m_szCamSize;
        System.Drawing.Size m_szCamResolution;

        public System.Drawing.Size CamSize
        {
            get { return this.m_szCamSize; }
            set { this.m_szCamSize = value; }
        }
        public System.Drawing.Size CamResolution
        {
            get { return this.m_szCamResolution; }
            set { this.m_szCamResolution = value; }
        }

        bool bStatusData;
        bool bReadyGrap = true;
        public bool Status
        {
            get { return this.bStatusData; }
            set { this.bStatusData = value; }
        }

        public string toString()
        {
            return "STC_CLC500A";
        }

        public string Graping(string strSaveName)
        {
            
            if (bReadyGrap == true) strSaveName = "이미지를 확보합니다.\r\n";
            else strSaveName = "이미지를 확보안합니다.\r\n";

            return strSaveName;
        }
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFIFlashLights
    {
        uint LightPort { get; set; }
        uint LightType { get; set; }
        uint LightNo { get; set; }
        uint LightCh { get; set; }

        bool Status { get; set; }
        int LightForce { get; set; }
        string toString();
        string SetLightForce(int nLightForce);
    }

    public class PAFCInp_FlashLight : PAFIFlashLights
    {
        uint m_uLightPort, m_uLightType, m_uLightNo, m_uLightCh;
        public uint LightPort
        {
            get { return this.m_uLightPort; }
            set { this.m_uLightPort = value; }
        }

        public uint LightType
        {
            get { return this.m_uLightType; }
            set { this.m_uLightType = value; }
        }

        public uint LightNo
        {
            get { return this.m_uLightNo; }
            set { this.m_uLightNo = value; }
        }

        public uint LightCh
        {
            get { return this.m_uLightCh; }
            set { this.m_uLightCh = value; }
        }

        public int LightForce
        {
            get { return this.nLightForce; }
            set { this.nLightForce = value; }
        }

        bool bStatus;
        int nLightForce;

        public bool Status
        {
            get { return this.bStatus; }
            set { this.bStatus = value; }
        }        

        public string toString()
        {
            return "Inp_FlashLight";
        }

        public string SetLightForce(int nLightForce)
        {
            string strResultData;
            bool bReadyLight = true;
            if (bReadyLight == true) strResultData = "조명 설정 진행합니다..\r\n";
            else strResultData = "조명 설정 진행안합니다..\r\n";

            return strResultData;
        }
    }

    //Carrier Feeding Assembly : Cylinder : Carrier Stopper Cylinder 1EA : Y010
    public class PAFCInp_JFLLS_100 : PAFIFlashLights
    {
        uint m_uLightPort, m_uLightType, m_uLightNo, m_uLightCh;
        public uint LightPort
        {
            get { return this.m_uLightPort; }
            set { this.m_uLightPort = value; }
        }

        public uint LightType
        {
            get { return this.m_uLightType; }
            set { this.m_uLightType = value; }
        }

        public uint LightNo
        {
            get { return this.m_uLightNo; }
            set { this.m_uLightNo = value; }
        }

        public uint LightCh
        {
            get { return this.m_uLightCh; }
            set { this.m_uLightCh = value; }
        }


        bool bStatus;
        int nLightForce;

        public bool Status
        {
            get { return this.bStatus; }
            set { this.bStatus = value; }
        }

        public int LightForce
        {
            get { return this.nLightForce; }
            set { this.nLightForce = value; }
        }

        public string toString()
        {
            return "JFLLS_100";
        }

        public string SetLightForce(int nLightForce)
        {
            string strResultData;
            bool bReadyLight = true;
            if (bReadyLight == true) strResultData = "조명 설정 진행합니다..\r\n";
            else strResultData = "조명 설정 진행안합니다..\r\n";

            return strResultData;
        }
    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface PAFICylinders
    {
        bool Status{get; set;}       
        string toString();
        string MoveCylinder(string UpDown);
    }

    public class PAFCOtp_Cylinder : PAFICylinders
    {
        bool UpDownData;

        public bool Status
        {
            get { return this.UpDownData; }
            set { this.UpDownData = value; }
        }

        public string toString()
        {
            return "Otp_Cylinder";
        }

        public string MoveCylinder(string OpenCloseData)
        {
            string moveCylinderStr;
            if (OpenCloseData.Trim().ToUpper() == "UP") moveCylinderStr = "실린더를 올립니다.\r\n";
            else moveCylinderStr = "실린더를 올립니다.\r\n";

            return moveCylinderStr;
        }
    }

}
