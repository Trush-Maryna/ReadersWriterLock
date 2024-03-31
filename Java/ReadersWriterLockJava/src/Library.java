import javax.swing.*;
import java.awt.event.ActionEvent;
import java.util.Random;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class Library extends JFrame {
    private final ReadWriteLock readWriteLock = new ReentrantReadWriteLock();
    private final Random random = new Random();
    private boolean textBoxUpdateRequested = false;
    private JTextArea txtBox;

    public Library() {
        initComponents();
    }

    private void initComponents() {
        setTitle("Library");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(400, 300);
        setLocationRelativeTo(null);

        txtBox = new JTextArea();
        JScrollPane scrollPane = new JScrollPane(txtBox);
        getContentPane().add(scrollPane);

        JButton btnStart = new JButton("Start");
        btnStart.addActionListener(this::btnStartActionPerformed);
        getContentPane().add(btnStart, "South");
    }

    private void btnStartActionPerformed(ActionEvent evt) {
        txtBox.setText("");
        textBoxUpdateRequested = true;
        for (int i = 1; i <= 5; i++) {
            Reader reader = new Reader("Reader " + i, readWriteLock.readLock(), random.nextInt(200) + 200, txtBox);
            new Thread(reader).start();
        }

        for (int i = 1; i <= 2; i++) {
            Writer writer = new Writer("Writer " + i, readWriteLock.writeLock(), random.nextInt(200) + 200, txtBox);
            new Thread(writer).start();
        }
    }

    public boolean isTextBoxUpdateRequested() {
        return textBoxUpdateRequested;
    }

    public void resetTextBoxUpdateFlag() {
        textBoxUpdateRequested = false;
    }

    public static void main(String[] args) {
        java.awt.EventQueue.invokeLater(() -> new Library().setVisible(true));
    }
}