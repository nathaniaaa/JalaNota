using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaNota
{
    [Table("Nelayan")]
    public class Nelayan : BaseModel
    {
        [PrimaryKey("IDNelayan")]
        public int IDNelayan { get; set; }

        [Column("NamaNelayan")]
        public string NamaNelayan { get; set; }

        [Column("UsnNelayan")]
        public string UsnNelayan { get; set; }

        [Column("PasswordNelayan")]
        public string PasswordNelayan { get; set; }

    }
}