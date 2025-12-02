
namespace exercise_5_garage_1._0
{
    internal class Bus : Vehicle
    {
        public int NumberOfSeats { get; set; }

        public Bus() : this(regNr: "", color: ConsoleColor.Black, nrOfWheels: 0, nrOfSeats: 0) { }

        public Bus(string regNr, ConsoleColor color, int nrOfWheels, int nrOfSeats) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = VehicleEnumType.Bus;
            NumberOfSeats = nrOfSeats;
        }

    }
}