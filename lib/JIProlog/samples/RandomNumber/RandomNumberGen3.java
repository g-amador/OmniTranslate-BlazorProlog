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
import java.util.*;

public class RandomNumberGen3 extends JIPXCall
{
    Random m_random;

    int m_nMin = -1, m_nMax = -1;
    JIPNumber m_rand;
  
    public boolean unify(JIPCons input, Hashtable varsTbl)
    {
        // input is a List like [min, max]

        // set min max parameters only the first time
        if(m_random == null)
        {
	        // initialize the parameters
            m_random = new Random()

            JIPTerm term = input.getNth(1);
            if(term instanceof JIPVariable)
                term = ((JIPVariable)term).getValue();
            
            JIPNumber min = (JIPNumber)term;
            
            term = input.getNth(2);
            if(term instanceof JIPVariable)
                term = ((JIPVariable)term).getValue();
            
            JIPNumber max = (JIPNumber)term;
            
            m_nMin = (int)min.getValue();
            m_nMax = (int)max.getValue();
        }
            
        int n = (int)(m_random.nextDouble() * (m_nMax - m_nMin)) + m_nMin;
        m_rand = JIPNumber.create(n);
        return input.getNth(3).unify(m_rand, varsTbl);
    }
    
    public boolean hasMoreChoicePoints()
    {
        return true;
    }

}

