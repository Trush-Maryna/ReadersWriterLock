namespace ReadersWriterLock
{
    internal class Reader
    {
        private readonly ILock readLock;
        private readonly string name;
        private readonly int readingTime;
        private readonly TextBox txtBox;

        public Reader(string name, ILock readLock, int readingTime, TextBox txtBox)
        {
            this.name = name;
            this.readLock = readLock;
            this.readingTime = readingTime;
            this.txtBox = txtBox;
        }

        public Reader(string name, ILock readLock, TextBox txtBox) : this(name, readLock, 250, txtBox)
        {
        }

        public void Read()
        {
            txtBox.BeginInvoke(new Action(() => txtBox.AppendText($"{name} begin ")));
            Thread.Sleep(readingTime);
            txtBox.BeginInvoke(new Action(() => txtBox.AppendText($"\r\n{name} finish after reading {readingTime}ms \r\n")));
        }

        public void Run()
        {
            readLock.Lock();
            try
            {
                Read();
            }
            catch (ThreadInterruptedException e)
            {
                txtBox.Invoke(new Action(() => txtBox.AppendText($"InterruptedException when reading: {e} \r\n")));
                Thread.CurrentThread.Interrupt();
            }
            finally
            {
                readLock.Unlock();
            }
        }
    }
}
