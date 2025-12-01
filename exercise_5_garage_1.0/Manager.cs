


using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection;

namespace exercise_5_garage_1._0
{
    internal class Manager
    {

        private UI _ui;
        private Handler _handler = new Handler();

        public Manager(UI ui)
        {
            _ui = ui;
        }

        internal void Run()
        {
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            bool keepLooping = true;
            string prompt = "Your selection: ";

            var mainMenu = new Dictionary<ConsoleKey, Action>()
                {
                    { ConsoleKey.D1,      ListParkedVehicles },
                    { ConsoleKey.NumPad1, ListParkedVehicles },
                    { ConsoleKey.D2,      ListVehicleTypesAndAmount },
                    { ConsoleKey.NumPad2, ListVehicleTypesAndAmount },
                    { ConsoleKey.D3,      CheckInVehicle },
                    { ConsoleKey.NumPad3, CheckInVehicle },
                    { ConsoleKey.D4,      CheckOutVehicle },
                    { ConsoleKey.NumPad4, CheckOutVehicle },
                    { ConsoleKey.D5,      CreateGarage },
                    { ConsoleKey.NumPad5, CreateGarage },
                    { ConsoleKey.D6,      AddVehiclesToGarage},
                    { ConsoleKey.NumPad6, AddVehiclesToGarage },
                    { ConsoleKey.D7,      SearchForSpecificVehicleByRegNr},
                    { ConsoleKey.NumPad7, SearchForSpecificVehicleByRegNr },
                    { ConsoleKey.D8,      SearchGarage },
                    { ConsoleKey.NumPad8, SearchGarage },
                    { ConsoleKey.Escape,  () => {keepLooping = false; } }
                };

            while (keepLooping)
            {
                string headLine = "********************\n"
                                + "* Select an action *\n"
                                + "********************\n";
                _ui.WriteHeadLine(headLine);

                string menuOptions = "Select one of the menu items below:\n"
                                   + "1     - List all parked vehicles\n"
                                   + "2     - List all vehicle types and how many of each there are in the garage\n"
                                   + "3     - Check-in vehicle in the garage\n"
                                   + "4     - Check-out vehicle from the garage\n"
                                   + "5     - Create new garage\n"
                                   + "6     - Add specific amount of random vehicles to the garage\n"
                                   + "7     - Search for specific vehicle by registration number\n"
                                   + "8     - Search the garage for vehicles\n"
                                   + "<ESC> - Exit program\n"
                                   + "\n"
                                   + prompt;
                _ui.WriteMenuOptions(menuOptions);

                var keyPressed = _ui.GetKey();
                _ui.Write("\n");

                if (mainMenu.ContainsKey(keyPressed))
                {
                    mainMenu[keyPressed]?.Invoke();
                    prompt = "Your selection: ";
                }
                else
                    prompt = $"Invalid selection. Please try again: ";
            }
        }

        private void SearchForSpecificVehicleByRegNr()
        {
            string headLine = "******************************************************\n"
                            + "* Search for specific vehicle by registration number *\n"
                            + "******************************************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            string regNr;
            string prompt;
            IEnumerable<Vehicle>? queryResult;
            prompt = "Enter the registration number for the vehicle to find (<ESC> for main menu): ";
            do
            {
                queryResult = null;
                _ui.Write(prompt);
                if (_ui.EscapeOrReadLine(out regNr))
                    return;
                try
                {
                    _handler.Search("RegistrationNr", regNr, ref queryResult);
                    break;
                }
                catch (Exception ex)
                {
                    _ui.Write($"** {ex.Message}\n");
                    continue;
                }
            } while (true);

            if (queryResult!.Count() == 1)
                ListVehicles(queryResult!);
            else
                _ui.Write($"There is no vehicle with registration number \"{regNr}\" in the garage.\n");

            _ui.PressAnyKeyToContinue();
        }

