namespace ReadersWriterLock
{
    public partial class Library : Form
    {
        private readonly ReaderWriterLock readWriteLock = new ReaderWriterLock();
        private readonly Random random = new Random();
        private bool textBoxUpdateRequested = false;

        public Library()
        {
            InitializeComponent();
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            TxtBox.Clear();
            textBoxUpdateRequested = true;
            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= 5; i++)
            {
                Reader reader = new Reader($"Reader {i}", readWriteLock.readLock(), random.Next(200, 400), TxtBox);
                tasks.Add(Task.Run(() => reader.Run()));
            }

            for (int i = 1; i <= 2; i++)
            {
                Writer writer = new Writer($"Writer {i}", readWriteLock.writeLock(), random.Next(200, 400), TxtBox);
                tasks.Add(Task.Run(() => writer.Run()));
            }

            await Task.WhenAll(tasks);
        }

        public bool IsTextBoxUpdateRequested()
        {
            lock (this)
            {
                return textBoxUpdateRequested;
            }
        }

        public void ResetTextBoxUpdateFlag()
        {
            lock (this)
            {
                textBoxUpdateRequested = false;
            }
        }
    }
}