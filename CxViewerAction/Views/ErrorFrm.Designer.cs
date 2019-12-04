namespace CxViewerAction.Views
{
    partial class ErrorFrm
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
            this.txtErrorDescription = new System.Windows.Forms.TextBox();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.btnRelogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtErrorDescription
            // 
            this.txtErrorDescription.Location = new System.Drawing.Point(13, 13);
            this.txtErrorDescription.Multiline = true;
            this.txtErrorDescription.Name = "txtErrorDescription";
            this.txtErrorDescription.ReadOnly = true;
            this.txtErrorDescription.Size = new System.Drawing.Size(462, 229);
            this.txtErrorDescription.TabIndex = 0;
            // 
            // btnReconnect
            // 
            this.btnReconnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReconnect.Location = new System.Drawing.Point(238, 248);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(75, 23);
            this.btnReconnect.TabIndex = 1;
            this.btnReconnect.Text = "Reconnect";
            this.btnReconnect.UseVisualStyleBackColor = true;
            // 
            // btnRelogin
            // 
            this.btnRelogin.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnRelogin.Location = new System.Drawing.Point(319, 248);
            this.btnRelogin.Name = "btnRelogin";
            this.btnRelogin.Size = new System.Drawing.Size(75, 23);
            this.btnRelogin.TabIndex = 2;
            this.btnRelogin.Text = "Relogin";
            this.btnRelogin.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(400, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ErrorFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(487, 283);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRelogin);
            this.Controls.Add(this.btnReconnect);
            this.Controls.Add(this.txtErrorDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ErrorFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error occured";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtErrorDescription;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Button btnRelogin;
        private System.Windows.Forms.Button btnCancel;
    }
}