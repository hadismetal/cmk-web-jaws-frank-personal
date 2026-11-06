using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class UploadS
{    
    // public static string NoImage => App.ImagesDir + "null.png";
    public static string NoImage
    {
        get { return App.ImagesDir + "null.png"; }
    }

    public static string[] ImageFormat
    {
        get { return new string[] { ".jpg", ".jpeg", ".png" }; }
    }

    // public static string[] ImageFormat => new string[3] { ".jpg", ".jpeg", ".png" };

    public static void Image(Image img, string Path)
    {
        string empty = string.Empty;
        empty += "https://frank.cmk.co.id/Images.aspx?path=";
        empty += ((Path == null) ? NoImage : Path);
        img.ImageUrl = empty;
    }

    public static bool HasFile(FileUpload file)
    {
        return true && file.PostedFile.FileName != "" && file.PostedFile.InputStream != null && file.PostedFile != null;
    }

    public static bool FormatValid(FileUpload file, HtmlGenericControl lb, string[] format)
    {
        bool flag = false;
        string name = file.PostedFile.FileName.Trim().ToLower();
        string ext = GetExt(name);
        IEnumerable<string> enumerable = Enumerable.Select<string, string>((IEnumerable<string>)format, (Func<string, string>)((string p) => p));
        foreach (string item in enumerable)
        {
            if (ext == item)
            {
                flag = true;
                break;
            }
        }

        lb.Visible = !flag;
        return flag;
    }

    public static bool CreateDir(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string FilePart(string Name, int Part)
    {
        int num = Name.LastIndexOf('.');
        int length = Name.Length - num;
        if (Part == 0)
        {
            return Name.Substring(0, num);
        }

        return Name.Substring(num, length);
    }

    public static string GetName(string Name)
    {
        return FilePart(Name, 0);
    }

    public static string GetExt(string Name)
    {
        return FilePart(Name, 1);
    }
}