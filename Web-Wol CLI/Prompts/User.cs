using PromptCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Wol.Prompts
{
    internal class User
    {
        /// <summary>
        /// Data model to add a user.
        /// </summary>
        internal class Add
        {
            [Input("Email (at least a true email)")]
            public string Email { get; set; }

            [Input("Password (must be secure, it's up to you)")]
            public string Password { get; set; }

            [Input("Name (of the account, can be whatever you want)")]
            public string Name { get; set; }
        }

        /// <summary>
        /// Data model to delete a user.
        /// </summary>
        internal class Delete
        {
            [Input("ID (of the account to delete)")]
            public string Id { get; set; }
        }
    }
}
