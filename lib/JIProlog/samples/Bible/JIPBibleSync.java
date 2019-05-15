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

import com.ugos.JIProlog.engine.*;

import java.io.*;
import java.awt.*;
import java.util.*;

/*********************************************
 * JIPBible                                  *
 * Syncronous calls to JIPEngine             *
 * Sample provided by JIProlog               *
 * By Ugo Chirico, Copyrighted               *
 * 17/03/2001                                *
 * www.ugosweb.com/jiprolog                  *
 *********************************************/

public class JIPBibleSync
{
    // main
    public static void main(String args[])
    {
        // New instance of prolog engine
        JIPEngine jip = new JIPEngine();
        JIPTerm queryTerm = null;
        
        // parse query
        try
        {
            // consult file
            // files are searched in the search path
            jip.consultFile("bible.pl");
            
            queryTerm = jip.getTermParser().parseTerm("?- father(X, Y).");
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
        JIPQuery jipQuery = jip.openSynchronousQuery(queryTerm);
        JIPTerm solution;
            
        try
        {
            // Collect solutions
            Vector vect = new Vector();
            
            // Loop while there is another solution
            while ((solution = jipQuery.nextSolution()) != null)
            {
                vect.addElement(solution);
            }
                        
            // wait a moment to do something
            try
            {
                Thread.currentThread().sleep(5000);
            }
            catch(InterruptedException ex){};
            
            // print solutions
            for (int i = 0; i < vect.size(); i++)
                System.out.println(vect.elementAt(i));
        }
        catch(JIPRuntimeException ex)
        {
            System.out.println(ex.getMessage());
        }
        
        System.exit(0); // needed to close threads by AWT if shareware
    }
}
