/******************************
 * JIProlog application example
 * By Ugo Chirico
 *
 * 12/02/2003
 *
 * Copyright (C) 1999-2003 Ugo Chirico
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
 ******************************/
 
 % this program shows hot to use JIProlog Standalone Interpreter 
 
 % main/0 predicated called by JIProlog Standalone Interpreter 
 

main :-
 	write('-------------------------------------'),nl,
    write('Hello!'),nl,
    write('You are using JIProlog Standalone Interpreter v'),
    ver(X),    
    write(X), nl,	
	write('-------------------------------------'),nl,
    write('Bye Bye'),
    nl.
    