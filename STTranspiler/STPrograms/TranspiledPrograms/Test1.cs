namespace Test1
{
    public class Test1
    {
        private static void Main(string[] args)
        {
            int Index;
            int Variable1 = 3;
            int Variable2 = 4;
            if (GLOBAL_VARIABLES.WYNIK == 0)
            {
                for (Index = 1; 
                    Index != (-10 > 0 ? -10 + 1 : -10 - 1); 
                    Index = Index + -1)
                {
                    if (Index % 2 == 0)
                    {
                        GLOBAL_VARIABLES.WYNIK = 
                            GLOBAL_VARIABLES.WYNIK + Variable1;
                        GLOBAL_VARIABLES.Variable1Count = 
                            GLOBAL_VARIABLES.Variable1Count + 1;
                    }
                    else
                    {
                        GLOBAL_VARIABLES.WYNIK = 
                            GLOBAL_VARIABLES.WYNIK + Variable2;
                        GLOBAL_VARIABLES.Variable2Count = 
                            GLOBAL_VARIABLES.Variable2Count + 1;
                    }
                }
            }

            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }
    }
}

public static class GLOBAL_VARIABLES
{
    public static int WYNIK { get; set; }
    public static int Variable1Count { get; set; } = 0;
    public static int Variable2Count { get; set; } = 0;

    public static void PrintGlobalVariables()
    {
        var type = typeof(GLOBAL_VARIABLES);
        var properties = type.GetProperties(
            System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.Public);

        System.Console.WriteLine("---------------- BEGIN " +
            "GLOBAL_VARIABLES ----------------\n");

        foreach (System.Reflection.PropertyInfo property in properties)
        {
            System.Console.WriteLine("{0} = {1}", property.Name, 
                property.GetValue(null).ToString());
        }

        System.Console.WriteLine("\n---------------- END " +
            "GLOBAL_VARIABLES ----------------\n\n");
    }
}