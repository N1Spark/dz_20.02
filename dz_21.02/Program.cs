using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace dz_21._02
{
    class Program
    {
        public class Credit_Card
        {
            public delegate void Events();
            public string Name { get; set; }
            public string Num { get; set; }
            public DateTime Term { get; set; }
            public string Pin { get; set; }
            public double Credit { get; set; }
            public double Balance { get; private set; }

            public event Events Add;
            public event Events Draw;
            public event Events UseCredit;
            public event Events Target;
            public event Events PinChange;
            public Credit_Card(string name, string num, DateTime term, double credit)
            {
                Name = name;
                Num = num;
                Term = term;
                Credit = credit;
                Balance = 0;
            }
            public void Init()
            {
                Console.Write("Номер карты: ");
                Num = Console.ReadLine();
                Console.Write("ФИО:");
                Name = Console.ReadLine();
                Console.Write("Срок карты: ");
                string date = Console.ReadLine();
                Term.ToString(date);
                Console.Write("PIN: ");
                Pin = Console.ReadLine();
                Console.Write("Кредитный лимит: ");
                Credit = Convert.ToDouble(Console.ReadLine());
                Console.Write("Баланс: ");
                Balance = Convert.ToDouble(Console.ReadLine());
            }
            public void Repl()
            {
                Console.Write("Введите сумму для пополнения счета: ");
                double sum = Convert.ToDouble(Console.ReadLine());
                Balance += sum;
                Console.WriteLine($"Ваш текущий баланс: {Balance}");
                Add?.Invoke();
            }
            public void Pull()
            {
                Console.Write("Введите сколько денег вы хотите снять: ");
                double buf = Convert.ToDouble(Console.ReadLine());
                if (Balance < buf || Balance == 0)
                {
                    Console.WriteLine($"Недостаточно денег");
                    Console.Write("Введите сумму которую вы хотите снять с кредитного лимита: ");
                    double sum = Convert.ToDouble(Console.ReadLine());
                    if (Credit == 0 && Credit < sum)
                    {
                        Console.WriteLine($"Вы потратили весь кредитный лимит");
                        return;
                    }
                    Credit -= sum;
                }
                else
                {
                    Balance -= buf;
                    Console.WriteLine($"Ваш текущий баланс = {Balance}");
                    Draw?.Invoke();
                }
                if (Balance < 0)
                {
                    Console.WriteLine($"Вы использовали кредитные деньги = {Balance * -1}.");
                    UseCredit?.Invoke();
                }
            }
            public void Targ()
            {
                Console.Write("Введите сумму вашей цели: ");
                double targ = Convert.ToDouble(Console.ReadLine());

                if (Balance == targ || Balance > targ)
                {
                    Console.WriteLine($"Вы достигли цели\nЦель: {targ}\nВаш баланс: {Balance}");
                    return;
                }
                Console.WriteLine($"Вы не достигли цели\nЦель: {targ}\nВаш баланс: {Balance}");
            }
            public void ChangePin()
            {
                Console.WriteLine($"Ваш текущий пароль: {Pin}");
                Console.Write($"Ведите новый пароль: ");
                Pin = Console.ReadLine();
            }

        }
        static void Main(string[] args)
        {
            Credit_Card card = new Credit_Card("Исрафил", "4444 1111 3333 5555", DateTime.Now.AddYears(2), 2000);
            card.Add += Added;
            card.Draw += OnFundsSpent;
            card.UseCredit += UsedCredit;
            card.Target += TargetReached;
            card.PinChange += PinChanged;
            while (true)
            {
                Console.WriteLine("1. Пополнить счет\n2. Снять деньги\n3. Проверить баланс\n4. Изменить PIN\n5. Выход");
                Console.Write("Выберите действие: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        card.Repl();
                        break;
                    case 2:
                        card.Pull();
                        break;
                    case 3:
                        card.Targ();
                        break;
                    case 4:
                        card.ChangePin();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("\nНеверное значение");
                        Thread.Sleep(3000);
                        Console.Clear();
                        break;
                }
            }
        }
        static void Added() => Console.WriteLine("\nСчет пополнен\n");
        static void OnFundsSpent() => Console.WriteLine("\nДеньги списаны с вашего счета\n");
        static void UsedCredit() => Console.WriteLine("\nВы использовали весь кредитный лимит\n");
        static void TargetReached() => Console.WriteLine("\nВы достигли цели\n");
        static void PinChanged() => Console.WriteLine("\nPIN был изменен\n");
    }
}
