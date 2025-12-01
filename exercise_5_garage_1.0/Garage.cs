using System.Collections;

namespace exercise_5_garage_1._0
{
    internal class Garage<T> : IEnumerable<T> where T : Vehicle
    {
        //QN 2511271058: By implementing the IEnumerable of T interface you...
        // get access to the extension methods for IEnumerable in the LINQ
        // Enumerable class.
        // But how exactly does the connection between, in this case, a Garage
        // instance and the array it contains (_vehicles) work as its possible
        // to get the length of the _vehicles array from an instance of garage
        // like: garageobject.Count()
        // What exactly makes the extension method Count() act on the _vehicles
        // array instead of trying on the garage object itself?
        // Is it the implementation of the IEnumerable interface (the
        // GetEnumerator() methods) that makes this automagically (as usual
        // with MS stuff) work?

        private T[] _vehicles;

        public T this[int ix]
        {
            get { return _vehicles[ix]; }
            set { _vehicles[ix] = value; }
        }


        public Garage(int capacity)
        {
            _vehicles = new T[capacity];
        }

        public IEnumerator<T> GetEnumerator()
        {
            //foreach (T item in _vehicles)
            //{
            //    //....
            //    //....
            //    //....
            //    yield return item;
            //}
            return ((IEnumerable<T>)_vehicles).GetEnumerator();
        }

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _vehicles.GetEnumerator();
        }

        //internal int GetGaragePosition(Vehicle vehicle)
        //{
        //    return Array.IndexOf(_vehicles, vehicle);
        //}

    }
}