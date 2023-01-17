using System.Collections;
using System;
using System.Threading;

namespace Bank_Deposit
{
    class Program
    {
        public static void Main()
        {
            string[] userentry = File.ReadAllLines("Laba8Task2.txt");
            FileReader fileReader = new();
            BankAccount account = fileReader.StructureData(userentry);
            Display display = new();
            display.BeginWork(account);

            Console.ReadKey();
        }

    }
    public class FileReader
    {
        public BankAccount StructureData(string[] entry)
        {
            int balance = int.Parse(entry[0]);
            BankOperation[] operations = new BankOperation[entry.Length - 1];
            for (int i = 1; i < entry.Length; i++)
            {
                DateTime dateTime = Time(entry[i].Split(" | ")[0]);
                int moneyAmount = 0;
                string operationType = "";
                if (entry[i].Contains("revert"))
                {
                    moneyAmount = 0;
                    operationType = entry[i].Split(" | ")[1];
                }
                else
                {
                    moneyAmount = int.Parse(entry[i].Split(" | ")[1]);
                    operationType = entry[i].Split(" | ")[2];
                }
                operations[i - 1] = new BankOperation(dateTime, moneyAmount, operationType);
            }
            Sort(operations, new DateTimeComparer());
            return new BankAccount(balance, operations);
        }


        private DateTime Time(string entry)
        {
            string date = entry.Split(' ')[0];
            int year = int.Parse(date.Split('-')[0]);
            int month = int.Parse(date.Split('-')[1]);
            int day = int.Parse(date.Split('-')[2]);
            string time = entry.Split(' ')[1];
            int hours = int.Parse(time.Split(':')[0]);
            int minutes = int.Parse(time.Split(':')[1]);
            DateTime dateTime = new DateTime(year, month, day, hours, minutes, 0);
            return dateTime;
        }

