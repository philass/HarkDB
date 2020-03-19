#!/bin/bash

##################################################
# Script for running benchmarks of 
# different Futhark backends
# 
##################################################

DB_TYPE="[100][5]u32"
GCOL_TYPE="i32"
futhark dataset -g $DB_TYPE -g $GCOL_TYPE > dataset.data



