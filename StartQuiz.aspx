<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard.Master" AutoEventWireup="true" CodeBehind="StartQuiz.aspx.cs" Inherits="Quiz_Management_System.StartQuiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    body {
        font-family: 'Inter', sans-serif;
        background: #f8fafc;
    }

    .quiz-container {
        max-width: 900px;
        margin: 30px auto;
        padding: 0 15px;
    }

    .quiz-card {
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        padding: 30px;
    }

    .question-box {
        background: #f9fafb;
        border-left: 4px solid #3b82f6;
        padding: 20px;
        margin-bottom: 20px;
        border-radius: 8px;
    }

    .question-text {
        font-size: 17px;
        font-weight: 500;
        color: #1f2937;
        margin-bottom: 15px;
        line-height: 1.5;
    }

    .option-list {
        list-style: none;
        padding: 0;
        margin: 0;
    }

    .option-list li {
        margin-bottom: 8px;
    }

    .option-list input[type="radio"] {
        margin-right: 10px;
        transform: scale(1.1);
    }

    .option-list label {
        cursor: pointer;
        font-size: 15px;
        color: #4b5563;
    }

    .quiz-footer {
        margin-top: 30px;
        padding-top: 20px;
        border-top: 1px solid #e5e7eb;
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 15px;
    }

    .quiz-stats {
        display: flex;
        gap: 20px;
        align-items: center;
        flex-wrap: wrap;
    }

    .stat-item {
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 14px;
        color: #6b7280;
    }

    .stat-value {
        font-weight: 600;
        color: #1f2937;
    }

    .progress-item {
        display: flex;
        align-items: center;
        gap: 8px;
    }

    .progress-bar {
        width: 120px;
        height: 6px;
        background: #e5e7eb;
        border-radius: 3px;
        overflow: hidden;
    }

    .progress-fill {
        height: 100%;
        background: #3b82f6;
        border-radius: 3px;
        transition: width 0.3s ease;
    }

    .btn-next {
        background: #3b82f6;
        color: white;
        padding: 10px 24px;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
        transition: background 0.2s;
        min-width: 100px;
    }

    .btn-next:hover {
        background: #2563eb;
    }

    @media (max-width: 768px) {
        .quiz-footer {
            flex-direction: column;
            align-items: stretch;
        }
        
        .quiz-stats {
            justify-content: space-between;
        }
        
        .btn-next {
            width: 100%;
            margin-top: 10px;
        }
    }
</style>

<div class="quiz-container">
    <div class="quiz-card">
        <h2>Quiz Questions</h2>
        
        <asp:Panel ID="pnlQuestions" runat="server">
            <!-- QUESTION 1 -->
            <div class="question-box" id="question1" runat="server" visible="false">
                <div class="question-text">
                    <asp:Label ID="lblQuestion1" runat="server" />
                </div>
                <asp:RadioButtonList ID="rblOptions1" runat="server"
                    CssClass="option-list"
                    RepeatDirection="Vertical">
                </asp:RadioButtonList>
            </div>

            <!-- QUESTION 2 -->
            <div class="question-box" id="question2" runat="server" visible="false">
                <div class="question-text">
                    <asp:Label ID="lblQuestion2" runat="server" />
                </div>
                <asp:RadioButtonList ID="rblOptions2" runat="server"
                    CssClass="option-list"
                    RepeatDirection="Vertical">
                </asp:RadioButtonList>
            </div>

            <!-- QUESTION 3 -->
            <div class="question-box" id="question3" runat="server" visible="false">
                <div class="question-text">
                    <asp:Label ID="lblQuestion3" runat="server" />
                </div>
                <asp:RadioButtonList ID="rblOptions3" runat="server"
                    CssClass="option-list"
                    RepeatDirection="Vertical">
                </asp:RadioButtonList>
            </div>
        </asp:Panel>

        <div class="quiz-footer">
            <div class="quiz-stats">
                <div class="progress-item">
                    <span>Progress</span>
                    <div class="progress-bar">
                        <div class="progress-fill" id="progressFill" runat="server" style="width: 0%;"></div>
                    </div>
                    <span class="stat-value">
                        <asp:Label ID="lblProgress" runat="server" Text="0%"></asp:Label>
                    </span>
                </div>
                
                <div class="stat-item">
                    <span>Score:</span>
                    <span class="stat-value">
                        <asp:Label ID="lblCurrentScore" runat="server" Text="0"></asp:Label>
                    </span>
                </div>
                
                <div class="stat-item">
                    <span>Page:</span>
                    <span class="stat-value">
                        <asp:Label ID="lblPageInfo" runat="server"></asp:Label>
                    </span>
                </div>
            </div>
            
            <asp:Button ID="btnNext" runat="server"
                CssClass="btn-next"
                Text="Next"
                OnClick="btnNext_Click" />
        </div>
    </div>
</div>

<script>
    // Update progress bar width
    function updateProgressBar() {
        const progressValue = document.getElementById('<%= lblProgress.ClientID %>');
        const progressFill = document.getElementById('progressFill');
        if (progressValue && progressFill) {
            const width = progressValue.textContent.replace('%', '') + '%';
            progressFill.style.width = width;
        }
    }
    document.addEventListener('DOMContentLoaded', updateProgressBar);

    // Update on postback
    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            updateProgressBar();
        });
    }
</script>
</asp:Content>