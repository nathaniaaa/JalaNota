using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JalaNota
{
    [Table("JenisIkan")]
    public class JenisIkan : BaseModel
    {
        [PrimaryKey("IDIkan")]
        public int IDIkan { get; set; }

        [Column("NamaIkan")]
        public string NamaIkan { get; set; }

        [Column("HargaPerKg")]
        public double HargaPerKg { get; set; } 

        [JsonIgnore] 
        public string HargaFormatted
        {
            get
            {
                return $"Rp {this.HargaPerKg:N0}";
            }
        }
    }
}