        private void AddVehiclesToGarage()
        {
            string headLine = "********************************************************\n"
                            + "* Add specific amount of random vehicles to the garage *\n"
                            + "********************************************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            if (_handler.GarageIsFull())
            {
                _ui.Write($"The garage is full. Check out vehicles to make room for new ones.\n");
                _ui.PressAnyKeyToContinue();
                return;
            }

            int maxAdditionalVehicles = _handler.GarageCapacity - _handler.GetParkedVehicles().Count();
            _ui.Write($"Of the current {_handler.GarageCapacity} parking spots in the garage, {maxAdditionalVehicles} are free.\n");

            int vehiclesToAdd = 0;
            string prompt = $"Enter the number of additional 1 to {maxAdditionalVehicles} vehicles to park in the garage (<ESC> for main menu): ";
            string line;
            do
            {
                _ui.Write(prompt);
                if (_ui.EscapeOrReadLine(out line))
                    return;

                prompt = $"Invalid number of vehicles to add: \"{line}\"\n"
                    + $"Enter the number of additional 1 to {maxAdditionalVehicles} vehicles to park in the garage (<ESC> for main menu): ";
            } while (!Int32.TryParse(line, out vehiclesToAdd) || ((vehiclesToAdd < 1) || (maxAdditionalVehicles < vehiclesToAdd)));

            _ui.Write($"Adding {vehiclesToAdd} vehicles to the garage.\n");
            _handler.AddVehicles(vehiclesToAdd);
            _ui.PressAnyKeyToContinue();
        }

        private void ListVehicleTypesAndAmount()
        {
            string headLine = "***********************************************************************\n"
                            + "* List all vehicle types and how many of each there are in the garage *\n"
                            + "***********************************************************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }
        }

        private void SearchGarage()
        {
            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            string headLine = "**********************************\n"
                            + "* Search the garage for vehicles *\n"
                            + "**********************************\n";
            _ui.WriteHeadLine(headLine);
            List<string> SearchProps = _handler.GetPropNames<Vehicle>();
            IEnumerable<Vehicle>? query = null;
            FindVehicle(queryResult: ref query, searchPropStrings: ref SearchProps);
        }

