
namespace exercise_5_garage_1._0
{
    internal class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; private set; }

        public Motorcycle() { }

        public Motorcycle(string regNr, ConsoleColor color, int nrOfWheels, int cylinderVolume) : base(regNr, color, nrOfWheels)
        {
            //VehicleType = VehicleEnumType.Motorcycle;
            _vehicleType = StringToVehicleType("Motorcycle");
            //VehicleType = "Motorcycle";
            CylinderVolume = cylinderVolume;
        }

    }
}