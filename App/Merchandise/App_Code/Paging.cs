using System;
using System.Linq;
using System.Web.UI.WebControls;

public class Paging 
{
    public static void BootstrapPaging(Literal lit, string URL, int isumber, int ipitem, int limitperpage, int td = 0, int cp = 1, string cb = "1", int rp = 1, string rb = "1", int id = 0, int iBasic = -1, string xsearch = "")
    {
        int xtotaldataperpage = 10; // INITIAL VALUE MIN 10

        xtotaldataperpage = limitperpage;

        if (xtotaldataperpage < 10) { xtotaldataperpage = 10; }
        double xtotalpageresult = 1; // INITIAL VALUE

        string obj = "";
        string strF = "First";
        string strP = "Previous";
        string strN = "Next";
        string strL = "Last";

        xtotalpageresult = Math.Floor(Convert.ToDouble(td / xtotaldataperpage)) + 1;

        bool isF = false; string urlF = "#"; int vF = 1;
        bool isP = false; string urlP = "#"; int vP = 1;
        bool is1 = false; string url1 = "#"; int v1 = 1; bool isV1 = false;
        bool is2 = false; string url2 = "#"; int v2 = 2; bool isV2 = false;
        bool is3 = false; string url3 = "#"; int v3 = 3; bool isV3 = false;
        bool is4 = false; string url4 = "#"; int v4 = 4; bool isV4 = false;
        bool is5 = false; string url5 = "#"; int v5 = 5; bool isV5 = false;
        bool isN = false; string urlN = "#"; int vN = 6;
        bool isL = false; string urlL = "#"; int vL = Convert.ToInt32(xtotalpageresult);

        if (xtotalpageresult > 1)
        {
            //// SHOW BUTTON PAGING MINIMUM OF 2 PAGE
            isV1 = true;
            isV2 = true;
            isV3 = (xtotalpageresult >= 3);
            isV4 = (xtotalpageresult >= 4);
            isV5 = (xtotalpageresult >= 5);


            //if ((rpgo != 0))
            //{
            //    if (xtotalpageresult >= 5)
            //    {
            //        if (rpgo > Convert.ToInt32(xtotalpageresult) - 5)
            //        {
            //            rb = "5";
            //        }
            //        else if (rpgo <= 5)
            //        {
            //            rb = "1";
            //        }
            //    }
            //}

            //if (rpgo - 2 <= 0) { rp = rpgo; cb = "3"; v1 = 1; }
            //if (rpgo + 2 >= xtotalpageresult) { rp = rpgo; cb = "3"; v1 = Convert.ToInt32(xtotalpageresult) - 4; }

            /////////////////////////////////////// request button (rb) to current button (cb) : start
            if (rb == "F" && (rp <= 1)) { cb = "1"; cp = 1; }
            else if (rb == "L" && (rp >= 5)) 
            { 
                cb = "5"; cp = vL; 
            }
            else 
            {
            
                if (rb == "F") { cb = "1"; }
                else if (rb == "P") 
                {
                    if (cb == "2") { cb = "1"; }
                    else if (cb == "3") { cb = "2"; }
                    else if (cb == "4") { cb = "3"; }
                    else if (cb == "5") { cb = "4"; }
                }
                else if (rb == "N")
                {
                    if (cb == "1") { cb = "2"; }
                    else if (cb == "2") { cb = "3"; }
                    else if (cb == "3") { cb = "4"; }
                    else if (cb == "4") { cb = "5"; }
                }
                else if (rb == "L") { cb = "5"; }
                else
                {
                    cb = rb.ToString();
                }
            }

            /////////////////////////////////////// end
            
            /////////////////////////////////////// setting initial value : start
            v1 = (rp - Convert.ToInt32(cb)) + 1;
            /////////////////////////////////////// end


            /////////////////////////////////////// set current page from request page
            cp = rp;
            /////////////////////////////////////// end

            //if (cb == "1") { is1 = true; }
            //else if (cb == "5") { is5 = true; }
            //else 
                 if (cb == "1") { is1 = true; }
            else if (cb == "2") { is2 = true; }
            else if (cb == "3") { is3 = true; }
            else if (cb == "4") { is4 = true; }
            else if (cb == "5") { is5 = true; }

            /////////////////////////////////////// set Next and Last button
            if (xtotalpageresult >= 6 && rp != xtotalpageresult)
            {
                isN = true;
                isL = true;

                strL += " (" + (Convert.ToInt32(xtotalpageresult)).ToString() + ")";
            }

            /////////////////////////////////////// set First and Previous button
            if (xtotalpageresult >= 6 && rp != 1)
            {
                isF = true;
                isP = true;

                strF += " (1)";
            }

        }

        if (v5 > xtotalpageresult)
        {
            v1 = Convert.ToInt32(xtotalpageresult) - 4;
        }

        if (v1 <= 0)
        {
            v1 = 1;
        }

        // DO NOT CHANGE
        if (isV1 && isV2) { v2 = v1 + 1; }
        if (isV1 && isV2 && isV3) { v3 = v1 + 2; }
        if (isV1 && isV2 && isV3 && isV4) { v4 = v1 + 3; }
        if (isV1 && isV2 && isV3 && isV4 && isV5) { v5 = v1 + 4; }

        // SETTING PREVIOUS AND NEXT BUTTON Value
        vP = cp - 1; // wrong
        vN = cp + 1; // wrong

        // URL
        urlF = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + vF + "&rb=F" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        urlP = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + vP + "&rb=P" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        url1 = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + v1 + "&rb=1" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        url2 = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + v2 + "&rb=2" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        url3 = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + v3 + "&rb=3" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        url4 = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + v4 + "&rb=4" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        url5 = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + v5 + "&rb=5" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        urlN = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + vN + "&rb=N" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");
        urlL = URL + "?td=" + td + "&cp=" + cp + "&cb=" + cb + "&rp=" + vL + "&rb=L" + "&sumber=" + isumber + "&item=" + ipitem + (id > 0 ? ("&search=" + xsearch) : "") + (id > 0 ? ("&basic=" + iBasic) : "") + (id > 0 ? ("&id=" + id) : "");

        if (xtotalpageresult > 1)
        {
            obj += "<nav aria-label='Page navigation'>";

            obj += "<ul class='pagination' style='margin:0; padding:0; float:right;'>";
            obj += "<li " + (!isF ? classDisabled() : "") + ">";
            if (isF) { obj += "<a href='" + urlF + "' aria-label='First'>"; }
            obj += "<span aria-hidden='true'>" + strF + "</span>";
            if (isF) { obj += "</a>"; }
            obj += "</li>";
            obj += "<li " + (!isP ? classDisabled() : "") + ">";
            if (isP) { obj += "<a href='" + urlP + "' aria-label='Previous'>"; }
            obj += "<span aria-hidden='true'>" + strP + "</span>";
            if (isP) { obj += "</a>"; }
            obj += "</li>";
            if (isV1) { obj += "<li " + (is1 ? classActive() : "") + "><a href='" + url1 + "'>" + v1.ToString() + "</a></li>"; }
            if (isV2) { obj += "<li " + (is2 ? classActive() : "") + "><a href='" + url2 + "'>" + v2.ToString() + "</a></li>"; }
            if (isV3) { obj += "<li " + (is3 ? classActive() : "") + "><a href='" + url3 + "'>" + v3.ToString() + "</a></li>"; }
            if (isV4) { obj += "<li " + (is4 ? classActive() : "") + "><a href='" + url4 + "'>" + v4.ToString() + "</a></li>"; }
            if (isV5) { obj += "<li " + (is5 ? classActive() : "") + "><a href='" + url5 + "'>" + v5.ToString() + "</a></li>"; }
            obj += "<li " + (!isN ? classDisabled() : "") + ">";
            if (isN) { obj += "<a href='" + urlN + "' aria-label='Next'>"; }
            obj += "<span aria-hidden='true'>" + strN + "</span>";
            if (isN) { obj += "</a>"; }
            obj += "</li>";
            obj += "<li " + (!isL ? classDisabled() : "") + ">";
            if (isL) { obj += "<a href='" + urlL + "' aria-label='Last'>"; }
            obj += "<span aria-hidden='true'>" + strL + "</span>";
            if (isL) { obj += "</a>"; }
            obj += "</li>";
            obj += "</ul>";

            obj += "</nav>";

        }
        lit.Text = obj;

        /*
        <nav aria-label='Page navigation'>
            <ul class='pagination'>
                <li>
                    <a href='#' aria-label='Previous'>
                        <span aria-hidden='true'>&laquo;</span>
                    </a>
                </li>
                <li><a href='#'>1</a></li>
                <li><a href='#'>2</a></li>
                <li class='active'><a href='#'>3</a></li>
                <li><a href='#'>4</a></li>
                <li><a href='#'>5</a></li>
                <li>
                    <a href='#' aria-label='Next'>
                        <span aria-hidden='true'>&raquo;</span>
                    </a>
                </li>
            </ul>
        </nav>
        */
    }

    protected static string classDisabled()
    {
        return "class='disabled'";
    }

    protected static string classActive()
    {
        return "class='active'";
    }
}