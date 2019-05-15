package Converters;

import java.awt.GraphicsConfiguration;
import java.awt.HeadlessException;
import java.awt.Rectangle;
import java.util.logging.Logger;
import static java.util.logging.Logger.getLogger;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextPane;

/**
 *
 * @author gpaiva
 */
public class Converter extends JFrame {

    protected static final Logger LOG = getLogger(Converter.class.getName());

    protected static String IMAGE_PATH = null;
    protected static ImageIcon ICON = null;
    protected static ImageIcon BACKGROUND = null;

    protected JTextPane jTextPane0 = null;
    protected JTextPane jTextPane1 = null;
    protected String LANGUAGE = null;

    protected JPanel jContentPane = null;
    protected JScrollPane jScrollPane0 = null;
    protected JScrollPane jScrollPane1 = null;
    protected JButton jButton0 = null;
    protected JButton jButton1 = null;
    protected JButton jButton2 = null;
    protected JLabel jLabel0 = null;
    protected JLabel jLabel1 = null;

    /**
     * @throws HeadlessException
     */
    public Converter() throws HeadlessException {
        super();
    }

    /**
     * @param arg0
     */
    public Converter(GraphicsConfiguration arg0) {
        super(arg0);
    }

    /**
     * @param arg0
     * @throws HeadlessException
     */
    public Converter(String arg0) throws HeadlessException {
        super(arg0);
    }

    /**
     * @param arg0
     * @param arg1
     */
    public Converter(String arg0, GraphicsConfiguration arg1) {
        super(arg0, arg1);
    }

    /**
     * This method initializes jScrollPane
     *
     * @param jTextPane
     * @param jScrollPane
     * @param x
     * @param y
     * @param width
     * @param height
     * @return javax.swing.JScrollPane
     */
    protected JScrollPane getJScrollPane(
            JScrollPane jScrollPane, JTextPane jTextPane,
            int x, int y, int width, int height) {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setBounds(new Rectangle(x, y, width, height));
            jScrollPane.setViewportView(jTextPane);
        }
        return jScrollPane;
    }

    /**
     * This method initializes jTextPane
     *
     * @param jTextPane
     * @param text
     * @return javax.swing.JTextPane
     */
    protected JTextPane getJTextPane(JTextPane jTextPane, String text) {
        if (jTextPane == null) {
            jTextPane = new JTextPane();
        }
        jTextPane.setText(text);

        return jTextPane;
    }
}
