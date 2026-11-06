using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Globalization;

public class RepairLib
{
    public static string PATH_IMAGES = "D:/ImageRepair/Frank/";
    // public static string MANUFACTURE_CONNECTION = "Data Source=dbprodjaws.intra.cmk.co.id; Initial Catalog=dbmfg; User ID=ITJAWS; Password=9HPZhTUMNAgipum";

    public static string GetPathImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return "/Images.aspx?path=Images/not-found.jpg";
        }


        return "/Images.aspx?path=" + imagePath;
    }
}