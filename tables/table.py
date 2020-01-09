"""
Class for representing a data table
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
      
      # If no column names given, makes c1, ... cn column names
      headers = ["c" + str(i+1) for i in range(dimX)] # Super Fucking Greasy
    else:
      headers = col_names
    return (table, headers)
  raise Exception("We do not support loading this file type")


class Table:
  """
  Class for representing a data table.
  Consists of a schema and data
  """
  def __init__(self, file_name):
    table, headers = load_table(file_name)
    self._schema = headers
    self._data = data

  def get_schema(self):
    return self._schema

  def get_data(self):
    return self._data

