namespace GlobalVariables
{
    public class GlobalVariables
    {
        private static void Main(string[] args)
        {
            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }
    }
}

public static class GLOBAL_VARIABLES
{
    public static int Wartosc1 { get; set; } = 1;
    public static short Wartosc2 { get; set; } = 10;
    public static byte[] Wartosc3 { get; set; }
    public static uint Wartosc4 { get; set; }
    public static System.TimeSpan Wartosc5 { get; set; }

    public static void PrintGlobalVariables()
    {
        var type = typeof(GLOBAL_VARIABLES);
        var properties = type.GetProperties(
            System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.Public);

        System.Console.WriteLine("---------------- BEGIN " +
            "GLOBAL_VARIABLES ----------------\n");

        foreach (System.Reflection.PropertyInfo property 
            in properties)
        {
            System.Console.WriteLine("{0} = {1}", property.Name, 
                property.GetValue(null).ToString());
        }

        System.Console.WriteLine("\n---------------- END " +
            "GLOBAL_VARIABLES ----------------\n\n");
    }
}