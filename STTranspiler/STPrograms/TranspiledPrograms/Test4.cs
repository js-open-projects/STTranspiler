namespace Test4
{
    public class Test4
    {
        private static void Main(string[] args)
        {
            GLOBAL_VARIABLES.PrintGlobalVariables();
            System.Console.ReadKey();
        }

        public class Klasa1 : Klasa2
        {
            public void Metoda1()
            {
                GLOBAL_VARIABLES.GLOBALNA_SUMA = 
                    System.Math.Pow(
                        GLOBAL_VARIABLES.GLOBALNA1, 
                        GLOBAL_VARIABLES.GLOBALNA2);
            }
        }

        public class Klasa2 : ROOM
        {
        }

        public interface ROOM
        {
            void DAYTIME(); void NIGHTTIME();
        }

        public void SUM_GLOBAL()
        {
            GLOBAL_VARIABLES.GLOBALNA_SUMA = 
                GLOBAL_VARIABLES.GLOBALNA1 + 
                GLOBAL_VARIABLES.GLOBALNA2;
            return;
        }
    }
}

public static class GLOBAL_VARIABLES
{
    public static double GLOBALNA1 { get; set; } = 1;
    public static double GLOBALNA2 { get; set; } = 14;
    public static double GLOBALNA_SUMA { get; set; }

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