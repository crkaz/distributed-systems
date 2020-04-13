using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACWClient
{
    class TalkbackRequest : Request
    {
        public static void Hello()
        {
            string endpoint = "TalkBack/Hello";
            GetEndpoint(endpoint);
        }

        public static void Sort(string args)
        {
            string endpoint = "TalkBack/Sort?integers=";

            try
            {
                if (args.Length < 3) // Array created with a minimum of 3 chars "[_]".
                {
                    throw new Exception();
                }

                args = args.Substring(1, args.Length - 2); // Remove square brackets.
                string[] parsed = args.Split(',');
                int i = 0;
                foreach (string integer in parsed)
                {
                    int.Parse(integer); // Will catch non integer args.

                    if (i++ == 0)
                    {
                        endpoint += integer;
                    }
                    else
                    {
                        endpoint += "&integers=" + integer;
                    }
                }
                GetEndpoint(endpoint);
            }
            catch
            {
                Console.WriteLine("Invalid arguments.");
            }
        }
    }
}