        private bool FindVehicle(ref IEnumerable<Vehicle>? queryResult, ref List<string> searchPropStrings)
        {
            if (!_handler.GarageExists) { NoGarageMessage(); return false; }

            string line = "";
            string prompt;
            int selProp;
            int selection;

            _ui.Write("The following vehicle properties can be searched:\n");
            for (int i = 0; i < searchPropStrings.Count; i++)
            {
                _ui.Write($"{i+1} - {searchPropStrings[i]}\n");
            }

            prompt = $"\nEnter the property to search for 1-{searchPropStrings.Count} (<ESC> for main menu): ";
            do
            {
                _ui.Write(prompt);
                if (_ui.EscapeOrGetDigitStr(out selProp))
                    return (queryResult != null) && queryResult.Any();
                prompt = $"\n** Invalid property.\nEnter the property to search for 1-{searchPropStrings.Count} (<ESC> for main menu): ";
            } while ((selProp < 1) || (searchPropStrings.Count < selProp));
            _ui.Write("\n");
            selProp--; // Since the list is 0 based.

            prompt = $"Enter search string for the {searchPropStrings[selProp]} property (<ESC> for main menu): ";
            do
            {
                try
                {
                    _ui.Write(prompt);
                    if (_ui.EscapeOrReadLine(out line))
                        return (queryResult != null) && queryResult.Any();
                    _handler.Search(searchPropStrings[selProp], line, ref queryResult);
                    if ((queryResult == null) || !queryResult.Any())
                    {
                        _ui.Write("There are no vehicles matching this search!\n");
                        _ui.PressAnyKeyToContinue();
                        return false;
                    }
                    do
                    {
                        // If there only are 1 item in the searchPropStrings
                        // list at this point its not possible to refine the
                        // search further and item 2 in the menu shall not be
                        // presented. The same goes if there currently are only
                        // one match.
                        int nrMenuOpts = 1;
                        if ((searchPropStrings.Count > 1) && (queryResult.Count() > 1))
                            nrMenuOpts = 2;

                        _ui.Write($"There are {queryResult.Count()} vehicles matching this search. Do you want to:\n");
                        _ui.Write("1 - list the vehicles in the current search\n");
                        if (nrMenuOpts == 2)
                            _ui.Write("2 - refine the search with additional properties\n");

                        prompt = $"\nYour selection 1 or {nrMenuOpts} (<ESC> for main menu): ";
                        do
                        {
                            _ui.Write(prompt);
                            if (_ui.EscapeOrGetDigitStr(out selection))
                                return (queryResult != null) && queryResult.Any();
                            prompt = $"\n** Invalid choice. \nYour selection 1 or {nrMenuOpts} (<ESC> for main menu): ";
                        } while ((selection < 1) || (nrMenuOpts < selection));
                        _ui.Write("\n");

                        if (selection == 1)
                        {
                            _ui.Write($"Listing vehicles:\n");
                            ListVehicles(queryResult);
                            _ui.Write("\n");
                        }
                        else if (selection == 2)
                        {
                            // To refine the search with additional properties,
                            // make a recursive call to FindVehicle but make
                            // sure to remove the prop searched for in this
                            // call from searchPropStrings first.
                            searchPropStrings.RemoveAt(selProp);
                            return FindVehicle(ref queryResult, ref searchPropStrings);
                        }
                    } while (true);
                }
                catch (Exception ex)
                {
                    // Expect more or less to exclusively catch issues from the
                    // _handler.Search() call here.
                    prompt = $"Enter search string for the {searchPropStrings[selProp]} property (<ESC> for main menu): ";
                    _ui.Write($"** Invalid search string: \"{line}\""
                            + $"\n** {ex.Message}\n");
                }
            } while (true);
        }

        private void ListVehicles(IEnumerable<Vehicle> queryResult)
        {
            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            foreach (var vehicle in queryResult)
            {
                int garagePos = _handler.GetGaragePosition(vehicle);
                PropertyInfo[] props = vehicle.GetType().GetProperties();
                int fieldWidth = 0;
                foreach (var prop in props)
                    if (prop.Name.Length > fieldWidth)
                        fieldWidth = prop.Name.Length;
                _ui.Write($"Parking spot {garagePos + 1}\n");
                foreach (var prop in props)
                {
                    _ui.Write($"   {(prop.Name + ":").PadRight(fieldWidth + 1)} {prop.GetValue(vehicle)}\n");
                }
            }
        }

        private void NoGarageMessage()
        {
            _ui.Write("** There is no garage, you must select main menu item 5 and \"Create new garage\".\n");
            _ui.PressAnyKeyToContinue();
            return;
        }

        private void CheckOutVehicle()
        {
            string headLine = "*************************************\n"
                            + "* Check-out vehicle from the garage *\n"
                            + "*************************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            string regNr;
            string prompt;
            IEnumerable<Vehicle>? queryResult;
            prompt = "Enter the registration number for the vehicle to check out (<ESC> for main menu): ";
            do
            {
                queryResult = null;
                _ui.Write(prompt);
                if (_ui.EscapeOrReadLine(out regNr))
                    return;
                try
                {
                    if (!_handler.Search("RegistrationNr", regNr, ref queryResult))
                        prompt = $"There is no vehicle with registration number \"{regNr}\" in the garage.\n"
                            + "Enter the registration number for the vehicle to check out (<ESC> for main menu): ";
                }
                catch (Exception ex)
                {
                    _ui.Write($"** {ex.Message}\n");
                    prompt = "Enter the registration number for the vehicle to check out (<ESC> for main menu): ";
                    continue;
                }
            } while ((queryResult == null) || (queryResult.Count() != 1));
            string checkedOutRegNr = _handler.CheckOutVehicle(queryResult.ElementAt(0));
            if (checkedOutRegNr == regNr.ToUpper())
                _ui.Write($"Checked out vehicle with registration number \"{regNr}\".\n");
            else if (checkedOutRegNr == string.Empty)
                _ui.Write($"** Failed to check out vehicle with registration number \"{regNr}\".\n");
            else
                _ui.Write($"** Checked out vehicle with registration number \"{checkedOutRegNr}\".\n");
            _ui.PressAnyKeyToContinue();
        }

