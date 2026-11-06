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
    public static string PATH_IMAGES = "D:/ImageRepair/";
    public static string MANUFACTURE_CONNECTION = "Data Source=192.168.1.247,9899; Initial Catalog=dbmfg; User ID=ITDEV-MYAPPS; Password=nvTq6v8RmKVg3sC";

    public static string GetPathImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return "/Images.aspx?path=Images/not-found.jpg";
        }


        return "/Images.aspx?path=" + imagePath;
    }
}