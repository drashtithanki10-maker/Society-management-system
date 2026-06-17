using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Quiz_Management_System.Pages
{
    public partial class Result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USER_ID"] == null || Session["QUIZ_ID"] == null)
                {
                    Response.Redirect("~/UserContentPage.aspx");
                    return;
                }
                LoadResult();
            }
        }

        void LoadResult()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["mycon"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("P_RESULT", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", Convert.ToInt32(Session["USER_ID"]));
                    cmd.Parameters.AddWithValue("@QUIZ_ID", Convert.ToInt32(Session["QUIZ_ID"]));
                    cmd.Parameters.AddWithValue("@EVENT", "SelectLastResult");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblTotal.Text = dt.Rows[0]["TOTAL_MARKS"].ToString();
                        lblObtained.Text = dt.Rows[0]["OBTAINED_MARKS"].ToString();

                        bool status = Convert.ToBoolean(dt.Rows[0]["RESULT_STATUS"]);
                        lblStatus.Text = status ? "PASS" : "FAIL";
                        lblStatus.CssClass = status
                            ? "status-badge status-pass"
                            : "status-badge status-fail";
                    }
                    else
                    {
                        // No result found - show message
                        lblTotal.Text = "N/A";
                        lblObtained.Text = "N/A";
                        lblStatus.Text = "No result found";
                        lblStatus.CssClass = "status-badge status-fail";
                    }
                }
            }
            catch (Exception ex)
            {
                // Error handling
                lblTotal.Text = "Error";
                lblObtained.Text = "Error";
                lblStatus.Text = "Error loading result: " + ex.Message;
                lblStatus.CssClass = "status-badge status-fail";
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Clear quiz-related session data
            Session.Remove("QUIZ_ID");
            Session.Remove("Questions");
            Session.Remove("Index");
            Session.Remove("PageIndex");
            Session.Remove("Score");

            Response.Redirect("~/UserContentPage.aspx");
        }
    }
}