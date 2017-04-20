using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDbModel
{/// <summary>
/// Creats the table named Message in the Db
/// Using the Code first approch
/// Setting message Id as int and required
/// EmployeeMessage Column is required and VARCHAR with max length of 250
/// </summary>
    [Table("Message")]
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Cannot be greater than 60 characters")]
        [Column(TypeName = "varchar")]
        public string EmployeeMessage { get; set; }

        //setting up many relationship with WorkHour table 
        public virtual ICollection<Workhour> Workhours { get; set; }
    }
}
