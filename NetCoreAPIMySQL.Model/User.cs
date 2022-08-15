using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NetCoreAPIMySQL.Model
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        //public string Role { get; set; }
    }
}
