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
    /// Setting Workhour table 
    /// </summary>
    [Table("Workhour")]
    public class Workhour
    {
        /// <summary>
        /// Workhour ID set as required 
        /// Set as type int
        /// </summary>
        public int WorkhourID { get; set; }
        /// <summary>
        /// Hour column is set as required 
        /// saves int value
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        ///Saves if the employee was present
        ///Boolean ture if present and false if absent
        ///value saved is 0 for false and 1 for true
        /// </summary>
        public bool Present { get; set; }
        /// <summary>
        /// Date column to save the date for which the entry is made by the user
        /// </summary>
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }
        /// <summary>
        /// Saves Message id if there was a message left by the user 
        /// Is set as not required
        /// Is a FK to Message Table 
        /// </summary>
        public int? MessageId { get; set; }
        /// <summary>
        /// Saves Employee Id based on the user logged in to the webpage
        /// is a required filed 
        /// takes the string value that is employee "W" number
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Saves absent Id
        /// Is set as not required
        /// Is only saved if employee Choose the absent radio button
        /// FK to the absent table
        /// </summary>
        public int? AbsentId { get; set; }

       // public virtual ICollection<Employee> Employees { get; set; }
       public virtual Employee Employee { get; set; }
        /// <summary>
        /// Setting One Absent to many Workhours relationship
        /// </summary>
        public virtual Absent Absent { get; set; }
        /// <summary>
        /// Setting One Message to many workhours relationship
        /// </summary>
        public virtual Message Message { get; set; }

    }
}
