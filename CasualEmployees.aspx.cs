using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTrackerWeb.TrackerDbModel;

namespace TimeTrackerWeb
{
    public partial class CasualEmployees : System.Web.UI.Page
    {
        Uri serviceLocation = new Uri("http://localhost:5242");
        //variable that takes the current logged in user ID
        Default.Container service;
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
                    // userID.Text = User.Identity.Name;
                    userEmail = User.Identity.Name;

                }

                else
                { 
                    //if not redirect to log in page
                    Response.Redirect("~/Account/Login");
                }
            }
            errorLabel.Text = "";
        }
        /// <summary>
        /// Checks if the user is available in Db
        /// </summary>
        /// <returns>true is it is available false if not</returns>
        private bool checkUser()
        {
            try
            {
                userEmail = User.Identity.Name;
                var product = service.Employees.Where(p => p.Email == userEmail).SingleOrDefault();
                string empId = product.EmployeeId;
                if (empId != null)
                    return true;
                else
                    return false;
            }
            catch(Exception)
            {
                return false;
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
            Workhour newWorkhour = new Workhour();
            try
            {

                //check if the hours text box is empty
                int hrs;
                if (casualHoursTextBox.Text == "")
                {
                    hrs = 0;
                    messageDisplay("Hour field cant be left empty");
                }
                else
                {
                    hrs = Convert.ToInt16(casualHoursTextBox.Text);
                }
                string mesg = messageTextBox.Text.ToString();
                int msgId = 0;
                if (mesg != "")
                {
                    Message newMessage = new Message();
                    newMessage.EmployeeMessage = mesg;
                    service.AddToMessages(newMessage);
                    service.SaveChanges();
                    msgId = getMessageId(mesg);
                }
                //passign values to the WOrkhour's object newWorkhour
                newWorkhour.Present = true;
                newWorkhour.Hour = hrs;
                newWorkhour.Date = Calendar.SelectedDate.Date;
                
                newWorkhour.EmployeeId = getUserID();
                if (msgId > 0)
                {
                    newWorkhour.MessageId = msgId;
                }
                //loading the data from newWorkhour object to service container
                service.AddToWorkhours(newWorkhour);
                service.SaveChanges();
                messageDisplay("Success!!");
                ClearInputs(Page.Controls);
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Errror" + ex);
                errorLabel.Text = "<h1>Error Occured</h1>";
                messageDisplay("Failed: Please check all input fields");
            }
        }
        /// <summary>
        /// Clears all the taxtboxs and dropdown list
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
        /// Present radio button event handeler
        /// runs when the present radio button is clicked
        /// makes the absent list box drop down visible to false 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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