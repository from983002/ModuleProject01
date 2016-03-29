using System.Text;

namespace Pattern_Modules
{
    public class AbstractFactory
    {
    }


    //Reverse Assembly elements List
    //1.Servo Motor :       3EA :   HF-KP23, HF-KP13, HF-KP23B   
    //1.Step Motor  :       1EA :   CFK513AP2
    //2.Cyliender :         6EA :   MXH6_10_M9BL
    //2.SolValve    :       6EA :   SY3120_5LZ_C4
    //3.PhotoSensor :       6EA :   EE_SX671
    //4.BeamSensor  :       1EA :   EX_14A
    //7.Camera      :       1EA :   STC-CLC500A 2448/2048, 3.45um, 8bitbayer(bmp)
    //8.Light       :       1EA :   JFLLS-100 Port2번
    public class CAssemblyReverse : IAssemblyIngredientFactory
    {
        public CAssemblyReverse() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { new COtp_ServoMotor(), new COtp_ServoMotor(), new COtp_ServoMotor() };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { new COtp_StepMotor() };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(),
                                              new COtp_Cylinder(), new COtp_Cylinder()};
            return elementCylinders;
        }


        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = {   new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), 
                                            new CInp_Sensor(), new CInp_StepMotor_Home() };
            return elementSensors;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { new IFCarrier(),new IFCarrier(),new IFCarrier(),new IFCarrier(),new IFCarrier(),
                                            new IFCarrier(),new IFCarrier(),new IFCarrier(),new IFCarrier(),new IFCarrier()};
            return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { new IFProduct(),new IFProduct(),new IFProduct(),new IFProduct(),new IFProduct(),
                                            new IFProduct(),new IFProduct(),new IFProduct(),new IFProduct(),new IFProduct()};
            return elementProducts;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { new CInp_Camera() };
            return elementCameras;
        }

        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { new CInp_FlashLight() };
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
    public class CAssemblyReverseSub : IAssemblyIngredientFactory
    {
        public CAssemblyReverseSub() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { new COtp_StepMotor() };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = {   new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(),
                                                new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(), new COtp_Cylinder(),};
            return elementCylinders;
        }



        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), 
                                                    new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor()};
            return elementSensors;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { };
            return elementCameras;
        }

        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
        }
    }

    //Reject Assembly elements List
    //1.Servo Motor :       2EA :   HF-KP13    
    //2.Cyliender :         2EA :   MXH6_10_M9BL
    //3.PhotoSensor :       2EA :   EE_SX671
    //4.BeamSensor  :       2EA :   EX_14A
    //5.UVSensor    :       0EA :   
    //6.Barcode     :       0EA :   
    //7.Camera      :       1EA :   STC-CLC500A 2448/2048, 3.45um, 8bitbayer(bmp)
    //8.Light       :       1EA :   JFLLS-100 Port1번
    public class CAssemblyReject : IAssemblyIngredientFactory
    {
        public CAssemblyReject() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { new COtp_ServoMotor(), new COtp_ServoMotor() };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder(), new COtp_Cylinder() };
            return elementCylinders;
        }

        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor() };
            return elementSensors;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { new CInp_Camera() };
            return elementCameras;
        }
        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { new CInp_FlashLight() };
            return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
        }
    }


    //Hardening Assembly elements List
    //1.Servo Motor :       0EA :      
    //2.Cyliender :         2EA :   MXH6_10_M9BL
    //3.PhotoSensor :       0EA :      
    //4.BeamSensor  :       4EA :   EX_14A
    //5.UVSensor    :       1EA :   UV-On/Off Sensor
    //6.Barcode     :       1EA :   Barcode Reader
    public class AssemblyHardening : IAssemblyIngredientFactory
    {
        public AssemblyHardening() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder(), new COtp_Cylinder() };
            return elementCylinders;
        }

        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor() };
            return elementSensors;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { new CInp_BarcodeReading() };
            return elementBarcodeReaders;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { };
            return elementCameras;
        }
        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
        }
    }



    //Carrier Input Assembly elements List
    //1.Servo Motor :       1EA :      HF-KP13
    //2.Cyliender :           1EA :      MXH6_10_M9BL
    //3.PhotoSensor :       2EA :      EE_SX671
    //4.BeamSensor :        2EA :      EX_14A
    public class CAssemblyCarrierInput : IAssemblyIngredientFactory
    {
        public CAssemblyCarrierInput() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { new COtp_ServoMotor() };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder() };
            return elementCylinders;
        }



        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor() };
            return elementSensors;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }
        public ISensorUVs[] CreateSensorUVs()
        {
            ISensorUVs[] elementSensorUVs = { };
            return elementSensorUVs;
        }
        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { };
            return elementCameras;
        }
        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
        }
    }

    //Carrier Output Assembly elements List
    //1.Servo Motor :       1EA :      HF-KP13
    //2.Cyliender :           1EA :      MXH6_10_M9BL
    //3.PhotoSensor :       2EA :      EE_SX671
    //4.BeamSensor :        2EA :      EX_14A
    public class CAssemblyCarrierOutput : IAssemblyIngredientFactory
    {
        public CAssemblyCarrierOutput() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { new COtp_ServoMotor() };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { };
            return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder() };
            return elementCylinders;
        }

        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor() };
            return elementSensors;
        }

        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { };
            return elementBarcodeReaders;
        }
        public ISensorUVs[] CreateSensorUVs()
        {
            ISensorUVs[] elementSensorUVs = { };
            return elementSensorUVs;
        }
        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { };
            return elementCameras;
        }
        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { };
            return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
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
    public class CAssemblyCarrierFeeding : IAssemblyIngredientFactory
    {
        public CAssemblyCarrierFeeding() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = { new COtp_ServoMotor(), new COtp_ServoMotor() };
            return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { }; return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { new COtp_Cylinder(), new COtp_Cylinder() };
            return elementCylinders;
        }


        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { }; return elementBarcodeReaders;
        }

        public ISensorUVs[] CreateSensorUVs()
        {
            ISensorUVs[] elementSensorUVs = { }; return elementSensorUVs;
        }

        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = {new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(),
                                              new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(),
                                              new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor(), new CInp_Sensor()};
            return elementSensors;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { }; return elementCameras;
        }

        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { }; return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { }; return elementCarriers;
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { }; return elementProducts;
        }
    }


    public class CAssemblyCarrier : IAssemblyIngredientFactory
    {
        public CAssemblyCarrier() { }

        public IServoMotors[] CreateServoMotors()
        {
            IServoMotors[] elementServoMotors = {};return elementServoMotors;
        }

        public IStepMotors[] CreateStepMotors()
        {
            IStepMotors[] elementStopMotors = { }; return elementStopMotors;
        }

        public ICylinders[] CreateCylinders()
        {
            ICylinders[] elementCylinders = { }; return elementCylinders;
        }


        public IBarcodeReaders[] CreateBarcodeReaders()
        {
            IBarcodeReaders[] elementBarcodeReaders = { }; return elementBarcodeReaders;
        }

        public ISensorUVs[] CreateSensorUVs()
        {
            ISensorUVs[] elementSensorUVs = { }; return elementSensorUVs;
        }

        public ISensors[] CreateSensors()
        {
            ISensors[] elementSensors = { }; return elementSensors;
        }

        public ICameras[] CreateCameras()
        {
            ICameras[] elementCameras = { }; return elementCameras;
        }

        public IFlashLights[] CreateFlashLights()
        {
            IFlashLights[] elementFlashLights = { }; return elementFlashLights;
        }

        public ICarriers[] CreateCarriers()
        {
            ICarriers[] elementCarriers = { new IFCarrier(), new IFCarrier(), new IFCarrier(), new IFCarrier(), new IFCarrier(),
                                            new IFCarrier(), new IFCarrier(), new IFCarrier(), new IFCarrier(), new IFCarrier()}; 
            return elementCarriers;       
        }

        public IProducts[] CreateProducts()
        {
            IProducts[] elementProducts = { new IFProduct(), new IFProduct(), new IFProduct(), new IFProduct(), new IFProduct(), 
                                            new IFProduct(), new IFProduct(), new IFProduct(), new IFProduct(), new IFProduct()}; 
            return elementProducts;
        }
    }

    public interface IAssemblyIngredientFactory
    {
        IServoMotors[] CreateServoMotors();
        IStepMotors[] CreateStepMotors();
        ICylinders[] CreateCylinders();
        IBarcodeReaders[] CreateBarcodeReaders();
        ICameras[] CreateCameras();
        IFlashLights[] CreateFlashLights();
        ISensors[] CreateSensors();
        ICarriers[] CreateCarriers();
        IProducts[] CreateProducts();
    }


    public abstract class AEquipmentFactorys
    {
        public AEquipmentFactorys() { }
        public AEquipments OrderEquipment(string type)
        {
            AEquipments equipment;
            equipment = CreateEquipment(type);
            return equipment;
        }
        protected abstract AEquipments CreateEquipment(string type);
    }

    public class CEquipmentFactory_Reverse : AEquipmentFactorys
    {
        public CEquipmentFactory_Reverse() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyReverse();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Reverse\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Reverse\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class CEquipmentFactory_ReverseSub : AEquipmentFactorys
    {
        public CEquipmentFactory_ReverseSub() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyReverseSub();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : ReverseSub\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : ReverseSub\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class CEquipmentFactory_Reject : AEquipmentFactorys
    {
        public CEquipmentFactory_Reject() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyReject();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Reject\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Reject\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class CEquipmentFactory_Hardening : AEquipmentFactorys
    {
        public CEquipmentFactory_Hardening() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new AssemblyHardening();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Hardening\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Hardening\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class CEquipmentFactory_CarrierOutput : AEquipmentFactorys
    {
        public CEquipmentFactory_CarrierOutput() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyCarrierOutput();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierOutput\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierOutput\r\n";
                    break;
            }
            return equipment;
        }
    }

    public class CEquipmentFactory_CarrierInput : AEquipmentFactorys
    {
        public CEquipmentFactory_CarrierInput() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyCarrierInput();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierInput\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierInput\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class CEquipmentFactory_CarrierFeeding : AEquipmentFactorys
    {
        public CEquipmentFactory_CarrierFeeding() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyCarrierFeeding();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : CarrierFeeding\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : CarrierFeeding\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class CEquipmentFactory_Carrier : AEquipmentFactorys
    {
        public CEquipmentFactory_Carrier() { }

        protected override AEquipments CreateEquipment(string type)
        {
            AEquipments equipment = null;
            IAssemblyIngredientFactory ingredientFactory = new CAssemblyCarrier();

            switch (type)
            {
                case "Front":
                    equipment = new CEquipmentFronts(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Front : Carrier\r\n";
                    break;
                case "Rear":
                    equipment = new CEquipmentRears(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Rear : Carrier\r\n";
                    break;
            }
            return equipment;
        }
    }

    public abstract class AEquipments
    {
        protected IStepMotors[] motorSteps;
        protected IServoMotors[] motorServos;

        protected ICylinders[] cylinders;
        protected IBarcodeReaders[] barcodeReaders;
        protected ICameras[] cameras;
        protected IFlashLights[] flashLights;

        protected ISensors[] sensors;
        
        protected ICarriers[] carriers;
        protected IProducts[] products;

        public IProducts[] Products
        {
            get { return products; }
            set { products = value; }
        }

        public ICarriers[] Carriers
        {
            get { return carriers; }
            set { carriers = value; }
        }

        public IStepMotors[] StepMoters
        {
            get { return motorSteps; }
            set { motorSteps = value; }
        }

        public IServoMotors[] ServoMoters
        {
            get { return motorServos; }
            set { motorServos = value; }
        }

        public ICylinders[] Cylinders
        {
            get { return cylinders; }
            set { cylinders = value; }
        }

        public IBarcodeReaders[] BarcodeReaders
        {
            get { return barcodeReaders; }
            set { barcodeReaders = value; }
        }

        public ICameras[] Cameras
        {
            get { return cameras; }
            set { cameras = value; }
        }

        public IFlashLights[] FlashLights
        {
            get { return flashLights; }
            set { flashLights = value; }
        }

        public ISensors[] Sensors
        {
            get { return sensors; }
            set { sensors = value; }
        }

        public ISensors sensor;
        public ISensors Sensor
        {
            get
            {
                this.sensor = Sensors[0];
                return sensor;
            }
            set { sensor = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public AEquipments() { }
        public abstract string Prepare();
    }


    public class CEquipmentRears : AEquipments
    {
        IAssemblyIngredientFactory assemblyIngredientFactory;

        public CEquipmentRears(IAssemblyIngredientFactory assemblyIngredientFactory)
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
            carriers = assemblyIngredientFactory.CreateCarriers();
            products = assemblyIngredientFactory.CreateProducts();

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

            for (int i = 0; i < carriers.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Carriers : " + carriers[i].toString() + "\r\n");
                elementCount++;
            }

            for (int i = 0; i < products.Length; i++)
            {
                sb.Append(elementCount.ToString("00") + "  " + (i + 1).ToString("00") + "Products : " + products[i].toString() + "\r\n");
                elementCount++;
            }

            return sb.ToString();
        }
    }

    public class CEquipmentFronts : AEquipments
    {
        IAssemblyIngredientFactory assemblyIngredientFactory;

        public CEquipmentFronts(IAssemblyIngredientFactory assemblyIngredientFactory)
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
            carriers = assemblyIngredientFactory.CreateCarriers();
            products = assemblyIngredientFactory.CreateProducts();

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
    /// 구성 항목 : Sensors//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IProducts
    {
        bool VisionTested { get; set; }
        bool ReverseStatus { get; set; }
        bool PassFail { get; set; }
        int CurrentFlow { get; set; }
        int ArrayNumber { get; set; }
        int NowStep { get; set; }
        bool Status { get; set; }
        string toString();
    }

    /// <summary>
    /// ISensors 요소 : CInp_Sensor
    /// </summary>
    public class IFProduct : IProducts
    {
        bool m_Checked;
        public bool VisionTested
        {
            get { return this.m_Checked; }
            set { this.m_Checked = value; }
        }

        bool m_ReverseStatus;
        public bool ReverseStatus
        {
            get { return this.m_ReverseStatus; }
            set { this.m_ReverseStatus = value; }
        }

        bool m_PassFail;
        public bool PassFail
        {
            get { return this.m_PassFail; }
            set { this.m_PassFail = value; }
        }

        int m_CurrentFlow;
        public int CurrentFlow
        {
            get { return this.m_CurrentFlow; }
            set { this.m_CurrentFlow = value; }
        }

        int m_NowStep;
        public int NowStep
        {
            get { return this.m_NowStep; }
            set { this.m_NowStep = value; }
        }

        int m_ArrayNumber;
        public int ArrayNumber
        {
            get { return this.m_ArrayNumber; }
            set { this.m_ArrayNumber = value; }
        }

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


    /// <summary>
    /// 구성 항목 : Sensors//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ICarriers
    {
        bool[] UseFlagArray { get; set; }
        int NowMoveCount { get; set; }
        IProducts[] ProductsArray { get; set; }
        string BarcodeNumber { get; set; }
        double CurrentLocation { get; set; }
        int EnterNumber { get; set; }
        int InputCount { get; set; }
        bool Status { get; set; }
        string toString();
    }

    /// <summary>
    /// ISensors 요소 : CInp_Sensor
    /// </summary>
    public class IFCarrier : ICarriers
    {
        bool[] m_UseFlagArray;
        public bool[] UseFlagArray
        {
            get { return this.m_UseFlagArray; }
            set { this.m_UseFlagArray = value; }
        }

        int m_NowMoveCount;
        public int NowMoveCount
        {
            get { return this.m_NowMoveCount; }
            set { this.m_NowMoveCount = value; }
        }

        IProducts[] m_ProductsArray;
        public IProducts[] ProductsArray
        {
            get { return this.m_ProductsArray; }
            set { this.m_ProductsArray = value; }
        }

        string m_BarcodeNumber;
        public string BarcodeNumber
        {
            get { return this.m_BarcodeNumber; }
            set { this.m_BarcodeNumber = value; }
        }

        double m_CurrentLocation;
        public double CurrentLocation
        {
            get { return this.m_CurrentLocation; }
            set { this.m_CurrentLocation = value; }
        }

        int m_EnterNumber;
        public int EnterNumber
        {
            get { return this.m_EnterNumber; }
            set { this.m_EnterNumber = value; }
        }

        int m_InputCount;
        public int InputCount
        {
            get { return this.m_InputCount; }
            set { this.m_InputCount = value; }
        }

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


    /// <summary>
    /// 구성 항목 : Sensors//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ISensors
    {
        int InputMapNo { get; set;}
        bool Status { get; set; }
        string toString();
    }

    /// <summary>
    /// ISensors 요소 : CInp_Sensor
    /// </summary>
    public class CInp_Sensor : ISensors
    {
        int m_InputMapNo;
        public int InputMapNo
        {
            get { return this.m_InputMapNo; }
            set { this.m_InputMapNo = value; }
        }

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

    /// <summary>
    /// ISensors 요소 : CInp_StepMotor_Home
    /// 이 센서는 페스텍의 디바이스를 통해서 입력되는 상태 신호이다.
    /// </summary>
    public class CInp_StepMotor_Home : ISensors
    {      
        int m_InputMapNo;
        public int InputMapNo
        {
            get { return this.m_InputMapNo; }
            set { this.m_InputMapNo = value; }
        }


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
    /// 구성 항목 : ISensorsFastech//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ISensorsFastech
    {
        bool Status { get; set; }
        string toString();
    }

    /// <summary>
    /// ISensorsFastech 요소 : CInp_Fastech_Home
    /// </summary>
    public class CInp_Fastech_Home : ISensorsFastech
    {
        bool statusData = false;

        public bool Status
        {
            get { return statusData; }
            set { statusData = value; }
        }

        public string toString()
        {
            return "Inp_Fastech_Home";
        }
    }

    /// <summary>
    /// 구성 항목 : BarcodeReaders//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IBarcodeReaders
    {
        uint BarcodeComPortNo { get; set; }
        uint BarcodeBaudRate { get; set; }
        uint BarcodeDataBits { get; set; }
        char BarcodeParity { get; set; }
        uint BarcodeStopBits { get; set; }
        uint BarcodeReadRepeatCnt { get; set; }

        bool Status { get; set; }
        string toString();
        string ReadedData { get; }
    }

    /// <summary>
    /// BarcodeReaders 요소 : CInp_BarcodeReading
    /// </summary>
    public class CInp_BarcodeReading : IBarcodeReaders
    {
        // Bar Code
        uint m_uBCComPortNo;
        uint m_uBCBaudRate;
        uint m_uBCDataBits;
        char m_cBCParity;
        uint m_uBCStopBits;
        uint m_uBCReadRepeatCnt;

        public uint BarcodeComPortNo
        {
            get { return m_uBCComPortNo; }
            set { m_uBCComPortNo = value; }
        }

        public uint BarcodeBaudRate
        {
            get { return m_uBCBaudRate; }
            set { m_uBCBaudRate = value; }
        }

        public uint BarcodeDataBits
        {
            get { return m_uBCDataBits; }
            set { m_uBCDataBits = value; }
        }

        public char BarcodeParity
        {
            get { return m_cBCParity; }
            set { m_cBCParity = value; }
        }

        public uint BarcodeStopBits
        {
            get { return this.m_uBCStopBits; }
            set { this.m_uBCStopBits = value; }
        }

        public uint BarcodeReadRepeatCnt
        {
            get { return this.m_uBCReadRepeatCnt; }
            set { this.m_uBCReadRepeatCnt = value; }
        }

        bool bStatusData;
        string strReadedData;
        public bool Status
        {
            get { return this.bStatusData; }
            set { this.bStatusData = value; }
        }

        public string ReadedData
        {
            get { return this.strReadedData; }
        }

        public string toString()
        {
            return "BarcodeReader";
        }
    }

    /// <summary>
    /// 구성 항목 : UV Sensor//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ISensorUVs
    {
        bool Status { get; set; }
        string toString();
    }

    /// <summary>
    /// UV Sensor 요소 : CInp_UV_OnOff_Sensor
    /// </summary>
    public class CInp_UV_OnOff_Sensor : ISensorUVs
    {
        bool statusData;

        public bool Status
        {
            get { return this.statusData; }
            set { this.statusData = value; }
        }

        public string toString()
        {
            return "UV_OnOff_Sensor";
        }
    }


    /// <summary>
    /// 구성 항목 : 스텝 모터//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IStepMotors
    {
        byte SLVAE_NO { get; set; }
        double MOVE_VELOCITY { get; set; }
        double MOVE_ACCEL { get; set; }
        double JOG_VELOCITY { get; set; }
        double JOG_ACCEL { get; set; }
        double HOME_VELOCITY { get; set; }
        double HOME_ACCEL { get; set; }      

        uint[] PULSE_PER_REVOLUTION { get; set; }
        double[] AXIS_MAX_SPEED { get; set; }   
        double[] AXIS_START_SPEED { get; set; }    
        double[] AXIS_ACC_TIME { get; set; }          
        double[] AXIS_DEC_TIME { get; set; }               
        double[] SPEED_OVERRIDE { get; set; }       
        double[] JOG_SPEED { get; set; }      
        double[] JOG_START_SPEED { get; set; }        
        double[] JOG_ACC_DEC_TIME { get; set; }        
        uint[] ALARM_LOGIC { get; set; }        
        uint[] RUNSTOP_LOGIC { get; set; }          
        uint[] ALARM_RESET_LOGIC { get; set; }           
        double[] SW_LIMIT_PLUS_VALUE { get; set; }       
        double[] SW_LIMIT_MINUS_VALUE { get; set; }       
        uint[] SW_LIMIT_STOP_METHOD { get; set; }   
        uint[] HW_LIMIT_STOP_METHOD { get; set; }  
        uint[] LIMIT_SENSOR_LOGIC { get; set; }
        int[] ORIGIN_SPEED { get; set; }
        int[] ORIGIN_SEARCH_SPEED { get; set; } 
        double[] ORIGIN_ACC_DEC_TIME { get; set; }  
        uint[] ORIGIN_METHOD { get; set; }   
        uint[] ORIGIN_DIRECT { get; set; }  
        double[] ORIGIN_OFFSET { get; set; } 
        double[] ORIGIN_POSITION_SET { get; set; }     
        uint[] ORIGIN_SENSOR_LOGIC { get; set; }      
        uint[] STOP_CURRENT { get; set; }
        uint[] MOTION_DIRECT { get; set; }
        uint[] LIMIT_SENSOR_DIRECT { get; set; }          
        uint[] ENCODER_MULTIPLY_VALUE { get; set; }       
        uint[] ENCODER_DIRECT { get; set; }      
        System.Collections.ArrayList STEP_STATUS_DATA { get; set; }
        
        byte PortNo { get; set; }
        int Baudrate { get; set; }
        float Status { get; set; }
        string toString();
        string MoveInc(float movePoint);
        string MoveAbs(float movePoint);
    }

    /// <summary>
    /// 스텝모터 요소 : COtp_StepMotor
    /// </summary>
    public class COtp_StepMotor : IStepMotors
    {
        byte m_Slave_No;
        public byte SLVAE_NO
        {
            get { return this.m_Slave_No; }
            set { this.m_Slave_No = value; }
        }

        double m_Move_Velocity;
        public double MOVE_VELOCITY
        {
            get { return this.m_Move_Velocity; }
            set { this.m_Move_Velocity = value; }
        }
        double m_Move_Accel;
        public double MOVE_ACCEL
        {
            get { return this.m_Move_Accel; }
            set { this.m_Move_Accel = value; }
        }
        double m_Jog_Velocity;
        public double JOG_VELOCITY
        {
            get { return this.m_Jog_Velocity; }
            set { this.m_Jog_Velocity = value; }
        }
        double m_Jog_Accel;
        public double JOG_ACCEL
        {
            get { return this.m_Jog_Accel; }
            set { this.m_Jog_Accel = value; }
        }
        double m_Home_Velocity;
        public double HOME_VELOCITY
        {
            get { return this.m_Home_Velocity; }
            set { this.m_Home_Velocity = value; }
        }
        double m_Home_Accel;
        public double HOME_ACCEL
        {
            get { return this.m_Home_Accel; }
            set { this.m_Home_Accel = value; }
        }

        uint[] m_PULSE_PER_REVOLUTION = { 0, 15, 10 };
        public uint[] PULSE_PER_REVOLUTION
        {
            get { return this.m_PULSE_PER_REVOLUTION; }
            set { this.m_PULSE_PER_REVOLUTION = value; }
        }
        double[] m_AXIS_MAX_SPEED = { 1, 500000, 500000 };
        public double[] AXIS_MAX_SPEED
        {
            get { return this.m_AXIS_MAX_SPEED; }
            set { this.m_AXIS_MAX_SPEED = value; }
        }
        double[] m_AXIS_START_SPEED = { 1, 35000, 1 };
        public double[] AXIS_START_SPEED
        {
            get { return this.m_AXIS_START_SPEED; }
            set { this.m_AXIS_START_SPEED = value; }
        }
        double[] m_AXIS_ACC_TIME = { 1, 9999, 100 };
        public double[] AXIS_ACC_TIME
        {
            get { return this.m_AXIS_ACC_TIME; }
            set { this.m_AXIS_ACC_TIME = value; }
        }
        double[] m_AXIS_DEC_TIME = { 1, 9999, 100 };
        public double[] AXIS_DEC_TIME
        {
            get { return this.m_AXIS_DEC_TIME; }
            set { this.m_AXIS_DEC_TIME = value; }
        }
        double[] m_SPEED_OVERRIDE = { 1, 500, 100 };
        public double[] SPEED_OVERRIDE
        {
            get { return this.m_SPEED_OVERRIDE; }
            set { this.m_SPEED_OVERRIDE = value; }
        }
        double[] m_JOG_SPEED = { 1, 500000, 5000 };
        public double[] JOG_SPEED
        {
            get { return this.m_JOG_SPEED; }
            set { this.m_JOG_SPEED = value; }
        }
        double[] m_JOG_START_SPEED = { 1, 35000, 1 };
        public double[] JOG_START_SPEED
        {
            get { return this.m_JOG_START_SPEED; }
            set { this.m_JOG_START_SPEED = value; }
        }
        double[] m_JOG_ACC_DEC_TIME = { 1, 9999, 100 };
        public double[] JOG_ACC_DEC_TIME
        {
            get { return this.m_JOG_ACC_DEC_TIME; }
            set { this.m_JOG_ACC_DEC_TIME = value; }
        }
        uint[] m_ALARM_LOGIC = { 0, 1, 0 };
        public uint[] ALARM_LOGIC
        {
            get { return this.m_ALARM_LOGIC; }
            set { this.m_ALARM_LOGIC = value; }
        }
        uint[] m_RUNSTOP_LOGIC = { 0, 1, 0 };
        public uint[] RUNSTOP_LOGIC
        {
            get { return this.m_RUNSTOP_LOGIC; }
            set { this.m_RUNSTOP_LOGIC = value; }
        }
        uint[] m_ALARM_RESET_LOGIC = { 0, 1, 0 };
        public uint[] ALARM_RESET_LOGIC
        {
            get { return this.m_ALARM_RESET_LOGIC; }
            set { this.m_ALARM_RESET_LOGIC = value; }
        }
        double[] m_SW_LIMIT_PLUS_VALUE = { -134217727, 134217727, 134217727 };
        public double[] SW_LIMIT_PLUS_VALUE
        {
            get { return this.m_SW_LIMIT_PLUS_VALUE; }
            set { this.m_SW_LIMIT_PLUS_VALUE = value; }
        }
        double[] m_SW_LIMIT_MINUS_VALUE = { -134217727, 134217727, -134217727 };
        public double[] SW_LIMIT_MINUS_VALUE
        {
            get { return this.m_SW_LIMIT_MINUS_VALUE; }
            set { this.m_SW_LIMIT_MINUS_VALUE = value; }
        }
        uint[] m_SW_LIMIT_STOP_METHOD = { 0, 1, 0 };
        public uint[] SW_LIMIT_STOP_METHOD
        {
            get { return this.m_SW_LIMIT_STOP_METHOD; }
            set { this.m_SW_LIMIT_STOP_METHOD = value; }
        }
        uint[] m_HW_LIMIT_STOP_METHOD = { 0, 1, 0 };
        public uint[] HW_LIMIT_STOP_METHOD
        {
            get { return this.m_HW_LIMIT_STOP_METHOD; }
            set { this.m_HW_LIMIT_STOP_METHOD = value; }
        }
        uint[] m_LIMIT_SENSOR_LOGIC = { 0, 1, 0 };
        public uint[] LIMIT_SENSOR_LOGIC
        {
            get { return this.m_LIMIT_SENSOR_LOGIC; }
            set { this.m_LIMIT_SENSOR_LOGIC = value; }
        }
        int[] m_ORIGIN_SPEED = { 1, 500000, 5000 };
        public int[] ORIGIN_SPEED
        {
            get { return this.m_ORIGIN_SPEED; }
            set { this.m_ORIGIN_SPEED = value; }
        }
        int[] m_ORIGIN_SEARCH_SPEED = { 1, 50000, 1000 };
        public int[] ORIGIN_SEARCH_SPEED
        {
            get { return this.m_ORIGIN_SEARCH_SPEED; }
            set { this.m_ORIGIN_SEARCH_SPEED = value; }
        }
        double[] m_ORIGIN_ACC_DEC_TIME = { 1, 9999, 50 };
        public double[] ORIGIN_ACC_DEC_TIME
        {
            get { return this.m_ORIGIN_ACC_DEC_TIME; }
            set { this.m_ORIGIN_ACC_DEC_TIME = value; }
        }
        uint[] m_ORIGIN_METHOD = { 0, 2, 0 };
        public uint[] ORIGIN_METHOD
        {
            get { return this.m_ORIGIN_METHOD; }
            set { this.m_ORIGIN_METHOD = value; }
        }
        uint[] m_ORIGIN_DIRECT = { 0, 1, 0 };
        public uint[] ORIGIN_DIRECT
        {
            get { return this.m_ORIGIN_DIRECT; }
            set { this.m_ORIGIN_DIRECT = value; }
        }
        double[] m_ORIGIN_OFFSET = { -134217727, 134217727, 0 };
        public double[] ORIGIN_OFFSET
        {
            get { return this.m_ORIGIN_OFFSET; }
            set { this.m_ORIGIN_OFFSET = value; }
        }
        double[] m_ORIGIN_POSITION_SET = { -134217727, 134217727, 0 };
        public double[] ORIGIN_POSITION_SET
        {
            get { return this.m_ORIGIN_POSITION_SET; }
            set { this.m_ORIGIN_POSITION_SET = value; }
        }
        uint[] m_ORIGIN_SENSOR_LOGIC = { 0, 1, 0 };
        public uint[] ORIGIN_SENSOR_LOGIC
        {
            get { return this.m_ORIGIN_SENSOR_LOGIC; }
            set { this.m_ORIGIN_SENSOR_LOGIC = value; }
        }
        uint[] m_STOP_CURRENT = { 0, 100, 50 };
        public uint[] STOP_CURRENT
        {
            get { return this.m_STOP_CURRENT; }
            set { this.m_STOP_CURRENT = value; }
        }
        uint[] m_MOTION_DIRECT = { 0, 1, 0 };
        public uint[] MOTION_DIRECT
        {
            get { return this.m_MOTION_DIRECT; }
            set { this.m_MOTION_DIRECT = value; }
        }
        uint[] m_LIMIT_SENSOR_DIRECT = { 0, 1, 0 };
        public uint[] LIMIT_SENSOR_DIRECT
        {
            get { return this.m_LIMIT_SENSOR_DIRECT; }
            set { this.m_LIMIT_SENSOR_DIRECT = value; }
        }
        uint[] m_ENCODER_MULTIPLY_VALUE = { 0, 3, 0 };
        public uint[] ENCODER_MULTIPLY_VALUE
        {
            get { return this.m_ENCODER_MULTIPLY_VALUE; }
            set { this.m_ENCODER_MULTIPLY_VALUE = value; }
        }
        uint[] m_ENCODER_DIRECT = { 0, 1, 0 };
        public uint[] ENCODER_DIRECT
        {
            get { return this.m_ENCODER_DIRECT; }
            set { this.m_ENCODER_DIRECT = value; }
        }

        //StepStatusData
        System.Collections.ArrayList STEP_STATUS_VALUE;
        public System.Collections.ArrayList STEP_STATUS_DATA
        {
            get { return this.STEP_STATUS_VALUE; }
            set { this.STEP_STATUS_VALUE = value; }
        }

        byte m_iPortNo;
        public byte PortNo
        {
            get { return this.m_iPortNo; }
            set { this.m_iPortNo = value; }
        }

        int m_iBaudrate;
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


    /*
    /// <summary>
    /// 스텝모터 요소 : COtp_CFK513AP2
    /// </summary>
    public class COtp_CFK513AP2 : IStepMotors
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
    /// 스텝모터 요소 : COtp_RK545AMA
    /// </summary>
    public class COtp_RK545AMA : IStepMotors
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
    */

    /// <summary>
    /// 구성 항목 : 서보 모터//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IServoMotors
    {
        int AXIS_CONF_NO { get; set; }                   //0
        uint AXIS_CONF_PULSE_OUT_METHOD { get; set; }    //4
        uint AXIS_CONF_ENC_INPUT_METHOD { get; set; }    //3
        uint AXIS_CONF_INPOSITION { get; set; }          //2
        uint AXIS_CONF_ALARM { get; set; }               //1
        uint AXIS_CONF_NEG_END_LIMIT { get; set; }       //1
        uint AXIS_CONF_POS_END_LIMIT { get; set; }       //1
        double AXIS_CONF_MIN_VELOCITY { get; set; }        //1.000000
        double AXIS_CONF_MAX_VELOCITY { get; set; }        //700000.000000
        uint AXIS_CONF_HOME_SIGNAL { get; set; }         //4
        uint AXIS_CONF_HOME_LEVEL { get; set; }          //1
        int AXIS_CONF_HOME_DIR { get; set; }            //0
        uint AXIS_CONF_ZPHASE_LEVEL { get; set; }        //0
        uint AXIS_CONF_ZPHASE_USE { get; set; }        //0
        uint AXIS_CONF_STOP_SIGNAL_MODE { get; set; }    //0
        uint AXIS_CONF_STOP_SIGNAL_LEVEL { get; set; }   //1

        double AXIS_CONF_HOME_FIRST_VELOCITY { get; set; } //100.000000
        double AXIS_CONF_HOME_SECOND_VELOCITY { get; set; }//100.000000
        double AXIS_CONF_HOME_THIRD_VELOCITY { get; set; } //20.000000
        double AXIS_CONF_HOME_LAST_VELOCITY { get; set; }  //1.000000
        double AXIS_CONF_HOME_FIRST_ACCEL { get; set; }    //400.000000
        double AXIS_CONF_HOME_SECOND_ACCEL { get; set; }   //400.000000
        double AXIS_CONF_HOME_END_CLEAR_TIME { get; set; } //1000.000000
        double AXIS_CONF_HOME_END_OFFSET { get; set; }     //0.000000
        double AXIS_CONF_NEG_SOFT_LIMIT { get; set; }      //-134217728.000000
        double AXIS_CONF_POS_SOFT_LIMIT { get; set; }      //134217728.000000
        double AXIS_CONF_MOVE_PULSE { get; set; }          //1.000000
        double AXIS_CONF_MOVE_UNIT { get; set; }           //1.000000
        double AXIS_CONF_INIT_POSITION { get; set; }       //1000.000000
        double AXIS_CONF_INIT_VELOCITY { get; set; }       //200.000000
        double AXIS_CONF_INIT_ACCEL { get; set; }          //400.000000
        double AXIS_CONF_INIT_DECEL { get; set; }          //400.000000
        uint AXIS_CONF_INIT_ABSRELMODE { get; set; }     //0
        uint AXIS_CONF_INIT_PROFILEMODE { get; set; }    //0
        uint AXIS_CONF_SVON_LEVEL { get; set; }          //1
        uint AXIS_CONF_ALARM_RESET_LEVEL { get; set; }   //1
        uint AXIS_CONF_ENCODER_TYPE { get; set; }        //1
        uint AXIS_CONF_SOFT_LIMIT_SEL { get; set; }      //0
        uint AXIS_CONF_SOFT_LIMIT_STOP_MODE { get; set; }//0
        uint AXIS_CONF_SOFT_LIMIT_ENABLE { get; set; }   //0

        bool STATUS_SERVO_ON { get; set; }   
        bool STATUS_SERVO_ALRAM { get; set; }    
        bool STATUS_MOTION_IN { get; set; }      
        bool STATUS_HOME_ON { get; set; }        
        bool STATUS_LIMIT_POS { get; set; }      
        bool STATUS_LIMIT_NEG { get; set; }      
        double STATUS_CURRENT_LOCATION { get; set; }        //100.000000
        double STATUS_CURRENT_VELOCITY { get; set; }       //100.000000

        double AXIS_MEAS_HOME_VELOCITY { get; set; }    //100.000000
        double AXIS_MEAS_HOME_ACCEL { get; set; }       //100.000000
        double AXIS_MEAS_JOG_VELOCITY { get; set; }         //100.000000
        double AXIS_MEAS_JOG_ACCEL { get; set; }        //100.000000
        double AXIS_MEAS_MOVE_VELOCITY { get; set; }        //100.000000
        double AXIS_MEAS_MOVE_ACCEL { get; set; }       //100.000000

        double AXIS_MEAS_POS_LIMIT_LOCATION { get; set; }    //100.000000
        double AXIS_MEAS_NEG_LIMIT_LOCATION { get; set; }       //100.000000
        double AXIS_MEAS_CURRENT_LOCATION { get; set; }         //100.000000
        double AXIS_MEAS_CURRENT_VELOCITY { get; set; }        //100.000000
        double AXIS_MEAS_HOME_LOCATION { get; set; }        //100.000000
        //double AXIS_MEAS_Move_Accel { get; set; }       //100.000000
        System.Collections.ArrayList AXIS_STATUS_DATA { get; set; }

        string toString();
        string AxisMoveInc(double movePoint);
        string AxisMoveAbs(double movePoint);
        string AxisSearchHome(bool bSearching);
    }

    /// <summary>
    /// 서보모터 요소 : COtp_ServoMotor
    /// </summary>
    public class COtp_ServoMotor : IServoMotors
    {
        int AXIS_NO;//0                                         
        public int AXIS_CONF_NO
        { 
            get { return this.AXIS_NO; }
            set { this.AXIS_NO = value; } 
        }

        uint AXIS_PULSE_OUT_METHOD;//4                                         
        public uint AXIS_CONF_PULSE_OUT_METHOD
        {
            get { return this.AXIS_PULSE_OUT_METHOD; }
            set { this.AXIS_PULSE_OUT_METHOD = value; }
        }

        uint AXIS_ENC_INPUT_METHOD;//3                                       
        public uint AXIS_CONF_ENC_INPUT_METHOD
        {
            get { return this.AXIS_ENC_INPUT_METHOD; }
            set { this.AXIS_ENC_INPUT_METHOD = value; }
        }     
        
        public uint AXIS_INPOSITION;
        public uint AXIS_CONF_INPOSITION 
        {
            get { return this.AXIS_INPOSITION; }
            set { this.AXIS_INPOSITION = value; }
        }    
        
        uint AXIS_ALARM;
        public uint AXIS_CONF_ALARM
        {
            get { return this.AXIS_ALARM; }
            set { this.AXIS_ALARM = value; }
        }

        uint AXIS_NEG_END_LIMIT;
        public uint AXIS_CONF_NEG_END_LIMIT
        {
            get { return this.AXIS_NEG_END_LIMIT; }
            set { this.AXIS_NEG_END_LIMIT = value; }
        }

        uint AXIS_POS_END_LIMIT;
        public uint AXIS_CONF_POS_END_LIMIT
        {
            get { return this.AXIS_POS_END_LIMIT; }
            set { this.AXIS_POS_END_LIMIT = value; }
        }

        double AXIS_MIN_VELOCITY;
        public double AXIS_CONF_MIN_VELOCITY
        {
            get { return this.AXIS_MIN_VELOCITY; }
            set { this.AXIS_MIN_VELOCITY = value; }
        }

        double AXIS_MAX_VELOCITY;
        public double AXIS_CONF_MAX_VELOCITY
        {
            get { return this.AXIS_MAX_VELOCITY; }
            set { this.AXIS_MAX_VELOCITY = value; }
        }

        uint AXIS_HOME_SIGNAL;
        public uint AXIS_CONF_HOME_SIGNAL
        {
            get { return this.AXIS_HOME_SIGNAL; }
            set { this.AXIS_HOME_SIGNAL = value; }
        }

        uint AXIS_HOME_LEVEL;
        public uint AXIS_CONF_HOME_LEVEL
        {
            get { return this.AXIS_HOME_LEVEL; }
            set { this.AXIS_HOME_LEVEL = value; }
        }

        int AXIS_HOME_DIR;
        public int AXIS_CONF_HOME_DIR
        {
            get { return this.AXIS_HOME_DIR; }
            set { this.AXIS_HOME_DIR = value; }
        } 

        uint AXIS_ZPHASE_LEVEL;
        public uint AXIS_CONF_ZPHASE_LEVEL
        {
            get { return this.AXIS_ZPHASE_LEVEL; }
            set { this.AXIS_ZPHASE_LEVEL = value; }
        }

        uint AXIS_ZPHASE_USE;
        public uint AXIS_CONF_ZPHASE_USE
        {
            get { return this.AXIS_ZPHASE_USE; }
            set { this.AXIS_ZPHASE_USE = value; }
        }

        uint AXIS_STOP_SIGNAL_MODE;
        public uint AXIS_CONF_STOP_SIGNAL_MODE
        {
            get { return this.AXIS_STOP_SIGNAL_MODE; }
            set { this.AXIS_STOP_SIGNAL_MODE = value; }
        } 

        uint AXIS_STOP_SIGNAL_LEVEL;
        public uint AXIS_CONF_STOP_SIGNAL_LEVEL
        {
            get { return this.AXIS_STOP_SIGNAL_LEVEL; }
            set { this.AXIS_STOP_SIGNAL_LEVEL = value; }
        } 

        double AXIS_HOME_FIRST_VELOCITY;
        public double AXIS_CONF_HOME_FIRST_VELOCITY
        {
            get { return this.AXIS_HOME_FIRST_VELOCITY; }
            set { this.AXIS_HOME_FIRST_VELOCITY = value; }
        } 

        double AXIS_HOME_SECOND_VELOCITY;
        public double AXIS_CONF_HOME_SECOND_VELOCITY
        {
            get { return this.AXIS_HOME_SECOND_VELOCITY; }
            set { this.AXIS_HOME_SECOND_VELOCITY = value; }
        } 

        double AXIS_HOME_THIRD_VELOCITY;
        public double AXIS_CONF_HOME_THIRD_VELOCITY
        {
            get { return this.AXIS_HOME_THIRD_VELOCITY; }
            set { this.AXIS_HOME_THIRD_VELOCITY = value; }
        } 

        double AXIS_HOME_LAST_VELOCITY;
        public double AXIS_CONF_HOME_LAST_VELOCITY
        {
            get { return this.AXIS_HOME_LAST_VELOCITY; }
            set { this.AXIS_HOME_LAST_VELOCITY = value; }
        } 

        double AXIS_HOME_FIRST_ACCEL;
        public double AXIS_CONF_HOME_FIRST_ACCEL
        {
            get { return this.AXIS_HOME_FIRST_ACCEL; }
            set { this.AXIS_HOME_FIRST_ACCEL = value; }
        } 

        double AXIS_HOME_SECOND_ACCEL;
        public double AXIS_CONF_HOME_SECOND_ACCEL
        {
            get { return this.AXIS_HOME_SECOND_ACCEL; }
            set { this.AXIS_HOME_SECOND_ACCEL = value; }
        } 

        double AXIS_HOME_END_CLEAR_TIME;
        public double AXIS_CONF_HOME_END_CLEAR_TIME
        {
            get { return this.AXIS_HOME_END_CLEAR_TIME; }
            set { this.AXIS_HOME_END_CLEAR_TIME = value; }
        } 

        double AXIS_HOME_END_OFFSET;
        public double AXIS_CONF_HOME_END_OFFSET
        {
            get { return this.AXIS_HOME_END_OFFSET; }
            set { this.AXIS_HOME_END_OFFSET = value; }
        } 

        double AXIS_NEG_SOFT_LIMIT;
        public double AXIS_CONF_NEG_SOFT_LIMIT
        {
            get { return this.AXIS_NEG_SOFT_LIMIT; }
            set { this.AXIS_NEG_SOFT_LIMIT = value; }
        } 

        double AXIS_POS_SOFT_LIMIT;
        public double AXIS_CONF_POS_SOFT_LIMIT
        {
            get { return this.AXIS_POS_SOFT_LIMIT; }
            set { this.AXIS_POS_SOFT_LIMIT = value; }
        }

        double AXIS_MOVE_PULSE;
        public double AXIS_CONF_MOVE_PULSE
        {
            get { return this.AXIS_MOVE_PULSE; }
            set { this.AXIS_MOVE_PULSE = value; }
        }

        double AXIS_MOVE_UNIT;
        public double AXIS_CONF_MOVE_UNIT
        {
            get { return this.AXIS_MOVE_UNIT; }
            set { this.AXIS_MOVE_UNIT = value; }
        }

        double AXIS_INIT_POSITION;
        public double AXIS_CONF_INIT_POSITION
        {
            get { return this.AXIS_INIT_POSITION; }
            set { this.AXIS_INIT_POSITION = value; }
        }

        double AXIS_INIT_VELOCITY;
        public double AXIS_CONF_INIT_VELOCITY
        {
            get { return this.AXIS_INIT_VELOCITY; }
            set { this.AXIS_INIT_VELOCITY = value; }
        }

        double AXIS_INIT_ACCEL;
        public double AXIS_CONF_INIT_ACCEL
        {
            get { return this.AXIS_INIT_ACCEL; }
            set { this.AXIS_INIT_ACCEL = value; }
        }

        double AXIS_INIT_DECEL;
        public double AXIS_CONF_INIT_DECEL
        {
            get { return this.AXIS_INIT_DECEL; }
            set { this.AXIS_INIT_DECEL = value; }
        }

        uint AXIS_INIT_ABSRELMODE;
        public uint AXIS_CONF_INIT_ABSRELMODE
        {
            get { return this.AXIS_INIT_ABSRELMODE; }
            set { this.AXIS_INIT_ABSRELMODE = value; }
        }

        uint AXIS_INIT_PROFILEMODE;
        public uint AXIS_CONF_INIT_PROFILEMODE
        {
            get { return this.AXIS_INIT_PROFILEMODE; }
            set { this.AXIS_INIT_PROFILEMODE = value; }
        }

        uint AXIS_SVON_LEVEL;
        public uint AXIS_CONF_SVON_LEVEL
        {
            get { return this.AXIS_SVON_LEVEL; }
            set { this.AXIS_SVON_LEVEL = value; }
        }

        uint AXIS_ALARM_RESET_LEVEL;
        public uint AXIS_CONF_ALARM_RESET_LEVEL
        {
            get { return this.AXIS_ALARM_RESET_LEVEL; }
            set { this.AXIS_ALARM_RESET_LEVEL = value; }
        }

        uint AXIS_ENCODER_TYPE;
        public uint AXIS_CONF_ENCODER_TYPE
        {
            get { return this.AXIS_ENCODER_TYPE; }
            set { this.AXIS_ENCODER_TYPE = value; }
        }

        uint AXIS_SOFT_LIMIT_SEL;
        public uint AXIS_CONF_SOFT_LIMIT_SEL
        {
            get { return this.AXIS_SOFT_LIMIT_SEL; }
            set { this.AXIS_SOFT_LIMIT_SEL = value; }
        }

        uint AXIS_SOFT_LIMIT_STOP_MODE;
        public uint AXIS_CONF_SOFT_LIMIT_STOP_MODE
        {
            get { return this.AXIS_SOFT_LIMIT_STOP_MODE; }
            set { this.AXIS_SOFT_LIMIT_STOP_MODE = value; }
        }

        uint AXIS_SOFT_LIMIT_ENABLE;
        public uint AXIS_CONF_SOFT_LIMIT_ENABLE
        {
            get { return this.AXIS_SOFT_LIMIT_ENABLE; }
            set { this.AXIS_SOFT_LIMIT_ENABLE = value; }
        }

        //double AXIS_MEAS_HOME_VELOCITY { get; set; }    //100.000000

        double AXIS_HOME_VELOCITY;
        public double AXIS_MEAS_HOME_VELOCITY
        {
            get { return this.AXIS_HOME_VELOCITY; }
            set { this.AXIS_HOME_VELOCITY = value; }
        }

        //double AXIS_MEAS_HOME_ACCEL { get; set; }       //100.000000
        double AXIS_HOME_ACCEL;
        public double AXIS_MEAS_HOME_ACCEL
        {
            get { return this.AXIS_HOME_ACCEL; }
            set { this.AXIS_HOME_ACCEL = value; }
        }

        //double AXIS_MEAS_JOG_VELOCITY { get; set; }         //100.000000
        double AXIS_JOG_VELOCITY;
        public double AXIS_MEAS_JOG_VELOCITY
        {
            get { return this.AXIS_JOG_VELOCITY; }
            set { this.AXIS_JOG_VELOCITY = value; }
        }

        //double AXIS_MEAS_JOG_ACCEL { get; set; }        //100.000000
        double AXIS_JOG_ACCEL;
        public double AXIS_MEAS_JOG_ACCEL
        {
            get { return this.AXIS_JOG_ACCEL; }
            set { this.AXIS_JOG_ACCEL = value; }
        }

        //double AXIS_MEAS_MOVE_VELOCITY { get; set; }        //100.000000
        double AXIS_MOVE_VELOCITY;
        public double AXIS_MEAS_MOVE_VELOCITY
        {
            get { return this.AXIS_MOVE_VELOCITY; }
            set { this.AXIS_MOVE_VELOCITY = value; }
        }

        //double AXIS_MEAS_MOVE_ACCEL { get; set; }       //100.000000
        double AXIS_MOVE_ACCEL;
        public double AXIS_MEAS_MOVE_ACCEL
        {
            get { return this.AXIS_MOVE_ACCEL; }
            set { this.AXIS_MOVE_ACCEL = value; }
        }


        //double AXIS_MEAS_POS_LIMIT_LOCATION { get; set; }    //100.000000
        double AXIS_POS_LIMIT_LOCATION;
        public double AXIS_MEAS_POS_LIMIT_LOCATION
        {
            get { return this.AXIS_POS_LIMIT_LOCATION; }
            set { this.AXIS_POS_LIMIT_LOCATION = value; }
        }

        //double AXIS_MEAS_NEG_LIMIT_LOCATION { get; set; }       //100.000000
        double AXIS_NEG_LIMIT_LOCATION;
        public double AXIS_MEAS_NEG_LIMIT_LOCATION
        {
            get { return this.AXIS_NEG_LIMIT_LOCATION; }
            set { this.AXIS_NEG_LIMIT_LOCATION = value; }
        }

        //double AXIS_MEAS_CURRENT_LOCATION { get; set; }         //100.000000
        double AXIS_CURRENT_LOCATION;
        public double AXIS_MEAS_CURRENT_LOCATION
        {
            get { return this.AXIS_CURRENT_LOCATION; }
            set { this.AXIS_CURRENT_LOCATION = value; }
        }

         //double AXIS_MEAS_CURRENT_SPEED { get; set; }        //100.000000
         double AXIS_CURRENT_VELOCITY;
         public double AXIS_MEAS_CURRENT_VELOCITY
         {
             get { return this.AXIS_CURRENT_VELOCITY; }
             set { this.AXIS_CURRENT_VELOCITY = value; }
         }

        double AXIS_HOME_LOCATION;
        public double AXIS_MEAS_HOME_LOCATION
        {
            get { return this.AXIS_HOME_LOCATION; }
            set { this.AXIS_HOME_LOCATION = value; }
        }

        //AxisStatusData
        System.Collections.ArrayList AXIS_STATUS_VALUE;
        public System.Collections.ArrayList AXIS_STATUS_DATA
        {
            get { return this.AXIS_STATUS_VALUE; }
            set { this.AXIS_STATUS_VALUE = value; }
        }

        bool CURRENT_SERVO_ON;
        public bool STATUS_SERVO_ON
        {
            get { return this.CURRENT_SERVO_ON; }
            set { this.CURRENT_SERVO_ON = value; }
        }
        
        bool CURRENT_SERVO_ALRAM;
        public bool STATUS_SERVO_ALRAM
        {
            get { return this.CURRENT_SERVO_ALRAM; }
            set { this.CURRENT_SERVO_ALRAM = value; }
        }

        bool CURRENT_MOTION_IN;// { get; set; }
        public bool STATUS_MOTION_IN
        {
            get { return this.CURRENT_MOTION_IN; }
            set { this.CURRENT_MOTION_IN = value; }
        }

        bool CURRENT_HOME_ON;// { get; set; }
        public bool STATUS_HOME_ON
        {
            get { return this.CURRENT_HOME_ON; }
            set { this.CURRENT_HOME_ON = value; }
        }

        bool CURRENT_LIMIT_POS;// { get; set; }
        public bool STATUS_LIMIT_POS
        {
            get { return this.CURRENT_LIMIT_POS; }
            set { this.CURRENT_LIMIT_POS = value; }
        }

        bool CURRENT_LIMIT_NEG;// { get; set; }
        public bool STATUS_LIMIT_NEG
        {
            get { return this.CURRENT_LIMIT_NEG; }
            set { this.CURRENT_LIMIT_NEG = value; }
        }

        double CURRENT_LOCATION;// { get; set; }        //100.000000
        public double STATUS_CURRENT_LOCATION
        {
            get { return this.CURRENT_LOCATION; }
            set { this.CURRENT_LOCATION = value; }
        }

        double CURRENT_VELOCITY;// { get; set; }       //100.000000
        public double STATUS_CURRENT_VELOCITY
        {
            get { return this.CURRENT_VELOCITY; }
            set { this.CURRENT_VELOCITY = value; }
        }

        //Input, Output Signal Setting
        //++ 지정 축의 Inposition(위치결정완료) 신호 Active Level/사용유무를 설정합니다.
        // - Inposition 신호를 사용안함으로 설정하면 모션제어 칩에서 펄스출력이 완료될 때 즉시구동 종료됩니다.
        // ※ [CAUTION] Inposition 신호를 사용함으로 설정하면 모션제어 칩에서 펄스출력이 완료된 후 서보팩으로 부터 
        //              Inposition(위치결정완료) 신호가 Active될 때 까지 모션 구동중으로 됩니다.
        // ※ [CAUTION] Inposition 신호를 사용할 때 Active Level이 맞지않으면 최초 한번 구동 후 모션구동이 종료되지않아 
        //              다음 구동을 할 수 없게 됩니다. 
       
        //CAXM.AxmSignalSetInpos(m_lAxisNo, duUse);

        //CAXM.AxmSignalSetServoAlarmResetLevel(m_lAxisNo, duLevel);       

        
//         duRetCode = CAXM.AxmSignalGetLimit(m_lAxisNo, ref duStopMode, ref duPosigitaveLevel, ref duNegativeLevel);
//         if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
//         {
//             duNegativeLevel = ConvertComboToAxm(ref cboELimitN);
//             duPosigitaveLevel = ConvertComboToAxm(ref cboELimitP);
//             CAXM.AxmSignalSetLimit(m_lAxisNo, duStopMode, duPosigitaveLevel, duNegativeLevel);
//         }


        //CAXM.AxmSignalSetZphaseLevel(m_lAxisNo, duLevel);

        //CAXM.AxmSignalSetStop(m_lAxisNo, duStopMode, duLevel);

        //CAXM.AxmSignalSetServoOnLevel(m_lAxisNo, duLevel);

        //CAXM.AxmSignalSetServoAlarm(m_lAxisNo, duUse);

        //CAXDev.AxmSignalSetEncoderType(m_lAxisNo, duEncoderType);
        
        /// <summary>
        /// Pulse, Encoder Method & Move Parameter Setting     
        /// </summary>        
        //++ 지정 축의 펄스 출력 방식을 설정합니다.
        //uMethod : (0)OneHighLowHigh   - 1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
        //          (1)OneHighHighLow   - 1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
        //          (2)OneLowLowHigh    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
        //          (3)OneLowHighLow    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
        //          (4)TwoCcwCwHigh     - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High     
        //          (5)TwoCcwCwLow      - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low     
        //          (6)TwoCwCcwHigh     - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
        //          (7)TwoCwCcwLow      - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
        //          (8)TwoPhase         - 2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
        //          (9)TwoPhaseReverse  - 2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
       
        //CAXM.AxmMotSetPulseOutMethod(m_lAxisNo, duMethod);


        //++ 지정 축의 Encoder 입력 방식을 설정합니다.
        // uMethod : (0)ObverseUpDownMode - 정방향 Up/Down
        //           (1)ObverseSqr1Mode   - 정방향 1체배
        //           (2)ObverseSqr2Mode   - 정방향 2체배
        //           (3)ObverseSqr4Mode   - 정방향 4체배
        //           (4)ReverseUpDownMode - 역방향 Up/Down
        //           (5)ReverseSqr1Mode   - 역방향 1체배
        //           (6)ReverseSqr2Mode   - 역방향 2체배
        //           (7)ReverseSqr4Mode   - 역방향 4체배
        
        //CAXM.AxmMotSetEncInputMethod(m_lAxisNo, duMethod);

        //++ 지정 축의 구동 좌표계를 설정합니다. 
        // duAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
        //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
       
        //CAXM.AxmMotSetAbsRelMode(m_lAxisNo, duAbsRelMode);


        //++ 지정 축의 구동 속도 프로파일 모드를 설정합니다.
        // uProfileMode : (0)SYM_TRAPEZOID_MODE  - Symmetric Trapezoid
        //                (1)ASYM_TRAPEZOID_MODE - Asymmetric Trapezoid
        //                (2)QUASI_S_CURVE_MODE  - Symmetric Quasi-S Curve
        //                (3)SYM_S_CURVE_MODE    - Symmetric S Curve
        //                (4)ASYM_S_CURVE_MODE   - Asymmetric S Curve
       
        //CAXM.AxmMotSetProfileMode(m_lAxisNo, duProfileMode);

        /// <summary>
        /// Home Searching Setting     
        /// CAXM.AxmHomeSetMethod(m_lAxisNo, lHomeDir, duHomeSignal, duZphas, dHomeClrTime, dHomeOffset);
        /// </summary>        
       
        //++ 지정 축의 원점신호 Active Level을 설정합니다.
        //CAXM.AxmHomeSetSignalLevel(m_lAxisNo, duHomeLevel);

        //CAXM.AxmHomeSetMethod(m_lAxisNo, lHomeDir, duHomeSignal, duZphas, dHomeClrTime, dHomeOffset);

        /// <summary>
        /// Software Limit Setting     
        /// </summary>        
        //소프트웨어 리미트 사용가능한지 정보를 가지는 멤버변수
       
        //m_uSoftwareLimitRetCode = CAXM.AxmSignalGetSoftLimit(m_lAxisNo, ref duUse, ref duStopMode, ref duSelection, ref dPositivePos, ref dNegativePos);
        //if (m_uSoftwareLimitRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
//        {
//          dNegativePos = Convert.ToDouble(edtSwPosN.Text);
//          dPositivePos = Convert.ToDouble(edtSwPosP.Text);
//          CAXM.AxmSignalSetSoftLimit(m_lAxisNo, duUse, duStopMode, duSelection, dPositivePos, dNegativePos);
//        }
        
        //소프트웨어 리미트 사용할지 정보를 가지는 멤버변수
        // uUse       : (0)DISABLE        - 소프트웨어 리미트 기능을 사용하지 않습니다.
        //              (1)ENABLE         - 소프트웨어 리미트 기능을 사용합니다.
        
        //소프트웨어 리미트 스톱모드 정보를 가지는 멤버변수
        // uStopMode  : (0)EMERGENCY_STOP - 소프트웨어 리미트 영역을 벗어날 경우 급정지합니다.
        //              (1)SLOWDOWN_STOP  - 소프트웨어 리미트 영역을 벗어날 경우 감속정지합니다.
       
        //소프트웨어 리미트 기준위치를 가지는 멤버변수
        // uSelection : (0)COMMAND        - 기준위치를 지령위치로 합니다.
        //              (1)ACTUAL         - 기준위치를 엔코더 위치로 합니다.
       

        /// <summary>
        /// User Move Parameter Setting     
        /// CAXM.AxmMotSetParaLoad(m_lAxisNo, m_dUserInitPos, m_dUserInitVel, m_dUserInitAccel, m_dUserInitDecel);	
        /// </summary>        
       
       
        public string AxisSearchHome(bool bSearching)
        {
            string strSearchingData = "SearchHome Function!";
            return strSearchingData;
        }

        public string toString()
        {
            return "Otp_ServoMotor";
        }

        public string AxisMoveInc(double movePoint)
        {
            string moveIncStr = "상대 좌료:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }

        public string AxisMoveAbs(double movePoint)
        {
            string moveIncStr = "절대 좌표:" + movePoint.ToString() + " 로 이동합니다.\r\n";
            return moveIncStr;
        }
    }


    /// <summary>
    /// 구성 항목 : 카메라//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ICameras
    {
        System.Drawing.Size CamSize { get; set; }
        System.Drawing.Size CamResolution { get; set; }
        bool Status { get; set; }
        string toString();
        string Graping(string strSaveName);
    }

    /// <summary>
    /// 카메라 요소 : CInp_Camera
    /// </summary>
    public class CInp_Camera : ICameras
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
    /// <summary>
    /// 카메라 요소 : CInp_XCL_5005
    /// </summary>
    public class CInp_XCL_5005 : ICameras
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

    /// <summary>
    /// 카메라 요소 : CInp_STC_CLC500A
    /// </summary>
    public class CInp_STC_CLC500A : ICameras
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
    /// 구성 항목 : 조명//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IFlashLights
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

    /// <summary>
    /// 조명 요소 : 플레쉬 라이트//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class CInp_FlashLight : IFlashLights
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

    /// <summary>
    /// 조명 요소 : JFLLS_100//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class CInp_JFLLS_100 : IFlashLights
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
    /// 구성요소군 : 실린더//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface ICylinders
    {
        int OutputMapNo { get; set; }
        bool Status { get; set; }
        string toString();
        string MoveCylinder(string UpDown);
    }

    public class COtp_Cylinder : ICylinders
    {
        int m_OutputMapNo;
        public int OutputMapNo 
        {
            get { return this.m_OutputMapNo; }
            set { this.m_OutputMapNo = value; } 
        }

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
