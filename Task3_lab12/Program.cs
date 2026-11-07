using System;

namespace BukovynaAirport
{
    class Journal
    {
        private int minutes;

        public Journal(int startHour, int startMinute)
        {
            minutes = startHour * 60 + startMinute;
        }

        public void Write(string source, string message)
        {
            int hour = minutes / 60;
            int minute = minutes % 60;
            Console.WriteLine("[" + hour.ToString("00") + ":" + minute.ToString("00") + "] [" + source + "] " + message);
            minutes += 1;
        }
    }

    interface IBoardDevice
    {
        string Name { get; }
        void Show(Journal journal, string content);
    }

    class LedPanel : IBoardDevice
    {
        public string Name { get { return "LED-панель"; } }

        public void Show(Journal journal, string content)
        {
            journal.Write(Name, "Великі символи: " + content);
        }
    }

    class LcdScreen : IBoardDevice
    {
        public string Name { get { return "LCD-екран"; } }

        public void Show(Journal journal, string content)
        {
            journal.Write(Name, "Рядки на екрані: " + content);
        }
    }

    class MobileApp : IBoardDevice
    {
        public string Name { get { return "Мобільний застосунок"; } }

        public void Show(Journal journal, string content)
        {
            journal.Write(Name, "Картка у застосунку: " + content);
        }
    }

    class Projector : IBoardDevice
    {
        public string Name { get { return "Проектор"; } }

        public void Show(Journal journal, string content)
        {
            journal.Write(Name, "Зображення на стіні: " + content);
        }
    }

    abstract class FlightBoard
    {
        private IBoardDevice device;

        public FlightBoard(IBoardDevice device)
        {
            this.device = device;
        }

        public void Connect(IBoardDevice device)
        {
            this.device = device;
        }

        public void Show(Journal journal)
        {
            journal.Write(Kind, "Формує вміст: " + Content);
            device.Show(journal, Content);
        }

        protected abstract string Kind { get; }
        protected abstract string Content { get; }
    }

    class DepartureBoard : FlightBoard
    {
        private readonly string flightNumber;
        private readonly string destination;
        private readonly string time;

        public DepartureBoard(string flightNumber, string destination, string time, IBoardDevice device) : base(device)
        {
            this.flightNumber = flightNumber;
            this.destination = destination;
            this.time = time;
        }

        protected override string Kind { get { return "Табло вильоту"; } }
        protected override string Content { get { return flightNumber + ", " + destination + ", " + time; } }
    }

    class ArrivalBoard : FlightBoard
    {
        private readonly string flightNumber;
        private readonly string origin;
        private readonly string time;

        public ArrivalBoard(string flightNumber, string origin, string time, IBoardDevice device) : base(device)
        {
            this.flightNumber = flightNumber;
            this.origin = origin;
            this.time = time;
        }

        protected override string Kind { get { return "Табло прильоту"; } }
        protected override string Content { get { return flightNumber + ", " + origin + ", " + time; } }
    }

    class GateBoard : FlightBoard
    {
        private readonly string flightNumber;
        private readonly string gate;

        public GateBoard(string flightNumber, string gate, IBoardDevice device) : base(device)
        {
            this.flightNumber = flightNumber;
            this.gate = gate;
        }

        protected override string Kind { get { return "Табло виходу"; } }
        protected override string Content { get { return flightNumber + ", вихід " + gate; } }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Situation3();
        }

        static void Situation3()
        {
            Journal journal = new Journal(10, 0);

            DepartureBoard departure = new DepartureBoard("PS-101", "виліт до Києва", "10:30", new LedPanel());
            departure.Show(journal);
            departure.Connect(new LcdScreen());
            departure.Show(journal);

            GateBoard gate = new GateBoard("PS-101", "B12", new MobileApp());
            gate.Show(journal);

            ArrivalBoard arrival = new ArrivalBoard("PS-202", "приліт зі Львова", "11:05", new Projector());
            arrival.Show(journal);
        }
    }
}
