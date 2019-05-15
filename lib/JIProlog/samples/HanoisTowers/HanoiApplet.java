/*****************************************
 * 09/19/2002
 *
 * Copyright (C) 2002-2004 Ugo Chirico
 * http://www.ugosweb.com/jiprolog
 *
 * This is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *****************************************/


import java.awt.*;
import java.applet.*;
import java.util.Stack;
import java.awt.event.*;
import java.io.*;
import java.util.StringTokenizer;
import java.net.*;

import com.ugos.JIProlog.engine.*;

public class HanoiApplet extends Applet implements MouseListener, ActionListener, ItemListener, JIPEventListener
{
    public static final int MAX   = 10;
    public static final int MIN   = 1;
    public static final int BASE  = 3;
    public static final int BEGIN = 5;
        
    private TextField m_txtField;
    private TextField m_txtMoves;
    
    private Button m_btnMove;
    private Button m_btnReset;
    private Button m_btnSolution;
    private Choice m_chHeight;
              
    private HanoisTower m_towers[];
    private HanoisTower m_orgTower;
    private HanoisTower m_destTower;
    
    private int m_nHeight;
    private int m_nMoves;
    private int m_nQuery;
    
    private Frame m_mainFrame;
    
    private JIPEngine m_jip;
    
    public void init()
    {
        super.init();
    
        // Create PrologEngine and load hanoi.txt
        m_jip = new JIPEngine();
        
        try
        {
            m_jip.consultStream(getClass().getResourceAsStream("/hanoi.txt"), "hanoi.txt");
        }
        catch(JIPSyntaxErrorException ex)
        {
            add(new Label(ex.toString()));
            return;
        }
        catch(IOException ex)
        {
            ex.printStackTrace();
            add(new Label(ex.toString()));
            return;
        }
        
        m_jip.addEventListener(this);
       
        m_towers = new HanoisTower[3];
        
        setLayout(new BorderLayout());
        setBackground(Color.cyan);
        
        // Header Panel
        Panel header = new Panel();
        header.setLayout(new GridLayout(2,1));
        header.setBackground(Color.cyan.darker());
        header.setForeground(Color.white);
        

        Label lb = new Label("Hanoi's Towers", Label.CENTER);
        lb.setFont(new Font("TimesRoman", Font.BOLD, 18));
        header.add(lb);
        lb = new Label("Powered by JIProlog - http://www.ugosweb.com/jiprolog", Label.CENTER);
        lb.setFont(new Font("Dialog", Font.BOLD, 12));
        lb.addMouseListener(this);
        lb.setCursor(new Cursor(Cursor.HAND_CURSOR));
        header.add(lb);
                
        add("North", header);
                        
        Panel towersPanel = new Panel();
        towersPanel.setLayout(new GridLayout(1,3,1,1));
            
        // add tower 1
        HanoisTower tower = new HanoisTower(BEGIN);
        tower.addMouseListener(this);
        towersPanel.add(tower);
        m_towers[0] = tower;

        // add tower 2
        tower = new HanoisTower(0);
        tower.addMouseListener(this);
        towersPanel.add(tower);
        m_towers[1] = tower;

        // add tower 3
        tower = new HanoisTower(0);
        tower.addMouseListener(this);
        towersPanel.add(tower);
        m_towers[2] = tower;
        
        add("Center", towersPanel);
        
        Panel footer = new Panel();
        footer.setLayout(new GridLayout(2, 1));
        
        
        Panel panButton = new Panel();
        panButton.setLayout(new FlowLayout());
        panButton.setBackground(Color.cyan.darker());
        
        m_btnMove     = new Button("Move");
        m_btnReset    = new Button("Reset");
        m_btnSolution = new Button("Solution");
        m_chHeight    = new Choice();
        m_txtMoves    = new TextField(2);
        
        for(int i = BASE; i < MAX + 1; i++)
        {
            m_chHeight.add(Integer.toString(i));
        }
        
        m_chHeight.select(BEGIN - 3);
        m_btnMove.addActionListener(this);
        m_btnReset.addActionListener(this);
        m_btnSolution.addActionListener(this);
        m_chHeight.addItemListener(this);
   
        panButton.add(m_btnMove);
        panButton.add(m_btnSolution);
        panButton.add(m_btnReset);
        panButton.add(new Label("Plates:", Label.RIGHT));
        panButton.add(m_chHeight);
        panButton.add(new Label("Moves:", Label.RIGHT));
        panButton.add(m_txtMoves);
        
        m_txtMoves.setEnabled(false);
        m_txtMoves.setBackground(Color.white);

        footer.add(panButton);
        
        Panel txtPan = new Panel();
        txtPan.setBackground(Color.cyan.darker());

        m_txtField  = new TextField(50);
        m_txtField.setBackground(Color.white);
        txtPan.add(m_txtField);
        footer.add(txtPan);
                                
        add("South", footer);

        m_orgTower = null;
        m_destTower = null;
        
        m_btnMove.setEnabled(false);
        
        m_nHeight = BEGIN;
        m_nMoves  = 0;
        
        Container ct = getParent();
        while(! (ct instanceof Frame))
        {
            ct = ct.getParent();
        }
        
        m_mainFrame = (Frame)ct;
    }
    
