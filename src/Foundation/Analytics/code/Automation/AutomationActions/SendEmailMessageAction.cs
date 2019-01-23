using Sitecore.Analytics.Automation;
using Sitecore.Analytics.Automation.Diagnostics.PerformanceCounters;
using Sitecore.Analytics.Automation.Pipelines.PrepareEmail;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Pipelines.GetVisitorEmailAddress;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;

namespace MCH.Foundation.Analytics.Automation.AutomationActions
{
    /// <summary>
    /// Extends the Sitecore.Analytics.Automation.AutomationActions.SendEmailMessageAction with the posiblility to use an smpt address
    /// from a contact which is not registered as a user.
    /// </summary>
    public class SendEmailMessageAction : IAutomationAction
    {
        public static class Parameters
        {
            public static readonly string BaseSiteUrl = "BaseSiteURL";

            public static readonly string FromName = "FromName";

            public static readonly string FromEmail = "FromEmail";

            public static readonly string Subject = "Subject";

            public static readonly string Content = "Content";

            public static readonly string FixedEmail = "FixedEmail";
        }

        public AutomationActionResult Execute(AutomationActionContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            ProcessEmailMessageArgs args = new ProcessEmailMessageArgs();
            SendEmailMessageAction.InitMailServerSettings(args);
            if (!SendEmailMessageAction.InitMessageSettings(args, context))
            {
                Log.Error("Email Subject, Source (From:) or Destination (To:) not found. Send Email will not be executed.", base.GetType());
                return AutomationActionResult.Continue;
            }
            try
            {
                CorePipeline.Run("processEmailMessage", args);
                AutomationCount.EmailsSent.Increment(1L);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, base.GetType());
            }
            return AutomationActionResult.Continue;
        }

        private static string GetVisitorEmail(string fixedEmail, Contact contact)
        {
            if (!string.IsNullOrEmpty(fixedEmail))
            {
                return fixedEmail;
            }

            //MCH Extension
            //gets the first smpt address from a contact which is not registered as a user.
            var contactAddresses =  contact.GetFacet<Sitecore.Analytics.Model.Entities.IContactEmailAddresses>("Emails");
            if (contactAddresses != null && !string.IsNullOrEmpty(contactAddresses.Entries.Keys.FirstOrDefault()))
            {
                return contactAddresses.Entries[contactAddresses.Entries.Keys.FirstOrDefault()].SmtpAddress;
            }

            string identifier = contact.Identifiers.Identifier;
            if (string.IsNullOrEmpty(identifier))
            {
                return string.Empty;
            }
            FindContactEmailAddressArgs findContactEmailAddressArgs = new FindContactEmailAddressArgs(identifier, contact);
            CorePipeline.Run("findVisitorEmailAddress", findContactEmailAddressArgs);
            return findContactEmailAddressArgs.Result;
        }

        private static void InitMailServerSettings(ProcessEmailMessageArgs args)
        {
            args.Host = Settings.MailServer;
            args.Port = Settings.MailServerPort;
            string mailServerUserName = Settings.MailServerUserName;
            if (!string.IsNullOrEmpty(mailServerUserName))
            {
                args.Credentials = new NetworkCredential(mailServerUserName.Replace("\\", "\\\\"), Settings.MailServerPassword);
            }
        }

        private static bool InitMessageSettings(ProcessEmailMessageArgs args, AutomationActionContext context)
        {
            NameValueCollection parameters = context.Parameters;
            string text = parameters[SendEmailMessageAction.Parameters.Subject];
            string text2 = SendEmailMessageAction.FormatEmailAddressArgument(parameters[SendEmailMessageAction.Parameters.FromName], parameters[SendEmailMessageAction.Parameters.FromEmail]);
            string visitorEmail = SendEmailMessageAction.GetVisitorEmail(parameters[SendEmailMessageAction.Parameters.FixedEmail], context.Contact);
            if (new string[]
            {
                text,
                text2,
                visitorEmail
            }.Any(new Func<string, bool>(string.IsNullOrEmpty)))
            {
                return false;
            }
            args.IsBodyHtml = true;
            args.BaseUrl = parameters[SendEmailMessageAction.Parameters.BaseSiteUrl];
            args.Mail.Append(parameters[SendEmailMessageAction.Parameters.Content]);
            args.From = text2;
            args.To.Append(visitorEmail.Replace(";", ","));
            args.Subject.Append(text);
            return true;
        }

        private static string FormatEmailAddressArgument(string name, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(name))
            {
                return string.Format("\"{0}\" <{1}>", name, email);
            }
            return email;
        }
    }
}