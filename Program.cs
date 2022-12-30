using System;
using System.Threading;

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