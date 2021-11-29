using PromptCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Wol.Prompts
{
    internal class Device
    {
        /// <summary>
        /// Data model to add a device.
        /// </summary>
        internal class Add
        {
            [Input("ID (of the account that will hold the device)")]
            public string IdWwUser { get; set; }

            [Input("Name (of the device, can be whatever you want)")]
            public string Name { get; set; }

            [Select(typeof(string), "Type (of the device)", "Linux", "MacOS", "Windows", "Other")]
            public string Type { get; set; }

            [Select(typeof(string), "Control (if the control is enabled)", "False", "True")]
            public string Control { get; set; }

            [Input("Interface (Ethernet interface name)")]
            public string Interface { get; set; }

            [Input("MAC (Mac address of the Ethernet device port)")]
            public string MAC { get; set; }

            [Input("Remote type (software you use for remote access)")]
            public string RemoteType { get; set; }

            [Input("Remote login")]
            public string RemoteLogin { get; set; }

            [Input("Remote password (I swear they are encrypted!)")]
            public string RemotePassword { get; set; }
        }

        /// <summary>
        /// Data model to add a device.
        /// </summary>
        internal class Delete
        {
            [Input("ID (of the account to delete)")]
            public string Id { get; set; }
        }
    }
}
