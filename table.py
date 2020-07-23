"""
Class for representing a data table
"""

import numpy as np
import pandas as pd

def load_df(df):
    # Possibly need to add a check to make sure the numpy values are of the same type
    return df.to_numpy(), list(df) 

def load_np(nparray, col_names=None):
    if col_names == None:
        return nparray, ["col" + str(i + 1) for i in range(nparray.shape[0])]
    else:
        return nparray, col_names

def load_file(file_name, col_names=None):
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
        if col_names is None:
            (_, dim_x) = table.shape

            # If no column names given, makes c1, ... cn column names
            headers = ["c" + str(i+1)
                       for i in range(dim_x)]  # Super Fucking Greasy
        else:
            headers = col_names
        return (table, headers)
    raise Exception("We do not support loading this file type")

def load_table(table_name, table):
    if type(table) == type(pd.DataFrame()):
        return load_df(table)
    elif type(table) == type(np.array([[]])):
        return load_np(table)
    elif type(table) == type(""):
        return load_file(table)
    else:
        raise Exception("Table is not in a file, numpy array or dataframe")

class Table:
    """
    Class for representing a data table.
    Consists of a schema and data
    """

    def __init__(self, table_name, file_name):
        self._table_name = table_name
        table, headers = load_table(table_name, file_name)
        self._schema = headers
        self._data = table

    def get_schema(self):
        """
        return schema
        """
        return self._schema

    def get_data(self):
        """
        return data
        """
        return self._data

    def get_name(self):
        """
        return name
        """
        return self._table_name
