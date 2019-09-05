namespace CxViewerAction.Views
{

  partial class LoginFrm
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
            this.components = new System.ComponentModel.Container();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.validatorLoginForm = new System.Windows.Forms.ErrorProvider(this.components);
            this.rbUseCredentials = new System.Windows.Forms.RadioButton();
            this.rbUseCurrentUser = new System.Windows.Forms.RadioButton();
            this.RadioButtonSaml = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.validatorLoginForm)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 15);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(117, 12);
            this.txtServer.MaxLength = 255;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(291, 20);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "10.50.8.142";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(117, 110);
            this.txtUserName.MaxLength = 255;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(291, 20);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.Text = "GOL_SBC\\";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(12, 113);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(63, 13);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "User Name:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(117, 136);
            this.txtPassword.MaxLength = 400;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(291, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 139);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password:";
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(332, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(251, 163);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // validatorLoginForm
            // 
            this.validatorLoginForm.ContainerControl = this;
            // 
            // rbUseCredentials
            // 
            this.rbUseCredentials.AutoSize = true;
            this.rbUseCredentials.Checked = true;
            this.rbUseCredentials.Location = new System.Drawing.Point(47, 86);
            this.rbUseCredentials.Name = "rbUseCredentials";
            this.rbUseCredentials.Size = new System.Drawing.Size(99, 17);
            this.rbUseCredentials.TabIndex = 7;
            this.rbUseCredentials.TabStop = true;
            this.rbUseCredentials.Text = "Use Credentials";
            this.rbUseCredentials.UseVisualStyleBackColor = true;
            this.rbUseCredentials.CheckedChanged += new System.EventHandler(this.rbUseCredentials_CheckedChanged);
            // 
            // rbUseCurrentUser
            // 
            this.rbUseCurrentUser.AutoSize = true;
            this.rbUseCurrentUser.Location = new System.Drawing.Point(47, 63);
            this.rbUseCurrentUser.Name = "rbUseCurrentUser";
            this.rbUseCurrentUser.Size = new System.Drawing.Size(106, 17);
            this.rbUseCurrentUser.TabIndex = 6;
            this.rbUseCurrentUser.Text = "Use Current User";
            this.rbUseCurrentUser.UseVisualStyleBackColor = true;
            // 
            // RadioButtonSaml
            // 
            this.RadioButtonSaml.AutoSize = true;
            this.RadioButtonSaml.Location = new System.Drawing.Point(47, 40);
            this.RadioButtonSaml.Name = "RadioButtonSaml";
            this.RadioButtonSaml.Size = new System.Drawing.Size(54, 17);
            this.RadioButtonSaml.TabIndex = 8;
            this.RadioButtonSaml.TabStop = true;
            this.RadioButtonSaml.Text = "SAML";
            this.RadioButtonSaml.UseVisualStyleBackColor = true;
            // 
            // LoginFrm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(437, 197);
            this.Controls.Add(this.RadioButtonSaml);
            this.Controls.Add(this.rbUseCredentials);
            this.Controls.Add(this.rbUseCurrentUser);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginFrm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.validatorLoginForm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblServer;
    private System.Windows.Forms.TextBox txtServer;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
	private System.Windows.Forms.ErrorProvider validatorLoginForm;
    private System.Windows.Forms.RadioButton rbUseCredentials;
    private System.Windows.Forms.RadioButton rbUseCurrentUser;
        private System.Windows.Forms.RadioButton RadioButtonSaml;
    }
}