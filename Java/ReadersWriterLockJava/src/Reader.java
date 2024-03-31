import javax.swing.*;
import java.util.concurrent.locks.Lock;

class Reader implements Runnable {
    private final Lock readLock;
    private final String name;
    private final int readingTime;
    private final JTextArea txtBox;

    public Reader(String name, Lock readLock, int readingTime, JTextArea txtBox) {
        this.name = name;
        this.readLock = readLock;
        this.readingTime = readingTime;
        this.txtBox = txtBox;
    }

    public Reader(String name, Lock readLock, JTextArea txtBox) {
        this(name, readLock, 250, txtBox);
    }

    public void read() {
        SwingUtilities.invokeLater(() -> txtBox.append(name + " begin "));
        try {
            Thread.sleep(readingTime);
        } catch (InterruptedException e) {
            txtBox.append("\nInterruptedException when reading: " + e + "\n");
            Thread.currentThread().interrupt();
        }
        SwingUtilities.invokeLater(() -> txtBox.append("\n" + name + " finish after reading " + readingTime + "ms\n"));
    }

    @Override
    public void run() {
        readLock.lock();
        try {
            read();
        } finally {
            readLock.unlock();
        }
    }
}