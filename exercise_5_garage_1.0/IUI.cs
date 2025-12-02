
namespace exercise_5_garage_1._0
{
    internal interface IUI
    {
        bool EscapeOrGetDigitStr(out int digit);
        bool EscapeOrReadLine(out string line);
        ConsoleKey GetKey();
        void ListConsoleColors(string prompt = "Listing available console colors and their names:");
        void PressAnyKeyToContinue(string message = "\nPress any key to continue: ");
        void Write(string str);
        void WriteHeadLine(string headLine);
        void WriteMenuOptions(string menuOptions);
    }
}