/*
    MADE BY: Gonçalo Amador, Rui Brás
*/

#include<stdio.h>
#include<stdlib.h>
#include<windows.h>

void print_errors(char error)
{
	switch(error)
	{
		case 'm': 
			MessageBox(0,"ERROR IN MEMORY ALOCATION!","FATAL ERROR",MB_OK | MB_ICONERROR);
			exit(EXIT_FAILURE);
            break;
		case 'r': 
			MessageBox(0,"ERROR IN MEMORY REALOCATION!","FATAL ERROR",MB_OK | MB_ICONERROR);
			exit(EXIT_FAILURE);
			break;
		case 'o': 
			MessageBox(0,"ERROR OPENING FILE!","FATAL ERROR",MB_OK | MB_ICONERROR);
			exit(EXIT_FAILURE);
			break;
		case 'a':
			MessageBox(0,"ERROR, INVALID NUMBER OF ARGUMENTS!","FATAL ERROR",MB_OK | MB_ICONERROR);
			exit(EXIT_FAILURE);
			break;
		default: 
			MessageBox(0,"UNKNOWN ERROR!","FATAL ERROR",MB_OK | MB_ICONERROR);
			exit(EXIT_FAILURE);
        
	}    
}