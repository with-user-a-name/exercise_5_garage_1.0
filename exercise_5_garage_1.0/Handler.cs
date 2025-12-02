
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace exercise_5_garage_1._0
{
    internal class Handler : IHandler
    {
        IGarage<Vehicle>? _garage = null;

        private static Random random = new Random();

        public bool GarageExists => _garage != null;

        public int GarageCapacity
        {
            get
            {
                if (_garage != null)
                {
                    return _garage.Count();
                }
                return 0;
            }
        }

        private static string RandomRegistrationNr()
        {
            const int length = 6;
            const string alphas = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
            const string nums = "0123456789";
            string alphaStr = new string(Enumerable.Repeat(alphas, length / 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            string numStr = new string(Enumerable.Repeat(nums, length / 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return alphaStr + numStr;
        }

        public bool CheckInVehicle(Vehicle vehicle)
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            for (int i = 0; i < _garage.Count(); i++)
            {
                if (_garage[i] == null)
                {
                    _garage[i] = vehicle;
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Vehicle> GetParkedVehicles()
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            return _garage.Where(p => p != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public bool Search(string propertyName, object value, ref IEnumerable<Vehicle>? source)
        {
            if (propertyName == null)
                throw new ArgumentNullException("Parameter propertyName cannot be null.");
            if (value == null)
                throw new ArgumentNullException("Parameter value cannot be null.");
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            // For the first search we start with the garage, on following
            // refined searches source will no longer be null.
            if (source == null)
                source = _garage;

            // Get the type of the object
            Type type = typeof(Vehicle);

            // Get the PropertyInfo object for the specified property
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"Parameter propertyName does not seem to exist in {nameof(Vehicle)}.");

            if (propertyInfo.PropertyType == typeof(string))
            {
                // In case the Vehicle property (propertyName) is of type
                // string its desirable to make the match case insensitive.
                // This is done by converting both the vehicle property value,
                // represented by propertyInfo, and the value to string before
                // comparing using the Equals method.
                if (value.ToString() == string.Empty)
                    throw new ArgumentException("Parameter value may not be an empty string.");

                source = source
                    .Where(p => p != null)
                    .Where(p => value.ToString()!.Equals(propertyInfo.GetValue(p)?.ToString() ?? "", StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                // The Vehicle property (propertyName) may, apart from the
                // string case handeled in "if" above, be of any type, so for
                // comparisons to work the parameter value has to be converted
                // to the same type, if possible. If not, enjoy one of the
                // following:
                //    InvalidCastException, FormatException
                //    OverflowException,    ArgumentNullException
                dynamic convertedUnboxedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                source = source
                    .Where(p => p != null)
                    .Where(p =>
                    {
                        dynamic unboxedValue = propertyInfo.GetValue(p) ?? "";
                        return unboxedValue == convertedUnboxedValue;
                    });
            }
            return source.Any();
        }

        public List<string> GetPropNames<T>()
        {
            var propNames = new List<string>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                propNames.Add(prop.Name);
            }
            return propNames;
        }

        public int GetGaragePosition(Vehicle vehicle)
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");
            return Array.IndexOf(_garage.ToArray(), vehicle);
        }

        public string CheckOutVehicle(Vehicle vehicle)
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            int pos = GetGaragePosition(vehicle);
            if (pos == -1)
                return string.Empty;
            if (_garage[pos] == null)
                return string.Empty;

            string regNr = _garage[pos].RegistrationNr;
            _garage[pos] = null!;
            return regNr;
        }

        public bool GarageIsFull()
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            if (GetParkedVehicles().Count() < _garage.Count())
            {
                return false;
            }
            return true;
        }

        public void CreateGarage(int vehicleCapacity)
        {
            _garage = new Garage<Vehicle>(vehicleCapacity);
        }

        public void AddVehicles(int vehiclesToAdd)
        {
            if (_garage == null)
                throw new InvalidOperationException("There is no garage (_garage == null)!");

            Vehicle? vehicle = null;
            for (int i = 0; i < vehiclesToAdd; i++)
            {
                do
                {
                    vehicle = GetNewRandomVehicle();
                    if (vehicle == null)
                        break;
                } while (_garage.Where(p => p != null)
                                .Where(p => p.RegistrationNr == vehicle.RegistrationNr)
                                .Any());

                if (vehicle == null)
                    continue;

                CheckInVehicle(vehicle);
            }

            //vehicle = new Airplane("aaa111", ConsoleColor.Magenta, 4, 5);
            //CheckInVehicle(vehicle);

            //vehicle = new Motorcycle("ccc333", ConsoleColor.Red, 4, 750);
            //CheckInVehicle(vehicle);

            //vehicle = new Car("ddd444", ConsoleColor.Green, 4, FuelType.Gasoline);
            //CheckInVehicle(vehicle);

            //vehicle = new Bus("eee555", ConsoleColor.Green, 6, 83);
            //CheckInVehicle(vehicle);

            //vehicle = new Boat("bbb222", ConsoleColor.Magenta, 4, length: 20);
            //CheckInVehicle(vehicle);

            //vehicle = new Vehicle("fff666", ConsoleColor.Cyan, 2);
            //CheckInVehicle(vehicle);

        }

        private Vehicle? GetNewRandomVehicle()
        {
            VehicleEnumType vehicleType = (VehicleEnumType)random.Next(0, 5);

            Type theVehicleType = Type.GetType("exercise_5_garage_1._0." + vehicleType.ToString())!;
            if (theVehicleType == null)
                return null;
            var vehicleObject = Activator.CreateInstance(theVehicleType);
            if (vehicleObject == null)
                return null;

            string regNr = RandomRegistrationNr();
            ConsoleColor color = (ConsoleColor)random.Next(0, 16);
            int nrOfWheels = random.Next(0, 19);

            ((Vehicle)vehicleObject).RegistrationNr = regNr;
            ((Vehicle)vehicleObject).Color = color.ToString();
            ((Vehicle)vehicleObject).NumberOfWheels = nrOfWheels;

            switch (vehicleType)
            {
                case VehicleEnumType.Airplane:
                    int nrOfEngines = random.Next(1, 13);
                    ((Airplane)vehicleObject).NrOfEngines = nrOfEngines;
                    break;
                case VehicleEnumType.Motorcycle:
                    int cylinderVolume = random.Next(50, 2001);
                    ((Motorcycle)vehicleObject).CylinderVolume = cylinderVolume;
                    break;
                case VehicleEnumType.Car:
                    FuelType fuel = (FuelType)random.Next(0, 2);
                    ((Car)vehicleObject).Fuel = fuel.ToString();
                    break;
                case VehicleEnumType.Bus:
                    int numberOfSeats = random.Next(5, 81);
                    ((Bus)vehicleObject).NumberOfSeats = numberOfSeats;
                    break;
                case VehicleEnumType.Boat:
                    int length = random.Next(5, 101);
                    ((Boat)vehicleObject).Length = length;
                    ((Boat)vehicleObject).NumberOfWheels = 0;
                    break;
                default:
                    break;
            }

            return (Vehicle)vehicleObject;
        }
    }
}