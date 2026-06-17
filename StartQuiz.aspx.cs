using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Quiz_Management_System
{
    public partial class StartQuiz : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["mycon"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check user login
                if (Session["USER_ID"] == null)
                {
                    Response.Redirect("~/LoginPage.aspx");
                    return;
                }

                // Get quizId from query string
                if (Request.QueryString["quizId"] == null)
                {
                    Response.Redirect("~/UserContentPage.aspx");
                    return;
                }

                int quizId;
                if (!int.TryParse(Request.QueryString["quizId"], out quizId))
                {
                    Response.Redirect("~/UserContentPage.aspx");
                    return;
                }

                // Save in session
                Session["QUIZ_ID"] = quizId;

                // Load questions
                LoadQuestions();

                // Initialize sessions
                Session["Index"] = 0;
                Session["PageIndex"] = 0;
                Session["Score"] = 0;

                // Show first page of questions
                ShowQuestionsPage();
            }
        }

        // Load questions from database
        private void LoadQuestions()
        {
            SqlCommand cmd = new SqlCommand("P_QuizWiseQuestions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QUIZ_ID", Session["QUIZ_ID"]);
            cmd.Parameters.AddWithValue("@EVENT", "GetQuestionsByQuiz");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Session["Questions"] = dt;
        }

        // Display questions page
        private void ShowQuestionsPage()
        {
            DataTable dt = (DataTable)Session["Questions"];
            int totalQuestions = dt.Rows.Count;
            int pageIndex = (int)Session["PageIndex"];
            int startIndex = pageIndex * 3;

            // Hide all questions
            question1.Visible = question2.Visible = question3.Visible = false;
            rblOptions1.Items.Clear();
            rblOptions2.Items.Clear();
            rblOptions3.Items.Clear();

            if (startIndex < totalQuestions)
            {
                lblQuestion1.Text = dt.Rows[startIndex]["QUESTION_TEXT"].ToString();
                AddOptions(rblOptions1, dt.Rows[startIndex]);
                question1.Visible = true;
            }

            if (startIndex + 1 < totalQuestions)
            {
                lblQuestion2.Text = dt.Rows[startIndex + 1]["QUESTION_TEXT"].ToString();
                AddOptions(rblOptions2, dt.Rows[startIndex + 1]);
                question2.Visible = true;
            }

            if (startIndex + 2 < totalQuestions)
            {
                lblQuestion3.Text = dt.Rows[startIndex + 2]["QUESTION_TEXT"].ToString();
                AddOptions(rblOptions3, dt.Rows[startIndex + 2]);
                question3.Visible = true;
            }

            // Update page info
            int pageNumber = pageIndex + 1;
            int totalPages = (int)Math.Ceiling((double)totalQuestions / 3);
            lblPageInfo.Text = $" (Page {pageNumber} of {totalPages})";

            // Update progress
            int answered = (int)Session["Index"];
            int progress = (int)((double)answered / totalQuestions * 100);
            lblProgress.Text = progress + "%";
        }

        // Add options to RadioButtonList
        private void AddOptions(RadioButtonList rbl, DataRow row)
        {
            rbl.Items.Add(new ListItem(row["OPTION_A"].ToString(), "A"));
            rbl.Items.Add(new ListItem(row["OPTION_B"].ToString(), "B"));
            rbl.Items.Add(new ListItem(row["OPTION_C"].ToString(), "C"));
            rbl.Items.Add(new ListItem(row["OPTION_D"].ToString(), "D"));
        }

        // Next button click
        protected void btnNext_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["Questions"];
            int pageIndex = (int)Session["PageIndex"];
            int startIndex = pageIndex * 3;

            ProcessAnswers(startIndex);

            pageIndex++;
            Session["PageIndex"] = pageIndex;

            if (pageIndex * 3 >= dt.Rows.Count)
            {
                SaveResult();
                Response.Redirect("~/Result.aspx");
            }
            else
            {
                ShowQuestionsPage();
            }
        }

        // Process selected answers
        private void ProcessAnswers(int startIndex)
        {
            DataTable dt = (DataTable)Session["Questions"];
            int score = (int)Session["Score"];
            int index = (int)Session["Index"];

            if (question1.Visible && rblOptions1.SelectedItem != null)
            {
                if (rblOptions1.SelectedValue == dt.Rows[startIndex]["CORRECT_OPTION"].ToString())
                    score += Convert.ToInt32(dt.Rows[startIndex]["MARKS"]);
                index++;
            }

            if (question2.Visible && rblOptions2.SelectedItem != null)
            {
                if (rblOptions2.SelectedValue == dt.Rows[startIndex + 1]["CORRECT_OPTION"].ToString())
                    score += Convert.ToInt32(dt.Rows[startIndex + 1]["MARKS"]);
                index++;
            }

            if (question3.Visible && rblOptions3.SelectedItem != null)
            {
                if (rblOptions3.SelectedValue == dt.Rows[startIndex + 2]["CORRECT_OPTION"].ToString())
                    score += Convert.ToInt32(dt.Rows[startIndex + 2]["MARKS"]);
                index++;
            }

            Session["Score"] = score;
            Session["Index"] = index;
            lblCurrentScore.Text = score.ToString();
        }

        // Save result to database
        private void SaveResult()
        {
            DataTable dt = (DataTable)Session["Questions"];
            int obtained = (int)Session["Score"];
            int total = 0;

            foreach (DataRow row in dt.Rows)
                total += Convert.ToInt32(row["MARKS"]);

            SqlCommand cmd = new SqlCommand("P_RESULT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@USER_ID", Session["USER_ID"]);
            cmd.Parameters.AddWithValue("@QUIZ_ID", Session["QUIZ_ID"]);
            cmd.Parameters.AddWithValue("@TOTAL_MARKS", total);
            cmd.Parameters.AddWithValue("@OBTAINED_MARKS", obtained);
            cmd.Parameters.AddWithValue("@EVENT", "Add");

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
