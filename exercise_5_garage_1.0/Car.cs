

using System.Drawing;

namespace exercise_5_garage_1._0
{

    internal class Car : Vehicle
    {
        private FuelType _fuel;

        public string Fuel
        {
            get { return _fuel.ToString(); }
            set
            {
                // Set the Fuel as a case insensitive string representation of
                // the FuelType enum values or as the integer number for
                // the enum value (starting at 1 and NOT 0).
                bool found = false;
                int fuelNr;
                if (Int32.TryParse(value, out fuelNr))
                {
                    // 1. Check if value is parsable as int
                    if (Enum.IsDefined(typeof(FuelType), fuelNr - 1))
                    {
                        found = true;
                        _fuel = (FuelType)(fuelNr - 1);
                    }
                }
                else
                {
                    // 2. Check if string matches any of the FuelType values.
                    // Loop instead of Enum.Parse to get case insensitive match.
                    foreach (FuelType fuel in Enum.GetValues<FuelType>())
                    {
                        if (value.ToLower() == fuel.ToString().ToLower())
                        {
                            found = true;
                            _fuel = fuel;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    _fuel = (FuelType)0;
                    // Had to remove exceptions from the property setters (see to 2512010739 for details).
                    //throw new ArgumentException($"Invalid fuel: \"{value}\"", nameof(value));
                }
            }
        }

        public Car() : this(regNr: "", color: ConsoleColor.Black, nrOfWheels: 0, fuel: FuelType.Gasoline) { }

        public Car(string regNr, ConsoleColor color, int nrOfWheels, FuelType fuel) : base(regNr, color, nrOfWheels)
        {
            _vehicleType = VehicleEnumType.Car;
            _fuel = fuel;
        }
    }
}