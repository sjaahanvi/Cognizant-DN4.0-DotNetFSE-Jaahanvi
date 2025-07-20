using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChatApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtUsername;
        private TextBox txtMessage;
        private RichTextBox rtbChatHistory;
        private Button btnSend;
        private Button btnConnect;
        private Button btnDisconnect;
        private Label lblUsername;
        private Label lblMessage;
        private Label lblStatus;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Panel pnlConnection;
        private Panel pnlChat;
        private GroupBox grpConnection;
        private GroupBox grpChat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtUsername = new TextBox();
            this.txtMessage = new TextBox();
            this.rtbChatHistory = new RichTextBox();
            this.btnSend = new Button();
            this.btnConnect = new Button();
            this.btnDisconnect = new Button();
            this.lblUsername = new Label();
            this.lblMessage = new Label();
            this.lblStatus = new Label();
            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();
            this.pnlConnection = new Panel();
            this.pnlChat = new Panel();
            this.grpConnection = new GroupBox();
            this.grpChat = new GroupBox();

            this.statusStrip.SuspendLayout();
            this.pnlConnection.SuspendLayout();
            this.pnlChat.SuspendLayout();
            this.grpConnection.SuspendLayout();
            this.grpChat.SuspendLayout();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 500);
            this.Text = "Kafka Chat Application";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 500);

            // Status Strip
            this.statusStrip.Items.AddRange(new ToolStripItem[] { this.statusLabel });
            this.statusStrip.Location = new Point(0, 477);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(600, 23);
            this.statusStrip.TabIndex = 0;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(118, 17);
            this.statusLabel.Text = "Not connected";

            // Connection Group
            this.grpConnection.Controls.Add(this.pnlConnection);
            this.grpConnection.Dock = DockStyle.Top;
            this.grpConnection.Location = new Point(10, 10);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new Size(580, 80);
            this.grpConnection.TabIndex = 1;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Connection";

            // Connection Panel
            this.pnlConnection.Controls.Add(this.lblUsername);
            this.pnlConnection.Controls.Add(this.txtUsername);
            this.pnlConnection.Controls.Add(this.btnConnect);
            this.pnlConnection.Controls.Add(this.btnDisconnect);
            this.pnlConnection.Dock = DockStyle.Fill;
            this.pnlConnection.Location = new Point(3, 19);
            this.pnlConnection.Name = "pnlConnection";
            this.pnlConnection.Size = new Size(574, 58);
            this.pnlConnection.TabIndex = 0;

            // Username Label
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(10, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(65, 15);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username:";

            // Username TextBox
            this.txtUsername.Location = new Point(85, 17);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 23);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "User1";

            // Connect Button
            this.btnConnect.Location = new Point(300, 16);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new Size(80, 25);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new EventHandler(this.btnConnect_Click);

            // Disconnect Button
            this.btnDisconnect.Location = new Point(390, 16);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new Size(80, 25);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Click += new EventHandler(this.btnDisconnect_Click);

            // Chat Group
            this.grpChat.Controls.Add(this.pnlChat);
            this.grpChat.Dock = DockStyle.Fill;
            this.grpChat.Location = new Point(10, 90);
            this.grpChat.Name = "grpChat";
            this.grpChat.Size = new Size(580, 387);
            this.grpChat.TabIndex = 2;
            this.grpChat.TabStop = false;
            this.grpChat.Text = "Chat";

            // Chat Panel
            this.pnlChat.Controls.Add(this.rtbChatHistory);
            this.pnlChat.Controls.Add(this.lblMessage);
            this.pnlChat.Controls.Add(this.txtMessage);
            this.pnlChat.Controls.Add(this.btnSend);
            this.pnlChat.Dock = DockStyle.Fill;
            this.pnlChat.Location = new Point(3, 19);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new Size(574, 365);
            this.pnlChat.TabIndex = 0;

            // Chat History RichTextBox
            this.rtbChatHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.rtbChatHistory.BackColor = Color.White;
            this.rtbChatHistory.Location = new Point(10, 10);
            this.rtbChatHistory.Name = "rtbChatHistory";
            this.rtbChatHistory.ReadOnly = true;
            this.rtbChatHistory.Size = new Size(554, 300);
            this.rtbChatHistory.TabIndex = 0;
            this.rtbChatHistory.Text = "";

            // Message Label
            this.lblMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new Point(10, 325);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(56, 15);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Message:";

            // Message TextBox
            this.txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtMessage.Location = new Point(75, 322);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new Size(400, 23);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.Enabled = false;
            this.txtMessage.KeyPress += new KeyPressEventHandler(this.txtMessage_KeyPress);

            // Send Button
            this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnSend.Location = new Point(484, 321);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new Size(80, 25);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Enabled = false;
            this.btnSend.Click += new EventHandler(this.btnSend_Click);

            // Form layout
            this.Controls.Add(this.grpChat);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.statusStrip);
            this.Padding = new Padding(10, 10, 10, 0);

            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlConnection.ResumeLayout(false);
            this.pnlConnection.PerformLayout();
            this.pnlChat.ResumeLayout(false);
            this.pnlChat.PerformLayout();
            this.grpConnection.ResumeLayout(false);
            this.grpChat.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
