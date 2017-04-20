using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTrackerWeb.TrackerDbModel;

namespace TimeTrackerWeb
{
    public partial class NewEmployee : System.Web.UI.Page
    {
        Uri serviceLocation = new Uri("http://localhost:5242");
        Default.Container service;
        string userEmail;
        Boolean isAdmin = new Boolean();
        protected void Page_Load(object sender, EventArgs e)
        {
            service = new Default.Container(serviceLocation);
            if (!IsPostBack)
            {
                //Code validates if the user is in the DB
                if (User.Identity.IsAuthenticated)
                {
                    
                    userEmail = User.Identity.Name;
                    //Following code validate if the User has Admin rights
                    isAdmin = IsAdminCheck();
                    if (!isAdmin)
                    {
                        messageDisplay("Need Admin Access");
                        
                        Response.Redirect("~/Account/Login");                      
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Login");
                }
                //validation code ends here  
            }
            LoadDescriptionList();
            LoadCampusList();
            LoadPermissionList();
        }
        /// <summary>
        /// Check if the logged in user has Admin rights
        /// 
        /// </summary>
        /// <returns>true is the logged in user is admin and false if the logged in user is not Admin</returns>
        private bool IsAdminCheck()
        {
            try
            {
                userEmail = User.Identity.Name;
                var product = service.Employees.Where(p => p.Email == userEmail).SingleOrDefault();
                string permission = product.Permission;
                if (permission == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                messageDisplay("user not found in database");
                return false;
            }

        }
        /// <summary>
        /// pre populate the permissions dropdown box
        /// </summary>
        private void LoadPermissionList()
        {
            if (!IsPostBack)
            {
                permissionDropDownList.Items.Add("User");
                permissionDropDownList.Items.Add("Admin");
                permissionDropDownList.Items.Add("Manager");
            }            
        }

        /// <summary>
        /// Load campus list in the campus dropdown list box
        /// </summary>
        private void LoadCampusList()
        {
            if (!IsPostBack)
            {
                campusDropDownList.DataTextField = "Name";
                campusDropDownList.DataSource = service.Campuses.ToList();
                campusDropDownList.DataBind();
            }           
        }
        /// <summary>
        /// Load the Descriptions list box, like, Faculty, Management etc.
        /// </summary>
        private void LoadDescriptionList()
        {
            if (!IsPostBack)
            {
                titleDropDownList.DataTextField = "Title";
                titleDropDownList.DataSource = service.Descriptions.ToList();
                titleDropDownList.DataBind();
            }
            
        }
        /// <summary>
        /// submit button gets all the data filled in the web form
        /// Pass the data to newEmployee object
        /// Load the data from the new employee to Container
        /// WE are using ODATA functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void submitButton_Click(object sender, EventArgs e)
        {
            //checks if the page is valid
            if (Page.IsValid)
            {
               // errorLabel.Text = "Processing";
            }
            else
            {
                errorLabel.Text = "Not Valid";
                valSummaryForm.Visible = true;
            }


            Employee newEmployee = new Employee();
            try
            {
                //loading data from the form to the object
                newEmployee.EmployeeId = employeeIdTextBox.Text.ToString();
                newEmployee.FirstName = firstNameTextBox.Text.ToString();
                newEmployee.LastName = lastNameTextBox.Text.ToString();
                newEmployee.Email = eMailTextBox.Text.ToString();
                newEmployee.Manager = managerIdTextBox.Text.ToString();
                newEmployee.Password = passwordTextBox.Text.ToString();
                newEmployee.CampusId = getCampusId(campusDropDownList.SelectedItem.ToString());
                newEmployee.DescriptionId = getDescriptionId(titleDropDownList.SelectedItem.ToString());
                newEmployee.Permission = permissionDropDownList.SelectedItem.ToString();

                //loading data from the object to container
                service.AddToEmployees(newEmployee);
                service.SaveChanges();
                messageDisplay("Successfully added employee");
                ClearInputs(Page.Controls);
            }
            catch (Exception ex)
            {

                errorLabel.Text = "<h1>Error Occured</h1>";
                messageDisplay("Falied: please check all input feilds");
            }
        }

        /// <summary>
        /// Clear all the textboxes and dropdown list boxes
        /// </summary>
        /// <param name="ctrls"></param>
        private void ClearInputs(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).ClearSelection();

                ClearInputs(ctrl.Controls);
            }
        }
        /// <summary>
        /// Gets the description/Title id based on the text selected in the dropdown box
        /// </summary>
        /// <param name="title">Empoyee's Title/Description</param>
        /// <returns>Description ID is returned back</returns>
        private int? getDescriptionId(string title)
        {
            var product = service.Descriptions.Where(p => p.Title == title).SingleOrDefault();
            int descId = product.DescriptionId;
            return descId;
        }
        /// <summary>
        /// gets the campus ID based on the text selected in the dropdown box
        /// </summary>
        /// <param name="v">Name of the campus</param>
        /// <returns>Campus ID </returns>
        private int? getCampusId(string v)
        {
            var product = service.Campuses.Where(p => p.Name == v).SingleOrDefault();
            int campusId = product.CampusId;
            return campusId;
        }

        /// <summary>
        /// Display the message on the screen
        /// </summary>
        /// <param name="messageToDisplay">Message to be displayed is passed as an argument</param>
        private void messageDisplay(string messageToDisplay)
        {
            //Display success message.
            string message = messageToDisplay;
            string script = "window.onload = function(){ alert('";
            script += message;
            script += "')};";
            ClientScript.RegisterStartupScript(this.GetType(), "Message", script, true);
        }

    }
}