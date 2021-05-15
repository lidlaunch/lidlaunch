using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class BulkOrderAttachmentData
    {

        public int CreateBulkOrderAttachment(BulkOrderAttachment attachment)
        {
            var data = new SQLData();
            var returnId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkOrderAttachment", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("attachmentId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", attachment.BulkOrderId);
                    sqlComm.Parameters.AddWithValue("@attachmentType", attachment.AttachmentType);
                    sqlComm.Parameters.AddWithValue("@attachmentName", attachment.AttachmentName);
                    sqlComm.Parameters.AddWithValue("@attachmentPath", attachment.AttachmentPath);
                    sqlComm.Parameters.AddWithValue("@attachmentComment", attachment.AttachmentComment);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    returnId = (int)returnParameter.Value;


                }

                return returnId;
            }
            catch (Exception ex)
            {
                return returnId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool UpdateBulkOrderAttachment(BulkOrderAttachment attachment)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderAttachment", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", attachment.Id);
                    sqlComm.Parameters.AddWithValue("@attachmentType", attachment.AttachmentType);
                    sqlComm.Parameters.AddWithValue("@attachmentName", attachment.AttachmentName);
                    sqlComm.Parameters.AddWithValue("@attachmentPath", attachment.AttachmentPath);
                    sqlComm.Parameters.AddWithValue("@deleted", attachment.Deleted);
                    sqlComm.Parameters.AddWithValue("@attachmentComment", attachment.AttachmentComment);

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

        public List<BulkOrderAttachment> GetBulkOrderAttachments(int bulkOrderId)
        {
            var data = new SQLData();
            List<BulkOrderAttachment> lstAttachments = new List<BulkOrderAttachment>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderAttachments", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                BulkOrderAttachment item = new BulkOrderAttachment();
                                item.Id = Convert.ToInt32(dr["Id"].ToString());
                                item.AttachmentName = Convert.ToString(dr["AttachmentName"].ToString());
                                item.AttachmentPath = Convert.ToString(dr["AttachmentPath"].ToString());
                                item.AttachmentType = Convert.ToString(dr["AttachmentType"].ToString());
                                item.AttachmentComment = Convert.ToString(dr["AttachmentComment"].ToString());
                                item.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                                item.BulkOrderId = Convert.ToInt32(dr["BulkOrderId"].ToString());
                                item.UploadDate = Convert.ToDateTime(dr["UploadDate"].ToString());
                                
                                lstAttachments.Add(item);

                            }

                        }

                    }

                }
                return lstAttachments;
            }
            catch (Exception ex)
            {
                return lstAttachments;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public BulkOrderAttachment GetBulkOrderAttachment(int attachmentId)
        {
            var data = new SQLData();
            BulkOrderAttachment item = new BulkOrderAttachment();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderAttachment", data.conn);
                    sqlComm.Parameters.AddWithValue("@attachmentId", attachmentId);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                                DataRow dr = ds.Tables[0].Rows[0];
                                item.Id = Convert.ToInt32(dr["Id"].ToString());
                                item.AttachmentName = Convert.ToString(dr["AttachmentName"].ToString());
                                item.AttachmentPath = Convert.ToString(dr["AttachmentPath"].ToString());
                                item.AttachmentType = Convert.ToString(dr["AttachmentType"].ToString());
                                item.AttachmentComment = Convert.ToString(dr["AttachmentComment"].ToString());
                                item.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                                item.BulkOrderId = Convert.ToInt32(dr["BulkOrderId"].ToString());
                                item.UploadDate = Convert.ToDateTime(dr["UploadDate"].ToString());
                        }

                    }

                }
                return item;
            }
            catch (Exception ex)
            {
                return item;
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