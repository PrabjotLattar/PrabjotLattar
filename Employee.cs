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
    /// Creats Employee Table
    /// </summary>
    [Table("Employee")]
    public class Employee
    {

        /// <summary>
        /// Setting Employee Id required and String
        /// To take employees "W"number as ID
        /// </summary>
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [MaxLength(15, ErrorMessage = "Cannot be greater than 15 characters")]
        public string EmployeeId { get; set; }


        /// <summary>
        /// Column first name is set with Varchar type nd max length 50
        /// is set to be required
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string FirstName { get; set; }

        //set last name column   
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; }

        /// <summary>
        /// set EMail column with type Varchar
        /// Set as required field
        /// max length of 50
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        /// <summary>
        /// Manager Column is set with type Varchar
        /// Will take managers id that will be  "W" number
        /// </summary>
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string Manager { get; set; }

        /// <summary>
        /// Sets column for the password
        /// Is set as required field
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string Password { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// Sets description other column
        /// Is not Required
        /// Currently not being used
        /// It can be used if want to give option to Admin to self define the employee Title
        /// </summary>
        [MaxLength(80, ErrorMessage = "Cannot be greater than 80 characters")]
        [Column(TypeName = "varchar")]
        public string DescriptionOther { get; set; }

        /// <summary>
        /// Saves the predefined list of the permissions to be choosed for the user
        /// currenlty its Admin User Manager
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Cannot be greater than 50 characters")]
        [Column(TypeName = "varchar")]
        public string Permission { get; set; }
        /// <summary>
        /// saves the campus Id of the employee entered in the Db
        /// Is a FK to the Campus table
        /// </summary>
        [Required]
        public int CampusId { get; set; }
        /// <summary>
        /// Saves the Description ID
        /// Is a FK to Description table
        /// Is Not required in Databse
        /// 
        /// </summary>
        public int? DescriptionId { get; set; }        
        //setting setting One Campus to Many Employee relationship
        public virtual Campus Campus { get; set; }
        //setting one Description to many employees relationship
        public virtual Description Description { get; set; }
        //setting One Employee to Many Workhours relationship
        public virtual ICollection<Workhour> Workhours { get; set; }

    }
}
