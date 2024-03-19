namespace ReadersWriterLock
{
    internal class Writer
    {
        private readonly ILock writeLock;
        private readonly string name;
        private readonly int writingTime;
        private readonly TextBox txtBox;

        public Writer(string name, ILock writeLock, int writingTime, TextBox txtBox)
        {
            this.name = name;
            this.writeLock = writeLock;
            this.writingTime = writingTime;
            this.txtBox = txtBox;
        }

        public Writer(string name, ILock writeLock, TextBox txtBox) : this(name, writeLock, 250, txtBox)
        {
        }

        public void Write()
        {
            txtBox.BeginInvoke(new Action(() => txtBox.AppendText($"{name} begin ")));
            Thread.Sleep(writingTime);
            txtBox.BeginInvoke(new Action(() => txtBox.AppendText($"\r\n{name} finish after reading {writingTime}ms \r\n")));
        }

        public void Run()
        {
            writeLock.Lock();
            try
            {
                Write();
            }
            catch (ThreadInterruptedException e)
            {
                txtBox.Invoke(new Action(() => txtBox.AppendText($"InterruptedException when writing: {e} \r\n")));
                Thread.CurrentThread.Interrupt();
            }
            finally
            {
                writeLock.Unlock();
            }
        }
    }
}
