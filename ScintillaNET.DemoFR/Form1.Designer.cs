namespace ScintillaNET.DemoFR
{
	partial class Form1
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
			if (disposing && (components != null)) {
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
			this.m_rPanel = new System.Windows.Forms.Panel();
			this.m_rButtonSearch = new System.Windows.Forms.Button();
			this.m_rButtonSetReadOnly = new System.Windows.Forms.Button();
			this.m_rButtonSetReadWrite = new System.Windows.Forms.Button();
			this.m_rCheckBoxReadOnly = new System.Windows.Forms.CheckBox();
			this.m_rButtonCopyIntoClipboard = new System.Windows.Forms.Button();
			this.m_rButtonReadFromClipboard = new System.Windows.Forms.Button();
			this.m_rButtonCopyRTF = new System.Windows.Forms.Button();
			this.m_rButton_GoToLine = new System.Windows.Forms.Button();
			this.m_rButtonTestDeleteRange = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_rPanel
			// 
			this.m_rPanel.BackColor = System.Drawing.Color.Linen;
			this.m_rPanel.Location = new System.Drawing.Point(12, 216);
			this.m_rPanel.Name = "m_rPanel";
			this.m_rPanel.Size = new System.Drawing.Size(677, 261);
			this.m_rPanel.TabIndex = 1;
			// 
			// m_rButtonSearch
			// 
			this.m_rButtonSearch.Location = new System.Drawing.Point(12, 28);
			this.m_rButtonSearch.Name = "m_rButtonSearch";
			this.m_rButtonSearch.Size = new System.Drawing.Size(100, 37);
			this.m_rButtonSearch.TabIndex = 2;
			this.m_rButtonSearch.Text = "Search";
			this.m_rButtonSearch.UseVisualStyleBackColor = true;
			this.m_rButtonSearch.Click += new System.EventHandler(this.m_rButtonSearch_Click);
			// 
			// m_rButtonSetReadOnly
			// 
			this.m_rButtonSetReadOnly.BackColor = System.Drawing.Color.LightCoral;
			this.m_rButtonSetReadOnly.Location = new System.Drawing.Point(161, 31);
			this.m_rButtonSetReadOnly.Name = "m_rButtonSetReadOnly";
			this.m_rButtonSetReadOnly.Size = new System.Drawing.Size(96, 34);
			this.m_rButtonSetReadOnly.TabIndex = 3;
			this.m_rButtonSetReadOnly.Text = "Set ReadOnly";
			this.m_rButtonSetReadOnly.UseVisualStyleBackColor = false;
			this.m_rButtonSetReadOnly.Click += new System.EventHandler(this.m_rButtonSetReadOnly_Click);
			// 
			// m_rButtonSetReadWrite
			// 
			this.m_rButtonSetReadWrite.BackColor = System.Drawing.Color.LightGreen;
			this.m_rButtonSetReadWrite.Location = new System.Drawing.Point(161, 71);
			this.m_rButtonSetReadWrite.Name = "m_rButtonSetReadWrite";
			this.m_rButtonSetReadWrite.Size = new System.Drawing.Size(96, 34);
			this.m_rButtonSetReadWrite.TabIndex = 4;
			this.m_rButtonSetReadWrite.Text = "Set ReadWrite";
			this.m_rButtonSetReadWrite.UseVisualStyleBackColor = false;
			this.m_rButtonSetReadWrite.Click += new System.EventHandler(this.m_rButtonSetReadWrite_Click);
			// 
			// m_rCheckBoxReadOnly
			// 
			this.m_rCheckBoxReadOnly.AutoSize = true;
			this.m_rCheckBoxReadOnly.Location = new System.Drawing.Point(161, 112);
			this.m_rCheckBoxReadOnly.Name = "m_rCheckBoxReadOnly";
			this.m_rCheckBoxReadOnly.Size = new System.Drawing.Size(90, 17);
			this.m_rCheckBoxReadOnly.TabIndex = 5;
			this.m_rCheckBoxReadOnly.Text = "set ReadOnly";
			this.m_rCheckBoxReadOnly.UseVisualStyleBackColor = true;
			this.m_rCheckBoxReadOnly.CheckedChanged += new System.EventHandler(this.m_rCheckBoxReadOnly_CheckedChanged);
			// 
			// m_rButtonCopyIntoClipboard
			// 
			this.m_rButtonCopyIntoClipboard.Location = new System.Drawing.Point(276, 32);
			this.m_rButtonCopyIntoClipboard.Name = "m_rButtonCopyIntoClipboard";
			this.m_rButtonCopyIntoClipboard.Size = new System.Drawing.Size(115, 32);
			this.m_rButtonCopyIntoClipboard.TabIndex = 6;
			this.m_rButtonCopyIntoClipboard.Text = "Copy into Clipboard";
			this.m_rButtonCopyIntoClipboard.UseVisualStyleBackColor = true;
			this.m_rButtonCopyIntoClipboard.Click += new System.EventHandler(this.m_rButtonCopyIntoClipboard_Click);
			// 
			// m_rButtonReadFromClipboard
			// 
			this.m_rButtonReadFromClipboard.Location = new System.Drawing.Point(276, 70);
			this.m_rButtonReadFromClipboard.Name = "m_rButtonReadFromClipboard";
			this.m_rButtonReadFromClipboard.Size = new System.Drawing.Size(115, 32);
			this.m_rButtonReadFromClipboard.TabIndex = 7;
			this.m_rButtonReadFromClipboard.Text = "Read Clipboard";
			this.m_rButtonReadFromClipboard.UseVisualStyleBackColor = true;
			this.m_rButtonReadFromClipboard.Click += new System.EventHandler(this.m_rButtonReadFromClipboard_Click);
			// 
			// m_rButtonCopyRTF
			// 
			this.m_rButtonCopyRTF.Location = new System.Drawing.Point(397, 31);
			this.m_rButtonCopyRTF.Name = "m_rButtonCopyRTF";
			this.m_rButtonCopyRTF.Size = new System.Drawing.Size(115, 32);
			this.m_rButtonCopyRTF.TabIndex = 8;
			this.m_rButtonCopyRTF.Text = "Copy RTF";
			this.m_rButtonCopyRTF.UseVisualStyleBackColor = true;
			this.m_rButtonCopyRTF.Click += new System.EventHandler(this.m_rButtonCopyRTF_Click);
			// 
			// m_rButton_GoToLine
			// 
			this.m_rButton_GoToLine.Location = new System.Drawing.Point(397, 71);
			this.m_rButton_GoToLine.Name = "m_rButton_GoToLine";
			this.m_rButton_GoToLine.Size = new System.Drawing.Size(75, 23);
			this.m_rButton_GoToLine.TabIndex = 9;
			this.m_rButton_GoToLine.Text = "GoTo line";
			this.m_rButton_GoToLine.UseVisualStyleBackColor = true;
			this.m_rButton_GoToLine.Click += new System.EventHandler(this.m_rButton_GoToLine_Click);
			// 
			// m_rButtonTestDeleteRange
			// 
			this.m_rButtonTestDeleteRange.Location = new System.Drawing.Point(478, 70);
			this.m_rButtonTestDeleteRange.Name = "m_rButtonTestDeleteRange";
			this.m_rButtonTestDeleteRange.Size = new System.Drawing.Size(110, 35);
			this.m_rButtonTestDeleteRange.TabIndex = 10;
			this.m_rButtonTestDeleteRange.Text = "TestDeleteRange";
			this.m_rButtonTestDeleteRange.UseVisualStyleBackColor = true;
			this.m_rButtonTestDeleteRange.Click += new System.EventHandler(this.m_rButtonTestDeleteRange_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 507);
			this.Controls.Add(this.m_rButtonTestDeleteRange);
			this.Controls.Add(this.m_rButton_GoToLine);
			this.Controls.Add(this.m_rButtonCopyRTF);
			this.Controls.Add(this.m_rButtonReadFromClipboard);
			this.Controls.Add(this.m_rButtonCopyIntoClipboard);
			this.Controls.Add(this.m_rCheckBoxReadOnly);
			this.Controls.Add(this.m_rButtonSetReadWrite);
			this.Controls.Add(this.m_rButtonSetReadOnly);
			this.Controls.Add(this.m_rButtonSearch);
			this.Controls.Add(this.m_rPanel);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Panel m_rPanel;
		private System.Windows.Forms.Button m_rButtonSearch;
		private System.Windows.Forms.Button m_rButtonSetReadOnly;
		private System.Windows.Forms.Button m_rButtonSetReadWrite;
		private System.Windows.Forms.CheckBox m_rCheckBoxReadOnly;
		private System.Windows.Forms.Button m_rButtonCopyIntoClipboard;
		private System.Windows.Forms.Button m_rButtonReadFromClipboard;
		private System.Windows.Forms.Button m_rButtonCopyRTF;
		private System.Windows.Forms.Button m_rButton_GoToLine;
		private System.Windows.Forms.Button m_rButtonTestDeleteRange;
	}
}

