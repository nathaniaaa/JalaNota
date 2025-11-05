using Postgrest.Attributes;
using Postgrest.Models;
using System;
using Newtonsoft.Json;

namespace JalaNota
{
    [Table("Setoran")]
    public class Setoran : BaseModel
    {
        [PrimaryKey("IDSetoran", false)]  
        public int IDSetoran { get; set; }

        [Column("IDNelayan")]
        public int IDNelayan { get; set; }

        [Column("IDIkan")]
        public int IDIkan { get; set; }

        [Column("IDAdmin")]
        public int IDAdmin { get; set; }

        [Column("WaktuSetor")]
        public DateTime WaktuSetor { get; set; }

        [Column("BeratKg")]
        public double BeratKg { get; set; }

        [Column("HargaTotal")]
        public double HargaTotal { get; set; }
    }
}