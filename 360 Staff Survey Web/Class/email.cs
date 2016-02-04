using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using System.Net.Mime;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _360_Staff_Survey_Web.Class
{
    public class email
    {
        public static bool SendMail(string message, string mailTo, string subject)
        {
            
            MailMessage ourMessage = new MailMessage();
            ourMessage.From = new System.Net.Mail.MailAddress("asc-360_LeadershipSystem@tp.edu.sg");
            ourMessage.To.Add(mailTo);
            ourMessage.Subject = subject;
            //var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Image/image006.png"));
            //inlineLogo.ContentId = Guid.NewGuid().ToString();
            ourMessage.IsBodyHtml = true;
            ourMessage.Body = "<div style=\"font-family:Arial\"><img src=\"@@IMAGE@@\" alt=\"\"><br /><br />" + message + "<br /><div style=\"font-family:Arial\"><img src=\"@@11@@\" alt=\"\"><br /><br />";


            // generate the contentID string using the datetime
            string contentID = "1234";

            // create the INLINE attachment

            //Change the attachmentpath to your own project's folder's path of the picture before running

            string attachmentPath = "C:\\Image\\image006.png";
            Attachment inline = new Attachment(attachmentPath);
            inline.ContentDisposition.Inline = true;
            inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            inline.ContentId = contentID;
            inline.ContentType.MediaType = "image/png";
            inline.ContentType.Name = "Name";
            ourMessage.Attachments.Add(inline);

            // replace the tag with the correct content ID
            ourMessage.Body = ourMessage.Body.Replace("@@IMAGE@@", "cid:" + contentID);



            string contentID1 = "1111";
            //Change the attachmentpath to your own project's folder's path of the picture before running

            string attachmentPath1 = "C:\\Image\\image001.png";
            //string attachmentPath1 = "C:\\Users\\user\\Documents\\GitHub\\360StaffFeedback\\360 Staff Survey Web\\Image\\image001.png";

            Attachment inline1 = new Attachment(attachmentPath1);
            inline1.ContentDisposition.Inline = true;
            inline1.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            inline1.ContentId = contentID1;
            inline1.ContentType.MediaType = "image/png";
            inline1.ContentType.Name = "Namee";
            ourMessage.Attachments.Add(inline1);
            ourMessage.Body = ourMessage.Body.Replace("@@11@@", "cid:" + contentID1);






            //ourMessage.Body = message;

            //string body = string.Format(@message, inlineLogo.ContentId);

            //var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            //view.LinkedResources.Add(inlineLogo);
            //ourMessage.AlternateViews.Add(view);


            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString("<img src=cid:logo>", null, "text/html");
            //LinkedResource logo = new LinkedResource("D:\\360 old\\360 Staff Survey Web\\360 Staff Survey Web\\Image\\image006.png");
            //logo.ContentId = "logo";
            //htmlView.LinkedResources.Add(logo);
            //ourMessage.AlternateViews.Add(htmlView);

            //AlternateView htmlView1 = AlternateView.CreateAlternateViewFromString("<img src=cid:logo1>", null, "text/html");
            //LinkedResource logo1 = new LinkedResource("D:\\360 old\\360 Staff Survey Web\\360 Staff Survey Web\\Image\\image001.png");
            //logo1.ContentId = "logo1";
            //htmlView1.LinkedResources.Add(logo1);
            //ourMessage.AlternateViews.Add(htmlView1);

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp-application.tp.edu.sg");
            try
            {
                smtp.Send(ourMessage);      // Send your mail.
                return true;                // IF Mail sended Successfully
            }
            catch (Exception)
            {
                return false;               // Send error
            }
        }


     
    }
}