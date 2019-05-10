
package ASCIIfromToConverter;

import static java.awt.Color.BLUE;
import java.awt.Dimension;
import static java.awt.Font.PLAIN;
import java.awt.Graphics;
import java.awt.GraphicsConfiguration;
import java.awt.HeadlessException;
import java.awt.Rectangle;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import static java.lang.Runtime.getRuntime;
import static java.lang.System.exit;
import java.util.logging.Logger;
import static java.util.logging.Logger.getLogger;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextPane;
import static javax.swing.SwingConstants.CENTER;
import static javax.swing.SwingUtilities.invokeLater;


public class MorseFromTwoAsciiConverter extends JFrame {

    private static final long serialVersionUID = 1L;
    private static JTextPane jTextPane = null;
    private static JTextPane jTextPane1 = null;
    private static ImageIcon ICON = new ImageIcon("img\\icon.jpg");  //  @jve:decl-index=0:
    private static ImageIcon BACKGROUND = new ImageIcon("img\\background.jpg");  //  @jve:decl-index=0:
    private static String LANGUAGE = "MORSE";
    private static final Logger LOG = getLogger(MorseFromTwoAsciiConverter.class.getName());
    /**
     * @param args
     */
    public static void main(String[] args) {
//		if(args.length==1) {
//		LANGUAGE = args[0];
        invokeLater(() -> {
    MorseFromTwoAsciiConverter thisClass = new MorseFromTwoAsciiConverter();
    thisClass.setDefaultCloseOperation(EXIT_ON_CLOSE);
    thisClass.setVisible(true);
});
//		}
//		else System.out.println("java -jar MorseFromTwoAsciiConverter.java [LANGUAGE]");
    }

    private JPanel jContentPane = null;

    private JScrollPane jScrollPane = null;

    private JScrollPane jScrollPane1 = null;

    private JButton jButton = null;

    private JButton jButton1 = null;

    private JButton jButton2 = null;

    private JLabel jLabel = null;

    private JLabel jLabel1 = null;


    /**
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter() throws HeadlessException {
        super();
        this.initialize();
    }

    /**
     * @param arg0
     */
    public MorseFromTwoAsciiConverter(GraphicsConfiguration arg0) {
        super(arg0);
        this.initialize();
    }

    /**
     * @param arg0
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter(String arg0) throws HeadlessException {
        super(arg0);
        this.initialize();
    }

    /**
     * @param arg0
     * @param arg1
     */
    public MorseFromTwoAsciiConverter(String arg0, GraphicsConfiguration arg1) {
        super(arg0, arg1);
        this.initialize();
    }

    /**
     * This method initializes jScrollPane
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane() {
        if (this.jScrollPane == null) {
            this.jScrollPane = new JScrollPane();
            this.jScrollPane.setBounds(new Rectangle(15, 60, 136, 136));
            this.jScrollPane.setViewportView(this.getJTextPane());
        }
        return this.jScrollPane;
    }

    /**
     * This method initializes jScrollPane1
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane1() {
        if (this.jScrollPane1 == null) {
            this.jScrollPane1 = new JScrollPane();
            this.jScrollPane1.setBounds(new Rectangle(300, 60, 136, 136));
            this.jScrollPane1.setViewportView(this.getJTextPane1());
        }
        return this.jScrollPane1;
    }

    /**
     * This method initializes jButton
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton() {
        if (this.jButton == null) {
            this.jButton = new JButton();
            this.jButton.setBounds(new Rectangle(165, 75, 121, 31));
            this.jButton.setText("< " + LANGUAGE);
            this.jButton.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert text to target language
                this.saveFile_JTextPane1();
                Runtime r = getRuntime();
                Process p = null;
                try {
                    p = r.exec("TEXT2" + LANGUAGE + ".cmd");
                } catch (IOException ex) {
                    LOG.severe(ex.getMessage());
                }
                this.loadFile_JTextPane();
            });
        }
        return this.jButton;
    }

    /**
     * This method initializes jButton1
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton1() {
        if (this.jButton1 == null) {
            this.jButton1 = new JButton();
            this.jButton1.setBounds(new Rectangle(165, 150, 121, 31));
            this.jButton1.setText("TEXT >");
            this.jButton1.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert language to text
                this.saveFile_JTextPane();
                Runtime r = getRuntime();
                Process p = null;
                try {
                    p = r.exec(LANGUAGE + "2TEXT.cmd");
                } catch (IOException ex) {
                    LOG.severe(ex.getMessage());
                }
                this.loadFile_JTextPane1();
            });
        }
        return this.jButton1;
    }

    /**
     * This method initializes jButton2
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton2() {
        if (this.jButton2 == null) {
            this.jButton2 = new JButton();
            this.jButton2.setBounds(new Rectangle(180, 225, 91, 31));
            this.jButton2.setText("EXIT");
            this.jButton2.addActionListener((java.awt.event.ActionEvent e) -> {
                File file1 = new File("text.txt");
                File file2 = new File(LANGUAGE + ".txt");
                file1.delete();
                file2.delete();
                exit(0);
            });
        }
        return this.jButton2;
    }

    /**
     * This method initializes jTextPane
     *
     * @return javax.swing.JTextPane
     */
    private JTextPane getJTextPane() {
        if (jTextPane == null) {
            jTextPane = new JTextPane();
            this.loadFile_JTextPane();
        }
        return jTextPane;
    }

