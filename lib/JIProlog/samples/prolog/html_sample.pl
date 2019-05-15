/***************************************************************
 * 03/12/2002
 *
 * Copyright (C) 2002 Ugo Chirico
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
 ****************************************************************/

/**********************************
 * create an HTML document
 *********************************/

create_html_file(X):-
   tell(X),
   % create root element
   html_tag('html', [], [], Html),
   % create head
   html_tag('head', [], [], Head),
   % create title
   html_text('my html page', T_Title),
   html_tag('title', [], [T_Title], Title),
   % create body
   html_tag('body', [], [], Body),
   %create attributes for body
   html_attribute('bgcolor', '#F0F0FF', BGColorAtt),
   % append attributes to body
   html_append_attribute(BGColorAtt, Body, Body1),
   % create child
   html_text('HTML in Prolog', Title1),
   html_tag('h1', [], [Title1], H1_Title),
   % append child
   html_append_child(H1_Title, Body1, Body2),
   % create child 2
   html_tag('hr', [], [], HR),
   % append child
   html_append_child(HR, Body2, Body3),
   % create child 3
   html_tag('p', [], [], P),
   %create attributes
   html_attribute('align', 'left', AlignAtt),
   % append attributes
   html_append_attribute(AlignAtt, P, P1),
   % append child
   html_append_child(P1, Body3, Body4),
   % create child 5
   html_text('To fall in love for you', Title2),
   % append child
   html_append_child(Title2, Body4, Body5),
   % append Title to Head
   html_append_child(Title, Head, Head1),
   % append Head to root
   html_append_child(Head1, Html, Html1),
   % append Body to root
   html_append_child(Body5, Html1, Html2),
   % create document
   html_document([], Html2, Doc),
   html_write_document(Doc),
   !,
   told.

/**********************************
 * Browse an html document
 *********************************/

browse_html_file(FilePath):-
   see(FilePath),
   html_read_document(X),
   write(X), nl,
   browse(X),
   !,
   seen.

browse([]):-!.

browse([Element|Rest]):-
   !,
   browse(Element),
   browse(Rest).

browse(Element):-
   html_object_type(Element, html_document),
   !,
   html_document_root(Element, Root),
   write('html document'), nl,
   write('Root: '), nl,
   browse(Root),
   nl.

browse(Element):-
   html_object_type(Element, html_tag),
   !,
   html_object_name(Element, Name),
   write('<Name>: '),
   write(Name), nl,
   html_tag_attributes(Element, Attributes),
   write('Attributes: '), nl,
   browse(Attributes),
   html_tag_children(Element, Children),
   write('Children: '), nl,
   browse(Children),
   write('<\\Name>:'), write(Name), nl.

browse(Element):-
   html_object_type(Element, html_comment),
   !,
   html_object_value(Element, Value),
   write('Comment: '),nl,
   write(Value), nl.

browse(Element):-
   html_object_type(Element, html_text),
   !,
   html_object_value(Element, Value),
   write('Text: '),nl,
   write(Value), nl.

browse(Element):-
   html_object_type(Element, html_attribute),
   !,
   html_object_name(Element, Name),
   html_object_value(Element, Value),
   write('Attribute: '),nl,
   write(Name), nl,
   write(Value), nl.
