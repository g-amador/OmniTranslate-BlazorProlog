/*****************************************
 * 09/07/2004
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
import java.util.*;
import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;

/**
 * The servelet class.
 */
public class JIPServlet extends HttpServlet
{
    private static final String SERVLET_ADDRESS = "http://www.mycgiserver.com/servlet/ugosweb.jiprolog.JIPServWrap";//"JIPServlet";
    
    private static final String LOG_PATH = "/members/5YApVvo5AA3NscEF6vRoonMPn2ac0Gp4/JIPServletLog.log";
    
    /** JIProlog engine*/
    private JIPEngine m_engine = new JIPEngine();
    /** hashtable for pending query */
    private Hashtable m_queryTbl = new Hashtable(100);
    
    /** Log file*/
    private static PrintWriter m_log;
    
    static
    {
        // create log file
        try
        {
            m_log = new PrintWriter(new FileWriter(LOG_PATH, true), true);
        }
        catch(IOException ex)
        {
            m_log = null;
        }
    }
    
    /** Called when someone accesses the servlet. */
    public void doGet(HttpServletRequest request, HttpServletResponse response)
        throws IOException, ServletException
    {
        m_log.println("-----------------------------------------------------");
        m_log.println("New Connection by " + request.getRemoteAddr());
        m_log.println("Host: " + request.getRemoteHost());
        m_log.println("User: " + request.getRemoteUser());
        m_log.println("Session ID: " + request.getRequestedSessionId());
        
        // Get the query which was supplied when someone submitted the form.
        String strQuery = request.getParameter("query");
        
        // check query
        if(strQuery != null)
        {
            // check if there is a pending query
            String strIDQuery = request.getParameter("id");
            if(strIDQuery != null)
            {
                // if so close it
                JIPQuery query = (JIPQuery)m_queryTbl.get(strIDQuery);
                if(query != null)
                    query.close();
            }
            
            // create new query
            try
            {
                // open new query
                JIPQuery query = m_engine.openSynchronousQuery(strQuery);
                
                // call the query
                JIPTerm solution = query.nextSolution();
                
                m_log.println("Solution: " + solution);
                m_log.println("more: " + !query.isDeterministic());
                
                // check if solution
                if(solution != null)
                {
                    // add query to the table
                    m_queryTbl.put(Integer.toString(query.hashCode()), query);
                }

                // show solution page
                solutionPage(solution, query, response);
            }
            catch(Throwable ex)
            {
                errorPage(ex, response);
                //throw new ServletException("Syntax error in query", ex);
            }
        }
        else
        {
            // check if there is a pending query
            String strIDQuery = request.getParameter("id");
            if(strIDQuery != null)
            {
                // if so call nextsolution
                JIPQuery query = (JIPQuery)m_queryTbl.get(strIDQuery);
                if((query == null) || query.isClosed())
                    throw new ServletException("Query has been closed");
                
                try
                {
                    JIPTerm solution = query.nextSolution();
                    
                    m_log.println("Solution: " + solution);
                    m_log.println("more: " + !query.isDeterministic());
                    
                    // show solution page
                    solutionPage(solution, query, response);
                }
                catch(Throwable ex)
                {
                    errorPage(ex, response);
                }
            }
            else
            {
                // first time
                //show start page
                startPage(response);
            }
        }
    }
    