    public void stop()
    {
        if(m_nQuery != 0)
            m_jip.closeQuery(m_nQuery);
    }
    
    private void reset()
    {
        m_towers[0].reset(m_nHeight);
        m_towers[1].reset(0);
        m_towers[2].reset(0);
        m_nMoves = 0;
        m_txtMoves.setText("0");
        m_txtField.setText("");
        
        m_btnSolution.setEnabled(true);
        m_btnMove.setEnabled(true);
        m_chHeight.setEnabled(true);
        
        setCursor(Cursor.getPredefinedCursor(Cursor.DEFAULT_CURSOR));
    }
    
    private void move(HanoisTower org, HanoisTower dest)
    {
        int nTop1 = org.peek();
        //System.out.println(nTop1);

        int nTop2 = dest.peek();
        //System.out.println(nTop2);
            
        if(nTop1 >= nTop2)
        {
            m_txtField.setText(" NOT ALLOWED! ");
            MessageDialog dlg = new MessageDialog(m_mainFrame, "Hanoi's Towers", "Warning: the move you selected is not allowed", new Dimension(250, 120));
            dlg.show();
            
            Toolkit.getDefaultToolkit().beep();
            
            repaint();
        }
        else
        {
            //System.out.println("Move");
            dest.push(org.pop());
            //m_txtField.setText(" OK ");
        }
        
        org.setBackground(Color.cyan);
        dest.setBackground(Color.cyan);
        m_orgTower = null;
        m_destTower = null;
        m_btnMove.setEnabled(false);
        m_nMoves++;
        m_txtMoves.setText(Integer.toString(m_nMoves));
        
        //System.out.println(" OK ");
        
        if(m_towers[0].getPlates() == 0 && m_towers[1].getPlates() == m_nHeight)
        {
            m_btnMove.setEnabled(false);
                    
            MessageDialog dlg = new MessageDialog(m_mainFrame, "Hanoi's Towers", "You win in " + m_txtMoves.getText() + " moves", new Dimension(250, 120));
            dlg.show();
            
            repaint();
            
            m_btnMove.setEnabled(false);
           // System.out.println("Urra!");
        }
    }
    
    public void mouseClicked(MouseEvent e)
    {
        if(e.getSource() instanceof Label)
        {
            try
            {
                getAppletContext().showDocument(new URL("http://www.ugosweb.com/jiprolog"), "_blank");
            }
            catch(Exception ex)
            {
            }
            
            return;
        }

        // Get the tower selected
        HanoisTower tower = (HanoisTower)e.getSource();
        
        // Udpate the state of the selection
        if (m_orgTower == null)
        {
            m_orgTower = tower;
            tower.setBackground(Color.pink);
        }
        else if(tower == m_orgTower)
        {
            m_orgTower.setBackground(Color.cyan);
            if(m_destTower != null)
                m_destTower.setBackground(Color.cyan);
                
            m_orgTower = null;
            m_destTower = null;
            m_btnMove.setEnabled(false);
        }
        else if(m_destTower == null)
        {
            m_destTower = tower;
            tower.setBackground(Color.orange);
            m_btnMove.setEnabled(true);
        }
        else if(tower == m_destTower)
        {
            m_destTower = null;
            tower.setBackground(Color.cyan);
        }
        else
        {
            m_orgTower.setBackground(Color.cyan);
            m_destTower.setBackground(Color.cyan);
            m_orgTower = null;
            m_destTower = null;
            m_btnMove.setEnabled(false);
        }
        
        m_txtField.setText("");
    }
    
