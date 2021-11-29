using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Wol
{
    /// <summary>
    /// Options for the CLI tool.
    /// </summary>
    internal class Options
    {
        /// <summary>
        /// Path of the database to use
        /// </summary>
        [Value(index: 0, Required = true, HelpText = "Database path to use.")]
        public string DatabasePath { get; set; }

        /// <summary>
        /// Type of the action to perform.
        /// </summary>
        [Value(index: 1, Required = true, HelpText = "Type of the action. Default = user", Default = "user")]
        public string Type { get; set; }

        /// <summary>
        /// Type of the action to perform.
        /// </summary>
        [Option(shortName: 'm', longName: "mode", Required = false, HelpText = "Mode do perform between C (create), R (read), D (delete). Default = R", Default = "R")]
        public string Mode { get; set; }
    }
}
