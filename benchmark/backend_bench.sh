#!/bin/bash

##################################################
# Script for running benchmarks of 
# different Futhark backends
##################################################

# Set parameters for futhark
# dataset generation

benchmark () {
    # Run with different backends
    # Sequential Comparison
    #futhark bench --backend=python ../futhark/$1.fut
    futhark bench --backend=c ../futhark/$1.fut
    # Parralel Backend Comparison
    #futhark bench --backend=pyopencl ../futhark/$1.fut
    futhark bench --backend=opencl ../futhark/$1.fut
    #futhark bench --backend=cuda ../futhark/$1.fut
    # Delete Files in Benchmarking process
    rm ../futhark/dataset_$1.data
    rm ../futhark/$1.c
    rm ../futhark/$1
}

sizes=( 100 1000 10000 100000 1000000 10000000 )
if [ "$1" = "-s" ]; 
then
	for i in "${sizes[@]}"
	do
	    DB_TYPE="[$i][20]u32"
	    DB_BOUND="--u32-bounds=0:100"
	    SCOL_TYPE="[4]i32"
	    SCOL_BOUND="--i32-bounds=3:19"
	    futhark dataset $DB_BOUND -g $DB_TYPE $SCOL_BOUND \
				      -g $SCOL_TYPE > ../futhark/dataset_select_where.data
	    
	    ls ../futhark/       # For Debugging purposes
	    benchmark select_where
	done
elif [ "$1" = "-g" ]; 
then
    DB_TYPE="[100][20]u32"
    DB_BOUND="--u32-bounds=0:100"
    GCOL_TYPE="i32"
    GCOL_BOUND="--i32-bounds=0:2"
    SCOL_TYPE="[4]i32"
    SCOL_BOUND="--i32-bounds=3:19"
    TCOL_TYPE="[4]i32"
    TCOL_BOUND="--i32-bounds=0:4"
    
    # Print Commands as they run
    #   set -x


    # Create Dataset file for benchmarking
    futhark dataset $DB_BOUND -g $DB_TYPE $GCOL_BOUND \
    			      -g $GCOL_TYPE $SCOL_BOUND \
                              -g $SCOL_TYPE $TCOL_BOUND \
    			      -g $TCOL_TYPE > ../futhark/dataset_groupby.data
    
    ls ../futhark/       # For Debugging purposes
    benchmark groupby    
else
    echo "Pass -g or -s Flag"
fi
