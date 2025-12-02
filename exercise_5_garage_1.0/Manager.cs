


using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection;

namespace exercise_5_garage_1._0
{
    internal class Manager
    {

        private IUI _ui;
        private IHandler _handler = new Handler();

        public Manager(IUI ui)
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
                    prompt = $"** Invalid selection. Please try again: ";
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

                prompt = $"** Invalid number of vehicles to add: \"{line}\"\n"
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

            int fieldWidth = (Enum.GetNames(typeof(VehicleEnumType))
                   .OrderByDescending(s => s.Length)
                   .FirstOrDefault() ?? "").Length;

            _ui.Write($"\n");
            foreach (var vehicTp in Enum.GetNames(typeof(VehicleEnumType)))
            {
                IEnumerable<Vehicle>? queryResult = null;
                try
                {
                    if (_handler.Search("VehicleType", vehicTp, ref queryResult))
                        _ui.Write($"{(vehicTp + ":").PadRight(fieldWidth + 1)} {queryResult?.Count()}\n");
                    else
                        _ui.Write($"{(vehicTp + ":").PadRight(fieldWidth + 1)} 0\n");
                }
                catch (Exception ex)
                {
                    _ui.Write($"** Failed to list data for \"{vehicTp}\".\n");
                    _ui.Write($"** {ex.Message}\n");
                }
            }
            _ui.PressAnyKeyToContinue();
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
                _ui.Write($"{(vehicTp + 1)} - {(VehicleEnumType)vehicTp}\n");
            _ui.Write("\n");

            VehicleEnumType vehicleType;
            while (true)
            {
                _ui.WriteMenuOptions("Select the vehicle type from the above list (<ESC> for main menu): ");
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
                _ui.Write($"Selected vehicle type: \"{vehicleType}\"\n");
                break;
            }

            Type theVehicleType = Type.GetType("exercise_5_garage_1._0." + vehicleType.ToString())!;
            if (theVehicleType == null)
            {
                _ui.Write($"** (theVehicleType == null) returning to main menu\n");
                _ui.PressAnyKeyToContinue();
                return;
            }
            var vehicleObject = Activator.CreateInstance(theVehicleType);
            if (vehicleObject == null)
            {
                _ui.Write($"** (vehicleObject == null) returning to main menu\n");
                _ui.PressAnyKeyToContinue();
                return;
            }

            IEnumerable<PropertyInfo> props = vehicleObject.GetType().GetProperties().Where(p => p.CanWrite);
            foreach (PropertyInfo prop in props)
            {
                while (true)
                {
                    _ui.WriteMenuOptions($"Enter {prop.Name} for the {vehicleObject.GetType().Name} (<ESC> for main menu): ");
                    if (_ui.EscapeOrReadLine(out line))
                        return;

                    string errPrompt = $"** Invalid property value: \"{line}\"\n";
                    dynamic? convertedValue = null;
                    try
                    {
                        convertedValue = Convert.ChangeType(line, prop.PropertyType);
                        prop.SetValue(vehicleObject, convertedValue);
                    }
                    catch (TargetInvocationException tie)
                    {
                        // Unwrap the inner exception and handle it...
                        _ui.Write(errPrompt);
                        _ui.Write($"** An error (can never be caught!?) occurred: {tie.Message}\n");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        //TODO 2512010739: Why is it impossible to catch exceptions from the property setters here ????!!!!!
                        // As it seems to be such a pain in the xxx to catch exceptions thrown from the depths of
                        // the prop.SetValue() call it might be neccessary to find an acceptable workaround that could
                        // suffice until a better solution is found...
                        //
                        // An ugly way, just to get the job done, would be to let the property setters assign
                        // values representing an unset state like an empty string, null or perhaps max negative for
                        // ingegers etc. and then readback the value set by prop.SetValue() and see if its the same as the
                        // value set.
                        //
                        // At least exceptions from Convert.ChangeType() are caught here.
                        _ui.Write(errPrompt);
                        _ui.Write($"** {ex.Message}\n");
                        continue;
                    }
                    dynamic? propValue = prop.GetValue(vehicleObject);
                    if (propValue == null)
                        return;

                    if (line.Equals(string.Empty))
                        _ui.Write(errPrompt);
                    else if (((prop.PropertyType == typeof(string)) && (propValue.ToUpper() == convertedValue.ToUpper()))
                        ||   (propValue == convertedValue))
                    {
                        _ui.Write($"{prop.Name} property is set to: \"{prop.GetValue(vehicleObject)}\"\n");
                        break;
                    }
                    else
                        _ui.Write(errPrompt);
                }
            }

            IEnumerable<Vehicle>? queryResult = null;
            try
            {
                _handler.Search("RegistrationNr", ((Vehicle)vehicleObject).RegistrationNr, ref queryResult);
            }
            catch (Exception ex)
            {
                _ui.Write($"** {ex.Message}\n");
            }
            if (queryResult!.Count() > 0)
            {
                _ui.Write($"There already is a vehicle parked with registration number \"{((Vehicle)vehicleObject).RegistrationNr}\".\n");
                ListVehicles(queryResult!);
                _ui.PressAnyKeyToContinue();
                return;
            }

            _ui.Write($"\nThe following vehicle information is used when checking in the {vehicleObject.GetType().Name} to the garage:\n");
            props = vehicleObject.GetType().GetProperties();
            int fieldWidth = 0;
            foreach (PropertyInfo prop in props)
                if (prop.Name.Length > fieldWidth)
                    fieldWidth = prop.Name.Length;
            foreach (PropertyInfo prop in props)
            {
                _ui.Write($"   {(prop.Name + ":").PadRight(fieldWidth + 1)} {prop.GetValue(vehicleObject)}\n");
            }
            _handler.CheckInVehicle((Vehicle)vehicleObject);

            _ui.PressAnyKeyToContinue();
            return;

            //_ui.ListConsoleColors("Select color for the vehicle, available colors are listed below:\n");
            //_ui.Write("\n");
            //while (true)
            //{
            //    _ui.WriteMenuOptions("Pick the closest matching color from the above list (<ESC> for main menu): ");
            //    if (_ui.EscapeOrReadLine(out line))
            //        return;

            //    try
            //    {
            //        vehicle.Color = line;
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        _ui.Write($"{ex.Message}\n");
            //        continue;
            //    }

            //    _ui.Write($"Using vehicle color: \"{vehicle.Color}\"\n");
            //    break;
            //}
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
                    _ui.Write($"** Invalid vehicle capacity: \"{line}\"\n");
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