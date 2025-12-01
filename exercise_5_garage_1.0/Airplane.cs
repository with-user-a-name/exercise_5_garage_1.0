
namespace exercise_5_garage_1._0
{
    internal class Airplane : Vehicle
    {
        public int NrOfEngines { get; set; }

        public Airplane() { }

        public Airplane(string regNr, ConsoleColor color, int nrOfWheels, int nrOfEngines) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = StringToVehicleType("Airplane");
            //VehicleType = "Airplane";
            NrOfEngines = nrOfEngines;
        }

    }
}