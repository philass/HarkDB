#!/usr/bin/env python3
"""
Provided functionality for Creating Datasets for benchmarking

Example Usage
-----
./generate filename NROWS NCOLS
-----
"""

import sys
import numpy as np
import pandas as pd

if len(sys.argv) == 4:
    FILE_NAME = sys.argv[1]
    NROWS = int(sys.argv[2])
    NCOLS = int(sys.argv[3])
    HEADERS = ["h" + str(i + 1) for i in range(NCOLS)]
    DATA = np.random.rand(NROWS, NCOLS)
    pd.DataFrame(DATA).to_csv(FILE_NAME, header=HEADERS, index=None)

else:
    print("Wrong number of command line arguements")
