:- [morse2text].
:- qsave_program('MORSE2TEXT.exe',[goal(descodif('morse.txt','text.txt')), emulator(swi('bin/xpce-stub.exe'))]).
:- [text2morse].
:- qsave_program('TEXT2MORSE.exe',[goal(descodif('text.txt','morse.txt')), emulator(swi('bin/xpce-stub.exe'))]).
:- halt.
