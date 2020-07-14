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




