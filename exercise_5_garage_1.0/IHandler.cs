
namespace exercise_5_garage_1._0
{
    internal interface IHandler
    {
        int GarageCapacity { get; }
        bool GarageExists { get; }

        void AddVehicles(int vehiclesToAdd);
        bool CheckInVehicle(Vehicle vehicle);
        string CheckOutVehicle(Vehicle vehicle);
        void CreateGarage(int vehicleCapacity);
        bool GarageIsFull();
        int GetGaragePosition(Vehicle vehicle);
        IEnumerable<Vehicle> GetParkedVehicles();
        List<string> GetPropNames<T>();
        bool Search(string propertyName, object value, ref IEnumerable<Vehicle>? source);
    }
}