using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestingCApp;

namespace Cli {
    class Program {
        static void Main(string[] args) {
            var showHelp = args.Length==0 || args.ExistsOption('h');
            var cliName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            var im = "\t";  // intermediate margin

            if (!showHelp) {
                try {
                    //var @struct = args.ValueOfOption('s', "list");
                    var totalTime = args
                        .ValueOfOption('t', "60")
                        .ToLong();
                    var optTime = args
                        .ValueOfOption('o', "30")
                        .ToLong();
                    var initNbhStr = args.ValueOfOption('n');
                    var initNbh = initNbhStr?.ToFinalCommandTypes();

                    //Console.WriteLine($"Usando {StructureInfo(@struct)}...");
                    Console.WriteLine($"Configurado un máximo total de {totalTime} segundos.");
                    Console.WriteLine(
                        $"Configurado un máximo de {optTime} segundos de optimización por " +
                        $"solución.");

                    var runner = new Ivnp(totalTime, optTime);
                    runner.FileName = args[0];

                    if (initNbh != default) {
                        Console.WriteLine($"Analizando solamente la vecindad de comandos finales: {initNbhStr}.\n");
                        Console.WriteLine("Comienzo del algoritmo...\n");
                        runner.Run(initNbh);
                    } else {
                        Console.WriteLine();
                        runner.Run();
                    }

                    Console.WriteLine("\nHecho :)");

                } catch (InvalidOptionOrValueException) {
                    showHelp = true;
                }
            }

            if (showHelp) {
                Console.WriteLine(
                    $"\nUso: {cliName} archivo_del_problema [opciones]\n\n" +
                    "Opciones:\n" +
                    $"-h, --help{im}\t Muestra esta ayuda.\n" +
                    $"-t, --total-time{im} Máxima cantidad de segundos que debe consumir " +
                        "el algoritmo. Valor por defecto: 60.\n" +
                    $"-o, --optimization-time{im} Máxima cantidad de segundos que debe consumir " +
                        "la optimización de una solución. Valor por defecto: 30.\n" +
                    $"-n, --neighborhood{im} Analizar solamente la vecindad compuesta por los comandos " +
                        "finales dados. Estos son:\n" +
                        "\tc\t intercambiar clientes, y\n" +
                        "\tb\t insertar cliente.\n\n" +
                        "\tEjemplo de uso:\n" +
                        $"\t{cliName} A-n64-k9.vrp -n ccb"
                    //$"-s, --structure{im} Estructura de datos a utilizar para almacenar los clientes en el algoritmo. " +
                    //    "Los posibles valores son:\n" +
                    //    "\t list\t para emplear la lista de C# (valor por defecto),\n" +
                    //    "\t avl\t para emplear un AVL, o\n" +
                    //    "\t skip\t para emplear una Skip Lists."
                );
            }

            //while (true) {
            //    var opt = Console.ReadLine()[0];
            //    Console.WriteLine(args.ValueOfOption(opt));
            //    Console.WriteLine(args.ExistsOption(opt));
            //}
        }

        private static string StructureInfo(string @struct) {
            return @struct switch {
                "avl" => "AVL",
                "skip" => "Skip Lists",
                "list" => "lista de C#",
                _ => throw new InvalidOptionOrValueException()
            };
        }
    }
}
