

namespace SkillsInternationalClient.Controls
{
    public partial class LoadingOverlay : UserControl
    {
        public LoadingOverlay(Form parent)
        {
            InitializeComponent();
            this.Parent = parent;
            this.Parent.Resize += HandleParentResize;

            this.Parent.Controls.Add(this);

            this.Visible = false;
            this.Hide();

            CenterOverlay();

        }

        private void CenterOverlay()
        {
            int overylayWidth = this.Width;
            int overylayHeight = this.Height;

            int parentWidth = this.Parent.ClientSize.Width;
            int parentHeight = this.Parent.ClientSize.Height;

            this.Location = new Point(
            (parentWidth - overylayWidth) / 2,
            (parentHeight - overylayHeight) / 2
            );
        }

        private void HandleParentResize(object? sender, EventArgs e)
        {
            CenterOverlay();

        }


        public void ShowOverlay(string? message = "Loading... Please wait.")
        {
            this.BringToFront();
            this.Show();

            foreach (Control ctrl in this.Parent.Controls)
            {
                if (ctrl != this)
                {
                    ctrl.Enabled = false;
                }
            }

            this.Visible = true;



            this.LabelMessage.Text = message;


        }
        public void HideOverlay()
        {
            this.Visible = false;
            this.Hide();
            this.LabelMessage.Text = "";


            foreach (Control ctrl in this.Parent.Controls)
            {
                ctrl.Enabled = true;
            }
        }
    }
}
