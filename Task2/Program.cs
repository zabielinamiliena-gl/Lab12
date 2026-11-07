using System;
using System.Collections.Generic;

namespace BukovynaAirportSeats
{
    public class SeatClassFlyweight
    {
        public string ClassName { get; }
        public double SeatWidth { get; }
        public double SeatPitch { get; }
        public double ReclineAngle { get; }
        public List<string> Amenities { get; }
        public string ImagePath { get; }

        public SeatClassFlyweight(string className, double width, double pitch, double recline, List<string> amenities, string imagePath)
        {
            ClassName = className;
            SeatWidth = width;
            SeatPitch = pitch;
            ReclineAngle = recline;
            Amenities = amenities;
            ImagePath = imagePath;
        }

        public void DisplayInfo(int row, char letter, string passenger)
        {
            string passInfo = string.IsNullOrEmpty(passenger) ? "Вільне" : passenger;
            Console.WriteLine($"Місце {row}{letter} [{ClassName}] | Пасажир: {passInfo} | Ширина: {SeatWidth}см | Крок: {SeatPitch}см | Нахил: {ReclineAngle}°");
        }
    }

    public class SeatClassFactory
    {
        private readonly Dictionary<string, SeatClassFlyweight> _flyweights = new Dictionary<string, SeatClassFlyweight>();

        public SeatClassFlyweight GetSeatClass(string className)
        {
            if (!_flyweights.ContainsKey(className))
            {
                switch (className)
                {
                    case "Business":
                        _flyweights[className] = new SeatClassFlyweight(
                            "Бізнес-клас",
                            55.0,
                            100.0,
                            160.0,
                            new List<string> { "Wi-Fi", "Преміум харчування", "Шумопоглинаючі навушники" },
                            "business_seat.jpg"
                        );
                        break;
                    case "Economy":
                        _flyweights[className] = new SeatClassFlyweight(
                            "Економ-клас",
                            43.0,
                            78.0,
                            105.0,
                            new List<string> { "Напої" },
                            "economy_seat.jpg"
                        );
                        break;
                    case "First":
                        _flyweights[className] = new SeatClassFlyweight(
                            "Перший клас",
                            65.0,
                            150.0,
                            180.0,
                            new List<string> { "Wi-Fi", "Спальне місце", "Окреме меню" },
                            "first_seat.jpg"
                        );
                        break;
                }
            }

            return _flyweights[className];
        }

        public int CreatedFlyweightsCount => _flyweights.Count;
    }

    public class Seat
    {
        public int Row { get; }
        public char Letter { get; }
        public string Passenger { get; set; }
        public SeatClassFlyweight SharedData { get; }

        public Seat(int row, char letter, SeatClassFlyweight sharedData, string passenger = null)
        {
            Row = row;
            Letter = letter;
            SharedData = sharedData;
            Passenger = passenger;
        }

        public void Display()
        {
            SharedData.DisplayInfo(Row, Letter, Passenger);
        }
    }

    public class Flight
    {
        public string FlightNumber { get; }
        public List<Seat> Seats { get; } = new List<Seat>();

        public Flight(string flightNumber)
        {
            FlightNumber = flightNumber;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var factory = new SeatClassFactory();
            var flights = new List<Flight>();
            char[] seatLetters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
            int totalSeatsCount = 0;

            for (int i = 1; i <= 20; i++)
            {
                var flight = new Flight($"PS-{100 + i}");

                for (int row = 1; row <= 30; row++)
                {
                    string classType = row <= 3 ? "Business" : "Economy";
                    var sharedData = factory.GetSeatClass(classType);

                    foreach (char letter in seatLetters)
                    {
                        var seat = new Seat(row, letter, sharedData);
                        flight.Seats.Add(seat);
                        totalSeatsCount++;
                    }
                }

                flights.Add(flight);
            }

            Console.WriteLine("--- СТАТИСТИКА ВИКОРИСТАННЯ ПАМ'ЯТІ ---\n");
            Console.WriteLine($"Згенеровано рейсів: {flights.Count}");
            Console.WriteLine($"Загальна кількість об'єктів місць: {totalSeatsCount}");
            Console.WriteLine($"Кількість створених об'єктів спільних даних (Flyweight): {factory.CreatedFlyweightsCount}\n");

            Console.WriteLine("--- ПЕРЕВІРКА ВУЗЛІВ ТА ПОСИЛАНЬ НА ОБ'ЄКТ ---\n");
            
            var seatFlight1 = flights[0].Seats[0];
            var seatFlight2 = flights[1].Seats[0];

            Console.WriteLine("Рейс №1 (місце 1A):");
            seatFlight1.Display();

            Console.WriteLine("\nРейс №2 (місце 1A):");
            seatFlight2.Display();

            bool isSameReference = object.ReferenceEquals(seatFlight1.SharedData, seatFlight2.SharedData);
            
            Console.WriteLine($"\nЧи посилаються місця різних рейсів на один і той самий об'єкт у пам'яті? {isSameReference}");
            Console.WriteLine($"HashCode спільних даних для Рейсу №1: {seatFlight1.SharedData.GetHashCode()}");
            Console.WriteLine($"HashCode спільних даних для Рейсу №2: {seatFlight2.SharedData.GetHashCode()}");
        }
    }
}