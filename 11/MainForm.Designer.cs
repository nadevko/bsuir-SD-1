namespace sd1._11
{
	partial class MainForm
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
			textBoxCoordinates = new TextBox();
			textBoxA = new TextBox();
			textBoxB = new TextBox();
			textBoxResult = new TextBox();
			SuspendLayout();
			//
			// textBoxCoordinates
			//
			textBoxCoordinates.Location = new Point(12, 12);
			textBoxCoordinates.Name = "textBoxCoordinates";
			textBoxCoordinates.ReadOnly = true;
			textBoxCoordinates.Size = new Size(260, 23);
			textBoxCoordinates.TabIndex = 0;
			textBoxCoordinates.TextAlign = HorizontalAlignment.Center;
			//
			// textBoxA
			//
			textBoxA.Location = new Point(12, 41);
			textBoxA.Name = "textBoxA";
			textBoxA.Size = new Size(70, 23);
			textBoxA.TabIndex = 1;
			textBoxA.TextChanged += textBoxResult_UpdateValue;
			//
			// textBoxB
			//
			textBoxB.Location = new Point(88, 41);
			textBoxB.Name = "textBoxB";
			textBoxB.Size = new Size(70, 23);
			textBoxB.TabIndex = 2;
			textBoxB.TextChanged += textBoxResult_UpdateValue;
			//
			// textBoxResult
			//
			textBoxResult.Location = new Point(164, 41);
			textBoxResult.Name = "textBoxResult";
			textBoxResult.ReadOnly = true;
			textBoxResult.Size = new Size(108, 23);
			textBoxResult.TabIndex = 3;
			textBoxResult.MouseClick += textBoxResult_MouseClick;
			//
			// MainForm
			//
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(284, 73);
			Controls.Add(textBoxResult);
			Controls.Add(textBoxB);
			Controls.Add(textBoxA);
			Controls.Add(textBoxCoordinates);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterParent;
			Text = "MainForm";
			MouseMove += MainForm_MouseMove;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBoxCoordinates;
		private TextBox textBoxA;
		private TextBox textBoxB;
		private TextBox textBoxResult;
	}
}
