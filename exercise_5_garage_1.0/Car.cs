
using Microsoft.VisualBasic.FileIO;

namespace exercise_5_garage_1._0
{

    internal class Car : Vehicle
    {
        public FuelType Fuel { get; private set; }

        public Car() { }

        public Car(string regNr, ConsoleColor color, int nrOfWheels, FuelType fuel) : base(regNr, color, nrOfWheels)
        {
            //VehicleType = VehicleEnumType.Car;
            _vehicleType = StringToVehicleType("Car");
            //VehicleType = "Car";
            Fuel = fuel;
        }
    }
}