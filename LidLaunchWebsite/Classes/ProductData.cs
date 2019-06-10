using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class ProductData
    {
        public int CreateProduct(string name, string description, int designId, int designerId, int typeId, int colorId)
        {
            var data = new SQLData();
            var productId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateProduct", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("productId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@description", description);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@designerId", designerId);
                    sqlComm.Parameters.AddWithValue("@typeId", typeId);
                    sqlComm.Parameters.AddWithValue("@colorId", colorId);
                    

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    productId = (int)returnParameter.Value;

                }

                return productId;
            }
            catch (Exception ex)
            {
                return productId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool CreateProductType(int productId, int typeId, string previewImage, int colorId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateProductType", data.conn);
                    sqlComm.Parameters.AddWithValue("@productId", productId);
                    sqlComm.Parameters.AddWithValue("@typeId", typeId);
                    sqlComm.Parameters.AddWithValue("@previewImage", previewImage);
                    sqlComm.Parameters.AddWithValue("@colorId", colorId);

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
        public bool DeleteProduct(int productId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteProduct", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);

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
        public bool ApproveProduct(int productId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("ApproveProduct", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);

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
        
        public bool UpdateProduct(string name, string description, int productId, int categoryId, bool privateProduct, int parentProductId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateProduct", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@description", description);
                    sqlComm.Parameters.AddWithValue("@categoryId", categoryId);
                    sqlComm.Parameters.AddWithValue("@private", privateProduct);
                    sqlComm.Parameters.AddWithValue("@parentProductId", parentProductId);      
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
        public bool UpdateProductExisting(string name, string description, int productId, int categoryId, bool privateProduct, bool remove, int parentProductId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateProductExisting", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@description", description);
                    sqlComm.Parameters.AddWithValue("@categoryId", categoryId);
                    sqlComm.Parameters.AddWithValue("@private", privateProduct);
                    sqlComm.Parameters.AddWithValue("@hidden", remove);
                    sqlComm.Parameters.AddWithValue("@parentProductId", parentProductId);
                    
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
        public Product GetProductForProductPage(int productId)
        {
            Product model = new Product();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetProductForProductPage", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    Product product = new Product();
                    Design design = new Design();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        product.Id = Convert.ToInt32(dr["Id"]);
                        product.DesignerId = Convert.ToInt32(dr["DesignerId"]);
                        product.Name = Convert.ToString(dr["Name"]);
                        product.Description = Convert.ToString(dr["Description"]);
                        product.Private = Convert.ToBoolean(dr["Private"].ToString());
                        product.Hidden = Convert.ToBoolean(dr["Hidden"].ToString());
                        if (!(dr["ParentProductId"] is DBNull))
                        {
                            product.ParentProductId = Convert.ToInt32(dr["ParentProductId"]);
                        }
                        else
                        {
                            product.ParentProductId = 0;
                        }
                    }                   

                    model = product;
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
        public Product GetProduct(int productId, int typeId, int colorId)
        {
            Product model = new Product();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetProduct", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", productId);
                    sqlComm.Parameters.AddWithValue("@typeId", typeId);
                    sqlComm.Parameters.AddWithValue("@colorId", colorId);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    Product product = new Product();
                    Design design = new Design();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        product.Id = Convert.ToInt32(dr["Id"]);
                        product.DesignerId = Convert.ToInt32(dr["DesignerId"]);
                        product.Name = Convert.ToString(dr["Name"]);
                        product.Description = Convert.ToString(dr["Description"]);
                        product.Private = Convert.ToBoolean(dr["Private"].ToString());
                        product.Hidden = Convert.ToBoolean(dr["Hidden"].ToString());
                        product.TypeId = Convert.ToInt32(dr["TypeId"]); ;
                        product.TypeText = dr["TypeText"].ToString();
                        if (!(dr["ParentProductId"] is DBNull))
                        {
                            product.ParentProductId = Convert.ToInt32(dr["ParentProductId"]);
                        }
                        else
                        {
                            product.ParentProductId = 0;
                        }
                            
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[1].Rows[0];
                        design.Id = Convert.ToInt32(dr["Id"]);
                        design.PreviewImage = dr["PreviewImage"].ToString();
                        product.Design = design;
                    }

                    model = product;
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
        public List<Product> GetArtworkForDigitizing()
        {
            List<Product> model = new List<Product>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetArtworkForDigitizing", data.conn);
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
                        Product product = new Product();
                        Design design = new Design();
                        product.Name = Convert.ToString(dr["Name"]);
                        design.Id = Convert.ToInt32(dr["DesignId"]);
                        design.ArtSource = dr["ArtSource"].ToString();
                        design.PreviewImage = dr["PreviewImage"].ToString();
                        design.EmbroideredHeight = Convert.ToDecimal(dr["EmbroideredHeight"]);
                        design.EmbroideredWidth = Convert.ToDecimal(dr["EmbroideredWidth"]);
                        design.DigitizedFile = dr["DigitizedFile"].ToString();
                        design.DigitizedPreview = dr["DigitizedPreview"].ToString();
                        design.DigitizedInfoImage = dr["DigitizedInfoImage"].ToString();
                        product.Design = design;
                        model.Add(product);
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
        public List<Product> GetDesignerProductsForParentList(int designerId)
        {
            List<Product> model = new List<Product>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDesignerProductsForParentList", data.conn);
                    sqlComm.Parameters.AddWithValue("@designerId", designerId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Product product = new Product();
                        Design design = new Design();
                        product.Id = Convert.ToInt32(dr["Id"]);
                        product.DesignerId = Convert.ToInt32(dr["DesignerId"]);
                        product.Name = Convert.ToString(dr["Name"]);
                        product.Description = Convert.ToString(dr["Description"]);
                        design.ArtSource = dr["ArtSource"].ToString();
                        design.PreviewImage = dr["PreviewImage"].ToString();
                        product.Design = design;
                        model.Add(product);
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
        public List<Product> GetChildHatList(int parentProductId)
        {
            List<Product> model = new List<Product>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetChildHatList", data.conn);
                    sqlComm.Parameters.AddWithValue("@parentProductId", parentProductId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Product product = new Product();
                        Design design = new Design();
                        product.Id = Convert.ToInt32(dr["Id"]);
                        product.DesignerId = Convert.ToInt32(dr["DesignerId"]);
                        product.Name = Convert.ToString(dr["Name"]);
                        product.Description = Convert.ToString(dr["Description"]);
                        design.ArtSource = dr["ArtSource"].ToString();
                        design.PreviewImage = dr["PreviewImage"].ToString();
                        product.Design = design;
                        model.Add(product);
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
        public List<Product> GetProductsToApprove()
        {
            List<Product> model = new List<Product>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetProductsToApprove", data.conn);
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
                        Product product = new Product();
                        Design design = new Design();
                        product.Id = Convert.ToInt32(dr["Id"]);
                        product.DesignerId = Convert.ToInt32(dr["DesignerId"]);
                        product.Name = Convert.ToString(dr["Name"]);
                        product.Description = Convert.ToString(dr["Description"]);
                        design.ArtSource = dr["ArtSource"].ToString();
                        design.PreviewImage = dr["PreviewImage"].ToString();
                        product.Design = design;
                        model.Add(product);
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
        public List<WebsiteProduct> GetWebsiteProducts(string search, int category)
        {
            List<WebsiteProduct> lstWebsiteProducts = new List<WebsiteProduct>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetWebsiteProducts", data.conn);
                    if (search == null)
                    {
                        sqlComm.Parameters.AddWithValue("@search", "");
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@search", search);
                    }
                    if (category == 0)
                    {
                        sqlComm.Parameters.AddWithValue("@category", "");
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@category", category);
                    }
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {


                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        WebsiteProduct webProd = new WebsiteProduct();
                        webProd.AdddedDate = Convert.ToDateTime(row["AddedDate"]);
                        webProd.Description = row["Description"].ToString();
                        webProd.Name = row["Name"].ToString();
                        webProd.Image = row["Image"].ToString();
                        webProd.DesignerName = row["DesignerName"].ToString();
                        webProd.DesignerId = Convert.ToInt32(row["DesignerId"]);
                        webProd.ProductId = Convert.ToInt32(row["ProductID"]);
                        webProd.ParentCount = Convert.ToInt32(row["ParentCount"]);

                        lstWebsiteProducts.Add(webProd);
                    }
                   
                    return lstWebsiteProducts;
                }
                else
                {
                    return lstWebsiteProducts;
                }
            }
            catch (Exception ex)
            {
                return lstWebsiteProducts;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public List<WebsiteProduct> GetWebsiteProductsRecent()
        {
            List<WebsiteProduct> lstWebsiteProducts = new List<WebsiteProduct>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetWebsiteProductsRecent", data.conn);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        WebsiteProduct webProd = new WebsiteProduct();
                        webProd.AdddedDate = Convert.ToDateTime(row["AddedDate"]);
                        webProd.Description = row["Description"].ToString();
                        webProd.Name = row["Name"].ToString();
                        webProd.Image = row["Image"].ToString();
                        webProd.DesignerName = row["DesignerName"].ToString();
                        webProd.DesignerId = Convert.ToInt32(row["DesignerId"]);
                        webProd.ProductId = Convert.ToInt32(row["ProductID"]);
                        webProd.ParentCount = Convert.ToInt32(row["ParentCount"]);

                        lstWebsiteProducts.Add(webProd);
                    }

                    return lstWebsiteProducts;
                }
                else
                {
                    return lstWebsiteProducts;
                }
            }
            catch (Exception ex)
            {
                return lstWebsiteProducts;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public List<Category> GetCategories()
        {
            List<Category> lstCategories = new List<Category>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetCategories", data.conn);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Category category = new Category();
                        category.Id = Convert.ToInt32(row["Id"]);
                        category.Name = row["Name"].ToString();
                        //category.ParentId = Convert.ToInt32(row["ParentId"].ToString());


                        lstCategories.Add(category);
                    }

                    return lstCategories;
                }
                else
                {
                    return lstCategories;
                }
            }
            catch (Exception ex)
            {
                Category cat = new Category();
                cat.Name = ex.Message.ToString();
                cat.Id = 1;
                lstCategories.Add(cat);
                return lstCategories;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
                
        public List<HatType> GetProductHatTypes(int productId)
        {
            List<HatType> model = new List<HatType>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetProductHatTypes", data.conn);
                    sqlComm.Parameters.AddWithValue("@productId", productId);

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
                        var lstColors = new List<HatColor>();

                        foreach (DataRow dr2 in ds.Tables[1].Rows)
                        {
                            if (Convert.ToInt32(dr2["TypeId"]) == hatType.Id)
                            {
                                lstColors.Add(new HatColor { color = Convert.ToString(dr2["Color"]), colorCode = Convert.ToString(dr2["ColorCode"]), availableToCreate = Convert.ToBoolean(dr2["AvailableToCreate"]), colorId = Convert.ToInt32(dr2["ColorId"]), creationImage = Convert.ToString(dr2["ImagePreview"]) });
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
        


    }
}