        private static void Sort(Array array, DateTimeComparer comparer)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object element1 = array.GetValue(j - 1);
                    object element2 = array.GetValue(j);
                    if (comparer.Compare(element1, element2) < 0)
                    {
                        object temporary = array.GetValue(j);
                        array.SetValue(array.GetValue(j - 1), j);
                        array.SetValue(temporary, j - 1);
                    }

                }

            }

        }

    }
    class Display
    {
        public void BeginWork(BankAccount account)
        {
            int balance0 = account.Balance;
            if (!account.CheckHistory())
                Console.WriteLine("Извините, данные в Вашей банковской выписке некорректны.");
            else
            {
                Console.Write("Введите время, в момент которого Вы хотите увидеть свой баланс, в формате [YYYY-MM-DD HH:MM:SS]: ");
                string time = Console.ReadLine();
                DateTime dateTime = new DateTime();
                if (time == "")
                {
                    Console.WriteLine($"Ваш баланс на указанный момент времени составляет {account.Balance}");
                    return;
                }

                else
                    dateTime = ParseStringToDateTime(time);

                for (int i = 1; i < account.OperationHistory.Length; i++)
                {
                    bool flag = false;
                    if (dateTime < account.OperationHistory[0].DateTime)
                    {
                        Console.WriteLine($"Ваш баланс на указанный момент времени составляет {account.OperationHistory[0].CurrentBalance - account.OperationHistory[0].MoneyAmount}");
                        flag = true;
                    }
                    else if (dateTime < account.OperationHistory[i].DateTime)
                    {
                        Console.WriteLine($"Ваш баланс на указанный момент времени составляет {account.OperationHistory[i - 1].CurrentBalance}");
                        flag = true;
                    }

                    else if (dateTime > account.OperationHistory[^1].DateTime)
                    {
                        Console.WriteLine($"Ваш баланс на указанный момент времени составляет {account.OperationHistory[^1].CurrentBalance}");
                        flag = true;
                    }

                    if (flag) break;
                }

            }

        }

        private static DateTime ParseStringToDateTime(string entry)
        {
            string date = entry.Split(' ')[0];
            int year = int.Parse(date.Split('-')[0]);
            int month = int.Parse(date.Split('-')[1]);
            int day = int.Parse(date.Split('-')[2]);
            string time = entry.Split(' ')[1];
            int hours = int.Parse(time.Split(':')[0]);
            int minutes = int.Parse(time.Split(':')[1]);
            int seconds = int.Parse(time.Split(':')[2]);
            DateTime dateTime = new DateTime(year, month, day, hours, minutes, seconds);
            return dateTime;
        }

    }
    class DateTimeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            BankOperation op1 = (BankOperation)x;
            BankOperation op2 = (BankOperation)y;
            return -(op1.DateTime.CompareTo(op2.DateTime));
        }

    }
    public class BankOperation
    {
        public DateTime DateTime { get; }
        public int MoneyAmount { get; }
        public string OperationType { get; }
        public int CurrentBalance { get; set; }

        public BankOperation(DateTime time, int moneyAmount, string operationType)
        {
            DateTime = time;
            MoneyAmount = moneyAmount;
            OperationType = operationType;
        }
    }
    public class BankAccount
    {
        public int Balance { get; private set; }
        public BankOperation[] OperationHistory { get; }

        public BankAccount(int balance, BankOperation[] operationHistory)
        {
            Balance = balance;
            OperationHistory = operationHistory;
        }

        public bool CheckHistory()
        {
            for (int i = 0; i < OperationHistory.Length; i++)
            {
                if (OperationHistory[i].OperationType == "in")
                    Balance += OperationHistory[i].MoneyAmount;
                else if (OperationHistory[i].OperationType == "out")
                    Balance -= OperationHistory[i].MoneyAmount;
                else if (OperationHistory[i].OperationType == "revert")
                {
                    if (OperationHistory[i - 1].OperationType == "in")
                        Balance -= OperationHistory[i - 1].MoneyAmount;
                    else if (OperationHistory[i - 1].OperationType == "out")
                        Balance += OperationHistory[i - 1].MoneyAmount;
                }
                OperationHistory[i].CurrentBalance = Balance;
            }

            if (Balance < 0) return false;
            return true;

namespace LabWork8
{
    class Program
    {
        static void Main()
        {
            string[] userlaba8 = File.ReadAllLines("Laba8.txt");
            Subtitles[] subtitles = new Subtitles[userlaba8.Length];
            SubtitleCreator subtitleCreator = new SubtitleCreator();
            for (int i = 0; i < userlaba8.Length; i++)
            {
                subtitles[i] = subtitleCreator.CreateSubtitle(userlaba8[i]);
            }

            SubtitleOutputer display = new SubtitleOutputer(subtitles);
            display.BeignWork();

            Console.ReadLine();
        }

    }
    public class Subtitles
    {
        public int StartTime { get; }
        public int EndTime { get; }
        public string Position { get; }
        public ConsoleColor Color { get; }
        public string Text { get; }

        public Subtitles(int startTime, int endTime, string position, ConsoleColor color, string text)
        {
            StartTime = startTime;
            EndTime = endTime;
            Position = position;
            Color = color;
            Text = text;
        }

    }
    public class SubtitleCreator
    {
        public Subtitles CreateSubtitle(string laba8)
        {
            int st = GetStartTime(laba8);
            int et = GetEndTime(laba8);
            string position = GetPosition(laba8);
            ConsoleColor color = GetColor(laba8);
            string text = GetText(laba8);
            return new Subtitles(st, et, position, color, text);
        }

        private int GetStartTime(string laba8)
        {
            int startTime = int.Parse(laba8.Split(" - ")[0].Split(':')[1]);
            return startTime;
        }

        private int GetEndTime(string laba8)
        {
            int endTime = int.Parse(laba8.Split(" - ")[1].Split(' ')[0].Split(':')[1]);
            return endTime;
        }

        private string GetPosition(string laba8)
        {
            string position = "";
            if (laba8.Contains('['))
                position = laba8.Split('[')[1].Split(',')[0];
            else
                position = "Bottom";
            return position;
        }

        private ConsoleColor GetColor(string laba8)
        {
            ConsoleColor color;
            string subColor = "";
            if (laba8.Contains(']'))
                subColor = laba8.Split(']')[0].Split(", ")[1];
            switch (subColor)
            {
                case "Red":
                    color = ConsoleColor.DarkRed;
                    break;
                case "White":
                    color = ConsoleColor.White;
                    break;
                case "Yellow":
                    color = ConsoleColor.Yellow;
                    break;
                default:
                    color = ConsoleColor.Blue;
                    break;
            }
            return color;
        }

        private string GetText(string laba8)
        {
            string text;
            if (laba8.Contains('['))
                text = laba8.Split("] ")[1];
            else
                text = laba8.Substring(14);
            return text;
        }

    }
    public class SubtitleOutputer
    {
        private static int currentTime;
        private Subtitles[] subtitles;
        public SubtitleOutputer(Subtitles[] subtitles)
        {
            this.subtitles = subtitles;
        }

        public void BeignWork()
        {
            TimerCallback timerCallback = new TimerCallback(Check);
            Timer timer = new Timer(timerCallback, subtitles, 0, 1000);
        }

        private static void Check(object obj)
        {
            Subtitles[] laba8 = (Subtitles[])obj;
            foreach (Subtitles sub in laba8)
            {
                if (sub.StartTime == currentTime) ShowSubtitleOnConsole(sub);
                else if (sub.EndTime == currentTime) DeleteSubtitleFromConsole(sub);
            }

            currentTime++;
        }
        private static void ShowSubtitleOnConsole(Subtitles sub)
        {
            SetPosition(sub);
            Console.ForegroundColor = sub.Color;
            Console.Write(sub.Text);
        }

        private static void DeleteSubtitleFromConsole(Subtitles sub)
        {
            SetPosition(sub);
            for (int i = 0; i < sub.Text.Length; i++)
                Console.Write(" ");
        }

        private static void SetPosition(Subtitles sub)
        {
            switch (sub.Position)
            {
                case "Top":
                    Console.SetCursorPosition((Console.WindowWidth - sub.Text.Length) / 2, 1);
                    break;
                case "Right":
                    Console.SetCursorPosition(Console.WindowWidth - sub.Text.Length, (Console.WindowHeight - 1) / 2);
                    break;
                case "Bottom":
                    Console.SetCursorPosition((Console.WindowWidth - sub.Text.Length) / 2, Console.WindowHeight);
                    break;
                case "Left":
                    Console.SetCursorPosition(0, (Console.WindowHeight - 1) / 2);
                    break;
                default:
                    break;
            }

        }

    }

}

