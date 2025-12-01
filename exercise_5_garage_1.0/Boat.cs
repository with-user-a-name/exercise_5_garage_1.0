
namespace exercise_5_garage_1._0
{
    internal class Boat : Vehicle
    {
        public int Length { get; private set; }

        public Boat() { }

        public Boat(string regNr, ConsoleColor color, int nrOfWheels, int length) : base(regNr, color, nrOfWheels)
        {
            //VehicleType = VehicleEnumType.Boat;
            _vehicleType = StringToVehicleType("Boat");
            //VehicleType = "Boat";
            Length = length;
        }
    }
}