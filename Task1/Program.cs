using System;
using System.Collections.Generic;
using System.Linq;

namespace BukovynaAirport
{
    public interface ICargoComponent
    {
        string Name { get; }
        double GetWeight();
        int GetItemCount();
        void Display(int indentLevel = 0);
    }

    public class CargoItem : ICargoComponent
    {
        public string Name { get; }
        private readonly double _weight;

        public CargoItem(string name, double weight)
        {
            Name = name;
            _weight = weight;
        }

        public double GetWeight() => _weight;

        public int GetItemCount() => 1;

        public void Display(int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}- {Name}: {GetWeight():F1} кг (1 місце)");
        }
    }

    public class CargoComposite : ICargoComponent
    {
        public string Name { get; }
        private readonly List<ICargoComponent> _children = new List<ICargoComponent>();

        public CargoComposite(string name)
        {
            Name = name;
        }

        public void Add(ICargoComponent component)
        {
            _children.Add(component);
        }

        public void Remove(ICargoComponent component)
        {
            _children.Remove(component);
        }

        public double GetWeight() => _children.Sum(child => child.GetWeight());

        public int GetItemCount() => _children.Sum(child => child.GetItemCount());

        public void Display(int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}+ [{Name}] - Вага: {GetWeight():F1} кг | Місць: {GetItemCount()}");
            
            foreach (var child in _children)
            {
                child.Display(indentLevel + 1);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var suitcase1 = new CargoItem("Валіза Samsonite", 18.5);
            var suitcase2 = new CargoItem("Валіза American Tourister", 22.0);

            var pallet1 = new CargoComposite("Палета №1 (Електроніка)");
            pallet1.Add(new CargoItem("Посилка з ноутбуками", 12.3));
            pallet1.Add(new CargoItem("Посилка з планшетами", 6.7));

            var container1 = new CargoComposite("Контейнер AKE-101");
            container1.Add(suitcase1);
            container1.Add(suitcase2);
            container1.Add(pallet1);

            var container2 = new CargoComposite("Контейнер AKE-102");
            container2.Add(new CargoItem("Валіза дипломатична", 9.0));
            container2.Add(new CargoItem("Посилка з одягом", 14.2));

            var section1 = new CargoComposite("Багажний відсік №1 (Носовий)");
            section1.Add(container1);
            section1.Add(container2);

            var container3 = new CargoComposite("Контейнер DPE-201");
            container3.Add(new CargoItem("Великогабаритна валіза", 28.0));
            container3.Add(new CargoItem("Посилка експрес", 4.5));

            var section2 = new CargoComposite("Багажний відсік №2 (Хвостовий)");
            section2.Add(container3);

            var airplane = new CargoComposite("Літак Airbus A320");
            airplane.Add(section1);
            airplane.Add(section2);

            Console.WriteLine("--- ДЕРЕВО ВАНТАЖУ ЛІТАКА ---\n");
            airplane.Display();

            Console.WriteLine("\n--- РОЗРАХУНОК ВАГИ ДЛЯ ВУЗЛІВ ---\n");
            
            PrintCargoInfo(airplane);
            PrintCargoInfo(section1);
            PrintCargoInfo(container1);
            PrintCargoInfo(pallet1);
        }

        static void PrintCargoInfo(ICargoComponent component)
        {
            Console.WriteLine($"Вузол: \"{component.Name}\" | Загальна вага: {component.GetWeight():F1} кг | Всього місць: {component.GetItemCount()}");
        }
    }
}