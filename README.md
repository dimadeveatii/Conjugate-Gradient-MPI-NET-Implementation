ConjugateGradient MPI.NET
=================

An implementation of sequential and parallel Conjugate Gradient Algorithm using MPI.NET.
The algorithm solves the Ax=b sistems of linear equations, where A is symmetric and positive-definite.
For more information about this method visit the wiki page: http://en.wikipedia.org/wiki/Conjugate_gradient_method.

In order to run the parallel implementation you need to install 
the MPI.NET sdk from http://osl.iu.edu/research/mpi.net/software/ . 
You should then add reference to the MPI.dll library in the Parallel project, 
the dll is in C:\Program Files (x86)\MPI.NET\Lib\ after you install the MPI.NET sdk.
In order to run it on windows using the mpiexec util, first download and
install the Microsoft Compute Cluster Pack: http://www.microsoft.com/en-us/download/details.aspx?id=239.

Example: 
Running the sequential algorithm:
>>sequential input.txt
Running the parallel algorithm on 4 processes:
>>mpiexec -n 4 parallel.exe input.txt

where input.txt is a text file containing n lines and n+1 doubles in each column:
A11 A12 ... A1n b1
A21 A22 ... A2n b2
... ... ... ... ..
An1 An2 ... Ann bn

Please note that the algorithm computes correct values only for symmetric and positive-definite matrices A.
In the solution folder you can also find a utility to generate such matrices.