    public void actionPerformed(ActionEvent e)
    {
        if(e.getSource() == m_btnMove)
        {
            move(m_orgTower, m_destTower);
        }
        else if(e.getSource() == m_btnSolution)
        {
            reset();
            m_btnMove.setEnabled(false);
            m_btnSolution.setEnabled(false);
            m_btnReset.setEnabled(false);
            m_chHeight.setEnabled(false);
            setCursor(Cursor.getPredefinedCursor(Cursor.WAIT_CURSOR));

            String strQuery = "hanoi(" + Integer.toString(m_nHeight) + ", 1,2,3,X).";
            
            try
            {
                // syncronization is required to avoid that listeners is
                // called before the method openQuery returns the handle.
                synchronized(m_jip)
                {
                    m_nQuery = m_jip.openQuery(strQuery);
                }
            }
            catch(JIPSyntaxErrorException ex)
            {
                ex.printStackTrace();
            }
            
        }
        else if(e.getSource() == m_btnReset)
        {
            m_btnMove.setEnabled(false);
            reset();
        }
        else
        {
            //System.out.println(e.getActionCommand());
            //System.out.println(e.getSource());
        }
    }

    public void itemStateChanged(ItemEvent e)
    {
        if(e.getStateChange() == ItemEvent.SELECTED)
        {
            m_nHeight = Integer.parseInt((String)e.getItem());
            reset();
        }
    }
      
    public void solutionNotified(JIPEvent e)
    {
        // syncronization is required to avoid that listeners is
        // called before the method openQuery returns the handle.
        synchronized(m_jip)
        {
            JIPTerm sol = e.getTerm();
            
            if (sol == null)
                return;
            
            JIPVariable[] vars = sol.getVariables();
            String strSol = vars[0].toString(m_jip);
            //String strSol = strVars.substring(5, strVars.length() - 1);
            m_txtField.setText(strSol);
            
            // Apply moves
            StringTokenizer tok = new StringTokenizer(strSol, "[], ");
            int nTower1;
            int nTower2;
            
            while (tok.hasMoreElements())
            {
                nTower1 = Integer.parseInt((String)tok.nextElement()) - 1;
                //System.out.println(nTower1);
                nTower2 = Integer.parseInt((String)tok.nextElement()) - 1;
                //System.out.println(nTower2);
                
                move(m_towers[nTower1], m_towers[nTower2]);
                
                try
                {
                    Thread.currentThread().sleep(500);
                }
                catch(InterruptedException ex)
                {
                    ex.printStackTrace();
                }
            }
     
            setCursor(Cursor.getPredefinedCursor(Cursor.DEFAULT_CURSOR));
                    
            m_btnSolution.setEnabled(true);
            m_btnReset.setEnabled(true);
            m_chHeight.setEnabled(true);
                    
            m_jip.closeQuery(m_nQuery);
        }
    }
    
    // An error (exception) has been raised up by prolog engine
    public void errorNotified(JIPErrorEvent e)
    {
        MessageDialog dlg = new MessageDialog(m_mainFrame, "Hanoi's Towers", e.getError(), new Dimension(250, 120));
        dlg.show();
        
        reset();
    }


    public void termNotified(JIPEvent e)
    {
    }
    
    public void closeNotified(JIPEvent e)
    {
    }

    public void openNotified(JIPEvent e)
    {
    }
    
    public void endNotified(JIPEvent e)
    {
    }

    public void moreNotified(JIPEvent e)
    {
    }

    public void mouseReleased(MouseEvent e)
    {
    }
    
    public void mousePressed(MouseEvent e)
    {
    }

    public void mouseEntered(MouseEvent e)
    {
    }

    public void mouseExited(MouseEvent e)
    {
    }
    
    public String getAppletInfo()
    {
        return "Hanoi's Towers Applet v1.6.1- By Ugo Chirico \nPowered by JIProlog - Java Internet Prolog \nhttp://www.ugosweb.com/jiprolog";
    }
}
