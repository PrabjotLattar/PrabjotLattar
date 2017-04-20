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
    /// Creating Campus table
    /// </summary>
    [Table("Campus")]
    public class Campus
    {
        [Key]
        public int CampusId { get; set; }
        /// <summary>
        /// Creating Name Column 
        /// set as required, max length 50
        /// Gets preloaded with the name of all campuses
        /// campus dropsown list box gets the value from this table
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 40 characters")]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        //setting One Campus to Many Employees Relationship
        public virtual ICollection<Employee> Employees { get; set; }

    }
}
