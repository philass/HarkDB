#!/usr/bin/env python3
"""
File contains code for parsing SQL Queries
and Meta Commands
"""

from tables import table as tab
from parser import query as que
from CodeGen import futGen as fg
import numpy as np
import os

# Tables maps table names -> (table, headers)

def command_parser(query):
  """Parse the text into a SQL Query"""
  global tables
  keywords = query.split()
  print(keywords)
  if keywords[0][0] == '.':
    meta_command(keywords)
  else:
    q = que.Query(query)
    rep = fg.Representation(tables, q) 
    rep.generateFuthark()
    t_called = rep.get_table()
    idxs = rep.get_idxs()
    sys_call()
    from futhark import db_sel
    val = db_sel()
    res = val.select_from_where(t_called.get_data(), np.array(idxs).astype('int32'))
    print(res)


tables = []


def sys_call():
  os.system("cd futhark/ && make fut_lib")  

  
  
def meta_command(keywords):
  """
  Handles database meta commands
  which are command starting with "."
  """
  global tables
  if keywords[0] == ".import":
    if len(keywords) == 3:
      [file_name, table_name] = keywords[1:]
      t = tab.Table(table_name, file_name)
      tables += [t]
  else: 
    print("Didn't recognize command " + keywords[0])
        
 
user_input = input("harkdb> ")
while (user_input != ".exit"):
  command_parser(user_input)
  print([t.get_name() for t in tables])
  user_input = input("harkdb> ")


