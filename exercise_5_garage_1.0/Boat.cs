
namespace exercise_5_garage_1._0
{
    public class Boat : Vehicle
    {
        public int Length { get; set; }

        public Boat() : this(regNr:"", color:ConsoleColor.Black, nrOfWheels:0, length:0) { }
        
        public Boat(string regNr, ConsoleColor color, int nrOfWheels, int length) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = VehicleEnumType.Boat;
            Length = length;
        }
    }
}