using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CxViewerAction.Helpers;
using CxViewerAction.Entities;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Entities.WebServiceEntity;
using Common;
using EnvDTE;

namespace CxViewerAction.Views.DockedView
{
	/// <summary>
	/// Control represent addin custom settings form, available through "Tools" - "Options" menu item
	/// </summary>
    public partial class OptionsAuthCtrl : UserControl
    { 
		#region [Private Members]
		private string _server = "";
		private string _userName = "";
		private string _password = "";
		#endregion

		#region [Public Properties]

		#endregion

		#region [Constructors]

		public OptionsAuthCtrl()
		{
			InitializeComponent();
            if (Connect.IsLoaded)
            {
                BindDataToView();
            }
		}
		#endregion

		#region [Public Methods]

		/// <summary>
		/// Save click button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, EventArgs e)
		{
            save();
		}

        private void save()
        {
            if (!Connect.IsLoaded)
            {
                MessageBox.Show("Checkmarx Visual Studio Plugin is disable. \r\nPlease enable the plugin in Tools->Add-In Manager", "Information", MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (rbUseCredentials.Checked)
                {
                    CheckValidation();
                    if (!string.IsNullOrEmpty(errorProvider.GetError(txtServer)) ||
                           !string.IsNullOrEmpty(errorProvider.GetError(txtUserName)) ||
                           !string.IsNullOrEmpty(errorProvider.GetError(txtPassword))
                           )
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        return;
                    }
                }
                else
                {
                    txtServer_Validating(txtServer, new CancelEventArgs());
                    if (!string.IsNullOrEmpty(errorProvider.GetError(txtServer)) || string.IsNullOrEmpty(txtServer.Text))
                    {
                        return;
                    }
                }

                LoginData currentLogin = BindDataFromView();
                if (currentLogin.BindedProjects != null)
                {
                    currentLogin.BindedProjects.Clear();
                }
     
                CommonData.IsProjectBound = false;
                LoginHelper.IsLogged = false;
                LoginHelper.Save(currentLogin);
                try
                {
                    bool cancelPressed;           
                    LoginHelper.DoLoginWithoutForm(out cancelPressed, true);
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex);
                }

            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

		void CheckValidation()
		{
			txtServer_Validating(txtServer, new CancelEventArgs());
			txtUserName_Validating(txtUserName, new CancelEventArgs());
			txtPassword_Validating(txtPassword, new CancelEventArgs());
		}

		#endregion

		#region [Private Methods]

		/// <summary>
		/// Bind object data to form controls
		/// </summary>
		private void BindDataToView()
		{
			LoginData currentLogin = LoginHelper.Load(0);
          
			txtServer.Text = currentLogin.ServerDomain;
			txtUserName.Text = currentLogin.UserName;
			txtPassword.Text = currentLogin.Password;
            rbUseCurrentUser.Checked = currentLogin.SSO;
            radioButtonSaml.Checked = currentLogin.isSaml;
        }

		/// <summary>
		/// Bind form contol values to object data
		/// </summary>
		/// <returns></returns>
		private LoginData BindDataFromView()
		{
			LoginData currentLogin = LoginHelper.Load(0);

			currentLogin.ServerDomain = txtServer.Text;
			currentLogin.UserName = txtUserName.Text;
			currentLogin.Password = txtPassword.Text;
            currentLogin.SSO = rbUseCurrentUser.Checked;
            currentLogin.isSaml = radioButtonSaml.Checked;

			return currentLogin;
		}

		/// <summary>
		/// Validate server name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtServer_Validating(object sender, CancelEventArgs e)
		{
			if (string.IsNullOrEmpty(txtServer.Text))
			{
				errorProvider.SetError(txtServer, "Enter server domain");
				e.Cancel = true;
			}
			else
				errorProvider.SetError(txtServer, string.Empty);
		}

		/// <summary>
		/// Validate user name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtUserName_Validating(object sender, CancelEventArgs e)
		{
			if (string.IsNullOrEmpty(txtUserName.Text))
			{
				errorProvider.SetError(txtUserName, "Enter user name");
				e.Cancel = true;
			}
			else
				errorProvider.SetError(txtUserName, string.Empty);
		}

		/// <summary>
		/// Validate password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtPassword_Validating(object sender, CancelEventArgs e)
		{
			if (string.IsNullOrEmpty(txtPassword.Text))
			{
				errorProvider.SetError(txtPassword, "Enter password");
				e.Cancel = true;
			}
			else
				errorProvider.SetError(txtPassword, string.Empty);
		}

		/// <summary>
		/// "Restore default" button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRestore_Click(object sender, EventArgs e)
		{
			txtServer.Text = _server;
			txtUserName.Text = _userName;
			txtPassword.Text = _password;
		}

		/// <summary>
		/// Validate server and credentials - make test request to establish connection with server
		/// </summary>
		/// <param name="loginData"></param>
		/// <returns></returns>
		private LoginResult ValidateAuthData(LoginData loginData)
		{
			bool cancelPressed;
			bool isRelogin = false;
			if (LoginHelper.IsLogged)
			{
				isRelogin = true;
			}
			loginData.IsLogging = true;
			LoginResult res = LoginHelper.ExecuteLogin(loginData, out cancelPressed, isRelogin);

			return res;
			//return (res != null && res.IsSuccesfull && !cancelPressed);
		}
		#endregion

		private void OptionsAuthCtrl_VisibleChanged(object sender, EventArgs e)
		{
            if (Connect.IsLoaded)
            {
                BindDataToView();
                errorProvider.Clear();
            }
		}

		private void btTestConnection_Click(object sender, EventArgs e)
		{
			LoginData currentLogin = BindDataFromView();
			LoginResult results = ValidateAuthData(currentLogin);

			if (results.IsSuccesfull)
			{
				MessageBox.Show("Connection successful");
			}
			else
			{
                try
                {
                    if (results.CxWSResponseLoginData != null)
                    {
                        MessageBox.Show(string.Format("Connection failed: {0}", results.CxWSResponseLoginData.ErrorMessage), Common.Constants.ERR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Connection failed");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                    MessageBox.Show("Connection failed");
                }
               		
			}
		}

        public void OnOK()
        {
            save();
            if (rbUseCredentials.Checked)
            {
                if (Connect.IsLoaded && (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text)))
                {
                    MessageBox.Show("Cannot save. Data is not valid.", "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                if (Connect.IsLoaded && (string.IsNullOrEmpty(txtServer.Text)))
                {
                    MessageBox.Show("Cannot save. Data is not valid.", "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void rbUseCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUseCredentials.Checked)
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                errorProvider.SetError(txtUserName, "");
                errorProvider.SetError(txtPassword, "");
            }
        }

        private void radioButtonSaml_CheckedChanged(object sender, EventArgs e)
        {
            btTestConnection.Enabled = !radioButtonSaml.Checked;
        }

        private void PictureBoxLogo_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pictureBoxLogo, "MOD by Afonso");
        }
    }
}
