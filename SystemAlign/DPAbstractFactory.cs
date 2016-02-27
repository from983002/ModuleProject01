using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//각각의 항목 : 서보모터, 스텝모터, 압력센서, 반사센서, 솔레노이드밸브
//항목 요 
///서보모터 : X, Y, Z축 모터
///스텝모터 : ??, ??
///압력센서 : ??, ??
///반사센서 : 상단검출, 하단검출
///솔레밸브 : 전면구동, 후면구동등..
//
//Arm의 파생클래스에서 각각의 항목을 사용 유무를 결정한다.
//지역 선택 클래스에서 각항목의 요소를 선택한다.(모든항목)

namespace SystemAlign
{
    public interface IServoMotor
    {
        float Pulse{get; set;}
        
        string toString();
        string MoveSel(float rotate);
        string MoveAbs(float retate);
    }

    public interface IStepMotor
    {
        string toString();
        string MoveSel(float rotate);
        string MoveAbs(float rotate);
    }

    public interface IPressSenser
    {
        string toString();
    }

    public interface IReturnSenser
    {
        string toString();        
    }

    public interface ISolenoid
    {
        string toString();
        string MoveValve(bool flag);
    }

    public class Process1FrontSol : ISolenoid
    {
        bool status;
        public bool Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public string toString()
        {
            return "Solenoid : Process1ForntSol : toString\r\n";
        }

        public string MoveValve(bool flag)
        {
            Status = flag;
            return "Solenoid : Process1ForntSol : MoveValve\r\n";
        }
    }


    public class Process1RearSol : ISolenoid
    {
        bool status;
        public bool Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public string toString()
        {
            return "Solenoid : Process1RearSol : toString\r\n";
        }

        public string MoveValve(bool flag)
        {
            Status = flag;
            return "Solenoid : Process1RearSol : MoveValve\r\n";
        }
    }


