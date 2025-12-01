namespace exercise_5_garage_1._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ui = new UI();
            var manager = new Manager(ui);
            manager.Run();
            Console.WriteLine("Goodbye, World!");
        }
    }
}
