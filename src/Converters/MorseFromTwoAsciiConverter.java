package Converters;

import com.ugos.jiprolog.engine.JIPEngine;
import com.ugos.jiprolog.engine.JIPQuery;
import com.ugos.jiprolog.engine.JIPSyntaxErrorException;
import com.ugos.jiprolog.engine.JIPTerm;
import com.ugos.jiprolog.engine.JIPTermParser;
import com.ugos.jiprolog.engine.JIPVariable;
import static java.awt.Color.ORANGE;
import java.awt.Dimension;
import static java.awt.Font.PLAIN;
import java.awt.Graphics;
import java.awt.GraphicsConfiguration;
import java.awt.HeadlessException;
import java.awt.Rectangle;
import static java.lang.System.exit;
import static java.lang.System.out;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import static javax.swing.SwingConstants.CENTER;
import static javax.swing.SwingUtilities.invokeLater;

/**
 *
 * @author gpaiva
 */
public final class MorseFromTwoAsciiConverter extends Converter implements IConverter {

    private static JIPEngine jip;

    /**
     * @param args
     */
    public static void main(String[] args) {
//		if(args.length==1) {
//		LANGUAGE = args[0];

        // New instance of prolog engine
        jip = new JIPEngine();

        invokeLater(() -> {
            MorseFromTwoAsciiConverter thisClass = new MorseFromTwoAsciiConverter();
            thisClass.setDefaultCloseOperation(EXIT_ON_CLOSE);
            thisClass.setVisible(true);
        });
//		}
//		else System.out.println("java -jar MorseFromTwoAsciiConverter.java [LANGUAGE]");
    }

    /**
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter() throws HeadlessException {
        super();
        this.LANGUAGE = "MORSE";
        IMAGE_PATH = "assets/morse/img/";
        ICON = new ImageIcon(IMAGE_PATH + "icon.jpg");
        BACKGROUND = new ImageIcon(IMAGE_PATH + "background.jpg");
        this.initialize();
    }

    /**
     * @param arg0
     */
    public MorseFromTwoAsciiConverter(GraphicsConfiguration arg0) {
        super(arg0);
        this.LANGUAGE = "MORSE";
        IMAGE_PATH = "assets/morse/img/";
        ICON = new ImageIcon(IMAGE_PATH + "icon.jpg");
        BACKGROUND = new ImageIcon(IMAGE_PATH + "background.jpg");
        this.initialize();
    }

    /**
     * @param arg0
     * @throws HeadlessException
     */
    public MorseFromTwoAsciiConverter(String arg0) throws HeadlessException {
        super(arg0);
        this.LANGUAGE = "MORSE";
        IMAGE_PATH = "assets/morse/img/";
        ICON = new ImageIcon(IMAGE_PATH + "icon.jpg");
        BACKGROUND = new ImageIcon(IMAGE_PATH + "background.jpg");
        this.initialize();
    }