    public class Process1ReturnSenserTop : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process1ReturnSenserTop : toString\r\n";
        }
    }

    public class Process1ReturnSenserButton : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process1ReturnSenserButton : toString\r\n";
        }
    }

    public class Process2ReturnSenserTop : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process2ReturnSenserTop : toString\r\n";
        }
    }

    public class Process2ReturnSenserBottom : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process2ReturnSenserBottom : toString\r\n";
        }
    }

    public class Process3ReturnSenserTop : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process3ReturnSenserTop : toString\r\n";
        }
    }


    public class Process3ReturnSenserBottom : IReturnSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "ReturnSenser : Process3ReturnSenserBottom : toString\r\n";
        }
    }


    public class Process1PressSenser : IPressSenser 
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "PressSenser : Process1PressSenser : toString\r\n";
        }
    }

    public class Process2PressSenser : IPressSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "PressSenser : Process2PressSenser : toString\r\n";
        }
    }

    public class Process3PressSenser : IPressSenser
    {
        int pValue;
        public int PValue
        {
            get { return this.pValue; }
            set { this.pValue = value; }
        }

        public string toString()
        {
            return "PressSenser : Process3PressSenser : toString\r\n";
        }
    }

    public class Process1XMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process1XMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process1XMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process1XMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process1YMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }
        
        public string toString()
        {
            return "ServoMotor : Process1YMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process1YMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process1YMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process1ZMotor : IServoMotor
    {
        
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process1ZMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process1ZMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process1ZMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process2XMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process2XMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process2XMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process2XMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process2YMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process2YMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process2YMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process2YMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process2ZMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process2ZMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process2ZMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process2ZMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process3XMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process3XMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process3XMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process3XMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process3YMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process3YMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process3YMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process3YMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process3ZMotor : IServoMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "ServoMotor : Process3ZMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "ServoMotor : Process3ZMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "ServoMotor : Process3ZMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }


    public class Process1StepMotor : IStepMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "StepMotor : Process1StepMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "StepMotor : Process1StepMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "StepMotor : Process1StepMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }

    public class Process2StepMotor : IStepMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "StepMotor : Process2StepMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "StepMotor : Process2StepMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "StepMotor : Process2StepMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }

    public class Process3StepMotor : IStepMotor
    {
        float pulse;
        public float Pulse
        {
            get { return this.pulse; }
            set { this.pulse = value; }
        }

        public string toString()
        {
            return "StepMotor : Process3StepMotor : toString\r\n";
        }

        public string MoveSel(float rotate)
        {
            return "StepMotor : Process3StepMotor : " + rotate.ToString() + "MoveSel\r\n";
        }

        public string MoveAbs(float rotate)
        {
            return "StepMotor : Process3StepMotor : " + rotate.ToString() + "MoveAbs\r\n";
        }
    }

    public abstract class Equipment
    {
        private string name;
        protected IServoMotor[] servoMotor;
        protected IStepMotor stepMotor;
        protected IPressSenser pressSenser;
        protected IReturnSenser[] returnSenser;
        protected ISolenoid[] solenoid;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public Equipment()
        { }
        public abstract string Prepare();
    }

    public class EquipmentFront : Equipment
    {
        IArmIngredientFactory armIngredientFactory;

        public EquipmentFront(IArmIngredientFactory armIngredientFactory)
        {
            this.armIngredientFactory = armIngredientFactory;
        }

        public override string Prepare()
        {
            servoMotor = armIngredientFactory.CreateServoMotor();
            stepMotor = armIngredientFactory.CreateStepMotor();
            pressSenser = armIngredientFactory.CreatePressSenser();
            returnSenser = armIngredientFactory.CreateReturnSenser();
            solenoid = armIngredientFactory.CreateSolenoid();

			StringBuilder sb = new StringBuilder();
			sb.Append("Preparing " + Name + "\n");
			sb.Append(servoMotor[0].toString() +"\n");
            sb.Append(servoMotor[1].toString() +"\n");
            sb.Append(servoMotor[2].toString() +"\n");
			sb.Append(stepMotor.toString() +"\n");
			sb.Append(pressSenser.toString() +"\n");
            sb.Append(returnSenser[0].toString() +"\n");
            sb.Append(returnSenser[1].toString() +"\n");
            sb.Append(solenoid[0].toString() + "\n");
            sb.Append(solenoid[1].toString() + "\n");

			return sb.ToString();
        }
    }


    public class EquipmentRear : Equipment
    {
        IArmIngredientFactory armIngredientFactory;

        public EquipmentRear(IArmIngredientFactory armIngredientFactory)
        {
            this.armIngredientFactory = armIngredientFactory;
        }

        public override string Prepare()
        {
            servoMotor = armIngredientFactory.CreateServoMotor();
            stepMotor = armIngredientFactory.CreateStepMotor();
            pressSenser = armIngredientFactory.CreatePressSenser();
            returnSenser = armIngredientFactory.CreateReturnSenser();
            solenoid = armIngredientFactory.CreateSolenoid();

            StringBuilder sb = new StringBuilder();
            sb.Append("Preparing " + Name + "\n");
            sb.Append(servoMotor[0].toString() + "\n");
            sb.Append(servoMotor[1].toString() + "\n");
            sb.Append(servoMotor[2].toString() + "\n");
            sb.Append(stepMotor.toString() + "\n");
            sb.Append(pressSenser.toString() + "\n");
            sb.Append(returnSenser[0].toString() + "\n");
            sb.Append(returnSenser[1].toString() + "\n");
            sb.Append(solenoid[0].toString() + "\n");
            sb.Append(solenoid[1].toString() + "\n");

            return sb.ToString();
        }
    }


    public abstract class EquipmentFactory
    {
        public EquipmentFactory()        { }
        public Equipment OrderEquipment(string type)
        {
            Equipment equipment;
            equipment = CreateEquipment(type);
            equipment.Prepare();
            return equipment;
        }
        protected abstract Equipment CreateEquipment(string type);
    }

    public class EquipmentFactoryFirst : EquipmentFactory
    {
        public EquipmentFactoryFirst() { }

        protected override Equipment CreateEquipment(string type)
        {
            Equipment equipment = null;
            IArmIngredientFactory ingredientFactory = new ArmIngredientFactoryFirst();

            switch (type)
            {
                case "Front":
                    equipment = new EquipmentFront(ingredientFactory);
                    equipment.Name = "Equipment : Factory : First : Front\r\n";
                    break;
                case "Rear":
                    equipment = new EquipmentRear(ingredientFactory);
                    equipment.Name = "Equipment : Factory : First : Rear\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class EquipmentFactorySecond : EquipmentFactory
    {
        public EquipmentFactorySecond() { }

        protected override Equipment CreateEquipment(string type)
        {
            Equipment equipment = null;
            IArmIngredientFactory ingredientFactory = new ArmIngredientFactorySecond();

            switch (type)
            {
                case "Front":
                    equipment = new EquipmentFront(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Second : Front\r\n";
                    break;
                case "Rear":
                    equipment = new EquipmentRear(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Second : Rear\r\n";
                    break;
            }
            return equipment;
        }
    }


    public class EquipmentFactoryThird : EquipmentFactory
    {
        public EquipmentFactoryThird() { }

        protected override Equipment CreateEquipment(string type)
        {
            Equipment equipment = null;
            IArmIngredientFactory ingredientFactory = new ArmIngredientFactoryThird();

            switch (type)
            {
                case "Front":
                    equipment = new EquipmentFront(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Third : Front\r\n";
                    break;
                case "Rear":
                    equipment = new EquipmentRear(ingredientFactory);
                    equipment.Name = "Equipment : Factory : Third : Rear\r\n";
                    break;
            }
            return equipment;
        }
    }


    public interface IArmIngredientFactory
	{
		IServoMotor[] CreateServoMotor();
		IStepMotor CreateStepMotor();
		IPressSenser CreatePressSenser();
		IReturnSenser[] CreateReturnSenser();
		ISolenoid[] CreateSolenoid();
	}


    public class ArmIngredientFactoryFirst : IArmIngredientFactory
    {
        public ArmIngredientFactoryFirst()        { }

        public IServoMotor[] CreateServoMotor()
        {
            IServoMotor[] returnServoMotors = { new Process1XMotor(), new Process1YMotor(), new Process1ZMotor() };
            return returnServoMotors;
        }

        public IStepMotor CreateStepMotor()
        {
            return new Process1StepMotor();
        }

        public IPressSenser CreatePressSenser()
        {
            return new Process1PressSenser();
        }

        public IReturnSenser[] CreateReturnSenser()
        {
            IReturnSenser[] returnSensers = { new Process1ReturnSenserTop(), new Process1ReturnSenserButton() };
            return returnSensers;
        }

        public ISolenoid[] CreateSolenoid()
        {
            ISolenoid[] returnSolenoids = { new Process1FrontSol(), new Process1RearSol() };
            return returnSolenoids;
        }
    }


    public class ArmIngredientFactorySecond : IArmIngredientFactory
    {
        public ArmIngredientFactorySecond() { }

        public IServoMotor[] CreateServoMotor()
        {
            IServoMotor[] returnServoMotors = { new Process2XMotor(), new Process2YMotor(), new Process2ZMotor() };
            return returnServoMotors;
        }

        public IStepMotor CreateStepMotor()
        {
            return new Process1StepMotor();
        }

        public IPressSenser CreatePressSenser()
        {
            return new Process1PressSenser();
        }

        public IReturnSenser[] CreateReturnSenser()
        {
            IReturnSenser[] returnSensers = { new Process1ReturnSenserTop(), new Process1ReturnSenserButton() };
            return returnSensers;
        }

        public ISolenoid[] CreateSolenoid()
        {
            ISolenoid[] returnSolenoids = { new Process1FrontSol(), new Process1RearSol() };
            return returnSolenoids;
        }
    }


    public class ArmIngredientFactoryThird : IArmIngredientFactory
    {
        public ArmIngredientFactoryThird() { }

        public IServoMotor[] CreateServoMotor()
        {
            IServoMotor[] returnServoMotors = { new Process3XMotor(), new Process3YMotor(), new Process3ZMotor() };
            return returnServoMotors;
        }

        public IStepMotor CreateStepMotor()
        {
            return new Process1StepMotor();
        }

        public IPressSenser CreatePressSenser()
        {
            return new Process1PressSenser();
        }

        public IReturnSenser[] CreateReturnSenser()
        {
            IReturnSenser[] returnSensers = { new Process1ReturnSenserTop(), new Process1ReturnSenserButton() };
            return returnSensers;
        }

        public ISolenoid[] CreateSolenoid()
        {
            ISolenoid[] returnSolenoids = { new Process1FrontSol(), new Process1RearSol() };
            return returnSolenoids;
        }
    }
}