    /**
     * This method initializes jTextPane1
     *
     * @return javax.swing.JTextPane
     */
    private JTextPane getJTextPane1() {
        if (jTextPane1 == null) {
            jTextPane1 = new JTextPane();
            this.loadFile_JTextPane1();
        }
        return jTextPane1;
    }


    /**
     * This method initializes this
     *
     * @return void
     */
    private void initialize() {
        this.setContentPane(this.getJContentPane());
        this.setIconImage(ICON.getImage());
        this.setTitle(LANGUAGE + " TEXT TRANSLATOR");
        this.setBounds(new Rectangle(280, 220, 460, 305));
        this.setResizable(false);
    }

    /**
     * This method initializes jContentPane
     *
     * @return javax.swing.JPanel
     */
    private JPanel getJContentPane() {
        if (this.jContentPane == null) {
            this.jLabel1 = new JLabel();
            this.jLabel1.setBounds(new Rectangle(315, 15, 106, 31));
            this.jLabel1.setToolTipText("");
            this.jLabel1.setHorizontalTextPosition(CENTER);
            this.jLabel1.setHorizontalAlignment(CENTER);
            this.jLabel1.setText("TEXT");
            this.jLabel1.setForeground(BLUE);
            this.jLabel1.setFont(new java.awt.Font("ARIAL BLACK", PLAIN, 18));
            this.jLabel = new JLabel();
            this.jLabel.setBounds(new Rectangle(30, 15, 106, 31));
            this.jLabel.setHorizontalTextPosition(CENTER);
            this.jLabel.setHorizontalAlignment(CENTER);
            this.jLabel.setText(LANGUAGE);
            this.jLabel.setForeground(BLUE);
            this.jLabel.setFont(new java.awt.Font("ARIAL BLACK", PLAIN, 18));
            this.jContentPane = new JPanel() {
                /**
                 *
                 */
                private static final long serialVersionUID = 1L;

                @Override
                protected void paintComponent(Graphics g) {
                    //  Dispaly image at at full size
                    g.drawImage(BACKGROUND.getImage(), 0, 0, null);

                    //  Scale image to size of component
                    Dimension d = this.getSize();
                    g.drawImage(BACKGROUND.getImage(), 0, 0, d.width, d.height, null);

                    super.paintComponent(g);
                }
            };
            this.jContentPane.setOpaque(false);
            this.jContentPane.setLayout(null);
            this.jContentPane.add(this.getJScrollPane(), null);
            this.jContentPane.add(this.getJScrollPane1(), null);
            this.jContentPane.add(this.getJButton(), null);
            this.jContentPane.add(this.getJButton1(), null);
            this.jContentPane.add(this.getJButton2(), null);
            this.jContentPane.add(this.jLabel, null);
            this.jContentPane.add(this.jLabel1, null);
        }
        return this.jContentPane;
    }

    /**
     * This method loads a file to JTextPane
     *
     */
    private void loadFile_JTextPane() {
        File file = new File(LANGUAGE + ".txt");
        try {
            file.createNewFile();
            try (BufferedReader br = new BufferedReader(new FileReader(file))) {
                this.getJTextPane().read(br, null);
            }
        } catch (FileNotFoundException ex) {
            LOG.severe(ex.getMessage());
        } catch (IOException ex) {
            LOG.severe(ex.getMessage());
        }
    }

    /**
     * This method loads a file to JTextPane1
     *
     */
    private void loadFile_JTextPane1() {
        File file = new File("text.txt");
        try {
            file.createNewFile();
            try (BufferedReader br = new BufferedReader(new FileReader(file))) {
                this.getJTextPane1().read(br, null);
            }
        } catch (FileNotFoundException ex) {
            LOG.severe(ex.getMessage());
        } catch (IOException ex) {
            LOG.severe(ex.getMessage());
        }
    }

    /**
     * This method saves the language file
     *
     */
    private void saveFile_JTextPane() {
        File file = new File(LANGUAGE + ".txt");
        try {
            try (BufferedWriter bw = new BufferedWriter(new FileWriter(file))) {
                this.getJTextPane().write(bw);
            }
        } catch (FileNotFoundException e1) {
            LOG.severe(e1.getMessage());
        } catch (IOException e1) {
            LOG.severe(e1.getMessage());
        }
    }

    /**
     * This method saves the text file
     *
     */
    private void saveFile_JTextPane1() {
        File file = new File("text.txt");
        try {
            try (BufferedWriter bw = new BufferedWriter(new FileWriter(file))) {
                this.getJTextPane1().write(bw);
            }
        } catch (FileNotFoundException e1) {
            LOG.severe(e1.getMessage());
        } catch (IOException e1) {
            LOG.severe(e1.getMessage());
        }
    }
}
