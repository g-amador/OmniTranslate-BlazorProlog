/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ASCIIfromToConverter;

import java.awt.Color;
import java.awt.Dimension;
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

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextPane;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;

/**
 *
 * @author gpaiva
 */
public class MorseFromTwoAsciiConverter extends JFrame {

    private static final long serialVersionUID = 1L;

    private JPanel jContentPane = null;

    private JScrollPane jScrollPane = null;

    private JScrollPane jScrollPane1 = null;

    private JButton jButton = null;

    private JButton jButton1 = null;

    private JButton jButton2 = null;

    private JLabel jLabel = null;

    private JLabel jLabel1 = null;

    private static JTextPane jTextPane = null;

    private static JTextPane jTextPane1 = null;

    private static ImageIcon ICON = new ImageIcon("img\\icon.jpg");  //  @jve:decl-index=0:

    private static ImageIcon BACKGROUND = new ImageIcon("img\\background.jpg");  //  @jve:decl-index=0:

    private static String LANGUAGE = "MORSE";

    /**
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter() throws HeadlessException {
        super();
        initialize();
    }

    /**
     * @param arg0
     */
    public MorseFromTwoAsciiConverter(GraphicsConfiguration arg0) {
        super(arg0);
        initialize();
    }

    /**
     * @param arg0
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter(String arg0) throws HeadlessException {
        super(arg0);
        initialize();
    }

    /**
     * @param arg0
     * @param arg1
     */
    public MorseFromTwoAsciiConverter(String arg0, GraphicsConfiguration arg1) {
        super(arg0, arg1);
        initialize();
    }

    /**
     * This method initializes jScrollPane
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setBounds(new Rectangle(15, 60, 136, 136));
            jScrollPane.setViewportView(getJTextPane());
        }
        return jScrollPane;
    }

    /**
     * This method initializes jScrollPane1
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane1() {
        if (jScrollPane1 == null) {
            jScrollPane1 = new JScrollPane();
            jScrollPane1.setBounds(new Rectangle(300, 60, 136, 136));
            jScrollPane1.setViewportView(getJTextPane1());
        }
        return jScrollPane1;
    }

    /**
     * This method initializes jButton
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton() {
        if (jButton == null) {
            jButton = new JButton();
            jButton.setBounds(new Rectangle(165, 75, 121, 31));
            jButton.setText("< " + LANGUAGE);
            jButton.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert text to target language
                saveFile_JTextPane1();
                Runtime r = Runtime.getRuntime();
                Process p = null;
                try {
                    p = r.exec("TEXT2" + LANGUAGE + ".cmd");
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
                loadFile_JTextPane();
            });
        }
        return jButton;
    }

    /**
     * This method initializes jButton1
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton1() {
        if (jButton1 == null) {
            jButton1 = new JButton();
            jButton1.setBounds(new Rectangle(165, 150, 121, 31));
            jButton1.setText("TEXT >");
            jButton1.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert language to text
                saveFile_JTextPane();
                Runtime r = Runtime.getRuntime();
                Process p = null;
                try {
                    p = r.exec(LANGUAGE + "2TEXT.cmd");
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
                loadFile_JTextPane1();
            });
        }
        return jButton1;
    }

    /**
     * This method initializes jButton2
     *
     * @return javax.swing.JButton
     */
    private JButton getJButton2() {
        if (jButton2 == null) {
            jButton2 = new JButton();
            jButton2.setBounds(new Rectangle(180, 225, 91, 31));
            jButton2.setText("EXIT");
            jButton2.addActionListener((java.awt.event.ActionEvent e) -> {
                File file1 = new File("text.txt");
                File file2 = new File(LANGUAGE + ".txt");
                file1.delete();
                file2.delete();
                System.exit(0);
            });
        }
        return jButton2;
    }

    /**
     * This method initializes jTextPane
     *
     * @return javax.swing.JTextPane
     */
    private JTextPane getJTextPane() {
        if (jTextPane == null) {
            jTextPane = new JTextPane();
            loadFile_JTextPane();
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
            loadFile_JTextPane1();
        }
        return jTextPane1;
    }

    /**
     * @param args
     */
    public static void main(String[] args) {
//		if(args.length==1) {
//		LANGUAGE = args[0];
        SwingUtilities.invokeLater(() -> {
            MorseFromTwoAsciiConverter thisClass = new MorseFromTwoAsciiConverter();
            thisClass.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
            thisClass.setVisible(true);
        });
//		}
//		else System.out.println("java -jar MorseFromTwoAsciiConverter.java [LANGUAGE]");
    }

    /**
     * This method initializes this
     *
     * @return void
     */
    private void initialize() {
        this.setContentPane(getJContentPane());
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
        if (jContentPane == null) {
            jLabel1 = new JLabel();
            jLabel1.setBounds(new Rectangle(315, 15, 106, 31));
            jLabel1.setToolTipText("");
            jLabel1.setHorizontalTextPosition(SwingConstants.CENTER);
            jLabel1.setHorizontalAlignment(SwingConstants.CENTER);
            jLabel1.setText("TEXT");
            jLabel1.setForeground(Color.BLUE);
            jLabel1.setFont(new java.awt.Font("ARIAL BLACK", java.awt.Font.PLAIN, 18));
            jLabel = new JLabel();
            jLabel.setBounds(new Rectangle(30, 15, 106, 31));
            jLabel.setHorizontalTextPosition(SwingConstants.CENTER);
            jLabel.setHorizontalAlignment(SwingConstants.CENTER);
            jLabel.setText(LANGUAGE);
            jLabel.setForeground(Color.BLUE);
            jLabel.setFont(new java.awt.Font("ARIAL BLACK", java.awt.Font.PLAIN, 18));
            jContentPane = new JPanel() {
                /**
                 *
                 */
                private static final long serialVersionUID = 1L;

                @Override
                protected void paintComponent(Graphics g) {
                    //  Dispaly image at at full size
                    g.drawImage(BACKGROUND.getImage(), 0, 0, null);

                    //  Scale image to size of component
                    Dimension d = getSize();
                    g.drawImage(BACKGROUND.getImage(), 0, 0, d.width, d.height, null);

                    super.paintComponent(g);
                }
            };
            jContentPane.setOpaque(false);
            jContentPane.setLayout(null);
            jContentPane.add(getJScrollPane(), null);
            jContentPane.add(getJScrollPane1(), null);
            jContentPane.add(getJButton(), null);
            jContentPane.add(getJButton1(), null);
            jContentPane.add(getJButton2(), null);
            jContentPane.add(jLabel, null);
            jContentPane.add(jLabel1, null);
        }
        return jContentPane;
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
                getJTextPane().read(br, null);
            }
        } catch (FileNotFoundException ex) {
            ex.printStackTrace();
        } catch (IOException ex) {
            ex.printStackTrace();
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
                getJTextPane1().read(br, null);
            }
        } catch (FileNotFoundException ex) {
            ex.printStackTrace();
        } catch (IOException ex) {
            ex.printStackTrace();
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
                getJTextPane().write(bw);
            }
        } catch (FileNotFoundException e1) {
            e1.printStackTrace();
        } catch (IOException e1) {
            e1.printStackTrace();
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
                getJTextPane1().write(bw);
            }
        } catch (FileNotFoundException e1) {
            e1.printStackTrace();
        } catch (IOException e1) {
            e1.printStackTrace();
        }
    }
}
