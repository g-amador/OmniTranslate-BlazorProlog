#!/bin/sh
# Script for starting JIProlog Console on Unix,Linux and Mac.
#
# Set JIP_HOME to the installation path of JIProlog
# Set JAVA_HOME to the base of your Java JDK or JRE is

# Author: Ugo Chirico [05/12/2002]

if [ "$1" = --help ] ; then
	echo "This script loads the JIProlog Console."
	echo "Usage: jipconsole.sh "
	echo "Environment variables:"
	echo "HOME:			home directory of user"
	echo "JIP_HOME:		JIProlog installation directory"	
	echo "JAVA_HOME:	Java implementation base directory"
	echo "JAVA_CMD :    Java interpreter path"	
    exit 0
fi

# Check for JIP_HOME if not already set
if [ -z "$JIP_HOME" ] ; then    
    echo -n "You have not set the JIP_HOME environment variable. "
    JIP_HOME="~/Desktop/projects/code/git/GitHub/ASCIIfromToConverters/lib/JIProlog/"
    echo "Assuming: $JIP_HOME"
fi
     
if [ "$JAVA_HOME" = "" ] ; then
    echo "You have not set the JAVA_HOME environment variable."    
fi

# Allows for those with varying JDKs and standard calls to java
if [ -z "$JAVA_CMD" ] ; then
   if [ -z "$JAVA_HOME" ] ; then
      JAVA_CMD=java
   else
      JAVA_CMD="$JAVA_HOME/bin/java"
   fi   
fi

exec "$JAVA_CMD" -classpath "jipconsole.jar" com.ugos.jiprolog.gui.JIPConsole
