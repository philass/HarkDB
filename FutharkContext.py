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
import _test # Will be modified to something like sql_.fut
import numpy as np


class FutharkContext:

    def __init__(self):
        self.FutEnv = Futhark(_test)
        self.tables = {}

    def creat_table(self, table_name, table):
        """
        Stores a table if the table
        """
        table = Table(table_name, table) 
        # possibly need to do an exception check here or line above
        self.tables[table_name] = table
        """
        if type(table) == type(pd.DataFrame()):
            

        elif type(table) == type(np.array([[]])):

        elif type(table) == type(""):

        else:
            except("Table is not in a file, numpy array or dataframe")

        """
    


        
            

        #if pandas dataframe do something

        #if string assume it is a file and do something

        #if numpy do something

