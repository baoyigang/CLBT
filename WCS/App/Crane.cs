using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace App
{
    public class Crane
    {
        public int CraneNo { get; set; }
        public string TaskNo { get; set;}
        public string PalletCode { get; set; }
        public int TaskType { get; set;}
        public int Action { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Height { get; set; }
        public int ForkStatus { get; set; }
        public int ErrCode { get; set; }
        public int WalkCode { get; set; }
        public int UpDownCode { get; set; }
    }

    public delegate void CraneEventHandler(CraneEventArgs args);
    public class CraneEventArgs
    {
        private Crane _crane;
        public Crane crane
        {
            get
            {
                return _crane;
            }
        }
        public CraneEventArgs(Crane crane)
        {
            this._crane = crane;
        }
    }
    public class Cranes
    {
        public static event CraneEventHandler OnCrane = null;

        public static void CraneInfo(Crane crane)
        {
            if (OnCrane != null)
            {
                OnCrane(new CraneEventArgs(crane));
            }
        }
    }

    public class Car
    {
        public int CarNo { get; set; }
        public string TaskNo { get; set; }
        public int Position { get; set; }
        public int Action { get; set; }
        public int Load { get; set; }
        public int Status { get; set; }
        public int ErrCode { get; set; }
    }

    public delegate void CarEventHandler(CarEventArgs args);
    public class CarEventArgs
    {
        private Car _car;
        public Car car
        {
            get
            {
                return _car;
            }
        }
        public CarEventArgs(Car car)
        {
            this._car = car;
        }
    }
    public class Cars
    {
        public static event CarEventHandler OnCar = null;

        public static void CarInfo(Car car)
        {
            if (OnCar != null)
            {
                OnCar(new CarEventArgs(car));
            }
        }
    }
    public class Conveyor
    {
        public string ctlName { get; set; }
        public string ID { get; set; }
        public string value { get; set; }
        public string BarCode { get; set; }
    }

    public delegate void ConveyorEventHandler(ConveyorEventArgs args);
    public class ConveyorEventArgs
    {
        private Conveyor _conveyor;
        public Conveyor conveyor
        {
            get
            {
                return _conveyor;
            }
        }
        public ConveyorEventArgs(Conveyor conveyor)
        {
            this._conveyor = conveyor;
        }
    }
    public class Conveyors
    {
        public static event ConveyorEventHandler OnConveyor = null;

        public static void ConveyorInfo(Conveyor conveyor)
        {
            if (OnConveyor != null)
            {
                OnConveyor(new ConveyorEventArgs(conveyor));
            }
        }
    }
}
