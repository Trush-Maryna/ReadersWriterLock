namespace ReadersWriterLock
{
    public class ReaderWriterLock
    {
        private readonly object readerMutex = new object();
        private int currentReaderCount;

        private readonly HashSet<object> globalMutex = new HashSet<object>();
        private readonly ReadLock readerLock;
        private readonly WriteLock writerLock;

        public ReaderWriterLock()
        {
            currentReaderCount = 0;
            readerLock = new ReadLock(readerMutex, globalMutex, this);
            writerLock = new WriteLock(globalMutex);
        }

        public ILock readLock()
        {
            return readerLock;
        }

        public ILock writeLock()
        {
            return writerLock;
        }

        private bool DoesWriterOwnThisLock()
        {
            return globalMutex.Contains(writerLock);
        }

        private bool IsLockFree()
        {
            return globalMutex.Count == 0;
        }

        private class ReadLock : ILock
        {
            private readonly object readerMutex;
            private readonly HashSet<object> globalMutex;
            private readonly ReaderWriterLock parent;

            public ReadLock(object readerMutex, HashSet<object> globalMutex, ReaderWriterLock parent)
            {
                this.readerMutex = readerMutex;
                this.globalMutex = globalMutex;
                this.parent = parent;
            }

            public void Lock()
            {
                lock (readerMutex)
                {
                    parent.currentReaderCount++;
                    if (parent.currentReaderCount == 1)
                    {
                        Monitor.Enter(globalMutex);
                    }
                }
            }

            public void Unlock()
            {
                lock (readerMutex)
                {
                    parent.currentReaderCount--;

                    if (parent.currentReaderCount == 0)
                    {
                        lock (globalMutex)
                        {
                            globalMutex.Remove(this);
                            Monitor.PulseAll(globalMutex);
                        }
                    }
                }
            }
        }

        private class WriteLock : ILock
        {
            private readonly HashSet<object> globalMutex;

            public WriteLock(HashSet<object> globalMutex)
            {
                this.globalMutex = globalMutex;
            }

            public void Lock()
            {
                lock (globalMutex)
                {
                    while (globalMutex.Count != 0)
                    {
                        Monitor.Wait(globalMutex);
                    }
                    globalMutex.Add(this);
                }
            }

            public void Unlock()
            {
                lock (globalMutex)
                {
                    globalMutex.Remove(this);
                    Monitor.PulseAll(globalMutex);
                }
            }
        }
    }
}
