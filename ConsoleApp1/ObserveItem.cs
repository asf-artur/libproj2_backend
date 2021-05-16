using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class ObserveItem
    {
        public int X { get; set; }
    }

    public class Observable : IObservable<ObserveItem>
    {
        public List<IObserver<ObserveItem>> Observers { get; set; }

        public IDisposable Subscribe(IObserver<ObserveItem> observer)
        {
            if (! Observers.Contains(observer))
                Observers.Add(observer);
            return new Unsubscriber(Observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<ObserveItem>>_observers;
            private IObserver<ObserveItem> _observer;

            public Unsubscriber(List<IObserver<ObserveItem>> observers, IObserver<ObserveItem> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in Observers.ToArray())
                if (Observers.Contains(observer))
                    observer.OnCompleted();

            Observers.Clear();
        }
    }
}