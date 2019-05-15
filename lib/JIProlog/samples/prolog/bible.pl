
/*****************************************
 * bible.pl
 *
 * 09/19/2002
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


father(terach,abraham).
father(terach,nachor).
father(terach,haran).
father(abraham,isaac).
father(haran,lot):-!.
father(haran,milcah).
mother(sara,isaac).
male(terach).
male(abraham).
male(nachor).
male(haran).
male(isaac).
male(lot).
female(sarah).
female(milcah).
female(yiscah).
likes(X,pome).
son(X,Y):-father(Y,X),male(X).
daughter(X,Z):-father(Z,X),female(X).
granfather(X,Z):-father(X,Y),father(Y,Z).
