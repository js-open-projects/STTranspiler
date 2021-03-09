namespace ST__invalid_input
{
    public class ST__invalid_input
    {
        private static void Main(string[] args)
        {
            int Wartosc1 = 1;

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     ANALOG_SIGNAL_RANGE:
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     (
            */

            /*
                Cannot convert:
                     BIPOLAR_10V,
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     UNIPOLAR_10V,
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     UNIPOLAR_1_5V,
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     UNIPOLAR_0_5V,
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     UNIPOLAR_4_20_MA,
            */

            /*
                Cannot convert:

            */

            /*
                Cannot convert:
                     UNIPOLAR_0_20_MA)
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