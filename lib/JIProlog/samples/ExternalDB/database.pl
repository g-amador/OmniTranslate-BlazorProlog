/*****************************************
 * JIProlog External Database Management
 * By Ugo Chirico
 *
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

% set the path where JIPxdb.jar file is located
%:-load_library('c:/JIProlog30/lib/jipxdb.jar').

% declare employee/4 as external database of clauses in text format
% employee(ID, Name, Surname, Branch)
:-extern(employee/4, "com.ugos.JIProlog.extensions.database.TextClausesDatabase", "c:/JIProlog30/samples/ExternalDB/employee.txt").

% declare salary/2 as external database of clauses in prolog format
% salary(ID, Salary)
:-extern(salary/2, "com.ugos.JIProlog.extensions.database.PrologClausesDatabase", "c:/JIProlog30/samples/ExternalDB/salary.pl").


employees(X):-
    findall([A, B, C], employee(A, B, C, _Q), X).

salaries(X):-
    findall([Name, Surname, Salary], (employee(ID, Name, Surname, _), salary(ID, Salary)), X).

employee_details(ID, Name, Surname, Branch, Salary):-
    employee(ID, Name, Surname, Branch),
    salary(ID, Salary).



    
    
    

   

