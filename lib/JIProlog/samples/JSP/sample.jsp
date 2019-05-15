<HTML>
<HEAD><TITLE>
    Sample JSP
</TITLE></HEAD>

<%@ page language="java" import="java.util.Vector" %>
<%@ page import="com.ugos.JIProlog.engine.*" %>


<%
        // New instance of prolog engine
        JIPEngine jip = new JIPEngine();
        JIPTerm queryTerm = null;
        
        // collector for solution
        Vector vect = new Vector();
                
        // parse query
        try
        {
            // consult file
            // files are searched in the search path
            jip.consultFile("bible.pl");
            
            queryTerm = jip.getTermParser().parseTerm("father(X, Y).");
        }
        catch(JIPSyntaxErrorException ex)
        {
            ex.printStackTrace();
        }
        catch(IOException ex)
        {
            ex.printStackTrace();
        }
                
        // open Query
        JIPQuery jipQuery = jip.openSynchronousQuery(queryTerm);
        JIPTerm solution;
          
        try
        {
            // Collect solutions
            
            // Loop while there is another solution
            while ((solution = jipQuery.nextSolution()) != null)
            {
                vect.addElement(solution);
            }
        }
        catch(JIPRuntimeException ex)
        {
            System.out.println(ex.getMessage());
        }
 %>

<BODY BGCOLOR="white">


<B> Solutions: </B>
<ul>
<%
    
    // print solutions
    for(int i = 0; i < vect.size(); i++)
    {
%>
        <li> <%= vect.elementAt(i) %>
<%
    }
%>
</ul>
</BODY>
</HTML>

