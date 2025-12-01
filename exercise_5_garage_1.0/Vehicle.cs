using System.Drawing;
using System.Text.RegularExpressions;

namespace exercise_5_garage_1._0
{

    internal abstract class Vehicle
    {
        private string _regNr = string.Empty;
        private const int _MaxNrOfWheels = 18;
        private ConsoleColor _color;
        private int _nrOfWheels;

        protected VehicleEnumType _vehicleType;

        //public VehicleEnumType VehicleType
        //{
        //    get { return _vehicleType; }
        //    protected set { _vehicleType = value; }
        //}


        public string VehicleType
        {
            get { return _vehicleType.ToString(); }
            ////protected set
            ////{
            ////    // Set the VehicleType as a case insensitive string
            ////    // representation of VehicleEnumType values.
            ////    bool found = false;

            ////    // Check if string matches any of the VehicleEnumType values.
            ////    // Loop instead of Enum.Parse to get case insensitive match.
            ////    foreach (VehicleEnumType vehicleType in Enum.GetValues<VehicleEnumType>())
            ////    {
            ////        if (value.ToLower() == vehicleType.ToString().ToLower())
            ////        {
            ////            found = true;
            ////            _vehicleType = vehicleType;
            ////            break;
            ////        }
            ////    }

            ////    if (!found)
            ////        throw new ArgumentException($"Invalid vehicle type: \"{value}\"", nameof(value));
            ////}


            //set
            //{
            //    _vehicleType = StringToVehicleType(value);
            //    //// Set the VehicleType as a case insensitive string
            //    //// representation of VehicleEnumType values or as the integer
            //    //// number for the enum value (starting at 1 and NOT 0).
            //    //bool found = false;
            //    //int vehicleTypeNr;
            //    //if (Int32.TryParse(value, out vehicleTypeNr))
            //    //{
            //    //    // 1. Check if value is parsable as int
            //    //    if (Enum.IsDefined(typeof(VehicleEnumType), vehicleTypeNr - 1))
            //    //    {
            //    //        found = true;
            //    //        _vehicleType = (VehicleEnumType)(vehicleTypeNr - 1);
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    // Check if string matches any of the VehicleEnumType values.
            //    //    // Loop instead of Enum.Parse to get case insensitive match.
            //    //    foreach (VehicleEnumType vehicleType in Enum.GetValues<VehicleEnumType>())
            //    //    {
            //    //        if (value.ToLower() == vehicleType.ToString().ToLower())
            //    //        {
            //    //            found = true;
            //    //            _vehicleType = vehicleType;
            //    //            break;
            //    //        }
            //    //    }
            //    //}
            //    //if (!found)
            //    //{
            //    //    throw new ArgumentException($"Invalid vehicle type: \"{value}\"", nameof(value));
            //    //}
            //}
        }


        public string RegistrationNr
        {
            get { return _regNr; }
            set
            {
                _regNr = string.Empty;
                string rePatt = @"^[a-zA-ZåÅäÄöÖ]{3}\d{3}$";
                Regex rgx = new Regex(rePatt);
                Match m = rgx.Match(value);
                if (!m.Success)
                {
                    throw new ArgumentException(
                        $"Invalid registration number: \"{value}\"",
                        nameof(value));
                }
                _regNr = value.ToUpper();
            }
        }

        public string Color
        {
            get { return _color.ToString(); }
            set {
                // Set the Color as a case insensitive string representation of
                // the ConsoleColor enum values or as the integer number for
                // the enum value (starting at 1 and NOT 0).
                bool found = false;
                int colorNr;
                if (Int32.TryParse(value, out colorNr))
                {
                    // 1. Check if value is parsable as int
                    if (Enum.IsDefined(typeof(ConsoleColor), colorNr - 1))
                    {
                        found = true;
                        _color = (ConsoleColor)(colorNr - 1);
                    }
                }
                else
                {
                    // 2. Check if string matches any of the ConsoleColor values.
                    // Loop instead of Enum.Parse to get case insensitive match.
                    foreach (ConsoleColor color in Enum.GetValues<ConsoleColor>())
                    {
                        if (value.ToLower() == color.ToString().ToLower())
                        {
                            found = true;
                            _color = color;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    throw new ArgumentException($"Invalid color: \"{value}\"", nameof(value));
                }
            }
        }

        public int NumberOfWheels
        {
            get { return _nrOfWheels; }
            set
            {
                if (value > _MaxNrOfWheels)
                {
                    throw new ArgumentException($"Invalid nr of wheels: \"{value}\"", nameof(value));
                }
                _nrOfWheels = value;
            }
        }

        internal void SetNrOfWheels(string nrOfWheels)
        {
            int tmpNr;
            if (!Int32.TryParse(nrOfWheels, out tmpNr))
            {
                throw new ArgumentException($"Invalid nr of wheels: \"{nrOfWheels}\"", nameof(nrOfWheels));
            }
            NumberOfWheels = tmpNr;
        }

        internal static VehicleEnumType StringToVehicleType(string vehicleType)
        {
            // Set the VehicleType as a case insensitive string
            // representation of VehicleEnumType values or as the integer
            // number for the enum value (starting at 1 and NOT 0).
            int vehicleTypeNr;
            if (Int32.TryParse(vehicleType, out vehicleTypeNr))
            {
                // 1. Check if vehicleType is parsable as int
                if (Enum.IsDefined(typeof(VehicleEnumType), vehicleTypeNr - 1))
                    return (VehicleEnumType)(vehicleTypeNr - 1);
            }
            else
            {
                // Check if string matches any of the VehicleEnumType values.
                // Loop instead of Enum.Parse to get case insensitive match.
                foreach (VehicleEnumType vehicTp in Enum.GetValues<VehicleEnumType>())
                {
                    if (vehicleType.ToLower() == vehicTp.ToString().ToLower())
                        return vehicTp;
                }
            }
            throw new ArgumentException($"Invalid vehicle type: \"{vehicleType}\"", nameof(vehicleType));
        }

        public Vehicle() {}

        public Vehicle(string regNr, ConsoleColor color, int nrOfWheels)
        {
            RegistrationNr = regNr;
            _color         = color;
            NumberOfWheels     = nrOfWheels;
            _vehicleType = StringToVehicleType("Vehicle");
            ////VehicleType = VehicleEnumType.Vehicle;
            //VehicleType = StringToVehicleType("Vehicle");
        }

    }
}