namespace sd1._11
{
	public partial class MainForm : Form
	{
		public MainForm() =>
			InitializeComponent();

		private void MainForm_MouseMove(object? sender, MouseEventArgs e)
		{
			Text = $"({e.X};{e.Y})";
			textBoxCoordinates.Text = $"{e.X + e.Y}";
		}

		private void textBoxResult_UpdateValue(object? sender, EventArgs e)
		{
			_ = int.TryParse(textBoxA.Text, out int a);
			_ = int.TryParse(textBoxB.Text, out int b);
			textBoxResult.Text = $"{a ^ b}";
		}

		private void textBoxResult_MouseClick(object? sender, EventArgs e) =>
			Clipboard.SetText(textBoxResult.Text);
	}
}
