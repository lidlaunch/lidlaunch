using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class DesignData
    {
        public int CreateDesign(string artSource, string previewImage, decimal width, decimal height, decimal x, decimal y, decimal embroideredWidth, decimal embroideredHeight, decimal embroideredX, decimal embroideredY)
        {
            var data = new SQLData();
            var designerId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateDesign", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("designId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@artSource", artSource);
                    sqlComm.Parameters.AddWithValue("@previewImage", previewImage);
                    sqlComm.Parameters.AddWithValue("@width", width);
                    sqlComm.Parameters.AddWithValue("@height", height);
                    sqlComm.Parameters.AddWithValue("@x", x);
                    sqlComm.Parameters.AddWithValue("@y", y);
                    sqlComm.Parameters.AddWithValue("@embroideredWidth", embroideredWidth);
                    sqlComm.Parameters.AddWithValue("@embroideredHeight", embroideredHeight);
                    sqlComm.Parameters.AddWithValue("@embroideredX", embroideredX);
                    sqlComm.Parameters.AddWithValue("@embroideredY", embroideredY);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    designerId = (int)returnParameter.Value;

                }

                return designerId;
            }
            catch (Exception ex)
            {
                return designerId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }        
        public bool DeleteDesign(int designId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteDesign", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", designId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateDesignDigitizedFile(int designId, string digitizedFile)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesignDigitizedFile", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@digitizedFile", digitizedFile);                   

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateDesignDigitizedPreview(int designId, string digitizedPreview)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesignDigitizedPreview", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@digitizedPreview", digitizedPreview);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateDesignEMBFile(int designId, string embFileName)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesignEMBFile", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@embFileName", embFileName);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        
        public bool UpdateDesignDigitizedProductionSheet(int designId, string DigitizedProductionSheet)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesignDigitizedProductionSheet", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@DigitizedProductionSheet", DigitizedProductionSheet);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }        

    }
}