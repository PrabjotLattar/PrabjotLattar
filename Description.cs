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
    /// Create Description table
    /// saves the Title/description of the employee
    /// Like Manager, facalty etc
    /// </summary>
    [Table("Description")]
    public class Description
    {
        [Key]
        public int DescriptionId { get; set; }
        /// <summary>
        /// create Title column 
        /// is set as required 
        /// max length 60
        /// This table gets pre loaded with the pre defined titles 
        /// This table loads the title dropdown box on client web
        /// </summary>
        [Required]
        [MaxLength(60, ErrorMessage = "Cannot be greater than 60 characters")]
        [Column(TypeName = "varchar")]
        public string Title { get; set; }
        //setting One Description to many Employee relationship
        public virtual ICollection<Employee> Employees { get; set; }

    }
}
