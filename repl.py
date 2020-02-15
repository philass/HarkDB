# /usr/bin/env python3

"""
File contains code for parsing SQL Queries
and Meta Commands
"""
import os

from parser import query as que
import numpy as np

from tables import table as tab
from CodeGen import fut_gen as fg

# Tables maps table names -> (table, headers)


def command_parser(query):
    """Parse the text into a SQL Query"""
    global TABLES
    keywords = query.split()
    if keywords[0][0] == '.':
        meta_command(keywords)
    else:
        quer = que.Query(query)
        rep = fg.Representation(TABLES, quer)
        rep.generate_futhark()
        t_called = rep.get_table()
        idxs = rep.get_idxs()
        sys_call()
        from futhark import db_sel
        val = db_sel.db_sel()
        data = np.transpose(t_called.get_data().astype('float32'))
        data_t = data.T
        res1 = val.select_from_where(data_t, np.array(idxs).astype('int32'))
        print(data_t)
        print(res1)


TABLES = []


def sys_call():
    """
    Compiles and makes the GPU python library
    """
    os.system("cd futhark/ && make fut_lib")


def get_help():
    """
    Prints help information
    """
    print(".exit \t Exit this program")
    print(".import FILE TABLE \t import data from FILE into TABLE")


def meta_command(keywords):
    """
    Handles database meta commands
    which are command starting with "."
    """
    global TABLES
    if keywords[0] == ".import":
        if len(keywords) == 3:
            [file_name, table_name] = keywords[1:]
            table_to_add = tab.Table(table_name, file_name)
            TABLES += [table_to_add]
    elif keywords[0] == ".help":
        get_help()
    else:
        print("Didn't recognize command " + keywords[0])


USER_INPUT = input("harkdb> ")
while USER_INPUT != ".exit":
    command_parser(USER_INPUT)
    USER_INPUT = input("harkdb> ")