    public void solutionPage(JIPTerm term , JIPQuery query, HttpServletResponse response) throws IOException
    {
        // Set content type for the browser.
        response.setContentType("text/html");
        
        // Print the static part of the web page
        PrintWriter out = response.getWriter();
        
        out.println("<html>");
        out.println("<head>");
        out.println("<title>JIProlog servlet samples</title>");
        out.println("</head>");
        out.println("<body>");
        out.println("<h1> JIProlog servlet sample </h1>");
        out.println("<p> Copyright 2002 by Ugo Chirico, Home Page: <a href='http://www.ugosweb.com/jiprolog'>http://www.ugosweb.com/jiprolog</a></p>");
        out.println(" This page gives a demonstration about how JIProlog runs in a servlet<hr>");
        out.println("<b> Result is: <br>" + ((term == null) ? "no" : "yes") + "</b>");
        if(term != null)
        {
            JIPVariable[] vars = term.getVariables();
            for(int i = 0; i < vars.length; i++)
            {
                out.println("<br> - <b>" + vars[i].getName() + " = " + vars[i].toString(m_engine) + "</b>");
            }
        
            // check if more solution
            if(!query.isDeterministic())
            {
                out.println("<form action=\"" + SERVLET_ADDRESS + "\" method=GET>");
                out.println("<input type=hidden name=id value=" + query.hashCode() + ">");
                out.println("<b>Press <input type=submit value=More> for next solution</b>");
                out.println("</form>");
            }
            else
            {
                out.println("<br><b> No more solutions </b>");
            }
        }
        out.println("<hr><br><b> Enter another query and press Submit! </b>");
        out.println("<br> Example: member(X, [a,b,c]).");
        out.println("<form action=\""+ SERVLET_ADDRESS + "\" method=GET>");
        out.println("<b>Query </b><input type=text size=20 name=query>");
        out.println("<input type=hidden name=id value=" + query.hashCode() + ">");
        out.println("   <input type=submit value=Submit>");
        out.println("</form>");
        out.println("<hr>");
        out.println("</body>");
        out.println("</html>");
    }
    
    public void startPage(HttpServletResponse response) throws IOException
    {
        // Set content type for the browser.
        response.setContentType("text/html");
        
        // Print the static part of the web page
        PrintWriter out = response.getWriter();
        
        out.println("<html>");
        out.println("<head>");
        out.println("<title>JIProlog servlet sample</title>");
        out.println("</head>");
        out.println("<body>");
        out.println("<h1> JIProlog servlet sample </h1>");
        out.println("<p> Copyright 2002 by Ugo Chirico, Home Page: <a href='http://www.ugosweb.com/jiprolog'>http://www.ugosweb.com/jiprolog</a></p>");
        out.println(" This page gives a demonstration about how JIProlog runs in a servlet<hr>");
        out.println("<b> Enter your query and press Submit! </b>");
        out.println("<br> Example: member(X, [a,b,c]).");
        out.println("<form action=\"" + SERVLET_ADDRESS + "\" method=GET>");
        out.println("<b>Query </b><input type=text size=20 name=query>");
        out.println("<input type=submit value=Submit>");
        out.println("</form>");
        out.println("<hr>");
        out.println("</body>");
        out.println("</html>");
    }
    
    public void errorPage(Throwable ex, HttpServletResponse response) throws IOException
    {
        // Set content type for the browser.
        response.setContentType("text/html");
        
        // Print the static part of the web page
        PrintWriter out = response.getWriter();
        
        out.println("<html>");
        out.println("<head>");
        out.println("<title>JIProlog servlet sample</title>");
        out.println("</head>");
        out.println("<body>");
        out.println("<h1> JIProlog servlet sample page</h1>");
        out.println("<p> Copyright 2002-2004 by Ugo Chirico, Home Page: <a href='http://www.ugosweb.com/jiprolog'>http://www.ugosweb.com/jiprolog</a></p>");
        out.println("This page gives a demonstration about how JIProlog runs in a servelt<hr>");
        out.println("JIProlog has raised the following exception: <br><b>" + ex.toString() + "</b><hr>");
        out.println("<b>Enter a new query and press Submit! </b>");
        out.println("<br>Example: member(X, [a,b,c]).");
        out.println("<form action=\"" + SERVLET_ADDRESS + "\" method=GET>");
        out.println("<b>Query </b><input type=text size=20 name=query>");
        out.println("<input type=submit value=Submit>");
        out.println("</form>");
        out.println("<hr>");
        out.println("</body>");
        out.println("</html>");
    }
    
    protected void finalize()
    {
        // close log
        m_log.close();
        
        // close all pending query
        Enumeration enum = m_queryTbl.elements();
        while(enum.hasMoreElements())
        {
            ((JIPQuery)enum.nextElement()).close();
        }
    }
}

