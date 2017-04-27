/*
    Feito por: Gonçalo Amador
*/

#include<string.h>
#include "errors_msgout_WIN.h"

int main(int argc, char **argv)
{
    FILE *f, *aux;            //o ficheiro de onde vamos ler as linhas um 
                              //auxiliar onde vamos colocar os dados modificados 
    char c, nome=argv[1][0];  //primeiro caracter do nome do ficheiro para saber 
                              //quantos espaços a mais temos em cada linha e 
                              //para guardar cada caracter lido
    char *frase=NULL;        //vector dinamico onde vai ser guardada cada frase
    int tamanho=1;            //tamanho do vector dinamico 
    
    //garantir que o numero de argumentos esta correcto
    if(argc!=2)
       print_errors('a');

    //abertura dos ficheiros
    f=fopen(argv[1],"r");
    if(f==NULL)
       print_errors('o');
       
    aux=fopen("temp.txt","w");
    if(aux==NULL)
       print_errors('o');
       
    //ler cada caracter do ficheiro e coloca-lo numa string ate mudar de linha 
    //escrever essa linha em aux e limpar o conteudo do vector
    //fazer ate todo o ficheiro lido
    while(fscanf(f,"%c",&c)!=EOF)
    {                            
       //leitura
       if(c!='\n')
       {
          frase=(char*)realloc(frase,tamanho*sizeof(char));
          frase[tamanho-1]=c;
          tamanho++;
       }
       
	   //escrita
       if(c=='\n')
       {
          frase[tamanho-2]='\n';
          fwrite(frase,tamanho-1,sizeof(char),aux);
          tamanho=1;
       }
    }
    
    fclose(f);
    fclose(aux);
    free(frase);
    return 0;
}
