using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class UserData
    {
        public int CreateUser(string firstName, string lastName, string middleInitial, string email, string password)
        {
            var data = new SQLData();
            var userId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateUser", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("userId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@firstName", firstName);
                    sqlComm.Parameters.AddWithValue("@lastName", lastName);
                    sqlComm.Parameters.AddWithValue("@middleInitial", middleInitial);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@password", password);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    userId = (int)returnParameter.Value;

                }

                return userId;
            }
            catch (Exception ex)
            {
                return userId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateUser(string firstName, string lastName, string middleInitial, string email, string password, int userId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateUser", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", userId);
                    sqlComm.Parameters.AddWithValue("@firstName", firstName);
                    sqlComm.Parameters.AddWithValue("@lastName", lastName);
                    sqlComm.Parameters.AddWithValue("@middleInitial", middleInitial);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@password", password);                    

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
        public bool DeleteUser(int userId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteUser", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", userId);

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
        public User GetUser(int userId)
        {

            User model = new User();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetUser", data.conn);
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

                        User user = new User();
                        user.Id = Convert.ToInt32(dr["Id"]);
                        user.FirstName = dr["FirstName"].ToString();
                        user.LastName = dr["LastName"].ToString();
                        user.MiddleInitial = dr["MiddleInitial"].ToString();
                        user.Email = dr["Email"].ToString();
                        user.Role = Convert.ToInt32(dr["Role"]);

                        model = user;
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
        public List<User> GetUsers()
        {

            List<User> model = new List<User>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetAllUsers", data.conn);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            User user = new User();
                            user.Id = Convert.ToInt32(dr["Id"]);
                            user.FirstName = dr["FirstName"].ToString();
                            user.LastName = dr["LastName"].ToString();
                            user.MiddleInitial = dr["MiddleInitial"].ToString();
                            user.Email = dr["Email"].ToString();
                            user.Role = Convert.ToInt32(dr["Role"]);
                            user.Active = Convert.ToBoolean(dr["Active"]);

                            model.Add(user);
                        }
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
        public User LoginUser(string email, string password)
        {

            User model = new User();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("LoginUser", data.conn);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@password", password);

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

                        User user = new User();
                        user.Id = Convert.ToInt32(dr["Id"]);
                        user.FirstName = dr["FirstName"].ToString();
                        user.LastName = dr["LastName"].ToString();
                        user.MiddleInitial = dr["MiddleInitial"].ToString();
                        user.Email = dr["Email"].ToString();
                        user.Role = Convert.ToInt32(dr["Role"]);

                        model = user;
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
        public bool SetUserPasswordResetInfo(string email, string resetCode, DateTime resetExpiration)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateUserPasswordReset", data.conn);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@resetCode", resetCode);
                    sqlComm.Parameters.AddWithValue("@expireDate", resetExpiration);
                    
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
        public bool UpdatePassword(string email, string resetCode, string password)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateUserPassword", data.conn);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@resetCode", resetCode);
                    sqlComm.Parameters.AddWithValue("@password", password);

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