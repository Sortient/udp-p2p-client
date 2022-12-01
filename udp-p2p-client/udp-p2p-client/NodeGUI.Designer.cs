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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeGUI));
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstKnownNodes = new System.Windows.Forms.ListBox();
            this.btnKill = new System.Windows.Forms.Button();
            this.btnSort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrPing = new System.Windows.Forms.Timer(this.components);
            this.btnBotSend = new System.Windows.Forms.Button();
            this.timerBot = new System.Windows.Forms.Timer(this.components);
            this.btnPost = new System.Windows.Forms.Button();
            this.btnSendMalformedData = new System.Windows.Forms.Button();
            this.btnRebuildFromNetwork = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(14, 90);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(440, 344);
            this.txtOutput.TabIndex = 0;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(14, 443);
            this.txtInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(350, 27);
            this.txtInput.TabIndex = 1;
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
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
            this.lstKnownNodes.Location = new System.Drawing.Point(460, 90);
            this.lstKnownNodes.Name = "lstKnownNodes";
            this.lstKnownNodes.Size = new System.Drawing.Size(272, 346);
            this.lstKnownNodes.TabIndex = 3;
            // 
            // btnKill
            // 
            this.btnKill.Location = new System.Drawing.Point(460, 441);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(129, 32);
            this.btnKill.TabIndex = 2;
            this.btnKill.Text = "KILL CLIENT";
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // btnSort
            // 
            this.btnSort.Location = new System.Drawing.Point(595, 441);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(137, 32);
            this.btnSort.TabIndex = 4;
            this.btnSort.Text = "SORT";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 15F);
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 28);
            this.label1.TabIndex = 5;
            this.label1.Text = "{listening data}";
            // 
            // tmrPing
            // 
            this.tmrPing.Interval = 20000;
            // 
            // btnBotSend
            // 
            this.btnBotSend.BackColor = System.Drawing.Color.LightCoral;
            this.btnBotSend.Location = new System.Drawing.Point(460, 54);
            this.btnBotSend.Name = "btnBotSend";
            this.btnBotSend.Size = new System.Drawing.Size(272, 30);
            this.btnBotSend.TabIndex = 6;
            this.btnBotSend.Text = "Toggle Bot";
            this.btnBotSend.UseVisualStyleBackColor = false;
            this.btnBotSend.Click += new System.EventHandler(this.btnBotSend_Click);
            // 
            // timerBot
            // 
            this.timerBot.Interval = 2000;
            this.timerBot.Tick += new System.EventHandler(this.timerBot_Tick);
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(154, 489);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(146, 33);
            this.btnPost.TabIndex = 7;
            this.btnPost.Text = "Send external";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnSendMalformedData
            // 
            this.btnSendMalformedData.Location = new System.Drawing.Point(12, 489);
            this.btnSendMalformedData.Name = "btnSendMalformedData";
            this.btnSendMalformedData.Size = new System.Drawing.Size(136, 33);
            this.btnSendMalformedData.TabIndex = 8;
            this.btnSendMalformedData.Text = "Send Malformed";
            this.btnSendMalformedData.UseVisualStyleBackColor = true;
            this.btnSendMalformedData.Click += new System.EventHandler(this.btnSendMalformedData_Click);
            // 
            // btnRebuildFromNetwork
            // 
            this.btnRebuildFromNetwork.Font = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRebuildFromNetwork.Location = new System.Drawing.Point(306, 489);
            this.btnRebuildFromNetwork.Name = "btnRebuildFromNetwork";
            this.btnRebuildFromNetwork.Size = new System.Drawing.Size(124, 33);
            this.btnRebuildFromNetwork.TabIndex = 9;
            this.btnRebuildFromNetwork.Text = "Rebuild Chat Data";
            this.btnRebuildFromNetwork.UseVisualStyleBackColor = true;
            this.btnRebuildFromNetwork.Click += new System.EventHandler(this.btnRebuildFromNetwork_Click);
            // 
            // NodeGUI
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(744, 534);
            this.Controls.Add(this.btnRebuildFromNetwork);
            this.Controls.Add(this.btnSendMalformedData);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.btnBotSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.lstKnownNodes);
            this.Controls.Add(this.btnKill);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtOutput);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Button btnSort;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmrPing;
        private System.Windows.Forms.Button btnBotSend;
        private System.Windows.Forms.Timer timerBot;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnSendMalformedData;
        private System.Windows.Forms.Button btnRebuildFromNetwork;
    }
}