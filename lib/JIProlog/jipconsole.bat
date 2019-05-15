@echo off

REM ==================================
REM Batch for starting JIProlog console on Windows
REM Author: Ugo Chirico [05/12/2002]
REM ==================================

start java -classpath ".;%CLASSPATH%;jipconsole.jar" com.ugos.jiprolog.gui.JIPConsole  > log.txt



