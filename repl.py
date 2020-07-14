#!/usr/bin/env/python3

"""
Contains the CLI for HarkDB
"""

import moz_sql_parser as msp
import json

def line_parser(line):
    """Parse line"""
    keywords = line.split()
    if keywords[0][0] == '.':
        meta_command(keywords)
    else:
        query_parse(line)






