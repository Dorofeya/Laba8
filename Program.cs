using System.Collections;

namespace Anagrams
{
    class Program
    {
        static void Main()
        {
            string[] array = new string[] { "code", "doce", "ecod", "framer", "frame" };
            string[] changedArray = DeleteArray(array);
            foreach (string s in changedArray)
                Console.WriteLine(s);

        }

        private static string[] DeleteArray(string[] array)
        {
            List<string> list = new()
            {
                array[0]
            };

            for (int i = 1; i < array.Length; i++)
            {
                if (!array[i].IsAnagramTo(array[i - 1])) list.Add(array[i]);
            }

            list.Sort();
            return list.ToArray();
        }

    }
    public static class StringExtension
    {
        public static bool IsAnagramTo(this string word1, string word2)
        {
            bool result = true;
            char[] chars1 = word1.ToCharArray();
            char[] chars2 = word2.ToCharArray();
            AlphabetIndexComparer comparer = new();
            chars1.Sort(comparer);
            chars2.Sort(comparer);
            if (chars1.Length != chars2.Length)
                return false;
            else
            {
                for (int i = 0; i < chars1.Length; i++)
                {
                    if (chars1[i] != chars2[i])
                        result = false;
                }

            }

            return result;
        }

    }
    public static class ArrayExtensions
    {
        public static void Sort(this Array array, IComparer comparer)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object element1 = array.GetValue(j - 1);
                    object element2 = array.GetValue(j);
                    if (comparer.Compare(element1, element2) > 0)
                    {
                        object temporary = array.GetValue(j);
                        array.SetValue(array.GetValue(j - 1), j);
                        array.SetValue(temporary, j - 1);
                    }

                }

            }

        }

    }
    class AlphabetIndexComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            char c1 = (char)x;
            char c2 = (char)y;
            return c1.CompareTo(c2);
        }

    }

}
