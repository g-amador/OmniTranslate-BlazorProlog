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
import java.util.Stack;

public class HanoisTower extends Canvas
{
    static
    {
        initColors();
    }
    
    private static int PERC = 9;
    private static double DH = 0.1;
      
    private static Color[] m_colors;
    
    private static void initColors()
    {
        m_colors = new Color[HanoiApplet.MAX];
        for(int i = 0; i < HanoiApplet.MAX; i++)
        {
            m_colors[i] = new Color((float)Math.random(), (float)Math.random(), (float)Math.random());
        }
    }
    
    private Stack m_tower;
        
    public HanoisTower(int nHeight)
    {
        super();
        reset(nHeight);
    }
    
    public void reset(int nHeight)
    {
        initColors();
        
        m_tower = new Stack();
        for(int i = nHeight; i > 0; i--)
        {
            push(i);
        }
        repaint();
    }
    
    public int getPlates()
    {
        return m_tower.size();
    }
    
    public void paint(Graphics g)
    {
        super.paint(g);
        
        int nSize = m_tower.size();
        int nValue = 0;
        double l,h,x,y,dh,dl;

        //Rectangle rect = g.getClipBounds();
        Rectangle rect = new Rectangle(getSize());
        g.setClip(rect.x,rect.y, rect.width, rect.height);
        //g.clearRect(rect.x,rect.y, rect.width, rect.height);
        
        g.setColor(Color.black);
        
        g.drawRect(rect.x,rect.y, rect.width, rect.height);

        int nWidth = rect.width;
        int nHeigth = rect.height;
        
        //System.out.println(nWidth);
        //System.out.println(nHeigth);

        h  = ((double)nHeigth / (double)HanoiApplet.MAX) * (double)(PERC - 1) / 10.0;
        dh = (double)nHeigth / (double)HanoiApplet.MAX * (double)DH;
        dl = ((double)nWidth * (double)PERC / 10.0) / (double)HanoiApplet.MAX;
        
        //System.out.println(dh);
        //System.out.println(nSize);
        
        for (int i = 0; i < nSize; i++)
        {
            nValue = ((Integer)m_tower.elementAt(i)).intValue();
            //System.out.println(nValue);

            l = (double)nValue * dl;
            y = ((double)nHeigth - dh) - ((double)(i + 1) * (h + dh));
            x = ((double)nWidth / 2.0) - (l / 2.0);
            
            g.setColor(Color.white);
            g.fillRect((int)x + rect.x,(int)y + rect.y,(int)l,(int)h);
            //g.setColor(Color.black);
            //g.setColor(new Color((float)Math.random(), (float)Math.random(), (float)Math.random()));
            g.setColor(m_colors[nValue - 1]);
            g.drawRect((int)x + rect.x,(int)y + rect.y,(int)l,(int)h);
            g.fillRect((int)x + rect.x + 2,(int)y + rect.y + 2,(int)l,(int)h);
        
          //  System.out.println("Value:");
          //  System.out.println(l);
            //System.out.println(h);
            //System.out.println(x);
            //System.out.println(y);
        }
    }
    
    public int pop()
    {
        int nVal = m_tower.empty() ? Integer.MAX_VALUE : ((Integer)m_tower.pop()).intValue();
        repaint();
        return nVal;
    }
    
    public int peek()
    {
        return (m_tower.empty()) ? Integer.MAX_VALUE : ((Integer)m_tower.peek()).intValue();
    }
    
    public void push(int nVal)
    {
        m_tower.push(new Integer(nVal));
        repaint();
    }
}
