namespace ST__invalid_input_2
{
    public class ST__invalid_input_2
    {
        private static void Main(string[] args)
        {
            int Wartosc1 = 1;
            int Wartosc2 = 10;
            int Wartosc3;

            /*
                Cannot convert:
                     Wartosc3:=Wartosc1MODWartosc2;
            */

            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }
    }
}

public static class GLOBAL_VARIABLES
{
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