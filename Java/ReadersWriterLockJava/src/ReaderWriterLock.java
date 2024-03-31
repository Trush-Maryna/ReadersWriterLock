import java.util.HashSet;

class ReaderWriterLock {
    private int currentReaderCount;
    private final HashSet<Object> globalMutex = new HashSet<>();
    private final ReadLock readerLock;
    private final WriteLock writerLock;

    public ReaderWriterLock() {
        currentReaderCount = 0;
        Object readerMutex = new Object();
        readerLock = new ReadLock(readerMutex, globalMutex, this);
        writerLock = new WriteLock(globalMutex);
    }

    public ILock readLock() {
        return readerLock;
    }

    public ILock writeLock() {
        return writerLock;
    }

    private boolean doesWriterOwnThisLock() {
        return globalMutex.contains(writerLock);
    }

    private boolean isLockFree() {
        return globalMutex.isEmpty();
    }

    private record ReadLock(Object readerMutex, HashSet<Object> globalMutex, ReaderWriterLock parent) implements ILock {

        @Override
            public void lock() {
                synchronized (readerMutex) {
                    parent.currentReaderCount++;
                    if (parent.currentReaderCount == 1) {
                        synchronized (globalMutex) {
                            globalMutex.add(this);
                        }
                    }
                }
            }

            @Override
            public void unlock() {
                synchronized (readerMutex) {
                    parent.currentReaderCount--;

                    if (parent.currentReaderCount == 0) {
                        synchronized (globalMutex) {
                            globalMutex.remove(this);
                            globalMutex.notifyAll();
                        }
                    }
                }
            }
        }

    private record WriteLock(HashSet<Object> globalMutex) implements ILock {

        @Override
            public void lock() {
                synchronized (globalMutex) {
                    while (!globalMutex.isEmpty()) {
                        try {
                            globalMutex.wait();
                        } catch (InterruptedException e) {
                            Thread.currentThread().interrupt();
                        }
                    }
                    globalMutex.add(this);
                }
            }

            @Override
            public void unlock() {
                synchronized (globalMutex) {
                    globalMutex.remove(this);
                    globalMutex.notifyAll();
                }
            }
        }
}