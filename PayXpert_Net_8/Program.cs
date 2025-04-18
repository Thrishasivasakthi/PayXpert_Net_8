
using PayXpert.Main;
using System;
using System.Text;

namespace PayXpert_Net_8
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                MainModule mainModule = new MainModule();
                mainModule.Run();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine("Please ensure the dbconfig.txt file exists with proper database connection settings.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}