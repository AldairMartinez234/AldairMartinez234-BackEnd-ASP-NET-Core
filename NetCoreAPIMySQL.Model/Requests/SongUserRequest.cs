using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreAPIMySQL.Model.Requests
{
   public class SongUserRequest
    {
        public string user_id { get; set; }
        public string song_id { get; set; }
    }
}
