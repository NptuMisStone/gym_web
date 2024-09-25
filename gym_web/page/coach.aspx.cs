﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.IO;

public partial class page_coach : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            coachdata();
        }
    }
    private void coachdata()
    {
        try
        {
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                string sql = "select 健身教練圖片,健身教練姓名,註冊類型,健身教練編號 from 健身教練審核合併 where 審核狀態 = 1";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_coachdata.DataSource = dataReader;
                lv_coachdata.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Log or print the exception details
            Debug.WriteLine("Error retrieving data: " + ex.Message);
        }
    }
    protected void lv_coachdata_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "coach_detail")
        {
            // 取得教練編號，存入 Session，並跳轉至詳細頁面
            Session["coach_num"] = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("coach_detail.aspx");
        }
    }


    protected string GetImageUrl(object imageData, int quality)
    {
        if (imageData != null && imageData != DBNull.Value)
        {
            byte[] bytes = (byte[])imageData;

            using (MemoryStream originalStream = new MemoryStream(bytes))
            using (MemoryStream compressedStream = new MemoryStream())
            {
                // Decode the original image
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);

                // Create an EncoderParameters object to set the image quality
                System.Drawing.Imaging.EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders()
                    .First(codec => codec.MimeType == "image/jpeg");

                // Save the compressed image to the compressedStream
                originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                // Convert the compressed image to a base64 string
                byte[] compressedBytes = compressedStream.ToArray();
                string base64String = Convert.ToBase64String(compressedBytes);

                // Generate the data URI for the compressed image
                return "data:image/jpeg;base64," + base64String;
            }
        }
        else
        {
            return "img/user.png"; // 替代圖片的路徑
        }
    }
}