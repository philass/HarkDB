# HarkDB
[![Build Status](https://travis-ci.org/philass/HarkDB.svg?branch=master)](https://travis-ci.org/philiplassen/HarkDB)

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
- [CMake](https://cmake.org/download/) >= 3.11


# Installation and Building

Installing and building this git repository is as simple as
```bash
git clone https://github.com/philiplassen/HarkDB.git && cd HarkDB
mkdir build && cd build
cmake ..
```

# Usage

Simply use the following to spin up the CLI from the root directory
```bash
build/out
```
An example CLI session may look like the following
```bash
harkdb> .help
.exit 	 Exit this program
.help 	 Show this message
.import FILE TABLE 	 import data from FILE into TABLE
harkdb> .import tests/resources/DataTest.csv t1
harkdb> from t1 select col1, col3
6 6
0 0
0 0
0 0
0 0
6 6
1 3
harkdb> .exit
```

# Tests

The tests can be run with the following commands

```bash
cd build
cmake ..
make runtests
```
