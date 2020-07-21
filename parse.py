"""
Contains functions for transforming SQL statements into an IR 
that can be made into a futhark call
"""

from moz_sql_parser import parse
import json

def getIndex(elements, value):
    for i, v in enumerate(elements):
        if v == value:
            return i
    return -1


def sql_parse(tables, sql_statement):
    """
    Parses an SQL statement
    """

    """resolving the from table is problably the best palce to start this.

    Then we have access to the infromation we need about corresponding columns

    """

    js_obj = parse(sql_statement)
    table = None
    table_name = js_obj['from']
    if table_name in tables.get_name():
        table = tables[table_name]
    else:
        except(f"{table_name} is not in tables")

    """
    Now we resolve the select columns??
    """

    columns = table.get_schema()
    fut_cols_selects = []

    select_pairs = js_obj['select']

    for pair in select_pairs:
        # how do that pairs come
        if "value" in pair
            col_name =  pair["value"]
            idx = getIndex(columns, col_name)
            if idx < 0:
                except(f"{col_name} is not in the schema of table {table_name}")
            else:
                fut_cols_selects += [idx]
    return {"table": table.get_data(), "select": fut_cols_selects}
