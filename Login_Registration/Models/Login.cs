using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;

namespace Login_Registration.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email Required")]
        [DisplayName("Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email Format is wrong")]
        [StringLength(50, ErrorMessage = "Less than 50 characters")]
        public string email
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Required")]
        [DisplayName("Password")]
        [StringLength(30, ErrorMessage = ":Less than 30 characters")]
        public string password
        {
            get;
            set;
        }

        public bool IsUserExist(string email, string password)
        {
            bool flag = false;
            MySqlConnection connection = new MySqlConnection("server=localhost; user id=root; database=demo; password=1234");
            connection.Open();
            MySqlCommand command = new MySqlCommand("select count(*) from members where email = '" + email + "' and password = '" + password + "'", connection);
            flag = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return flag;
        }

    }


    public class Register
    {
        [Required(ErrorMessage = "FirstName Required:")]
        [DisplayName("First Name:")]
        [RegularExpression(@"^[a-zA-Z'.\s]{1,40}$", ErrorMessage = "Special Characters not allowed")]
        [StringLength(45, ErrorMessage = "Less than 45 characters")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "LastName Required:")]
        [RegularExpression(@"^[a-zA-Z'.\s]{1,40}$", ErrorMessage = "Special Characters  not allowed")]
        [DisplayName("Last Name:")]
        [StringLength(45, ErrorMessage = "Less than 45 characters")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Email Required:")]
        [DisplayName("Email:")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email Format is wrong")]
        [StringLength(45, ErrorMessage = "Less than 45 characters")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password Required:")]
        [DataType(DataType.Password)]
        [DisplayName("Password:")]
        [StringLength(45, ErrorMessage = "Less than 45 characters")]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required:")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confirm not matched.")]
        [Display(Name = "Confirm password:")]
        [StringLength(45, ErrorMessage = "Less than 45 characters")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter Verification Code")]
        [DisplayName("Verification Code:")]
        public string Captcha { get; set; }

        [Required(ErrorMessage = "You need to choose purchase now or later")]
        [DisplayName("Become our member:")]
        public Boolean purchaseMembership { get; set; }

        public bool IsUserExist(string email)
        {
            bool flag = false;
            MySqlConnection connection = new MySqlConnection
            ("server=localhost; user id=root; database=demo; password=1234");
            connection.Open();
            MySqlCommand command = new MySqlCommand("select count(*) from members where email = @email", connection);
            command.Parameters.AddWithValue("@email",email);
            flag = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return flag;
        }

        public bool Insert()
        {
            bool flag = false;
            if (!IsUserExist(email))
            {
                MySqlConnection connection = new MySqlConnection("server=localhost; user id=root; database=demo; password=1234");
                connection.Open();
                String text = "INSERT INTO members(firstName, lastName, password, email) VALUES(@firstName, @lastName, @password, @email)";
                MySqlCommand command = new MySqlCommand(text, connection);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@email", email);
                flag = Convert.ToBoolean(command.ExecuteNonQuery());
                String profile = "INSERT INTO profile(email) VALUES (@email)";
                command = new MySqlCommand(profile, connection);
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();
                connection.Close();
                return flag;
            }
            return flag;
        }
    }

    public class CaptchImageAction : ActionResult
    {
        public Color BackgroundColor { get; set; }
        public Color RandomTextColor { get; set; }
        public string RandomText { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            RenderCaptchaImage(context);
        }

        private void RenderCaptchaImage(ControllerContext context)
        {
            Bitmap objBmp = new Bitmap(150, 60);
            Graphics objGraphic = Graphics.FromImage(objBmp);
            objGraphic.Clear(BackgroundColor);
            SolidBrush objBrush = new SolidBrush(RandomTextColor);
            Font objFont = null;
            int a;
            string myFont, str;
            string[] crypticsFont = new string[11];
            crypticsFont[0] = "Times New roman";
            crypticsFont[1] = "Verdana";
            crypticsFont[2] = "Sylfaen";
            crypticsFont[3] = "Microsoft Sans Serif";
            crypticsFont[4] = "Algerian";
            crypticsFont[5] = "Agency FB";
            crypticsFont[6] = "Andalus";
            crypticsFont[7] = "Cambria";
            crypticsFont[8] = "Calibri";
            crypticsFont[9] = "Courier";
            crypticsFont[10] = "Tahoma";
            for (a = 0; a < RandomText.Length; a++)
            {
                myFont = crypticsFont[a];
                objFont = new Font(myFont, 18, FontStyle.Bold | FontStyle.Italic |
                                                                  FontStyle.Strikeout);
                str = RandomText.Substring(a, 1);
                objGraphic.DrawString(str, objFont, objBrush, a * 20, 20);
                objGraphic.Flush();
            }
            context.HttpContext.Response.ContentType = "image/GF";
            objBmp.Save(context.HttpContext.Response.OutputStream, ImageFormat.Gif);
            objFont.Dispose();
            objGraphic.Dispose();
            objBmp.Dispose();
        }

    }
}