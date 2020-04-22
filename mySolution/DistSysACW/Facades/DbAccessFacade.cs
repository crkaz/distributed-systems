using Microsoft.EntityFrameworkCore;
using System;

namespace DistSysACW.Facades
{
    public class DbAccessFacade
    {
        const int RETRIES = 3;

        /// <summary>
        /// Wraps database modification code in try-catch statements to monitor concurrent changes or other mishaps.
        /// </summary>
        /// <param name="action">Database modification to monitor with DbUpdateConcurrencyException</param>
        /// <returns></returns>
        public static bool ModifyDb(Action action)
        {
            try
            {
                for (int i = 0; i < RETRIES; i++)
                {
                    try
                    {
                        action.Invoke();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("Failed to fulfil action: database was modified by somebody else");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
            return false;
        }
    }
}
