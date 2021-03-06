using OmarFirstTask;
using OmarFirstTask.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cli {
    internal static class Helper {
        // ASSUMING IT COULDN'T BE 2 OPTIONS STARTING WITH THE SAME LETTER, I.E.
        // --help --home
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="optionLetter"></param>
        /// <param name="default">Value to return if option is not found.</param>
        /// <returns></returns>
        internal static string ValueOfOption(this string[] args, char optionLetter, string @default = default) {
            for (uint i = 0; i < args.Length; i++) {
                if (IsOption(args[i], optionLetter)) {// option found
                    if (i + 1 != args.Length) {
                        return args[i + 1];
                    }
                    throw new InvalidOptionOrValueException();// no provided value
                }
            }
            return @default;
        }

        internal static bool ExistsOption(this string[] args, char optionLetter) {
            return args.Any(a => IsOption(a, optionLetter));
        }

        private static bool IsOption(string arg, char optionLetter) {
            var regex = new Regex(@"\-?\-" + optionLetter);
            var match = regex.Match(arg);

            return match.Success && match.Index == 0;
        }

        internal static long ToLong(this string str) {
            if (long.TryParse(str, out var res)) {
                return res;
            }
            throw new InvalidOptionOrValueException();
        }

        internal static uint ToUint(this string str) {
            if (uint.TryParse(str, out var res)) {
                return res;
            }
            throw new InvalidOptionOrValueException();
        }

        internal static uint NotZero(this uint n) {
            if (n != 0) {
                return n;
            }
            throw new InvalidOptionOrValueException();
        }

        internal static IList<Type> ToFinalCommandTypes(this string finalCommandChars) {
            return finalCommandChars
                .Select(c => c switch {
                    'c' => typeof(SwapClients),
                    'b' => typeof(InsertClient),
                    'm' => typeof(InsertClientRndm),
                    'u' => typeof(InsertSubrouteRndm),
                    _ => throw new InvalidOptionOrValueException()
                })
                .ToList();
        }
    }
}
