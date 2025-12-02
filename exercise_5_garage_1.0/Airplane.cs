
namespace exercise_5_garage_1._0
{
    public class Airplane : Vehicle
    {
        public int NrOfEngines { get; set; }

        public Airplane() : this(regNr: "", color: ConsoleColor.Black, nrOfWheels: 0, nrOfEngines: 0) { }

        public Airplane(string regNr, ConsoleColor color, int nrOfWheels, int nrOfEngines) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = VehicleEnumType.Airplane;
            NrOfEngines = nrOfEngines;
        }
    }
}