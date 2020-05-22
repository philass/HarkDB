#!/usr/bin/env python3

import sys
import csv
import random

size = int(sys.argv[1])

header_line = ["c" + str(i) for i in range(20)]
for num in range(2):
    with open("data" + str(size) + "_" + str(num + 1) + ".csv", "w", newline='') as csvfile:
        writer = csv.writer(csvfile, delimiter=',')
        writer.writerow(header_line)
        for i in range(size):
            writer.writerow([random.randint(0, 100) for i in range(20)])

