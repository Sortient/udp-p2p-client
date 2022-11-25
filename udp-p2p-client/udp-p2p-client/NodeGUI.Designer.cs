namespace udp_p2p_client
{
    partial class NodeGUI
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
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstKnownNodes = new System.Windows.Forms.ListBox();
            this.btnKill = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(14, 14);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(440, 420);
            this.txtOutput.TabIndex = 0;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(14, 443);
            this.txtInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(350, 27);
            this.txtInput.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(370, 441);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(84, 30);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lstKnownNodes
            // 
            this.lstKnownNodes.FormattingEnabled = true;
            this.lstKnownNodes.ItemHeight = 19;
            this.lstKnownNodes.Location = new System.Drawing.Point(460, 14);
            this.lstKnownNodes.Name = "lstKnownNodes";
            this.lstKnownNodes.Size = new System.Drawing.Size(272, 422);
            this.lstKnownNodes.TabIndex = 3;
            // 
            // btnKill
            // 
            this.btnKill.Location = new System.Drawing.Point(460, 443);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(272, 30);
            this.btnKill.TabIndex = 2;
            this.btnKill.Text = "DEBUG: KILL CLIENT";
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // NodeGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 492);
            this.Controls.Add(this.lstKnownNodes);
            this.Controls.Add(this.btnKill);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtOutput);
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NodeGUI";
            this.Text = "P2P Messenger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NodeGUI_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtOutput;
        public System.Windows.Forms.TextBox txtInput;
        public System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.ListBox lstKnownNodes;
        public System.Windows.Forms.Button btnKill;
    }
}