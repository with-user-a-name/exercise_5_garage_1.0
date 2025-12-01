




namespace exercise_5_garage_1._0
{
    internal class UI
    {
        public UI()
        {
        }

        public ConsoleKey GetKey() => Console.ReadKey(intercept: true).Key;

        public bool EscapeOrGetDigitStr(out int digit)
        {
            // Console.Readkey(), especially when <ESC> is pressed, may result
            // in the first character in the output following the ReadKey is
            // missing. It seems that denying RedaKey to write the read
            // character (intercept:true) fixed this issue.
            digit = Int32.MinValue;
            var keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Write("\n");
                return true;
            }

            //if (keyInfo.Key == ConsoleKey.Enter)
            //    Console.WriteLine();
            //else 
            if ((keyInfo.Key == ConsoleKey.D0)
            || (keyInfo.Key == ConsoleKey.NumPad0))
                digit = 0;
            else if ((keyInfo.Key == ConsoleKey.D1)
            || (keyInfo.Key == ConsoleKey.NumPad1))
                digit = 1;
            else if ((keyInfo.Key == ConsoleKey.D2)
            || (keyInfo.Key == ConsoleKey.NumPad2))
                digit = 2;
            else if ((keyInfo.Key == ConsoleKey.D3)
            || (keyInfo.Key == ConsoleKey.NumPad3))
                digit = 3;
            else if ((keyInfo.Key == ConsoleKey.D4)
            || (keyInfo.Key == ConsoleKey.NumPad4))
                digit = 4;
            else if ((keyInfo.Key == ConsoleKey.D5)
            || (keyInfo.Key == ConsoleKey.NumPad5))
                digit = 5;
            else if ((keyInfo.Key == ConsoleKey.D6)
            || (keyInfo.Key == ConsoleKey.NumPad6))
                digit = 6;
            else if ((keyInfo.Key == ConsoleKey.D7)
            || (keyInfo.Key == ConsoleKey.NumPad7))
                digit = 7;
            else if ((keyInfo.Key == ConsoleKey.D8)
            || (keyInfo.Key == ConsoleKey.NumPad8))
                digit = 8;
            else if ((keyInfo.Key == ConsoleKey.D9)
            || (keyInfo.Key == ConsoleKey.NumPad9))
                digit = 9;
            
            return false;
        }

        /// <summary>
        /// Reads a line from the console and returns it in the line out
        /// parameter. If the first character is <ESC> line will contain the
        /// ConsoleKey string representation of <ESC> ("Escape").
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Returns true if <ESC> was the first character on the line, otherwise false.</returns>
        internal bool EscapeOrReadLine(out string line)
        {
            // "Peek" on first character to se if its <ESC>.
            var firstKeyInfo = Console.ReadKey();
            if (firstKeyInfo.Key == ConsoleKey.Escape)
            {
                line = firstKeyInfo.Key.ToString();
                return true;
            }
            if (firstKeyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                line = "";
            }
            else
                line = firstKeyInfo.KeyChar.ToString() + Console.ReadLine();
            return false;
        }

        internal void ListConsoleColors(string prompt = "Listing available console colors and their names:")
        {
            // To enter the block character '█' in win console:
            //    1. press <alt>
            //    2. enter the number 219
            //    3. release <alt>
            Console.ResetColor();
            //Console.Clear();
            Console.WriteLine(prompt);

            foreach (int colorNr in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.Write($"   {(colorNr+1),2}: #");
                Console.ForegroundColor = (ConsoleColor)colorNr;
                Console.Write($"████");
                Console.ResetColor();
                Console.WriteLine($"# - {(ConsoleColor)colorNr}");
            }
            Console.ResetColor();
            //PressAnyKeyToContinue("\nPress any key to return: ");
        }

        internal void PressAnyKeyToContinue(string message = "\nPress any key to continue: ")
        {
            Write(message);
            GetKey();
            Write("\n");
        }

        internal void Write(string str)
        {
            Console.Write(str);
        }

        internal void WriteHeadLine(string headLine)
        {
            Console.ResetColor();
            Console.Clear();
            Console.Write(headLine);
        }

        //internal void Clear()
        //{
        //    Console.Clear();
        //}

        internal void WriteMenuOptions(string menuOptions)
        {
            Console.Write(menuOptions);
        }
    }
}