        private void ListParkedVehicles()
        {
            string headLine = "****************************\n"
                            + "* List all parked vehicles *\n"
                            + "****************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            var parkedVehicles = _handler.GetParkedVehicles();
            if (parkedVehicles.Any())
                ListVehicles(_handler.GetParkedVehicles());
            else
                _ui.Write("There are no parked vehicles at the moment.\n");

            _ui.PressAnyKeyToContinue();
        }

        private void CheckInVehicle()
        {
            string line;
            string headLine = "**********************************\n"
                            + "* Check-in vehicle in the garage *\n"
                            + "**********************************\n";
            _ui.WriteHeadLine(headLine);

            if (!_handler.GarageExists) { NoGarageMessage(); return; }

            if (_handler.GarageIsFull())
            {
                _ui.Write($"The garage is full. Check out a vehicle to make room for a new one.\n");
                _ui.PressAnyKeyToContinue();
                return;
            }



            _ui.Write("Available vehicle types are listed below:\n");
            foreach (int vehicTp in Enum.GetValues(typeof(VehicleEnumType)))
            {
                _ui.Write($"{(vehicTp + 1)} - {(VehicleEnumType)vehicTp}\n");
            }
            _ui.Write("\n");

            VehicleEnumType vehicleType;
            while (true)
            {
                _ui.WriteMenuOptions("Pick the vehicle type from the above list (<ESC> for main menu): ");
                if (_ui.EscapeOrReadLine(out line))
                    return;
                
                try
                {
                    vehicleType =  Vehicle.StringToVehicleType(line);
                }
                catch (ArgumentException ex)
                {
                    _ui.Write($"{ex.Message}\n");
                    continue;
                }

                _ui.Write($"Using vehicle type: \"{vehicleType}\"\n");
                break;

            }

            Type theVehicleType = Type.GetType("exercise_5_garage_1._0." + vehicleType.ToString())!;
            if (theVehicleType == null)
            {
                _ui.Write($"(theVehicleType == null) ????\n");
                _ui.PressAnyKeyToContinue();
                return;
            }
            var vehicleObject = Activator.CreateInstance(theVehicleType);
            if (vehicleObject == null)
            {
                _ui.Write($"(vehicleObject == null) ????\n");
                _ui.PressAnyKeyToContinue();
                return;
            }
            _ui.Write($"Created instance of type: {vehicleObject.GetType().FullName}\n");



            _ui.Write($"Listing properties:\n");
            //PropertyInfo[] props = vehicleObject.GetType().GetProperties();
            IEnumerable<PropertyInfo> props = vehicleObject.GetType().GetProperties().Where(p => p.CanWrite);
            int fieldWidth = 0;
            foreach (PropertyInfo prop in props)
                if (prop.Name.Length > fieldWidth)
                    fieldWidth = prop.Name.Length;
            foreach (PropertyInfo prop in props)
            {
                _ui.Write($"   {(prop.Name + ":").PadRight(fieldWidth + 1)} {prop.GetValue(vehicleObject)}\n");

                while (true)
                {
                    _ui.WriteMenuOptions($"Enter {prop.Name} for the {vehicleObject.GetType().Name} (<ESC> for main menu): ");
                    if (_ui.EscapeOrReadLine(out line))
                        return;

                    try
                    {
                        var convertedValue = Convert.ChangeType(line, prop.PropertyType);
                        prop.SetValue(vehicleObject, convertedValue);
                    }
                    //catch (TargetInvocationException tie)
                    //{
                    //    // Unwrap the inner exception and handle it
                    //    Console.WriteLine("An error occurred: " + tie.InnerException.Message);
                    //}
                    catch (Exception ex)
                    {
                        //TODO 2512010739: Why is it impossible to catch exceptions from the property setters here ????!!!!!
                        _ui.Write($"From here #####:> {ex.Message}\n");
                        continue;
                    }

                    _ui.Write($"The property {prop.Name} is set to: \"{prop.GetValue(vehicleObject)}\"\n");
                    break;

                }


            }




            _ui.PressAnyKeyToContinue();
            return;


            Vehicle vehicle = new Airplane();
            while (true)
            {
                _ui.WriteMenuOptions("Enter the registration number for the vehicle (<ESC> for main menu): ");
                if (_ui.EscapeOrReadLine(out line))
                    return;
                try
                {
                    vehicle.RegistrationNr = line;
                }
                catch (ArgumentException ex)
                {
                    _ui.Write($"{ex.Message}\n");
                    continue;
                }
                _ui.Write($"Using registration number: \"{vehicle.RegistrationNr}\"\n");
                break;
            }

            _ui.ListConsoleColors("Select color for the vehicle, available colors are listed below:\n");
            _ui.Write("\n");
            while (true)
            {
                _ui.WriteMenuOptions("Pick the closest matching color from the above list (<ESC> for main menu): ");
                if (_ui.EscapeOrReadLine(out line))
                    return;

                try
                {
                    vehicle.Color = line;
                }
                catch (ArgumentException ex)
                {
                    _ui.Write($"{ex.Message}\n");
                    continue;
                }

                _ui.Write($"Using vehicle color: \"{vehicle.Color}\"\n");
                break;
            }


            while (true)
            {
                _ui.WriteMenuOptions("Enter the number of wheels on the vehicle (<ESC> for main menu): ");
                if (_ui.EscapeOrReadLine(out line))
                    return;
                try
                {
                    vehicle.SetNrOfWheels(line);
                }
                catch (ArgumentException ex)
                {
                    _ui.Write($"{ex.Message}\n");
                    continue;
                }
                _ui.Write($"The number of wheels have been set to: \"{vehicle.NumberOfWheels}\"\n");
                break;
            }

            _ui.Write("\nThe following vehicle information will be used when checking in the vehicle to the garage:\n"
                + $"   Registration Number: {vehicle.RegistrationNr}\n"
                + $"   Color:               {vehicle.Color}\n"
                + $"   Number of wheels:    {vehicle.NumberOfWheels}\n");

            _handler.CheckInVehicle(vehicle);
            _ui.PressAnyKeyToContinue();
        }

        private void CreateGarage()
        {
            string headLine = "*********************\n"
                            + "* Create new garage *\n"
                            + "*********************\n";
            _ui.WriteHeadLine(headLine);

            if (_handler.GarageExists)
                _ui.Write($"Existing garage with a capacity of {_handler.GarageCapacity} vehicles will be deleted whe the new garage is created.\n");

            int vehicleCapacity = 0;
            do
            {
                _ui.WriteMenuOptions("Enter the vehicle capacity for the garage (<ESC> for main menu): ");

                string line;
                if (_ui.EscapeOrReadLine(out line))
                    return;

                if (!Int32.TryParse(line, out vehicleCapacity) || (vehicleCapacity == 0))
                {
                    _ui.Write($"Invalid vehicle capacity: \"{line}\"\n");
                }
                else
                    break;
            } while (true);

            _ui.Write($"Creating a garage with a capacity of {vehicleCapacity} vehicles.\n");
            _handler.CreateGarage(vehicleCapacity);
            _ui.PressAnyKeyToContinue();
        }
    }
}