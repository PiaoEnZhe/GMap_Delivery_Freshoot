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

namespace Freshoot
{
    public partial class SignUp : Form
    {
        private static String DEFAULT_TEXT = "Input details for sign up.";
        private static String WRONG_INFO = "Input correct information for sign up.";
        private static String SIGNUP_IN_PROGRESS = "Sign up in progress ...";
        private static String SIGNUP_SUCCESS = "Sign up successful.";
        private static String SIGNUP_FAILED = "Sign up failed.";
        private static String WEAK_PASSWORD_TEXT = "Password must be longer than 6 letters.";
        private static String WRONG_PASSWORD_TEXT = "Password not matching. Retype correctly";

        IFirebaseClient client;
        FirebaseAuthService fService;

        public Form previousForm;

        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            ActiveControl = full_name_text;
            full_name_text.Focus();

            fService = new FirebaseAuthService(FirebaseContants.fAuthOptions);

            client = new FireSharp.FirebaseClient(FirebaseContants.Config);

            if (fService == null || client == null)
            {
                MessageBox.Show("Firebase Connection Failed, Check Your Network Status.");
                Close();
            }
        }
        
        private void full_name_text_Enter(object sender, EventArgs e)
        {
            if (full_name_text.Text == "Full Name") {
                full_name_text.Text = "";
                full_name_text.ForeColor = Color.Black;
            }
        }

        private void full_name_text_Leave(object sender, EventArgs e)
        {
            if (full_name_text.Text == "")
            {
                full_name_text.Text = "Full Name";
                full_name_text.ForeColor = Color.Silver;
            }
        }

        private void email_text_Enter(object sender, EventArgs e)
        {

            if (email_text.Text == "someone@example.com")
            {
                email_text.Text = "";
                email_text.ForeColor = Color.Black;
            }
        }

        private void email_text_Leave(object sender, EventArgs e)
        {
            if (email_text.Text == "")
            {
                email_text.Text = "someone@example.com";
                email_text.ForeColor = Color.Silver;
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Close();
            previousForm.Show();
        }

        private void SignUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            previousForm.Show();
        }

        private async void signup_button_Click(object sender, EventArgs e)
        {
            if (email_text.Text.Count() < 3 || password_text.Text.Count() < 6 || retypePsw_text.Text != password_text.Text || full_name_text.Text.Count() < 1) {
                info_status.Text = WRONG_INFO;
                info_status.ForeColor = Color.DarkRed;
                return;
            }
            SignUpNewUserRequest request = new SignUpNewUserRequest();
            request.Email = email_text.Text;
            request.Password = password_text.Text;

            info_status.Text = SIGNUP_IN_PROGRESS;
            info_status.ForeColor = Color.DarkGreen;

            try {
                SignUpNewUserResponse response = await fService.SignUpNewUser(request);

                String id = response.LocalId;

                var data = new UserData
                {
                    uid = id,
                    full_name = full_name_text.Text,
                    role = UserData.ROLE_FULFILLMENT
                };
                
                SetResponse response_user = await client.SetAsync("users/" + id, data);
                UserData result = response_user.ResultAs<UserData>();

                info_status.Text = SIGNUP_SUCCESS;
                info_status.ForeColor = Color.Black;
            } catch (FirebaseAuthException ex) {
                String msg = ex.Message;
                MessageBox.Show("Sign Up failed.");

                info_status.Text = SIGNUP_FAILED;
                info_status.ForeColor = Color.DarkRed;
            }
        }

        private void full_name_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {

                ActiveControl = email_text;
                email_text.Focus();
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

        private void email_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = password_text;
                password_text.Focus();
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

        private void password_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = retypePsw_text;
                retypePsw_text.Focus();
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

        private void retypePsw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = signup_button;
                signup_button.Focus();
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

        private void password_text_TextChanged(object sender, EventArgs e)
        {
            if (password_text.Text.Count() < 6)
            {
                info_status.Text = WEAK_PASSWORD_TEXT;
                info_status.ForeColor = Color.DarkRed;
            }
            else
            {
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

        private void retypePsw_TextChanged(object sender, EventArgs e)
        {
            if (password_text.Text.Count() < 6)
            {
                info_status.Text = WEAK_PASSWORD_TEXT;
                info_status.ForeColor = Color.DarkRed;
            }
            else if (password_text.Text != retypePsw_text.Text)
            {
                info_status.Text = WRONG_PASSWORD_TEXT;
                info_status.ForeColor = Color.DarkRed;
            }
            else
            {
                info_status.Text = DEFAULT_TEXT;
                info_status.ForeColor = Color.Black;
            }
        }

    }
}
