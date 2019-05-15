package Converters;

import javax.swing.JButton;
import javax.swing.JPanel;

/**
 *
 * @author gpaiva
 */
public interface IConverter {

    /**
     * This method initializes jButton0
     *
     * @return javax.swing.JButton
     */
    JButton getJButton0();

    /**
     * This method initializes jButton1
     *
     * @return javax.swing.JButton
     */
    JButton getJButton1();

    /**
     * This method initializes jButton2
     *
     * @return javax.swing.JButton
     */
    JButton getJButton2();

    /**
     * This method initializes this
     *
     */
    void initialize();

    /**
     * This method initializes jContentPane
     *
     * @return javax.swing.JPanel
     */
    JPanel getJContentPane();
}
