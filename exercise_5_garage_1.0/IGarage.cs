
namespace exercise_5_garage_1._0
{
    internal interface IGarage<T> : IEnumerable<T> where T : Vehicle
    {
        T this[int ix] { get; set; }
    }
}