    /**
     * @param arg0
     * @param arg1
     */
    public MorseFromTwoAsciiConverter(String arg0, GraphicsConfiguration arg1) {
        super(arg0, arg1);
        this.LANGUAGE = "MORSE";
        IMAGE_PATH = "assets/morse/img/";
        ICON = new ImageIcon(IMAGE_PATH + "icon.jpg");
        BACKGROUND = new ImageIcon(IMAGE_PATH + "background.jpg");
        this.initialize();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public JButton getJButton0() {
        if (this.jButton0 == null) {
            this.jButton0 = new JButton();
            this.jButton0.setBounds(new Rectangle(165, 75, 121, 31));
            this.jButton0.setText("< " + this.LANGUAGE);
            this.jButton0.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert text to target language

                JIPTerm queryTerm = null;

                // parse query
                try {
                    // consult file
                    jip.consultFile("assets/morse/src/converter.pl");    
                    String sentences = "[";
                    for (char c : jTextPane1.getText().toCharArray()) {
                        switch(c) {
                            case ' ': sentences += "\' \',"; break;
                            case '.': sentences += "\'.\',"; break;
                            case ',': sentences += "\',\',"; break;
                            case '?': sentences += "\'?\',"; break;
                            case '\'': sentences += "\'\\\'\',"; break;
                            case '!': sentences += "\'!\',"; break;
                            case '/': sentences += "\'/\',"; break;
                            case '(': sentences += "\'(\',"; break;
                            case ')': sentences += "\')\',"; break;
                            case '&': sentences += "\'&\',"; break;
                            case ':': sentences += "\':\',"; break;
                            case ';': sentences += "\';\',"; break;
                            case '=': sentences += "\'=\',"; break;
                            case '+': sentences += "\'+\',"; break;
                            case '-': sentences += "\'-\',"; break;
                            case '_': sentences += "\'_\',"; break;
                            case '\"': sentences += "\'\\\"\',"; break;
                            case '$': sentences += "\'$\',"; break;
                            case '@': sentences += "\'@\',"; break;
                            default: sentences += c + ",";
                        }
                    }
                    sentences += "]";
                    sentences = sentences.replaceAll(",]", "]");
                    System.out.println(sentences);
                    queryTerm = jip.getTermParser().parseTerm(
                            "sentence(" + sentences + ", M).");
                } catch (JIPSyntaxErrorException ex) {
                    // there is a syntax error in the query
                    LOG.info(ex.getMessage());
                    exit(0); // needed to close threads by AWT if shareware
                }

                // open a synchronous query
                JIPQuery jipQuery = jip.openSynchronousQuery(queryTerm);
                JIPTerm solution;

                // Loop while there is another solution
                while (jipQuery.hasMoreChoicePoints()) {
                    solution = jipQuery.nextSolution();
                    //out.println(solution);

                    JIPVariable[] vars = solution.getVariables();
                    for (JIPVariable var : vars) {
                        if (!var.isAnonymous()) {
                            System.out.print(var.getName() + " = " + var.toString(jip) + " ");
                            System.out.println();
                            String result = (var.toString(jip) + " ")
                                    .replace("[", "")
                                    .replace("]", "")
                                    .replace(",", " ");
                            this.getJTextPane(jTextPane0, result);
                        }
                    }
                }
            });
        }
        return this.jButton0;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public JButton getJButton1() {
        if (this.jButton1 == null) {
            this.jButton1 = new JButton();
            this.jButton1.setBounds(new Rectangle(165, 150, 121, 31));
            this.jButton1.setText("TEXT >");
            this.jButton1.addActionListener((java.awt.event.ActionEvent e) -> {
                // convert language to text
                JIPTerm query = null;

                try {
                    // parse query
                    JIPTermParser parser = jip.getTermParser();
                    query = parser.parseTerm("write('hello world'), nl.");
                } catch (JIPSyntaxErrorException ex) {
                    // there is a syntax error in the query
                    LOG.info(ex.getMessage());
                    exit(0);
                }

                // open a synchronous query
                JIPQuery jipQuery = jip.openSynchronousQuery(query);
                JIPTerm solution;

                // Loop while there is another solution
                while (jipQuery.hasMoreChoicePoints()) {
                    solution = jipQuery.nextSolution();
                    out.println(solution);
                    this.getJTextPane(jTextPane0, solution.toString());
                }

                /*
                this.saveFile_JTextPane(this.jTextPane0, this.LANGUAGE + ".txt");
                Runtime r = getRuntime();
                Process p = null;
                try {
                    p = r.exec(this.LANGUAGE + "2TEXT.cmd");
                } catch (IOException ex) {
                    LOG.severe(ex.getMessage());
                }
                this.loadFile_JTextPane(this.jTextPane1, "text.txt");*/
            });
        }
        return this.jButton1;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public JButton getJButton2() {
        if (this.jButton2 == null) {
            this.jButton2 = new JButton();
            this.jButton2.setBounds(new Rectangle(180, 225, 91, 31));
            this.jButton2.setText("EXIT");
            this.jButton2.addActionListener((java.awt.event.ActionEvent e) -> {
                //File file1 = new File("text.txt");
                //File file2 = new File(this.LANGUAGE + ".txt");
                //file1.delete();
                //file2.delete();
                exit(0);
            });
        }
        return this.jButton2;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void initialize() {
        this.setContentPane(this.getJContentPane());
        LOG.info(ICON.toString());
        LOG.info(BACKGROUND.toString());
        this.setIconImage(ICON.getImage());
        this.setTitle(this.LANGUAGE + " TEXT TRANSLATOR");
        this.setBounds(new Rectangle(280, 220, 460, 305));
        this.setResizable(false);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public JPanel getJContentPane() {
        if (this.jContentPane == null) {
            this.jLabel1 = new JLabel();
            this.jLabel1.setBounds(new Rectangle(315, 15, 106, 31));
            this.jLabel1.setToolTipText("");
            this.jLabel1.setHorizontalTextPosition(CENTER);
            this.jLabel1.setHorizontalAlignment(CENTER);
            this.jLabel1.setText("TEXT");
            this.jLabel1.setForeground(ORANGE);
            this.jLabel1.setFont(new java.awt.Font("ARIAL BLACK", PLAIN, 18));
            this.jLabel0 = new JLabel();
            this.jLabel0.setBounds(new Rectangle(30, 15, 106, 31));
            this.jLabel0.setHorizontalTextPosition(CENTER);
            this.jLabel0.setHorizontalAlignment(CENTER);
            this.jLabel0.setText(this.LANGUAGE);
            this.jLabel0.setForeground(ORANGE);
            this.jLabel0.setFont(new java.awt.Font("ARIAL BLACK", PLAIN, 18));
            this.jTextPane0 = this.getJTextPane(this.jTextPane0, "");
            this.jTextPane1 = this.getJTextPane(this.jTextPane1, "");
            this.jScrollPane0 = this.getJScrollPane(
                    this.jScrollPane0, this.jTextPane0, 15, 60, 136, 136);
            this.jScrollPane1 = this.getJScrollPane(
                    this.jScrollPane1, this.jTextPane1, 300, 60, 136, 136);
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
            this.jContentPane.add(jScrollPane0, null);
            this.jContentPane.add(jScrollPane1, null);
            this.jContentPane.add(this.getJButton0(), null);
            this.jContentPane.add(this.getJButton1(), null);
            this.jContentPane.add(this.getJButton2(), null);
            this.jContentPane.add(this.jLabel0, null);
            this.jContentPane.add(this.jLabel1, null);
        }
        return this.jContentPane;
    }
}
