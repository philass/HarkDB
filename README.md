# HarkDB
https://travis-ci.org/philiplassen/HarkDB.svg?branch=master

HarkDB is a Multi-backend highly parallel Query Engine. HarkDB is written in [Futhark](https://github.com/diku-dk/futhark), 
which allows it to compile down to a CUDA, OpenCL, or sequential C backend. 
HarkDB aims to run extremely fast on GPU's leveraging the code optimization of the futhark compiler.

Due to compiler limitations the query engine works with homogenous numeric data. It is SQL compatible and supports
the following Clauses
- Select
- From 
- Where
- Group By
- Having
- Sort By

HarkDB comes with a CLI. This CLI is meant to be functionaly similar to SQLite's CLI. 

# Requirements

- [Futhark Programming Language](https://github.com/diku-dk/futhark) 
- OpenCL/CUDA (neccesary for parallel execution)


# Installation

Installing from this git repository is as simple as
```bash
git clone https://github.com/philiplassen/HarkDB.git && make
```
