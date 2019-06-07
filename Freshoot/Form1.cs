using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Firebase.Auth;
using Firebase.Auth.Payloads;

using Freshoot.Firebase;
using Freshoot.Properties;

using DelveryManager_CSharp;

namespace Freshoot
{
    public partial class FreshootLoginForm : Form
    {
        IFirebaseClient client;
        FirebaseAuthService fService;

        public FreshootLoginForm()
        {
            InitializeComponent();
        }

        private void FreshootLoginForm_Load(object sender, EventArgs e)
        {
            remember_me_check.Checked = Settings.Default["remember_me"].ToString() == "True";

            if (remember_me_check.Checked)
            {
                email_text.Text = Settings.Default["email"].ToString();
                password_text.Text = Settings.Default["password"].ToString();
            }

            fService = new FirebaseAuthService(FirebaseContants.fAuthOptions);

            client = new FireSharp.FirebaseClient(FirebaseContants.Config);

            if (fService == null || client == null) {
                MessageBox.Show("Firebase Connection Failed, Check Your Network Status.");
                Close();
            }
        }

        private void logo_Click(object sender, EventArgs e)
        {

        }

        private void Email_address_enter(object sender, EventArgs e)
        {
            if (email_text.Text == "someone@example.com") {
                email_text.Text = "";
                email_text.ForeColor = Color.Black;
            }
        }

        private void Email_address_leave(object sender, EventArgs e)
        {
            if (email_text.Text == "")
            {
                email_text.Text = "someone@example.com";
                email_text.ForeColor = Color.Silver;
            }
        }
        
        private async void login_Click(object sender, EventArgs e)
        {
            if (email_text.Text.Count() > 0 && password_text.Text.Count() > 0) {
                VerifyPasswordRequest request = new VerifyPasswordRequest();
                request.Email = email_text.Text;
                request.Password = password_text.Text;
                try
                {
                    VerifyPasswordResponse response = await fService.VerifyPassword(request);

                    if (remember_me_check.Checked)
                    {
                        Settings.Default["email"] = email_text.Text;
                        Settings.Default["password"] = password_text.Text;
                        Settings.Default.Save();
                    }

                    String uid = response.LocalId;

                    FirebaseResponse user_record = await client.GetAsync("users/" + uid);
                    UserData data = user_record.ResultAs<UserData>();

                    if (data.role == UserData.ROLE_ORGANIZER || data.role == UserData.ROLE_ADMIN)
                    {
                        DeliveryApp deliveryApp = new DeliveryApp();
                        deliveryApp.ShowDialog();
                    }
                    else if (data.role == UserData.ROLE_FULFILLMENT)
                    {
                        FulFillmentStuffForm target = new FulFillmentStuffForm();
                        target.Show();
                        Close();
                    }
                }
                catch (FirebaseAuthException ex)
                {
                    String msg = ex.Message;
                    if (remember_me_check.Checked)
                    {
                        /*Settings.Default["email"] = "";
                        Settings.Default["password"] = "";
                        Settings.Default.Save();*/
                    }
                    MessageBox.Show("Email address or password may not correct.");
                }
                catch (Exception ex) {
                    MessageBox.Show("Network status may not good, Try again later.");
                    Close();
                }
                
            }
        }

        private void signUpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.Show();
            signUp.previousForm = this;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void remember_me_check_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["remember_me"] = remember_me_check.Checked;
            Settings.Default.Save();
        }
    }
}
