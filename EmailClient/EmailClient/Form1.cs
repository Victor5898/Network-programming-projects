using System.ComponentModel;

namespace EmailClient
{
    public partial class Form1 
    {
        EmailClient client;
        Form2 credentialsForm;
        public OpenFileDialog openFileDialog;
        private delegate void SafeCallVoidDelegate();

        public Form1()
        {
            InitializeComponent();
            client = new EmailClient();
            credentialsForm = new Form2(this);
            openFileDialog = new OpenFileDialog();
        }

        private void SendEmail_Click(object sender, EventArgs e)
        {
            if (!credentialsForm.IsLogin)
            {
                credentialsForm.ShowDialog();
                credentialsForm.Login();
            }

            client.EmailSender(SubjectTextBox.Text, MessageTextBox.Text, ToTextBox.Text, credentialsForm.EmailTextBox.Text, credentialsForm.PasswordTextBox.Text, ReplyToTextBox.Text, openFileDialog);

            lblAttachments.Text = "";
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if(LoginBtn.Text == "Login")
            {
                credentialsForm.ShowDialog();
                credentialsForm.Login();
            }
            else if(LoginBtn.Text == "Logout")
            {
                credentialsForm.Logout();
                client.authentificated = false;
            }
        }

        private void Pop3Button_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();

            VerifiyIfLogged();
            LoadingGIF.Visible = true;
            Thread thread = new Thread(Pop3ThreadMethod);
            thread.Start();
        }

        private void IMAPButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();

