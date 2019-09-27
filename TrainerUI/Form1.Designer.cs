namespace MnistTestUi
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
			this.openButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.buttonClear = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.textBoxParsedDigit = new System.Windows.Forms.TextBox();
			this.showButton = new System.Windows.Forms.Button();
			this.textBoxParsedNumber = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.multiTokenDrawPanel1 = new MnistTestUi.MultiTokenDrawPanel();
			this.drawPanel = new MnistTestUi.OneDigitDrawPanel();
			this.SuspendLayout();
			// 
			// openButton
			// 
			this.openButton.Location = new System.Drawing.Point(544, 174);
			this.openButton.Margin = new System.Windows.Forms.Padding(4);
			this.openButton.Name = "openButton";
			this.openButton.Size = new System.Drawing.Size(96, 65);
			this.openButton.TabIndex = 0;
			this.openButton.Text = "Open NN";
			this.openButton.UseVisualStyleBackColor = true;
			this.openButton.Click += new System.EventHandler(this.openButton_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(543, 102);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(96, 65);
			this.buttonClear.TabIndex = 2;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(543, 31);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 65);
			this.button1.TabIndex = 3;
			this.button1.Text = "Query";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBoxParsedDigit
			// 
			this.textBoxParsedDigit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxParsedDigit.Location = new System.Drawing.Point(303, 31);
			this.textBoxParsedDigit.Multiline = true;
			this.textBoxParsedDigit.Name = "textBoxParsedDigit";
			this.textBoxParsedDigit.Size = new System.Drawing.Size(234, 280);
			this.textBoxParsedDigit.TabIndex = 4;
			// 
			// showButton
			// 
			this.showButton.Location = new System.Drawing.Point(544, 246);
			this.showButton.Name = "showButton";
			this.showButton.Size = new System.Drawing.Size(96, 65);
			this.showButton.TabIndex = 5;
			this.showButton.Text = "Show Training Images";
			this.showButton.UseVisualStyleBackColor = true;
			this.showButton.Click += new System.EventHandler(this.showButton_Click);
			// 
			// textBoxParsedNumber
			// 
			this.textBoxParsedNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxParsedNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxParsedNumber.Location = new System.Drawing.Point(963, 337);
			this.textBoxParsedNumber.Multiline = true;
			this.textBoxParsedNumber.Name = "textBoxParsedNumber";
			this.textBoxParsedNumber.Size = new System.Drawing.Size(109, 201);
			this.textBoxParsedNumber.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 17);
			this.label1.TabIndex = 8;
			this.label1.Text = "Draw any digit here";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 316);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 17);
			this.label2.TabIndex = 9;
			this.label2.Text = "Draw any number here";
			// 
			// multiTokenDrawPanel1
			// 
			this.multiTokenDrawPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.multiTokenDrawPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.multiTokenDrawPanel1.Location = new System.Drawing.Point(18, 337);
			this.multiTokenDrawPanel1.Name = "multiTokenDrawPanel1";
			this.multiTokenDrawPanel1.Size = new System.Drawing.Size(939, 201);
			this.multiTokenDrawPanel1.TabIndex = 6;
			// 
			// drawPanel
			// 
			this.drawPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.drawPanel.Location = new System.Drawing.Point(16, 31);
			this.drawPanel.Margin = new System.Windows.Forms.Padding(4);
			this.drawPanel.Name = "drawPanel";
			this.drawPanel.Size = new System.Drawing.Size(280, 280);
			this.drawPanel.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1084, 553);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxParsedNumber);
			this.Controls.Add(this.multiTokenDrawPanel1);
			this.Controls.Add(this.showButton);
			this.Controls.Add(this.textBoxParsedDigit);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.drawPanel);
			this.Controls.Add(this.openButton);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "Mnist Test Form";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button openButton;
		private OneDigitDrawPanel drawPanel;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBoxParsedDigit;
		private System.Windows.Forms.Button showButton;
		private MultiTokenDrawPanel multiTokenDrawPanel1;
		private System.Windows.Forms.TextBox textBoxParsedNumber;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

