using System;

namespace BtoFanControl
{
    public class ConsoleLogger : ILogger
    {
        private int _currentWindowWidth;
        private int CurrentWindowWidth
        {
            get => _currentWindowWidth;
            set
            {
                if (_currentWindowWidth != value)
                {
                    _currentWindowWidth = value;
                    WindowWidthChanged();
                }
            }
        }

        private void WindowWidthChanged()
        {
            Console.Clear();
            Console.CursorVisible = false;
        }

        public void Information(string messageTemplate)
        {
            SetConsoleWindowAndCursor();

            foreach (var line in messageTemplate.Split('\n'))
            {
                Console.Write(line.PadRight(CurrentWindowWidth));
            }
        }

        private void SetConsoleWindowAndCursor()
        {
            CurrentWindowWidth = Console.WindowWidth;
            Console.SetCursorPosition(0, 0);
        }
    }
}
