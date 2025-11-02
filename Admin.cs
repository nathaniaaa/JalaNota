using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    [Table("Admin")]
    public class Admin : BaseModel
    {
        [PrimaryKey("IDAdmin")]
        public int IDAdmin { get; set; }

        [Column("NamaAdmin")]
        public string NamaAdmin { get; set; }

        [Column("UsernameAdmin")]
        public string UsernameAdmin { get; set; }

        [Column("PasswordAdmin")]
        public string PasswordAdmin { get; set; }
    }
}