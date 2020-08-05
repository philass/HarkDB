"""
Contains the Class that initializes the 
futhark context
"""

"""
from blazingsql import BlazingContext
bc = BlazingContext()
bc.create_table('game_1', df)
bc.sql('SELECT * FROM game_1 WHERE val > 4')


import numpy as np
import _sql
from futhark_ffi import Futhark

test = Futhark(_sql)
res = test.test3(np.arange(10))
test.from_futhark(res)


What am i trying to figure Out. How to match the API calls of blazingcontext

fc = FutharkContext() | test = Futhark(_sql)
fc.create_table('game_1', df) | add to dictionary of tables and names
fc.sql('select * from game_1 where val > 4') | parse_query && test.sql_query() && test.from_futhark


"""

from futhark_ffi import Futhark
from parse import sql_parse
import _main # Will be modified to something like sql_.fut
import numpy as np
from table import Table


class FutharkContext:

    def __init__(self):
        self.FutEnv = Futhark(_main)
        self.tables = {}

    def create_table(self, table_name, table):
        """
        Stores a table if the table
        """
        table = Table(table_name, table) 
        # possibly need to do an exception check here or line above
        self.tables[table_name] = table

    def drop_table(self, table_name):
        del self.tables[table_name]

    def sql(self, sql_statement):
        """ 
        Should call something here that takes two arguements 

        sql_parse(tables, sql_statement)
        """
        val_dic = sql_parse(self.tables, sql_statement)
        t1 = val_dic["table"]
        sel_col = val_dic["select"]
        if "groupbys" in val_dic:
            res = self.FutEnv.query_sel(t1, np.array(sel_col))
        else:
        return self.FutEnv.from_futhark(res)
