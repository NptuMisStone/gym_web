﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_comment_review : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            BindReport();
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
    private void BindReport() 
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT DISTINCT 評論編號,使用者圖片,使用者姓名,評分,評論日期,評論內容,檢舉原因  FROM [系統管理員-評論檢舉] ";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
            if (dataReader.HasRows)
            {
                gv_report.DataSource = dataReader;
                gv_report.DataBind();
            }
            else { 
                gv_report.DataSource = null;
                gv_report.DataBind();
                Label1.Visible=true;
            }
            connection.Close();
        }
    }

    protected void gv_report_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }
    private void DeleteReport( int comment_id) 
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string deleteReportSql = "DELETE FROM 評論檢舉 WHERE 評論編號=@comment_id";
            SqlCommand deleteReportCommand = new SqlCommand(deleteReportSql, connection);
            deleteReportCommand.Parameters.AddWithValue("comment_id", comment_id);
            deleteReportCommand.ExecuteNonQuery();
            
            string deleteCommentSql = "DELETE FROM 完成預約評論表 WHERE 評論編號=@comment_id";
            SqlCommand deleteCommentCommand = new SqlCommand(deleteCommentSql, connection);
            deleteCommentCommand.Parameters.AddWithValue("comment_id", comment_id);
            int rowsAffected = deleteCommentCommand.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '刪除成功',
                            text: '已刪除評論',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                BindReport();
            }

        }
    }
    private void CancelReport(int comment_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM 評論檢舉 where 評論編號=@comment_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("comment_id", comment_id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '撤銷成功',
                            text: '已撤銷評論',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                BindReport();
            }

        }
    }

    protected void gv_report_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //千萬別刪(刪除用)
    }

    protected void gv_report_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //千萬別刪(刪除用)
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        // 找到按下的按鈕所在的行
        Button btnDelete = (Button)sender;
        GridViewRow row = (GridViewRow)btnDelete.NamingContainer;

        // 獲取評論編號
        int comment_id = int.Parse(row.Cells[0].Text.Trim());

        // 執行刪除操作
        DeleteReport(comment_id);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // 找到按下的按鈕所在的行
        Button btnCancel = (Button)sender;
        GridViewRow row = (GridViewRow)btnCancel.NamingContainer;

        // 獲取評論編號
        int comment_id = int.Parse(row.Cells[0].Text.Trim());

        // 執行撤銷操作
        CancelReport(comment_id);
    }
}