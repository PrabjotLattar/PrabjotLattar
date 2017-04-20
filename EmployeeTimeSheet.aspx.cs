using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTrackerWeb.TrackerDbModel;

namespace TimeTrackerWeb
{
    public partial class EmployeeTimeSheet : System.Web.UI.Page
    {
        Uri serviceLocation = new Uri("http://localhost:5242");
        Default.Container service;
        //variable that takes the current logged in user ID
        string userEmail;
        //bool the cehck if the userId for logged in user is found in the database
        Boolean isThisUserExistsInDb;
        /// <summary>
        /// Executes on page load
        /// check if user is logded in
        /// check if the user iD is found for the logged in usser
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            service = new Default.Container(serviceLocation);
            //label to show message error occured
            labelMessage.Text = "";
            //setting the absent drop down list box to not visiable
            AbsentListBox.Visible = false;
            //calls the method to check if the user exists
            isThisUserExistsInDb = checkUser();
            if (!isThisUserExistsInDb)
            {
                messageDisplay("User not found in Database");
            }
            if (!IsPostBack)
            {
                //if user is logged in 
                if (User.Identity.IsAuthenticated)
                {
                    userEmail = User.Identity.Name;
                }
                else
                    //if not redirect to log in page
                    Response.Redirect("~/Account/Login");
                
            }
            //load the absent list drop down
            LoadAbsentList();
            errorLabel.Text = "";
            //sets the preset radion button checked by default 
            presentRadioButton.Checked = true;
        }
        /// <summary>
        /// Checks if the logged in user is available in Database
        /// </summary>
        /// <returns>true is it is available false if not</returns>
        private bool checkUser()
        {
            try
            {
                //passes the email address of the logged in user to this variable
                userEmail = User.Identity.Name;
                var product = service.Employees.Where(p => p.Email == userEmail).SingleOrDefault();
                string empId = product.EmployeeId;
                if (empId != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This stops the reload of the absent list 
        /// so the selected value can be retrived
        /// </summary>
        private void LoadAbsentList()
        {
            //condition only run one time and not on each post back 
            if (!IsPostBack)
            {
                AbsentListBox.DataTextField = "Name";
                AbsentListBox.DataSource = service.Absents.ToList();
                AbsentListBox.DataBind();
            }
           
        }
        /// <summary>
        /// Saves entered data to the database 
        /// validates the page
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void submitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //errorLabel.Text = "Processing";
            }
            Workhour newWorkhour = new Workhour();
            try
            {

                //if nothing is entered in hours feild passes 0 as value 
                //else the value from the text box
                int hrs;
                if (casualHoursTextBox.Text == "")
                {
                    hrs = 0;
                }
                else
                {
                    hrs = Convert.ToInt16(casualHoursTextBox.Text);
                }
                //passes the message from text box to variable
                string mesg = messageTextBox.Text.ToString();
                int msgId = 0;
                //if message is there it saves the message in the Db and gets the message id for that message
                if (mesg != "")
                {
                    Message newMessage = new Message();
                    newMessage.EmployeeMessage = mesg;
                    service.AddToMessages(newMessage);
                    service.SaveChanges();
                    //calls method to get the message ID for the just saved messsage
                    msgId = getMessageId(mesg);
                }
                if (presentRadioButton.Checked == true)
                {
                    newWorkhour.Present = true;
                }
                else
                {
                    newWorkhour.Present = false;
                    newWorkhour.AbsentId = getAbsentId(AbsentListBox.SelectedItem.ToString());
                }
                //date validation to check its not future dated
                newWorkhour.Hour = hrs;
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;
                if (Calendar.SelectedDate.Date <= date)
                {
                    newWorkhour.Date = Calendar.SelectedDate.Date;
                }
                else
                {
                    labelMessage.Text = "InValid date";
                }

                if (msgId > 0)
                {
                    newWorkhour.MessageId = msgId;
                }

                newWorkhour.EmployeeId = getUserID(); 
                //passes the data to be entered in the Db to the Service Container of ODATA
                service.AddToWorkhours(newWorkhour);
                service.SaveChanges();
                messageDisplay("Success!!");
                //calls method to get clear all the input fields
                ClearInputs(Page.Controls);
            }
            catch(Exception ex)
            {
                errorLabel.Text = "<h1>Error Occured</h1>";
                messageDisplay("Failed: Please check all input fields");
            }            
        }
        /// <summary>
        /// gets Employee ID and pass it in the data input field
        /// </summary>
        /// <returns>Employee ID</returns>
        private string getUserID()
        {
            userEmail = User.Identity.Name;
            var product = service.Employees.Where(p => p.Email == userEmail).SingleOrDefault();
            string empId = product.EmployeeId;
            return empId;
        }
        /// <summary>
        /// Clears all the input fields 
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
            absentRadioButton.Checked = false;
            presentRadioButton.Checked = true;
            
        }
        /// <summary>
        /// Gets the Absent ID to be saved as FK in WorkHour table
        /// </summary>
        /// <param name="reason"></param>
        /// <returns>Absent ID</returns>
        private int? getAbsentId(string reason)
        {

            var product = service.Absents.Where(p => p.Name == reason).SingleOrDefault();
            int absentId = product.AbsentId;
            return absentId;
        }
        /// <summary>
        /// Gets the message ID to be saved as Fk in WorkHour table
        /// </summary>
        /// <param name="mesg"></param>
        /// <returns>Message ID</returns>
        private int getMessageId(string mesg)
        {
            var product = service.Messages.Where(p => p.EmployeeMessage == mesg).SingleOrDefault();
            int mesgId = product.MessageId;
            return mesgId;
        }
        /// <summary>
        /// absent radio button event handeler
        /// runs when the absent radio button is clicked
        /// makes the absent list box drop down visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void absentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (absentRadioButton.Checked == true)
            {
                AbsentListBox.Visible = true;
            }
        }
        /// <summary>
        /// Present radio button event handeler
        /// runs when the present radio button is clicked
        /// makes the absent list box drop down visible to false 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void presentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (presentRadioButton.Checked == true)
            {
                AbsentListBox.Visible = false;
            }
        }
        /// <summary>
        /// Displays the message on the page
        /// </summary>
        /// <param name="messageToDisplay">Message to be displayed is passed in</param>
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