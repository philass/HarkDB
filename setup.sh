#!/bin/bash


##################################################
#
# Script for building HarkDB
#
#################################################

futhark c --library futhark/main.fut -o main
build_futhark_ffi main
