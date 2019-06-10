using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LidLaunchWebsite.Classes
{
    public class HatData
    {
        //public bool CreateProductType(int productId, int typeId, string previewImage, int colorId)
        //{
        //    var data = new SQLData();
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        using (data.conn)
        //        {
        //            SqlCommand sqlComm = new SqlCommand("CreateProductType", data.conn);
        //            sqlComm.Parameters.AddWithValue("@productId", productId);
        //            sqlComm.Parameters.AddWithValue("@typeId", typeId);
        //            sqlComm.Parameters.AddWithValue("@previewImage", previewImage);
        //            sqlComm.Parameters.AddWithValue("@colorId", colorId);

        //            sqlComm.CommandType = CommandType.StoredProcedure;
        //            data.conn.Open();
        //            sqlComm.ExecuteNonQuery();

        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        if (data.conn != null)
        //        {
        //            data.conn.Close();
        //        }
        //    }
        //}

        public List<HatType> GetHatTypes()
        {
            List<HatType> model = new List<HatType>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetHatTypes", data.conn);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        HatType hatType = new HatType();
                        hatType.Id = Convert.ToInt32(dr["Id"]);
                        hatType.Name = Convert.ToString(dr["Name"]);
                        hatType.Description = dr["Description"].ToString();
                        hatType.ProductImage = dr["ProductImage"].ToString();
                        hatType.BasePrice = Convert.ToDecimal(dr["BasePrice"]);
                        hatType.ManufacturerId = Convert.ToInt32(dr["ManufacturerId"]);
                        hatType.ProductIdentifier = dr["ProductIdentifier"].ToString(); 
                        var lstColors = new List<HatColor>();

                        foreach(DataRow dr2 in ds.Tables[1].Rows)
                        {
                            if(Convert.ToInt32(dr2["TypeId"]) == hatType.Id)
                            {
                                lstColors.Add(new HatColor { color = Convert.ToString(dr2["Color"]), colorCode = Convert.ToString(dr2["ColorCode"]), availableToCreate = Convert.ToBoolean(dr2["AvailableToCreate"]), colorId = Convert.ToInt32(dr2["ColorId"]), creationImage = Convert.ToString(dr2["CreationImage"]) });
                            }
                        }
                        lstColors = lstColors.OrderBy(c => c.colorId).ToList();
                        hatType.lstColors = lstColors;

                        model.Add(hatType);
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

        public HatType GetHatType(int hatTypeId)
        {
            HatType model = new HatType();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetHatType", data.conn);
                    sqlComm.Parameters.AddWithValue("@hatTypeId", hatTypeId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    HatType hatType = new HatType();
                    hatType.Id = Convert.ToInt32(dr["Id"]);
                    hatType.Name = Convert.ToString(dr["Name"]);
                    hatType.Description = dr["Description"].ToString();
                    hatType.ProductImage = dr["ProductImage"].ToString();
                    hatType.BasePrice = Convert.ToDecimal(dr["BasePrice"]);
                    hatType.ManufacturerId = Convert.ToInt32(dr["ManufacturerId"]);
                    hatType.ProductIdentifier = dr["ProductIdentifier"].ToString();
                    var lstColors = new List<HatColor>();

                    foreach (DataRow dr2 in ds.Tables[1].Rows)
                    {
                        if (Convert.ToInt32(dr2["TypeId"]) == hatType.Id)
                        {
                            lstColors.Add(new HatColor { color = Convert.ToString(dr2["Color"]), colorCode = Convert.ToString(dr2["ColorCode"]), availableToCreate = Convert.ToBoolean(dr2["AvailableToCreate"]), colorId = Convert.ToInt32(dr2["ColorId"]), creationImage = Convert.ToString(dr2["CreationImage"]) });
                        }
                    }
                    lstColors = lstColors.OrderBy(c => c.colorId).ToList();
                    hatType.lstColors = lstColors;

                    model = hatType;
                    
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
        public int CreateHatType(HatType hatType)
        {
            var data = new SQLData();
            var hatTypeId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateType", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("typeId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@name", hatType.Name);
                    sqlComm.Parameters.AddWithValue("@description", hatType.Description);
                    sqlComm.Parameters.AddWithValue("@productIdentifier", hatType.ProductIdentifier);
                    sqlComm.Parameters.AddWithValue("@productImage", hatType.ProductImage);
                    sqlComm.Parameters.AddWithValue("@manufacturerId", hatType.ManufacturerId);
                    sqlComm.Parameters.AddWithValue("@basePrice", hatType.BasePrice);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    hatTypeId = (int)returnParameter.Value;

                }

                return hatTypeId;
            }
            catch (Exception ex)
            {
                return hatTypeId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateHatType(HatType hatType)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateType", data.conn);
                    sqlComm.Parameters.AddWithValue("@typeId", hatType.Id);
                    sqlComm.Parameters.AddWithValue("@name", hatType.Name);
                    sqlComm.Parameters.AddWithValue("@description", hatType.Description);
                    sqlComm.Parameters.AddWithValue("@productIdentifier", hatType.ProductIdentifier);
                    sqlComm.Parameters.AddWithValue("@productImage", hatType.ProductImage);
                    sqlComm.Parameters.AddWithValue("@manufacturerId", hatType.ManufacturerId);
                    sqlComm.Parameters.AddWithValue("@basePrice", hatType.BasePrice);

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
        public bool CreateTypeColor(HatColor hatColor)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreatTypeColor", data.conn);
                    sqlComm.Parameters.AddWithValue("@typeId", hatColor.typeId);
                    sqlComm.Parameters.AddWithValue("@ColorId", hatColor.colorId);
                    sqlComm.Parameters.AddWithValue("@CreationImage", hatColor.creationImage);
                    sqlComm.Parameters.AddWithValue("@color", hatColor.color);
                    sqlComm.Parameters.AddWithValue("@availableToCreate", hatColor.availableToCreate);
                    sqlComm.Parameters.AddWithValue("@colorCode", hatColor.colorCode);

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