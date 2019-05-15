/***************************************************************
 * 15/10/2002
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
 * create an XML document
 *********************************/

create_xml_file(X):-
   tell(X),
   % create root element
   xml_element('bookstore', [], [], Root),
   % create child 1
   xml_element('book', [], [], Book1),
   %create attributes
   xml_attribute('author', 'Ugo Chirico', Att1),
   xml_attribute('pages', '321', Att2),
   xml_attribute('price', '50', Att3),
   % append attributes
   xml_append_attribute(Att1, Book1, Book11),
   xml_append_attribute(Att2, Book11, Book12),
   xml_append_attribute(Att3, Book12, Book13),
   % create child 
   xml_text('XML in Prolog', Title1),
   % append child
   xml_append_child(Title1, Book13, Book14),
   % create child 2
   xml_element('book', [], [], Book2),
   %create attributes
   xml_attribute('author', 'Ornella Simeoli', Att21),
   xml_attribute('pages', '276', Att22),
   xml_attribute('price', '150', Att23),
   % append attributes
   xml_append_attribute(Att21, Book2, Book21),
   xml_append_attribute(Att22, Book21, Book22),
   xml_append_attribute(Att23, Book22, Book23),
   % create child
   xml_text('To fall in love for you', Title2),
   % append child
   xml_append_child(Title2, Book23, Book24),
   % append children to root
   xml_append_child(Book14, Root, Root1),
   xml_append_child(Book24, Root1, Root2),
   % create document
   xml_document('1.0', [], [], Root2, Doc),
   xml_write_document(Doc),
   told.

/**********************************
 * Browse an XML document
 *********************************/

browse_xml_file(FilePath):-
   see(FilePath),
   xml_read_document(X),
   browse(X),
   !,
   seen. 

browse([]):-!.

browse([Element|Rest]):-
   browse(Element),
   browse(Rest).

browse(Element):-
   xml_object_type(Element, xml_document),
   xml_document_version(Element, Ver),
   (xml_document_encoding(Element, Enc) ; Enc = 'UTF8'),
   xml_document_root(Element, Root),
   write('XML document'), nl,
   write('Version: '),
   write(Ver), nl,
   write('Encoding: '), 
   write(Enc), nl,
   write('Root: '), nl,   
   browse(Root),
   nl.

browse(Element):-
   xml_object_type(Element, xml_element),
   xml_object_name(Element, Name),
   write('<Name>: '),
   write(Name), nl,
   xml_element_attributes(Element, Attributes),   
   write('Attributes: '), nl,
   browse(Attributes),
   xml_element_children(Element, Children),   
   write('Children: '), nl,
   browse(Children),
   write('<\\Name>:'), write(Name), nl.

browse(Element):-
   xml_object_type(Element, xml_comment),
   xml_object_value(Element, Value),   
   write('Comment: '),nl,
   write(Value), nl.

browse(Element):-
   xml_object_type(Element, xml_cdata),
   xml_object_value(Element, Value),   
   write('CDATA: '),nl,
   write(Value), nl.

browse(Element):-
   xml_object_type(Element, xml_text),
   xml_object_value(Element, Value),   
   write('Text: '),nl,
   write(Value), nl.

browse(Element):-
   xml_object_type(Element, xml_pi),
   xml_object_name(Element, Name),   
   xml_object_value(Element, Value),   
   write('Processing Instruction: '),nl,
   write(Name), nl,
   write(Value), nl.

browse(Element):-
   xml_object_type(Element, xml_attribute),
   xml_object_name(Element, Name),   
   xml_object_value(Element, Value),   
   write('Attribute: '),nl,
   write(Name), nl,
   write(Value), nl.

