# HarkDB
[![Build Status](https://travis-ci.org/philass/HarkDB.svg?branch=master)](https://travis-ci.org/philass/HarkDB)

HarkDB is a SQL Query Engine accelerated with the GPU. HarkDB is written in [Futhark](https://github.com/diku-dk/futhark), 
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

HarkDB aims to have very similary calling semantics to [BlazingDB](https://github.com/BlazingDB/blazingsql). 


# Requirements

- [Futhark Programming Language](https://github.com/diku-dk/futhark) 
- OpenCL/CUDA (neccesary for parallel execution)


# Installation and Building

Installing and building this git repository is as simple as
```bash
git clone https://github.com/philiplassen/HarkDB.git && cd HarkDB
./setup.sh
```

# Usage
An example program using HarkDB
```python
from FutharkContext import FutharkContext
import time

fc = FutharkContext()
fc.create_table('game_1', 'data.csv')
sel = fc.sql("select col1, col3 from game_1")
print(sel)
```
