﻿namespace ScintillaNET.DemoFR
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
			this.m_rButtonSearch.Location = new System.Drawing.Point(391, 28);
			this.m_rButtonSearch.Name = "m_rButtonSearch";
			this.m_rButtonSearch.Size = new System.Drawing.Size(115, 37);
			this.m_rButtonSearch.TabIndex = 2;
			this.m_rButtonSearch.Text = "Search";
			this.m_rButtonSearch.UseVisualStyleBackColor = true;
			this.m_rButtonSearch.Click += new System.EventHandler(this.m_rButtonSearch_Click);
			// 
			// m_rButtonSetReadOnly
			// 
			this.m_rButtonSetReadOnly.BackColor = System.Drawing.Color.LightCoral;
			this.m_rButtonSetReadOnly.Location = new System.Drawing.Point(540, 31);
			this.m_rButtonSetReadOnly.Name = "m_rButtonSetReadOnly";
			this.m_rButtonSetReadOnly.Size = new System.Drawing.Size(111, 34);
			this.m_rButtonSetReadOnly.TabIndex = 3;
			this.m_rButtonSetReadOnly.Text = "Set ReadOnly";
			this.m_rButtonSetReadOnly.UseVisualStyleBackColor = false;
			this.m_rButtonSetReadOnly.Click += new System.EventHandler(this.m_rButtonSetReadOnly_Click);
			// 
			// m_rButtonSetReadWrite
			// 
			this.m_rButtonSetReadWrite.BackColor = System.Drawing.Color.LightGreen;
			this.m_rButtonSetReadWrite.Location = new System.Drawing.Point(540, 71);
			this.m_rButtonSetReadWrite.Name = "m_rButtonSetReadWrite";
			this.m_rButtonSetReadWrite.Size = new System.Drawing.Size(111, 34);
			this.m_rButtonSetReadWrite.TabIndex = 4;
			this.m_rButtonSetReadWrite.Text = "Set ReadWrite";
			this.m_rButtonSetReadWrite.UseVisualStyleBackColor = false;
			this.m_rButtonSetReadWrite.Click += new System.EventHandler(this.m_rButtonSetReadWrite_Click);
			// 
			// m_rCheckBoxReadOnly
			// 
			this.m_rCheckBoxReadOnly.AutoSize = true;
			this.m_rCheckBoxReadOnly.Location = new System.Drawing.Point(540, 112);
			this.m_rCheckBoxReadOnly.Name = "m_rCheckBoxReadOnly";
			this.m_rCheckBoxReadOnly.Size = new System.Drawing.Size(90, 17);
			this.m_rCheckBoxReadOnly.TabIndex = 5;
			this.m_rCheckBoxReadOnly.Text = "set ReadOnly";
			this.m_rCheckBoxReadOnly.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 507);
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
	}
}

