namespace Freshoot
{
    partial class SignUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            this.signup_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.password_text = new System.Windows.Forms.TextBox();
            this.email_text = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.full_name = new System.Windows.Forms.Label();
            this.full_name_text = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.retypePsw_text = new System.Windows.Forms.TextBox();
            this.info_status = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // signup_button
            // 
            this.signup_button.BackColor = System.Drawing.SystemColors.HighlightText;
            this.signup_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signup_button.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.signup_button.Location = new System.Drawing.Point(350, 433);
            this.signup_button.Name = "signup_button";
            this.signup_button.Size = new System.Drawing.Size(100, 30);
            this.signup_button.TabIndex = 20;
            this.signup_button.Text = "Sign Up";
            this.signup_button.UseVisualStyleBackColor = false;
            this.signup_button.Click += new System.EventHandler(this.signup_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(293, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 113;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(293, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 112;
            this.label1.Text = "Email";
            // 
            // password_text
            // 
            this.password_text.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.password_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_text.Location = new System.Drawing.Point(290, 290);
            this.password_text.Name = "password_text";
            this.password_text.PasswordChar = '*';
            this.password_text.Size = new System.Drawing.Size(220, 26);
            this.password_text.TabIndex = 14;
            this.password_text.TextChanged += new System.EventHandler(this.password_text_TextChanged);
            this.password_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.password_text_KeyDown);
            // 
            // email_text
            // 
            this.email_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email_text.ForeColor = System.Drawing.Color.Silver;
            this.email_text.Location = new System.Drawing.Point(290, 230);
            this.email_text.Name = "email_text";
            this.email_text.Size = new System.Drawing.Size(220, 26);
            this.email_text.TabIndex = 11;
            this.email_text.Text = "someone@example.com";
            this.email_text.Enter += new System.EventHandler(this.email_text_Enter);
            this.email_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.email_text_KeyDown);
            this.email_text.Leave += new System.EventHandler(this.email_text_Leave);
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 100);
            this.panel2.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Freshoot.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(784, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // full_name
            // 
            this.full_name.AutoSize = true;
            this.full_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.full_name.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.full_name.Location = new System.Drawing.Point(293, 145);
            this.full_name.Name = "full_name";
            this.full_name.Size = new System.Drawing.Size(69, 16);
            this.full_name.TabIndex = 119;
            this.full_name.Text = "Full Name";
            // 
            // full_name_text
            // 
            this.full_name_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.full_name_text.ForeColor = System.Drawing.Color.Silver;
            this.full_name_text.Location = new System.Drawing.Point(290, 170);
            this.full_name_text.Name = "full_name_text";
            this.full_name_text.Size = new System.Drawing.Size(220, 26);
            this.full_name_text.TabIndex = 9;
            this.full_name_text.Text = "Full Name";
            this.full_name_text.Enter += new System.EventHandler(this.full_name_text_Enter);
            this.full_name_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.full_name_text_KeyDown);
            this.full_name_text.Leave += new System.EventHandler(this.full_name_text_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(293, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 16);
            this.label3.TabIndex = 211;
            this.label3.Text = "Retype Password";
            // 
            // retypePsw_text
            // 
            this.retypePsw_text.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.retypePsw_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retypePsw_text.Location = new System.Drawing.Point(290, 351);
            this.retypePsw_text.Name = "retypePsw_text";
            this.retypePsw_text.PasswordChar = '*';
            this.retypePsw_text.Size = new System.Drawing.Size(220, 26);
            this.retypePsw_text.TabIndex = 18;
            this.retypePsw_text.TextChanged += new System.EventHandler(this.retypePsw_TextChanged);
            this.retypePsw_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.retypePsw_KeyDown);
            // 
            // info_status
            // 
            this.info_status.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.info_status.Location = new System.Drawing.Point(300, 390);
            this.info_status.Name = "info_status";
            this.info_status.Size = new System.Drawing.Size(200, 20);
            this.info_status.TabIndex = 221;
            this.info_status.Text = "Input Details For Sign Up.";
            this.info_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Freshoot.Properties.Resources.back_arrow;
            this.pictureBox2.Location = new System.Drawing.Point(375, 486);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 23;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.back_Click);
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.info_status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.retypePsw_text);
            this.Controls.Add(this.full_name);
            this.Controls.Add(this.full_name_text);
            this.Controls.Add(this.signup_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.password_text);
            this.Controls.Add(this.email_text);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SignUp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SignUp_FormClosed);
            this.Load += new System.EventHandler(this.SignUp_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button signup_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox password_text;
        private System.Windows.Forms.TextBox email_text;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label full_name;
        private System.Windows.Forms.TextBox full_name_text;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox retypePsw_text;
        private System.Windows.Forms.Label info_status;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}