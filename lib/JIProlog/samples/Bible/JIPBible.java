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

import com.ugos.JIProlog.engine.*;
import java.io.*;
import java.awt.*;

/**************************************************
 * JIPBible                                       *
 * Asyncronous calls to JIPEngine using listeners *
 *                                                *
 * Sample provided by JIProlog                    *
 * Copyright By Ugo Chirico, All right reserved   *
 * 17/03/2001                                     *
 * www.ugosweb.com/jiprolog                       *
 **************************************************/


public class JIPBible implements JIPEventListener
{
    // main
    public static void main(String args[])
    {
        JIPBible bible = new JIPBible();
        
        bible.start();
    }
    
    public synchronized void start()
    {
        // New instance of prolog engine
        JIPEngine jip = new JIPEngine();
                        
        System.out.println(jip.getVersion());
        
        // add listeners
        jip.addEventListener(this);
                
        JIPTerm query = null;
                            
        // parse query
        try
        {
            // consult file
            // files are searched in the search path
            jip.consultFile("bible.pl");
        
            query = jip.getTermParser().parseTerm("?- father(X, Y).");
            
            // the latter is equivalent to
            // query = JIPTerm.parseTerm("?-father(X, Y).");
        }
        catch(JIPSyntaxErrorException ex)
        {
            ex.printStackTrace();
            System.exit(0);  // needed to close threads by AWT if shareware
        }
        catch(IOException ex)
        {
            ex.printStackTrace();
            System.exit(0);  // needed to close threads by AWT if shareware
        }
                
        // open Query
        synchronized(jip)
        {
            // It's better to have the first call under syncronization
            // to avoid that listeners is called before the method
            // openQuery returns the handle.
            int nQueryHandle = jip.openQuery(query);
        }
        
        // wait for end of the computation
        // because this is an application you must wait for the end of the query
        // in order to avoid the process to exit
        try
        {
           wait();
        }
        catch(InterruptedException ex)
        {
            ex.printStackTrace();
        }
        
        System.out.println("Bye Bye");
    }
    
    // open event occurred
    public void openNotified(JIPEvent e)
    {
        System.out.println("open");
    }

    // more event occurred
    public void moreNotified(JIPEvent e)
    {
        System.out.println("more");
    }
    
    // A solution event occurred
    public void solutionNotified(JIPEvent e)
    {
        // syncronization is required to avoid that listeners is
        // called before the method openQuery returns the handle.
        synchronized(e.getSource())
        {
            // get the query handle
            int nQueryHandle = e.getQueryHandle();
            
            System.out.println("Handle:" + e.getQueryHandle());
            System.out.println("solution:");
            System.out.println(e.getTerm());
            System.out.print("next? ");
    
            int cin = 0;
            
            // wait for user answer
            try
            {
                byte bin[] = new byte[100];
                System.in.read(bin);
                cin = bin[0];
            }
            catch(IOException ex)
            {
                ex.printStackTrace();
            }
                    
            // get the source of the query
            JIPEngine jip = e.getSource();
            
            if (cin == 'y')
                jip.nextSolution(nQueryHandle);
            else
                jip.closeQuery(nQueryHandle);
        }
    }
    
    // A Term has been notified with notify/2
    public void termNotified(JIPEvent e)
    {
        System.out.println("more");
    }
    
    // The end has been reached because there wasn't more solutions
    public void endNotified(JIPEvent e)
    {
        // syncronization is required to avoid that listeners is
        // called before the method openQuery returns the handle.
        synchronized(e.getSource())
        {
            System.out.println("end");
    
            // get the source of the query
            JIPEngine jip = e.getSource();
            // get the query handle
            int nQueryHandle = e.getQueryHandle();
    
            // close query
            jip.closeQuery(nQueryHandle);
        }
    }
    
    public synchronized void closeNotified(JIPEvent e)
    {
        // syncronization is required to avoid that listeners is
        // called before the method openQuery returns the handle.
        synchronized(e.getSource())
        {
            System.out.println("close");
            
            // notify in order to stop waiting and exit from the application
            notify();
        }

        System.exit(0);  // needed to close threads by AWT if shareware
    }

    // An error (exception) has been raised up by prolog engine
    public void errorNotified(JIPErrorEvent e)
    {
        // syncronization is required to avoid that listeners is
        // called before the method openQuery returns the handle.
        synchronized(e.getSource())
        {
            System.out.println("Error:");
            System.out.println(e.getError());
    
            // get the source of the query
            JIPEngine jip = e.getSource();
            // get the query handle
            int nQueryHandle = e.getQueryHandle();
    
            // close query
            jip.closeQuery(nQueryHandle);
        }
    }
}
