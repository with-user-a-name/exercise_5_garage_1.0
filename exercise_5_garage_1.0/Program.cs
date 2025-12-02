namespace exercise_5_garage_1._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUI ui = new UI();
            var manager = new Manager(ui);
            manager.Run();
        }
    }
}
