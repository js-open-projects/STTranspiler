namespace TEST3
{
    public class TEST3
    {
        private static void Main(string[] args)
        {
            GLOBAL_VARIABLES.WYNIK = 1 + GLOBAL_VARIABLES.WARTOSC1 * 2 - GLOBAL_VARIABLES.WARTOSC2 / (1 + 3 * (2 + System.Math.Pow(3, 4))) + (123 % 50 * (4 + 12.5) / 4) + (23 / 5 / 6);

            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }
    }
}

public static class GLOBAL_VARIABLES
{
    public static double WYNIK { get; set; }
    public static double WARTOSC1 { get; set; } = 14;
    public static double WARTOSC2 { get; set; } = 1;

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