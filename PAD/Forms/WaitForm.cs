using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAD
{
    public partial class WaitForm : Form
    {
        public Action Worker { get; set; }
        private ELanguage _activeLang;

        public WaitForm(Action worker, ELanguage lang)
        {
            InitializeComponent();

            if (worker == null)
                throw new ArgumentNullException();
            Worker = worker;
            _activeLang = lang;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Utility.LocalizeForm(this, _activeLang);
            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
