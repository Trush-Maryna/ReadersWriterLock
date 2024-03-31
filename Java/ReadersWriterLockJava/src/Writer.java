import javax.swing.*;
import java.util.concurrent.locks.Lock;

class Writer implements Runnable {
    private final Lock writeLock;
    private final String name;
    private final int writingTime;
    private final JTextArea txtBox;

    public Writer(String name, Lock writeLock, int writingTime, JTextArea txtBox) {
        this.name = name;
        this.writeLock = writeLock;
        this.writingTime = writingTime;
        this.txtBox = txtBox;
    }

    public Writer(String name, Lock writeLock, JTextArea txtBox) {
        this(name, writeLock, 250, txtBox);
    }

    public void write() {
        SwingUtilities.invokeLater(() -> txtBox.append(name + " begin "));
        try {
            Thread.sleep(writingTime);
        } catch (InterruptedException e) {
            txtBox.append("\nInterruptedException when writing: " + e + "\n");
            Thread.currentThread().interrupt();
        }
        SwingUtilities.invokeLater(() -> txtBox.append("\n" + name + " finish after writing " + writingTime + "ms\n"));
    }

    @Override
    public void run() {
        writeLock.lock();
        try {
            write();
        } finally {
            writeLock.unlock();
        }
    }
}