namespace UlterrariaLauncher
{
    partial class AchievementPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AchieveDesc = new System.Windows.Forms.Label();
            this.achieveTitle = new System.Windows.Forms.Label();
            this.achieveImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.achieveImg)).BeginInit();
            this.SuspendLayout();
            // 
            // AchieveDesc
            // 
            this.AchieveDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AchieveDesc.Location = new System.Drawing.Point(65, 22);
            this.AchieveDesc.Name = "AchieveDesc";
            this.AchieveDesc.Size = new System.Drawing.Size(302, 39);
            this.AchieveDesc.TabIndex = 5;
            this.AchieveDesc.Text = "Witty description of achievement";
            // 
            // achieveTitle
            // 
            this.achieveTitle.AutoSize = true;
            this.achieveTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.achieveTitle.Location = new System.Drawing.Point(62, 0);
            this.achieveTitle.Name = "achieveTitle";
            this.achieveTitle.Size = new System.Drawing.Size(35, 18);
            this.achieveTitle.TabIndex = 4;
            this.achieveTitle.Text = "Title";
            // 
            // achieveImg
            // 
            this.achieveImg.Location = new System.Drawing.Point(0, 0);
            this.achieveImg.Name = "achieveImg";
            this.achieveImg.Size = new System.Drawing.Size(61, 61);
            this.achieveImg.TabIndex = 3;
            this.achieveImg.TabStop = false;
            // 
            // AchievementPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AchieveDesc);
            this.Controls.Add(this.achieveTitle);
            this.Controls.Add(this.achieveImg);
            this.Name = "AchievementPanel";
            this.Size = new System.Drawing.Size(370, 62);
            ((System.ComponentModel.ISupportInitialize)(this.achieveImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AchieveDesc;
        private System.Windows.Forms.Label achieveTitle;
        private System.Windows.Forms.PictureBox achieveImg;

    }
}
