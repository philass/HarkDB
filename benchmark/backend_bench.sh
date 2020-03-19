#!/bin/bash

##################################################
# Script for running benchmarks of 
# different Futhark backends
# 
# By Philip Lassen
##################################################


# Set parameters for futhark
# dataset generation

DB_TYPE="[100][20]u32"
DB_BOUND="--u32-bounds=0:100"
GCOL_TYPE="i32"
GCOL_BOUND="--i32-bounds=0:2"
SCOL_TYPE="[4]i32"
SCOL_BOUND="--i32-bounds=3:19"
TCOL_TYPE="[4]i32"
TCOL_BOUND="--i32-bounds=0:4"

# Print Commands as they run
set -x

# Create Dataset file for benchmarking

futhark dataset $DB_BOUND -g $DB_TYPE $GCOL_BOUND \
			  -g $GCOL_TYPE $SCOL_BOUND \
                          -g $SCOL_TYPE $TCOL_BOUND \
			  -g $TCOL_TYPE > ../futhark/dataset.data

ls ../futhark/       # For Debugging purposes
futhark bench --backend=pyopencl ../futhark/groupby.fut


