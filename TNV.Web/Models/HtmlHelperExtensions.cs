using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace DatePickerHarness.UI
{
    public static class HtmlHelperExtensions
    {
        public static string DatePicker(this HtmlHelper helper, string name)
        {
            return helper.DatePicker(name, null);
        }

        public static string DatePicker(this HtmlHelper helper, string name, string imageUrl)
        {
            return helper.DatePicker(name, imageUrl, null);
        }

        public static string DatePicker(this HtmlHelper helper, string name, object date)
        {
            return helper.DatePicker(name, "/Content/Images/calendar.gif", date);
        }

        public static string DatePicker(this HtmlHelper helper, string name, string imageUrl, object date)
        {
            StringBuilder html = new StringBuilder();

            // Build our base input element
            html.Append("<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\"");

            // Model Binding Support
            if (date != null)
            {
                string dateValue = String.Empty;

                if (date is DateTime? && ((DateTime)date) != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToShortDateString();
                else if (date is DateTime && (DateTime)date != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToShortDateString();
                else if (date is string)
                    dateValue = (string)date;

                html.Append(" value=\"" + dateValue + "\"");
            }

            // We're hard-coding the width here, a better option would be to pass in html attributes and reflect through them
            // here ( default to 75px width if no style attributes )
            html.Append(" style=\"width: 100px;\" />");

            // Now we call the datepicker function, passing in our options.  Again, a future enhancement would be to
            // pass in date options as a list of attributes ( min dates, day/month/year formats, etc. )
            html.Append("<script type=\"text/javascript\">$(document).ready(function() { $('#" + name + "').datepicker({ showOn: 'button', buttonImage: '" + imageUrl + "', duration: 0 }); });</script>");

            return html.ToString();
        }
        
    }
}
