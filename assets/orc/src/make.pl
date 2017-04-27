:- [orc2text].
:- qsave_program('ORC2TEXT.exe',[goal(descodifica('orc.txt','text.txt')), emulator(swi('bin/xpce-stub.exe'))]).
:- [text2orc].
:- qsave_program('TEXT2ORC.exe',[goal(descodifica('text.txt','orc.txt')), emulator(swi('bin/xpce-stub.exe'))]).
:- halt.