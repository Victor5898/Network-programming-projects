namespace EmailClient
{
    partial class Form1 : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Label TitleLabel;
        private Label SubjectLabel;
        private Label ToLabel;
        private Label MessageLabel;
        private Button SendEmailButton;
        public TextBox SubjectTextBox;
        public TextBox ToTextBox;
        public RichTextBox MessageTextBox;
        public Button LoginBtn;
        public Label emailLabel;
        public Label userEmail;
        public Label attachmentLabel;
        public LinkLabel attachmentLink;
        private Label lblAttachments;
        private Label ReplyToLabel;
        private TextBox ReplyToTextBox;
        private DataGridView dataGridView;
        private Button POP3Button;
        private Button button3;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn From;
        private DataGridViewTextBoxColumn Message;
        private DataGridViewTextBoxColumn Attachments;
        private DataGridViewTextBoxColumn ReplyTo;
        private PictureBox LoadingGIF;
        private Button ClearBtn;
    }
}