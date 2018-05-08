using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LidLaunchWebsite.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["DesignerID"]) > 0)
                {                    
                    //get list of hat types
                    HatCreationHats model = new HatCreationHats();
                    ProductData data = new ProductData();
                    model.lstHatTypes = data.GetHatTypes();
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Designer", null);
                }                    
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }

        public ActionResult Complete()
        {
            CompleteModel model = new CompleteModel();
            ProductData productData = new ProductData();
            List<Category> lstCategories = new List<Category>();
            List<HatType> lstHatTypes = new List<HatType>();
            List<Product> lstParentProducts = new List<Product>();
            lstCategories = productData.GetCategories();
            lstHatTypes = productData.GetHatTypes();
            lstParentProducts = productData.GetDesignerProductsForParentList(Convert.ToInt32(Session["DesignerID"]));
            model.lstCategories = lstCategories;
            model.lstHatTypes = lstHatTypes;
            model.lstParentProducts = lstParentProducts;
            return View(model);
        }

        public ActionResult GetStarted()
        {
            Session["TempDesignArtworkImagePath"] = null;
            Session["ArtworkX"] = null;
            Session["ArtworkY"] = null;
            Session["ArtworkWidth"] = null;
            Session["ArtworkHeight"] = null;
            Session["FullImagePreview"] = null;
            Session["ProductID"] = null;
            Session["HatTypeID"] = 2;
            Session["ColorID"] = 1;
            return View();
        }
        public ActionResult ConvertArtwork()
        {
            return View();
        }

        public bool checkLoggedIn()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["DesignerID"]) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    

        public string UploadArtwork()
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn())
                {
                    //do nothing
                } else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        //var fileName = Path.GetFileName(file);
                        var extension = Path.GetExtension(fileContent.FileName);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                        if (extension != ".png")
                        {
                            returnValue = "PNG";
                        }
                        else
                        {
                            if (image.Width < 500 || image.Height < 500)
                            {
                                returnValue = "SIZE";
                            }
                            else
                            {
                                //HashSet<Color> colors = new HashSet<Color>();
                                //int totalColors = 0;
                                Bitmap bmp = new Bitmap(image);
                                bmp = Crop(bmp);
                                //for (int y = 0; y < bmp.Size.Height; y++)
                                //{
                                //    for (int x = 0; x < bmp.Size.Width; x++)
                                //    {
                                //        if(!colors.Contains(bmp.GetPixel(x, y)))
                                //        {6
                                //            colors.Add(bmp.GetPixel(x, y));
                                //        }                                        
                                //    }
                                //}
                                //totalColors = colors.Count;

                                //if(totalColors > 5)
                                //{
                                //    returnValue = "COLORS";
                                //} else
                                //{
                                var fileName = Guid.NewGuid().ToString() + extension;
                                var path = Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), fileName);
                                //using (var fileStream = System.IO.File.Create(path))
                                //{
                                //    stream.CopyTo(fileStream);
                                //}
                                bmp.Save(path);
                                Session["TempDesignArtworkImagePath"] = fileName;
                                returnValue = fileName;
                                //}                               
                                
                            }
                        }
                    }
                }                              
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }

        public Bitmap Crop(Bitmap bmp)
        {
            // Find the min/max non-white/transparent pixels
            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);

            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    if (!(pixelColor.A == 0))
                    {
                        if (x < min.X) min.X = x;
                        if (y < min.Y) min.Y = y;

                        if (x > max.X) max.X = x;
                        if (y > max.Y) max.Y = y;
                    }
                }
            }

            // Create a new bitmap from the crop rectangle
            Rectangle cropRectangle = new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
            Bitmap newBitmap = new Bitmap(cropRectangle.Width, cropRectangle.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.DrawImage(bmp, 0, 0, cropRectangle, GraphicsUnit.Pixel);
            }
            return newBitmap;
        }

        public string CreateDesign(string x, string y, string width, string height, string typeId, string colorId)
        {
            var tempFullPath = "";
            try
            {
                if (!checkLoggedIn())
                {
                    //do nothing
                }
                else
                {
                    var color = "";
                    if (Convert.ToInt32(colorId) == 1)
                    {
                        color = "Black";
                    }
                    if (Convert.ToInt32(colorId) == 2)
                    {
                        color = "White";
                    }
                    var artworkPath = Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["TempDesignArtworkImagePath"].ToString());

                    var hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "HatFrontBlack.png");
                    int hatTypeId = Convert.ToInt32(typeId);
                    //standard flexfit
                    if (hatTypeId == 2)
                    {
                        hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "HatFront" + color + ".png");
                    }
                    else if (hatTypeId == 3)
                    {
                        hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "TruckFront" + color + ".png");
                    }
                    else if (hatTypeId == 4)
                    {
                        hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "DadCapFront" + color + ".png");
                    }
                    else if (hatTypeId == 5)
                    {
                        hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "CurveSnapFront" + color + ".png");
                    }
                    else if (hatTypeId == 6)
                    {
                        hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "FlatSnapFront" + color + ".png");
                    }
                    
                    var hatBitmap = new System.Drawing.Bitmap(hatImagePath);
                    var artworkBitmap = new System.Drawing.Bitmap(artworkPath);

                    Int32 artworkWidth = Convert.ToInt32(width);
                    Int32 artworkHeight = Convert.ToInt32(height);
                    Int32 resizedWidth = Convert.ToInt32(Convert.ToInt32(width) * 1.33);
                    Int32 resizedHeight = Convert.ToInt32(Convert.ToInt32(height) * 1.33);
                    var maxArtworkWidth = 468;
                    var maxArtworkHeight = 180;
                    var artworkStartY = 165;
                    var artworkStartX = 68;
                    Int32 artworkX = Convert.ToInt32(artworkStartX + Convert.ToInt32(x));
                    Int32 artworkY = Convert.ToInt32(artworkStartY + Convert.ToInt32(y));
                    var hatHeight = 480;
                    var hatWidth = 600;

                    Session["ArtworkX"] = Convert.ToInt32(x);
                    Session["ArtworkY"] = Convert.ToInt32(y);
                    Session["ArtworkWidth"] = Convert.ToInt32(width);
                    Session["ArtworkHeight"] = Convert.ToInt32(height);
                    Session["HatTypeID"] = Convert.ToInt32(typeId);
                    Session["ColorID"] = Convert.ToInt32(colorId);

                    artworkBitmap = new System.Drawing.Bitmap(artworkBitmap, new System.Drawing.Size(resizedWidth, resizedHeight));

                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(hatBitmap);
                    g.DrawImage(artworkBitmap, new System.Drawing.Point(artworkX, artworkY));
                    //comment in to show rectangle
                    //System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                    //rect.Width = maxArtworkWidth;
                    //rect.Height = maxArtworkHeight;
                    //rect.X = artworkStartX;
                    //rect.Y = artworkStartY;
                    //System.Drawing.Color customColor = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
                    //System.Drawing.SolidBrush shadowBrush = new System.Drawing.SolidBrush(customColor);
                    //g.FillRectangles(shadowBrush, new System.Drawing.RectangleF[] { rect });
                    tempFullPath = Guid.NewGuid().ToString() + ".png";
                    Session["FullImagePreview"] = tempFullPath;
                    hatBitmap.Save(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), tempFullPath));

                    g.Dispose();
                    hatBitmap.Dispose();
                    artworkBitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            var json = new JavaScriptSerializer().Serialize(tempFullPath);
            return json;
        }

        public bool GeneratePreviewImage(string artworkPath, int productId, int hatTypeId, string color)
        {
            try
            {
                string hatImagePath = "";
                if (hatTypeId == 2)
                {
                    hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "HatFront" + color + ".png");
                }
                else if (hatTypeId == 3)
                {
                    hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "TruckFront" + color + ".png");
                }
                else if (hatTypeId == 4)
                {
                    hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "DadCapFront" + color + ".png");
                }
                else if (hatTypeId == 5)
                {
                    hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "CurveSnapFront" + color + ".png");
                }
                else if (hatTypeId == 6)
                {
                    hatImagePath = Path.Combine(Server.MapPath("~/Images/HatAssets"), "FlatSnapFront" + color + ".png");
                }
                var hatBitmap = new System.Drawing.Bitmap(hatImagePath);
                artworkPath = Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), artworkPath);
                var artworkBitmap = new System.Drawing.Bitmap(artworkPath);
                var fullPath = "";

                Int32 artworkWidth = Convert.ToInt32(Session["ArtworkWidth"]);
                Int32 artworkHeight = Convert.ToInt32(Session["ArtworkHeight"]);
                Int32 resizedWidth = Convert.ToInt32(Convert.ToInt32(artworkWidth) * 1.33);
                Int32 resizedHeight = Convert.ToInt32(Convert.ToInt32(artworkHeight) * 1.33);
                var artworkStartY = 165;
                var artworkStartX = 68;
                Int32 artworkX = Convert.ToInt32(artworkStartX + Convert.ToInt32(Session["ArtworkX"]));
                Int32 artworkY = Convert.ToInt32(artworkStartY + Convert.ToInt32(Session["ArtworkY"]));
                
                artworkBitmap = new System.Drawing.Bitmap(artworkBitmap, new System.Drawing.Size(resizedWidth, resizedHeight));

                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(hatBitmap);
                g.DrawImage(artworkBitmap, new System.Drawing.Point(artworkX, artworkY));
                
                fullPath = Guid.NewGuid().ToString() + ".png";
                hatBitmap.Save(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), fullPath));

                g.Dispose();
                hatBitmap.Dispose();
                artworkBitmap.Dispose();

                if (hatTypeId == 2)
                {
                    Session["Type2PreviewImage"] = fullPath;                    
                }
                else if (hatTypeId == 3)
                {
                    Session["Type3PreviewImage"] = fullPath;
                }
                else if (hatTypeId == 4)
                {
                    Session["Type4PreviewImage"] = fullPath;
                }
                else if (hatTypeId == 5)
                {
                    Session["Type5PreviewImage"] = fullPath;
                }
                else if (hatTypeId == 6)
                {
                    Session["Type6PreviewImage"] = fullPath;
                }               

            }
            catch (Exception ex)
            {
                return false;
            }
            
            return true;
        }

        public string AcceptDesign()
        {
            var success = false;
            try {
                if (!checkLoggedIn())
                {
                    //do nothing
                }
                else
                {
                    DesignData designData = new DesignData();
                    ProductData productData = new ProductData();
                    var width = Convert.ToDecimal(Session["ArtworkWidth"].ToString());
                    var height = Convert.ToDecimal(Session["ArtworkHeight"].ToString());
                    var x = Convert.ToDecimal(Session["ArtworkX"].ToString());
                    var y = Convert.ToDecimal(Session["ArtworkY"].ToString());
                    var emWidth = (decimal)6.5 * (width / 468);
                    var emHeight = (decimal)2.5 * (height / 180);
                    var emX = (decimal)6.5 * (x / 468);
                    var emY = (decimal)2.5 * (y / 180);
                    var currentHatTypeId = Convert.ToInt32(Session["HatTypeID"]);
                    var currentColorId = Convert.ToInt32(Session["ColorID"]);

                    if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["TempDesignArtworkImagePath"].ToString())))
                    {
                        System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["TempDesignArtworkImagePath"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["TempDesignArtworkImagePath"].ToString()));
                    }
                    if(!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["FullImagePreview"].ToString())))
                    {
                        System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["FullImagePreview"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["FullImagePreview"].ToString()));
                    }

                    var designId = designData.CreateDesign(Session["TempDesignArtworkImagePath"].ToString(), Session["FullImagePreview"].ToString(), width, height, x, y, Convert.ToDecimal(emWidth), Convert.ToDecimal(emHeight), Convert.ToDecimal(emX), Convert.ToDecimal(emY));
                    var productId = 0;
                    if (designId > 0)
                    {
                        productId = productData.CreateProduct("", "", designId, Convert.ToInt32(Session["DesignerID"].ToString()), currentHatTypeId, currentColorId);
                        if (productId > 0)
                        {
                            success = true;
                            Session["ProductID"] = productId;
                            var color = "";
                            if(currentColorId == 1)
                            {
                                color = "Black";
                            }
                            if (currentColorId == 2)
                            {
                                color = "White";
                            }
                            //genertate all hat type images
                            if (currentHatTypeId != 2)
                            {
                                GeneratePreviewImage(Session["TempDesignArtworkImagePath"].ToString(), productId, 2, color);
                            }
                            if (currentHatTypeId != 3)
                            {
                                GeneratePreviewImage(Session["TempDesignArtworkImagePath"].ToString(), productId, 3, color);
                            }
                            if (currentHatTypeId != 4)
                            {
                                GeneratePreviewImage(Session["TempDesignArtworkImagePath"].ToString(), productId, 4, color);
                            }
                            if (currentHatTypeId != 5)
                            {
                                GeneratePreviewImage(Session["TempDesignArtworkImagePath"].ToString(), productId, 5, color);
                            }
                            if (currentHatTypeId != 6)
                            {
                                GeneratePreviewImage(Session["TempDesignArtworkImagePath"].ToString(), productId, 6, color);
                            }
                        }
                        
                    } else
                    {
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }
        public string UpdateTypeId(string typeId)
        {
            Session["HatTypeID"] = Convert.ToInt32(typeId);
            return new JavaScriptSerializer().Serialize(true);
        }
        public string UpdateColor(string colorId)
        {
            Session["ColorID"] = Convert.ToInt32(colorId);
            return new JavaScriptSerializer().Serialize(true);
        }
        public string UpdateProduct(string name, string description, string categoryId, string privateProduct, List<Int32> hatTypes, string parentProductId)
        {
            if (!checkLoggedIn())
            {
                return "";
            }
            else
            {
                ProductData productData = new ProductData();
                var productId = Convert.ToInt32(Session["ProductID"]);
                var currentColorId = Convert.ToInt32(Session["ColorID"]);
                foreach ( Int32 typeId in hatTypes)
                {
                    if (typeId == Convert.ToInt32(Session["HatTypeID"]))
                    {
                        //do nothing
                    } else
                    {
                        if(typeId == 2)
                        {
                            productData.CreateProductType(productId, typeId, Session["Type2PreviewImage"].ToString(), currentColorId);
                            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type2PreviewImage"].ToString())))
                            {
                                System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["Type2PreviewImage"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type2PreviewImage"].ToString()));
                            }
                        }
                        if (typeId == 3)
                        {
                            productData.CreateProductType(productId, typeId, Session["Type3PreviewImage"].ToString(), currentColorId);
                            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type3PreviewImage"].ToString())))
                            {
                                System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["Type3PreviewImage"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type3PreviewImage"].ToString()));
                            }
                        }
                        if (typeId == 4)
                        {
                            productData.CreateProductType(productId, typeId, Session["Type4PreviewImage"].ToString(), currentColorId);
                            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type4PreviewImage"].ToString())))
                            {
                                System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["Type4PreviewImage"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type4PreviewImage"].ToString()));
                            }
                        }
                        if (typeId == 5)
                        {
                            productData.CreateProductType(productId, typeId, Session["Type5PreviewImage"].ToString(), currentColorId);
                            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type5PreviewImage"].ToString())))
                            {
                                System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["Type5PreviewImage"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type5PreviewImage"].ToString()));
                            }
                        }
                        if (typeId == 6)
                        {
                            productData.CreateProductType(productId, typeId, Session["Type6PreviewImage"].ToString(), currentColorId);
                            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type6PreviewImage"].ToString())))
                            {
                                System.IO.File.Copy(Path.Combine(Server.MapPath("~/Images/DesignImages/Temp"), Session["Type6PreviewImage"].ToString()), Path.Combine(Server.MapPath("~/Images/DesignImages/InUse"), Session["Type6PreviewImage"].ToString()));
                            }
                        }
                    }
                }

                var success = productData.UpdateProduct(name, description, Convert.ToInt32(Session["ProductID"].ToString()), Convert.ToInt32(categoryId), Convert.ToBoolean(privateProduct), Convert.ToInt32(parentProductId));
                var json = new JavaScriptSerializer().Serialize(Session["ProductID"].ToString());
                Session["TempDesignArtworkImagePath"] = null;
                Session["ArtworkX"] = null;
                Session["ArtworkY"] = null;
                Session["ArtworkWidth"] = null;
                Session["ArtworkHeight"] = null;
                Session["FullImagePreview"] = null;
                Session["ProductID"] = null;
                return json;
            }
        }
        public string UpdateProductExisting(string name, string description, string categoryId, string privateProduct, string remove, string parentProductId)
        {
            if (!checkLoggedIn())
            {
                return "";
            }
            else
            {
                ProductData productData = new ProductData();
                var productId = Convert.ToInt32(Session["ProductID"]);                

                var success = productData.UpdateProductExisting(name, description, Convert.ToInt32(Session["ProductID"].ToString()), Convert.ToInt32(categoryId), Convert.ToBoolean(privateProduct), Convert.ToBoolean(remove), Convert.ToInt32(parentProductId));
                var json = new JavaScriptSerializer().Serialize(Session["ProductID"].ToString());
                Session["TempDesignArtworkImagePath"] = null;
                Session["ArtworkX"] = null;
                Session["ArtworkY"] = null;
                Session["ArtworkWidth"] = null;
                Session["ArtworkHeight"] = null;
                Session["FullImagePreview"] = null;
                Session["ProductID"] = null;
                return json;
            }
        }      


    }
}