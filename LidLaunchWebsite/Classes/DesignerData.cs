using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class DesignerData
    {
        public int CreateDesigner(string shopName, string paypalAddress, string street, string city, string state, string zip, string phone, int userId)
        {
            var data = new SQLData();
            var designerId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateDesigner", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("designerId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@userId", userId);
                    sqlComm.Parameters.AddWithValue("@shopName", shopName);
                    sqlComm.Parameters.AddWithValue("@paypalAddress", paypalAddress);
                    sqlComm.Parameters.AddWithValue("@street", street);
                    sqlComm.Parameters.AddWithValue("@city", city);
                    sqlComm.Parameters.AddWithValue("@state", state);
                    sqlComm.Parameters.AddWithValue("@zip", zip);
                    sqlComm.Parameters.AddWithValue("@phone", phone);

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
        public bool UpdateDesigner(string shopName, string paypalAddress, string street, string city, string state, string zip, string phone, int designerId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesigner", data.conn);
                    sqlComm.Parameters.AddWithValue("@shopName", shopName);
                    sqlComm.Parameters.AddWithValue("@paypalAddress", paypalAddress);
                    sqlComm.Parameters.AddWithValue("@street", street);
                    sqlComm.Parameters.AddWithValue("@city", city);
                    sqlComm.Parameters.AddWithValue("@state", state);
                    sqlComm.Parameters.AddWithValue("@zip", zip);
                    sqlComm.Parameters.AddWithValue("@phone", phone);

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
        public bool DeleteDesigner(int designerId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteDesigner", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", designerId);

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
        public Designer GetDesigner(int userId)
        {
            Designer model = new Designer();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDesigner", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", userId);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        Designer designer = new Designer();
                        designer.Id = Convert.ToInt32(dr["Id"]);
                        designer.PaypalAddress = dr["PaypalAddress"].ToString();
                        //designer.FirstName = dr["FirstName"].ToString();
                        //designer.LastName = dr["LastName"].ToString();
                        //designer.MiddleInitial = dr["MiddleInitial"].ToString();
                        //designer.Email = dr["Email"].ToString();
                        //designer.Role = Convert.ToInt32(dr["Role"]);

                        model = designer;
                    }
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                return model;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public Designer GetDesignerByDesignerId(int designerId)
        {
            Designer model = new Designer();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDesignerByDesignerId", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", designerId);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        Designer designer = new Designer();
                        designer.Id = Convert.ToInt32(dr["Id"]);
                        designer.ShopName = dr["ShopName"].ToString();
                        designer.City = dr["City"].ToString();
                        designer.State = dr["State"].ToString();
                        //designer.FirstName = dr["FirstName"].ToString();
                        //designer.LastName = dr["LastName"].ToString();
                        //designer.MiddleInitial = dr["MiddleInitial"].ToString();
                        //designer.Email = dr["Email"].ToString();
                        //designer.Role = Convert.ToInt32(dr["Role"]);

                        model = designer;
                    }
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                return model;
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