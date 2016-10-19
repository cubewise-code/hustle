# hustle
Hustle is a small utility that can be used to manage threads when executing command line tools. The tool was built to take advantage parallel loading in IBM Cognos TM1, specifically the tm1runti.exe. Hustle enables you to specify the number of concurrent threads you want to be executed at any one time and pass a batch of commands to be executed on these threads.

Although Hustle was designed to be used with TM1 it can be used to execute any command line executable. It is great for run batch processes concurrently allowing you take advantage of all of your CPU cores.

Hustle is very simple to use, the command line tool takes 2 arguments:

* A path to text file that contains the commands to be executed
* The maximum number of threads to be used

Example:
```
hustle.exe "RunTIBatch.txt" 16
```

If for example you want to execute on a maximum of 16 threads and have 30 commands (or batches) to execute hustle will:

Start up 10 threads
As soon as 1 thread finishes start a new thread
Continue until all commands have been executed
Example batch file:

```
"C:\Program Files\Cognos\TM1\bin\tm1runti.exe" -process Cub.Flight -adminhost CW111 -server flightstats -user TI01 -pwd "" pYear=2000 pMonth=01
"C:\Program Files\Cognos\TM1\bin\tm1runti.exe" -process Cub.Flight -adminhost CW111 -server flightstats -user TI02 -pwd "" pYear=2000 pMonth=02
"C:\Program Files\Cognos\TM1\bin\tm1runti.exe" -process Cub.Flight -adminhost CW111 -server flightstats -user TI03 -pwd "" pYear=2000 pMonth=03
```

You can call the Hustle directly from a TM1 Turbo Integrator process using the ExecuteCommand function, using a 1 as the second argument forces the process to wait until ALL of the commands complete.

Example:

```
sCommand = 'C:\TM1\Tools\Hustle.exe "RunTIBatch.txt" 16';
ExecuteCommand(sCommand, 1);
``` 

Hustle is shared by Cubewise the developers of Pulse for TM1, a comprehensive administration tool for IBM Cognos TM1. Pulse enables you to manage TM1 in a way you haven't been able to do before:

* Montoring
* Documentation
* Relationship diagrams
* Change Tracking
* Source Control
* Migration
 
