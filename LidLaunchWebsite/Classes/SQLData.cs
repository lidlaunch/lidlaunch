using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class SQLData
    {
        public SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["dbconn"]);
    }
}