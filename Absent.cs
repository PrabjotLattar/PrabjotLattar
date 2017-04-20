using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDbModel
{
    /// <summary>
    /// Creating Absent table
    /// </summary>
    [Table ("Absent")]
    public class Absent
    {
        [Key]
        public int AbsentId { get; set; }

        /// <summary>
        /// Cleating Name Column in Absent table
        /// This table gets preloaded with the value
        /// Values like Sick Vacation Other
        /// Absent reason drop down list box gets the value from this Table
        /// </summary>
        [MaxLength(40, ErrorMessage = "Cannot be greater than 40 characters")]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        //Setting One Absent to Many Workhours relationship
        public virtual ICollection<Workhour> Workhours { get; set; }

    }
}