            VerifiyIfLogged();
            LoadingGIF.Visible = true;
            Thread thread = new Thread(ImapThreadMethod);
            thread.Start();
        }

        private void Pop3ThreadMethod()
        {
            client.FetchEmailsPOP3(credentialsForm.EmailTextBox.Text, credentialsForm.PasswordTextBox.Text);
            FetchTable();
            setLoaderToInvisible();
        }

        private void ImapThreadMethod()
        {
            client.FetchEmailsIMAP(credentialsForm.EmailTextBox.Text, credentialsForm.PasswordTextBox.Text);
            FetchTable();
            setLoaderToInvisible();
        }

        private void setLoaderToInvisible()
        {
            if (LoadingGIF.InvokeRequired)
            {
                LoadingGIF.Invoke(new Action(() => LoadingGIF.Visible = false));
            }
            else
            {
                LoadingGIF.Visible = false;
            }
        }

        private void attachmentLink_Click(object sender, EventArgs e)
        {
            lblAttachments.Text = "";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (File.Exists(filePath))
                    {
                        string fileName = Path.GetFileName(filePath);
                        lblAttachments.Text += fileName + Environment.NewLine;
                    }
                }
            }
        }

        private void VerifiyIfLogged()
        {
            if (!credentialsForm.IsLogin)
            {
                credentialsForm.ShowDialog();
                credentialsForm.Login();
            }
        }

        public void FetchTable()
        {
            string attachments;

            if (dataGridView.InvokeRequired)
            {
                var callClear = new SafeCallVoidDelegate(this.dataGridView.Rows.Clear);
                dataGridView.Invoke(callClear, new object[] { });
            }
            else
            {
                this.dataGridView.Rows.Clear();
            }

            foreach (var email in client.Emails)
            {
                if (email.Attachments.Count > 0)
                {
                    attachments = email.Attachments[0].FileName;

                    for (int i = 1; i < email.Attachments.Count; i++)
                    {
                        attachments = attachments + ", " + email.Attachments[i].FileName;
                    }
                }
                else
                {
                    attachments = "";
                }

                string[] content = { email.MessageId, email.Subject, email.From, email.Body, attachments, email.ReplyTo };

                dataGridView.Invoke(new Action(() => this.dataGridView.Rows.Add(content)));

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (dataGridView.Rows[i].Cells[4].Value != null && dataGridView.Rows[i].Cells[4].Value.ToString() != "")
                    {
                        dataGridView.Invoke(new Action(() => dataGridView[4, i].Style.BackColor = Color.AliceBlue));
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SubjectLabel = new System.Windows.Forms.Label();
            this.ToLabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SendEmailButton = new System.Windows.Forms.Button();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.ToTextBox = new System.Windows.Forms.TextBox();
            this.MessageTextBox = new System.Windows.Forms.RichTextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.userEmail = new System.Windows.Forms.Label();
            this.attachmentLabel = new System.Windows.Forms.Label();
            this.attachmentLink = new System.Windows.Forms.LinkLabel();
            this.lblAttachments = new System.Windows.Forms.Label();
            this.ReplyToLabel = new System.Windows.Forms.Label();
            this.ReplyToTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attachments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplyTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POP3Button = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.LoadingGIF = new System.Windows.Forms.PictureBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TitleLabel.Location = new System.Drawing.Point(279, 90);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(282, 38);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Send email message";
            // 
            // SubjectLabel
            // 
            this.SubjectLabel.AutoSize = true;
            this.SubjectLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SubjectLabel.Location = new System.Drawing.Point(76, 200);
            this.SubjectLabel.Name = "SubjectLabel";
            this.SubjectLabel.Size = new System.Drawing.Size(94, 31);
            this.SubjectLabel.TabIndex = 1;
            this.SubjectLabel.Text = "Subject";
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ToLabel.Location = new System.Drawing.Point(76, 262);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(39, 31);
            this.ToLabel.TabIndex = 2;
            this.ToLabel.Text = "To";
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MessageLabel.Location = new System.Drawing.Point(76, 322);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(106, 31);
            this.MessageLabel.TabIndex = 3;
            this.MessageLabel.Text = "Message";
            // 
            // SendEmailButton
            // 
            this.SendEmailButton.Location = new System.Drawing.Point(196, 878);
            this.SendEmailButton.Name = "SendEmailButton";
            this.SendEmailButton.Size = new System.Drawing.Size(171, 50);
            this.SendEmailButton.TabIndex = 7;
            this.SendEmailButton.Text = "Send";
            this.SendEmailButton.UseVisualStyleBackColor = true;
            this.SendEmailButton.Click += new System.EventHandler(this.SendEmail_Click);
            // 
            // LoginBtn
            // 
            this.LoginBtn.Location = new System.Drawing.Point(1716, 53);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(94, 29);
            this.LoginBtn.TabIndex = 8;
            this.LoginBtn.Text = "Login";
            this.LoginBtn.UseVisualStyleBackColor = true;
            this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.Location = new System.Drawing.Point(238, 200);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(461, 27);
            this.SubjectTextBox.TabIndex = 9;
            // 
            // ToTextBox
            // 
            this.ToTextBox.Location = new System.Drawing.Point(238, 262);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(461, 27);
            this.ToTextBox.TabIndex = 10;
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(238, 322);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(461, 265);
            this.MessageTextBox.TabIndex = 11;
            this.MessageTextBox.Text = "";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.emailLabel.Location = new System.Drawing.Point(1456, 62);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(50, 20);
            this.emailLabel.TabIndex = 12;
            this.emailLabel.Text = "User: ";
            this.emailLabel.Visible = false;
            // 
            // userEmail
            // 
            this.userEmail.AutoSize = true;
            this.userEmail.Location = new System.Drawing.Point(1512, 62);
            this.userEmail.Name = "userEmail";
            this.userEmail.Size = new System.Drawing.Size(46, 20);
            this.userEmail.TabIndex = 13;
            this.userEmail.Text = "email";
            this.userEmail.Visible = false;
            // 
            // attachmentLabel
            // 
            this.attachmentLabel.AutoSize = true;
            this.attachmentLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.attachmentLabel.Location = new System.Drawing.Point(76, 719);
            this.attachmentLabel.Name = "attachmentLabel";
            this.attachmentLabel.Size = new System.Drawing.Size(151, 31);
            this.attachmentLabel.TabIndex = 14;
            this.attachmentLabel.Text = "Attachments";
            // 
            // attachmentLink
            // 
            this.attachmentLink.AutoSize = true;
            this.attachmentLink.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.attachmentLink.Location = new System.Drawing.Point(238, 724);
            this.attachmentLink.Name = "attachmentLink";
            this.attachmentLink.Size = new System.Drawing.Size(145, 25);
            this.attachmentLink.TabIndex = 15;
            this.attachmentLink.TabStop = true;
            this.attachmentLink.Text = "Click here to add";
            this.attachmentLink.Click += new System.EventHandler(this.attachmentLink_Click);
            // 
            // lblAttachments
            // 
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.Location = new System.Drawing.Point(403, 704);
            this.lblAttachments.Name = "lblAttachments";
            this.lblAttachments.Size = new System.Drawing.Size(0, 20);
            this.lblAttachments.TabIndex = 16;
            // 
            // ReplyToLabel
            // 
            this.ReplyToLabel.AutoSize = true;
            this.ReplyToLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ReplyToLabel.Location = new System.Drawing.Point(76, 634);
            this.ReplyToLabel.Name = "ReplyToLabel";
            this.ReplyToLabel.Size = new System.Drawing.Size(104, 31);
            this.ReplyToLabel.TabIndex = 17;
            this.ReplyToLabel.Text = "Reply To";
            // 
            // ReplyToTextBox
            // 
            this.ReplyToTextBox.Location = new System.Drawing.Point(238, 634);
            this.ReplyToTextBox.Name = "ReplyToTextBox";
            this.ReplyToTextBox.Size = new System.Drawing.Size(461, 27);
            this.ReplyToTextBox.TabIndex = 18;
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Subject,
            this.From,
            this.Message,
            this.Attachments,
            this.ReplyTo});
            this.dataGridView.Location = new System.Drawing.Point(730, 123);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 29;
            this.dataGridView.Size = new System.Drawing.Size(1140, 735);
            this.dataGridView.TabIndex = 19;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTableEmails_DoubleClick);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 50;
            // 
            // Subject
            // 
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 6;
            this.Subject.Name = "Subject";
            this.Subject.Width = 125;
            // 
            // From
            // 
            this.From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.From.HeaderText = "From";
            this.From.MinimumWidth = 6;
            this.From.Name = "From";
            // 
            // Message
            // 
            this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Message.HeaderText = "Message";
            this.Message.MinimumWidth = 6;
            this.Message.Name = "Message";
            // 
            // Attachments
            // 
            this.Attachments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Attachments.HeaderText = "Attachments";
            this.Attachments.MinimumWidth = 6;
            this.Attachments.Name = "Attachments";
            // 
            // ReplyTo
            // 
            this.ReplyTo.HeaderText = "Reply To";
            this.ReplyTo.MinimumWidth = 6;
            this.ReplyTo.Name = "ReplyTo";
            this.ReplyTo.Width = 125;
            // 
            // POP3Button
            // 
            this.POP3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.POP3Button.Location = new System.Drawing.Point(943, 878);
            this.POP3Button.Name = "POP3Button";
            this.POP3Button.Size = new System.Drawing.Size(172, 29);
            this.POP3Button.TabIndex = 20;
            this.POP3Button.Text = "Fetch emails with POP3";
            this.POP3Button.UseVisualStyleBackColor = false;
            this.POP3Button.Click += new System.EventHandler(this.Pop3Button_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button3.Location = new System.Drawing.Point(1206, 878);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(182, 29);
            this.button3.TabIndex = 21;
            this.button3.Text = "Fetch emails with IMAP";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.IMAPButton_Click);
            // 
            // LoadingGIF
            // 
            this.LoadingGIF.Image = ((System.Drawing.Image)(resources.GetObject("LoadingGIF.Image")));
            this.LoadingGIF.Location = new System.Drawing.Point(1196, 383);
            this.LoadingGIF.Name = "LoadingGIF";
            this.LoadingGIF.Size = new System.Drawing.Size(251, 256);
            this.LoadingGIF.TabIndex = 22;
            this.LoadingGIF.TabStop = false;
            this.LoadingGIF.Visible = false;
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(448, 878);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(145, 50);
            this.ClearBtn.TabIndex = 23;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1905, 988);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.LoadingGIF);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.POP3Button);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.ReplyToTextBox);
            this.Controls.Add(this.ReplyToLabel);
            this.Controls.Add(this.lblAttachments);
            this.Controls.Add(this.attachmentLink);
            this.Controls.Add(this.attachmentLabel);
            this.Controls.Add(this.userEmail);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.ToTextBox);
            this.Controls.Add(this.SubjectTextBox);
            this.Controls.Add(this.LoginBtn);
            this.Controls.Add(this.SendEmailButton);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.ToLabel);
            this.Controls.Add(this.SubjectLabel);
            this.Controls.Add(this.TitleLabel);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGIF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void gridTableEmails_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                try
                {
                    List<AttachmentModel> attachments = client.Emails[e.RowIndex].Attachments;

                    foreach (var attachment in attachments)
                    {
                        string dir = @"C:\Email Attachments";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string filePath = Path.Combine(dir, attachment.FileName);

                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        BinaryWriter writer = new BinaryWriter(fileStream);
                        writer.Write(attachment.Content);
                        writer.Close();

                        //open downloaded file
                        openFileDialog.Multiselect = true;
                        openFileDialog.InitialDirectory = dir;
                        openFileDialog.ShowDialog();
                    }
                }
                catch(IOException)
                {
                    MessageBox.Show("IOException. Please download again files", "Exception");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubjectTextBox.Text = "";
            ToTextBox.Text = "";
            MessageTextBox.Text = "";
            lblAttachments.Text = "";
            ReplyToTextBox.Text = "";
            openFileDialog.Reset();
        }
    }
}