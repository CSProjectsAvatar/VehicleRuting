using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli {
    class Program {
        static void Main(string[] args) {
            var showHelp = args.ExistsOption('h');
            var cliName = "ivns";
            var im = "\t";  // intermediate margin

            try {
                var @struct = args.ValueOfOption('s', "list");
                Console.WriteLine($"Usando {StructureInfo(@struct)}...");

            } catch (InvalidOptionOrValueException) {

                showHelp = true;
            }

            if (showHelp) {
                Console.WriteLine(
                    $"\nUso: {cliName} archivo_del_problema [opciones]\n\n" +
                    "Opciones:\n" +
                    $"-h, --help{im} Muestra esta ayuda.\n" +
                    $"-s, --structure{im} Estructura de datos a utilizar para almacenar los clientes en el algoritmo. " +
                        "Los posibles valores son:\n" +
                        "\t list\t para emplear la lista de C# (valor por defecto),\n" +
                        "\t avl\t para emplear un AVL, o\n" +
                        "\t skip\t para emplear una Skip Lists."
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
