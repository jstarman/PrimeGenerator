#SoftSource Coding Competition - Prime Number Generation Revisited

##Summary
On November 3rd, 2010, Steve conducted Coding Competition III, which was the original prime number generation competition.  It turned out to be a really fun competition, and an excellent challenge.  In fact, it worked so well that it was quickly adopted, with some slight adjustments, as our practical coding exercise, which we still use as part of the interviewing process.
Time to take that original competition to the next level!
The objective of this competition is similar to the objective of the original competition - to code a solution to generate as many prime numbers as possible in 10 seconds, while satisfying all the requirements specified in the reference implementation.  
However, this time, your solutions will be run on a 32-core Windows VM hosted in Azure!
##TODO
1.	Open and run the PrimeCompetition solution, which includes my prime number generator reference implementation - GeneratorExample.
2.	Create your own prime number generator implementation by extending the GeneratorBase abstract class, being sure to satisfy all the requirements specified in the GeneratorExample summary comments.
3.	By the time limit, send me your prime number generator implementation as a single .cs file.  This file should contain your implementation of GeneratorBase, as well as any other code your solution depends on.
4.	I’ll embed your .cs file in the PrimeCompetition solution and build an EXE to run and validate your solution.  Your solution must run in the stock version of the PrimeCalculation solution.  That means no project configuration changes, no additional references, and no new or modified code outside your single .cs file.
5.	We’ll run all the EXEs on the 32-core machine to see who wins!  Your solution must pass all the validation tests at the end of the run to be eligible to win.
6.	Afterwards, we’ll discuss solution approaches.
##Solution
May I suggest a multi-threaded solution?  You could spin up your own Threads?  Or use the managed ThreadPool directly?  Or create BackgroundWorkers?  Or use the Task Parallel Library?  Or Parallel LINQ?  There are plenty of options, just don’t forget, whatever solution you come up with, it needs to satisfy all the requirements specified in the GeneratorExample summary comments.
Testing
You can certainly test your solution on your development laptop, but you may also want to test your solution on a machine with more processor cores.  If you haven’t already, you can activate the Azure subscription associated with your MSDN Enterprise subscription.  Once active, you can create a Virtual Machine with up to 16 cores, to be used for testing your solution.
