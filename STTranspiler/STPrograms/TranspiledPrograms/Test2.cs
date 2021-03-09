namespace Test2
{
    public class Test2
    {
        private static void Main(string[] args)
        {
            int Wartosc1_Test2 = 10;
            switch (Wartosc1_Test2)
            {
                case 1:
                case 2:
                    GLOBAL_VARIABLES.WYNIK_TEST2 = 1 + 2 * 4;

                    break;

                case 3:
                case 4:
                case 5:
                case 6:
                    GLOBAL_VARIABLES.WYNIK_TEST2 = 3 + 4 + 5 + 6;

                    break;

                default:
                    GLOBAL_VARIABLES.WYNIK_TEST2 = 15;

                    break;
            }

            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }
    }
}

public static class GLOBAL_VARIABLES
{
    public static int WYNIK_TEST2 { get; set; }

    public static void PrintGlobalVariables()
    {
        var type = typeof(GLOBAL_VARIABLES);
        var properties = type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        System.Console.WriteLine("---------------- BEGIN GLOBAL_VARIABLES ----------------\n");

        foreach (System.Reflection.PropertyInfo property in properties)
        {
            System.Console.WriteLine("{0} = {1}", property.Name, property.GetValue(null).ToString());
        }

        System.Console.WriteLine("\n---------------- END GLOBAL_VARIABLES ----------------\n\n");
    }
}