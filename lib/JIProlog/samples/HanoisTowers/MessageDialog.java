/*****************************************
 * 09/19/2002
 *
 * Copyright (C) 2002 Ugo Chirico
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

public final class MessageDialog extends Dialog
{
    private Button    m_btnOk;
      
    public MessageDialog(Frame parent, String strTitle, String strMessage, Dimension d)
    {
        super(parent, strTitle, true);

        // Crea Label da inserire nella finestra
        
        Label lbMessage = new Label(strMessage, Label.CENTER);
        
        GridBagLayout gbl = new GridBagLayout();
        GridBagConstraints gbc = new GridBagConstraints();
        setLayout(gbl);
        
        m_btnOk     = new Button("  Ok  ");
                
        gbc.gridwidth = GridBagConstraints.REMAINDER;
        gbc.weightx   = 2;
        gbc.fill      = GridBagConstraints.HORIZONTAL;
        gbc.insets    = new Insets(5, 5, 5, 5);
        gbl.setConstraints(lbMessage, gbc);
        add(lbMessage);
                
        gbc.gridwidth = GridBagConstraints.REMAINDER;
        gbc.weightx   = 1;
        gbc.anchor    = GridBagConstraints.CENTER;
        gbc.fill      = GridBagConstraints.NONE;
        gbc.ipadx     = 20;
        gbl.setConstraints(m_btnOk, gbc);
        add(m_btnOk);
    
    /*
          //Calcola la dimensioni della label
        Dimension dimLabel = getPreferredSize();
        int nHeight = dimLabel.height + 10;
        int nWidth = dimLabel.width + 10;
        
        System.out.println(nHeight);
        System.out.println(nWidth);
     */
        // Centra la dialog
        Dimension ds = Toolkit.getDefaultToolkit().getScreenSize();
        
        int nTop  = (ds.height - d.height) / 2;
        int nLeft = (ds.width  - d.width)  / 2;
        
        reshape(nLeft, nTop, d.width, d.height);
        setResizable(false);

    }
    
    public boolean action(Event evt, Object arg)
    {
        if(arg.equals(m_btnOk.getLabel()))
        {
            onOk();
            return true;
        }
        
        return super.action(evt, arg);
    }
    
    private void onOk()
    {
        onDestroy(null);
    }
    
    
    public boolean handleEvent(Event evt)
    {
        //System.out.println(evt);

        switch(evt.id)
        {
        case Event.WINDOW_DESTROY:
            return onDestroy(evt);
        case Event.WINDOW_EXPOSE:
            return onExpose(evt);
        case Event.WINDOW_ICONIFY:
            return onIconify(evt);
        case Event.WINDOW_DEICONIFY:
            return onDeiconify(evt);
        case Event.WINDOW_MOVED:
            return onMoved(evt);
        }

        return super.handleEvent(evt);
    }

    private boolean onDestroy(Event evt)
    {
        hide();
        dispose();
        return true;
    }

    private boolean onExpose(Event evt)
    {
        return super.handleEvent(evt);
    }

    private boolean onIconify(Event evt)
    {
        return super.handleEvent(evt);
    }

    private boolean onDeiconify(Event evt)
    {
        return super.handleEvent(evt);
    }

    private boolean onMoved(Event evt)
    {
        return super.handleEvent(evt);
    }
}
