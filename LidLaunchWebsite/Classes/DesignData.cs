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
        public Design GetDesign(int designId)
        {
            var data = new SQLData();
            Design design = new Design();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDesignByDesignId", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        
                        design.Id = Convert.ToInt32(dr["Id"].ToString());
                        design.ArtSource = Convert.ToString(dr["ArtSource"].ToString());
                        design.PreviewImage = Convert.ToString(dr["PreviewImage"].ToString());
                        design.DigitizedFile = Convert.ToString(dr["DigitizedFile"].ToString());
                        design.DigitizedProductionSheet = Convert.ToString(dr["DigitizedProductionSheet"].ToString());
                        design.EMBFile = Convert.ToString(dr["EMBFile"].ToString());
                        design.DigitizedPreview = Convert.ToString(dr["DigitizedPreview"].ToString());
                        design.Width = Convert.ToDecimal(dr["Width"].ToString());
                        design.Height = Convert.ToDecimal(dr["Height"].ToString());
                        design.X = Convert.ToDecimal(dr["X"].ToString());
                        design.Y = Convert.ToDecimal(dr["Y"].ToString());
                        design.EmbroideredWidth = Convert.ToDecimal(dr["EmbroideredWidth"].ToString());
                        design.EmbroideredHeight = Convert.ToDecimal(dr["EmbroideredHeight"].ToString());
                        design.EmbroideredX = Convert.ToDecimal(dr["EmbroideredX"].ToString());
                        design.EmbroideredY = Convert.ToDecimal(dr["EmbroideredY"].ToString());
                        design.CustomerApproved = Convert.ToBoolean(dr["CustomerApproved"].ToString());
                        design.InternallyApproved = Convert.ToBoolean(dr["InternallyApproved"].ToString());
                        design.Revision = Convert.ToBoolean(dr["Revision"].ToString());
                        design.RevisionStatus = Convert.ToString(dr["RevisionStatus"].ToString());
                        if (design.Revision)
                        {
                            if (design.RevisionStatus == "")
                            {
                                design.RevisionStatus = "1:Pending";
                            }
                        }
                        design.Name = Convert.ToString(dr["Name"].ToString());
                    }

                    }

                return design;
            }
            catch (Exception ex)
            {
                return design;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
            
                    

        public int CreateDesign(string artSource, string previewImage, decimal width, decimal height, decimal x, decimal y, decimal embroideredWidth, decimal embroideredHeight, decimal embroideredX, decimal embroideredY, string name, string dstFile, string pdfFile, string embFile, string previewPngFile)
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
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@dst", dstFile);
                    sqlComm.Parameters.AddWithValue("@pdf", pdfFile);
                    sqlComm.Parameters.AddWithValue("emb", embFile);
                    sqlComm.Parameters.AddWithValue("@previewPng", previewPngFile);

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

        public bool UpdateDesign(string artSource, string previewImage, decimal width, decimal height, decimal x, decimal y, decimal embroideredWidth, decimal embroideredHeight, decimal embroideredX, decimal embroideredY, string name, string dstFile, string pdfFile, string embFile, string previewPngFile, int designId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesign", data.conn);
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
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@dst", dstFile);
                    sqlComm.Parameters.AddWithValue("@pdf", pdfFile);
                    sqlComm.Parameters.AddWithValue("emb", embFile);
                    sqlComm.Parameters.AddWithValue("@previewPng", previewPngFile);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

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
        public bool MarkDesignDeleted(int designId)
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
        public bool UnapproveDesign(int designId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UnapproveDesign", data.conn);
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

        public bool UpdateDesignRevisionStatus(int designId, string revisionStatus)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDesignRevisionStatus", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@revisionStatus", revisionStatus);

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