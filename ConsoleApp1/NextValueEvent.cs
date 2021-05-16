using System;

namespace ConsoleApp1
{
    public class NextValueEvent
    {
        public event Action<int> Event = i => { Console.WriteLine($"NextValue: {i}"); };

        public NextValueEvent()
        {
            Event.Invoke(2);
        }
    }
}