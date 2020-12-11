namespace StarFoxBrowser
{
	partial class BrowserForm
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView = new System.Windows.Forms.TreeView();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.propertiesTab = new System.Windows.Forms.TabPage();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.previewTab = new System.Windows.Forms.TabPage();
			this.panel = new System.Windows.Forms.Panel();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.propertiesTab.SuspendLayout();
			this.previewTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl);
			this.splitContainer1.Size = new System.Drawing.Size(784, 561);
			this.splitContainer1.SplitterDistance = 261;
			this.splitContainer1.TabIndex = 0;
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.HideSelection = false;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(261, 561);
			this.treeView.TabIndex = 0;
			this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// tabControl
			// 
			this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl.Controls.Add(this.propertiesTab);
			this.tabControl.Controls.Add(this.previewTab);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(519, 561);
			this.tabControl.TabIndex = 2;
			// 
			// propertiesTab
			// 
			this.propertiesTab.Controls.Add(this.propertyGrid);
			this.propertiesTab.Location = new System.Drawing.Point(4, 4);
			this.propertiesTab.Name = "propertiesTab";
			this.propertiesTab.Padding = new System.Windows.Forms.Padding(3);
			this.propertiesTab.Size = new System.Drawing.Size(511, 535);
			this.propertiesTab.TabIndex = 0;
			this.propertiesTab.Text = "Properties";
			this.propertiesTab.UseVisualStyleBackColor = true;
			// 
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(3, 3);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(505, 529);
			this.propertyGrid.TabIndex = 3;
			// 
			// previewTab
			// 
			this.previewTab.Controls.Add(this.panel);
			this.previewTab.Controls.Add(this.pictureBox);
			this.previewTab.Location = new System.Drawing.Point(4, 4);
			this.previewTab.Name = "previewTab";
			this.previewTab.Padding = new System.Windows.Forms.Padding(3);
			this.previewTab.Size = new System.Drawing.Size(511, 535);
			this.previewTab.TabIndex = 1;
			this.previewTab.Text = "Preview";
			this.previewTab.UseVisualStyleBackColor = true;
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(3, 3);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(505, 529);
			this.panel.TabIndex = 4;
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(3, 3);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(505, 529);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 3;
			this.pictureBox.TabStop = false;
			// 
			// BrowserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.splitContainer1);
			this.Name = "BrowserForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Star Fox Browser";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.propertiesTab.ResumeLayout(false);
			this.previewTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage propertiesTab;
		private System.Windows.Forms.TabPage previewTab;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Panel panel;
	}
}

