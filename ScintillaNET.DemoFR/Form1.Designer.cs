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
			this.SuspendLayout();
			// 
			// m_rPanel
			// 
			this.m_rPanel.BackColor = System.Drawing.Color.Linen;
			this.m_rPanel.Location = new System.Drawing.Point(24, 125);
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.m_rButtonSearch);
			this.Controls.Add(this.m_rPanel);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel m_rPanel;
		private System.Windows.Forms.Button m_rButtonSearch;
	}
}

