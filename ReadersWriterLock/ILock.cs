namespace ReadersWriterLock
{
    public interface ILock
    {
        void Lock();
        void Unlock();
    }
}
