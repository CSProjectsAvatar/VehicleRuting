using OmarFirstTask;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static OmarFirstTask.Ivns;

namespace Cli {
    class Program {
        static void Main(string[] args) {
            var showHelp = args.Length==0 || args.ExistsOption('h');
            var cliName = "ivns";
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
                    var times = args
                        .ValueOfOption('x', "1")
                        .ToUint()
                        .NotZero();
                    var initNbhStr = args.ValueOfOption('n');

                    var initNbh = initNbhStr?.ToFinalCommandTypes();

                    //Console.WriteLine($"Usando {StructureInfo(@struct)}...");
                    Console.WriteLine($"Configurado un máximo total de {totalTime} segundos.");
                    Console.WriteLine(
                        $"Configurado un máximo de {optTime} segundos de optimización por " +
                        $"vecindad.");
                    Console.WriteLine($"Se ejecutarán {times} corridas.");

                    var runner = new Ivns(totalTime, optTime);
                    runner.FileName = args[0];

                    var runOut = new RunOutcome();
                    /// Mio @audit cargando una solucion
                    // runner.Run(continueFromFile: true);

                    if (initNbh != default) {
                        Console.WriteLine($"Analizando solamente la vecindad de comandos finales: {initNbhStr}.\n");
                        Console.WriteLine("Comienzo del algoritmo...\n");
                        runOut = runner.Run(initNbh, times);
                    } else {
                        Console.WriteLine();
                        runOut = runner.Run(times);
                    }

                    if (times > 1) {
                        Console.Write(
                            $"Resultados de las {times} corridas:\n" +
                            $"> Solución mínima: {runOut.MinSolution}\n" +
                            $"> Solución máxima: {runOut.MaxSolution}\n" +
                            $"> Solución promedio: {runOut.MeanSolution}\n" +
                            $"> Soluciones: {string.Join(", ", runOut.Solutions)}\n");
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
                    $"-t, --total-time{im} Máxima cantidad de segundos que debe consumir una corrida " +
                        "del algoritmo. Valor por defecto: 60.\n" +
                    $"-o, --optimization-time{im} Máxima cantidad de segundos que debe consumir " +
                        "la optimización de una solución en una vecindad dada. Valor por defecto: 30.\n" +
                    $"-n, --neighborhood{im} Analizar solamente la vecindad compuesta por los comandos " +
                        "finales dados. Estos son:\n" +
                        "\tc\t intercambiar clientes,\n" +
                        "\tb\t insertar cliente,\n\n" +
                        "\tm\t insertar cliente random,\n" +
                        "\tu\t insertar subruta random.\n\n" +
                        "\tEjemplo de uso:\n" +
                        $"\t{cliName} A-n64-k9.vrp -n ccb\n" +
                    $"-x, --x-times{im}\t Número de veces a ejecutar el algoritmo. Valor por defecto: 1."
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
