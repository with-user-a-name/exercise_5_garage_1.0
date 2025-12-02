
namespace exercise_5_garage_1._0
{
    internal class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; set; }

        public Motorcycle() : this(regNr: "", color: ConsoleColor.Black, nrOfWheels: 0, cylinderVolume: 0) { }

        public Motorcycle(string regNr, ConsoleColor color, int nrOfWheels, int cylinderVolume) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = VehicleEnumType.Motorcycle;
            CylinderVolume = cylinderVolume;
        }

    }
}