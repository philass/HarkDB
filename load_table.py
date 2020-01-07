"""
Code orchestrates the loading and naming of a table
from multiple representations into a table in memory
and so it can be called by the compiled Futhark Query
Engine
"""


import numpy as np
import pandas as pd

def load_table(file_name, col_names = None):
  """
  Tries to read data from csv or txt files.
  If the data is formatted properly returns
  a tuple of the headers and a numpy table
  of datatype i64 i.e (headers, table)
  """
  if file_name[-3:] == "csv":
    table = pd.read_csv(file_name)
    headers = table.columns.tolist()
    return (table.values, headers)
  if file_name[-3:] == "txt":
    table = np.loadtxt(file_name)
    if col_names == None:
      (_, dimX) = table.shape
      headers = [i+1 for i in range(dimX)] # Kinda Greasy
    else:
      headers = col_names
    return (table, headers)
  raise Exception("We do not support loading this file type")

    



    
