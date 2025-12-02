using System.Collections;

namespace exercise_5_garage_1._0
{
    internal class Garage<T> : IGarage<T> where T : Vehicle
    {

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
    }
}