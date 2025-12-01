
namespace exercise_5_garage_1._0
{
    internal class Bus : Vehicle
    {
        public int NumberOfSeats { get; private set; }

        public Bus() { }

        public Bus(string regNr, ConsoleColor color, int nrOfWheels, int nrOfSeats) : base(regNr, color, nrOfWheels)
        {
            //VehicleType = VehicleEnumType.Bus;
            _vehicleType = StringToVehicleType("Bus");
            //VehicleType = "Bus";
            NumberOfSeats = nrOfSeats;
        }

    }
}