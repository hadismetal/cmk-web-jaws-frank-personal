using System.Web.UI.WebControls;

public class Event 
{

    public static void generate_css_event(Literal x)
    {
        bool flag = false;
        int iDay = 0;
        int iMonth = 0;
        int iYear = 0;

        string css = "";

        iDay = System.DateTime.Now.Day;
        iMonth = System.DateTime.Now.Month;
        iYear = System.DateTime.Now.Year;

        // christmas
        if (iDay > (25 - 7) && iMonth == 12)
        {
            flag = true;

            css += "<style type='text/css'>";
            css += "body {";
            css += "    background: url('/Images/event/christmas.jpg') no-repeat center center fixed; ";
            css += "    -webkit-background-size: cover;";
            css += "    -moz-background-size: cover;";
            css += "    -o-background-size: cover;";
            css += "    background-size: cover;";
            css += "}";
            css += "</style>";
        }

        x.Text = (flag) ? css : "";

    }
}