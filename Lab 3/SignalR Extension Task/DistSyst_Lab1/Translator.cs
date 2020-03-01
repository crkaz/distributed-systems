using System;

namespace DistSyst_Lab1
{
    public class Translator : MarshalByRefObject
    {
        private int callCount;

        public Translator()
        {
            callCount = 0;
            Translate("Bepis");
        }

        public string Translate(string EnglishString)
        {
            Console.WriteLine("Received call");

            string[] words = EnglishString.Split(' ');
            string result = "";
            foreach (string word in words)
            {
                result += word.Substring(1);
                result += word.Substring(0, 1) + "ay ";
            }

            callCount++;

            return result;
        }
    }
}
