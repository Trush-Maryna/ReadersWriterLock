namespace ReadersWriterLock
{
    partial class Library
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnStart = new Button();
            TxtBox = new TextBox();
            SuspendLayout();
            // 
            // BtnStart
            // 
            BtnStart.BackColor = Color.DarkKhaki;
            BtnStart.Dock = DockStyle.Bottom;
            BtnStart.Font = new Font("Stencil", 15F);
            BtnStart.Location = new Point(0, 390);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(800, 60);
            BtnStart.TabIndex = 1;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = false;
            BtnStart.Click += BtnStart_Click;
            // 
            // TxtBox
            // 
            TxtBox.Dock = DockStyle.Top;
            TxtBox.Font = new Font("Segoe UI", 10F);
            TxtBox.Location = new Point(0, 0);
            TxtBox.Multiline = true;
            TxtBox.Name = "TxtBox";
            TxtBox.Size = new Size(800, 389);
            TxtBox.TabIndex = 2;
            // 
            // Library
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TxtBox);
            Controls.Add(BtnStart);
            Name = "Library";
            Text = "Library";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnStart;
        private TextBox TxtBox;
    }
}
