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
    if table_name in tables:
        table = tables[table_name]
    else:
        raise Exception(f"{table_name} is not in tables")

    """
    Now we resolve the select columns??
    """


    columns = table.get_schema()

    if "groupby" not in js_obs.keys():

        fut_cols_selects = []

        select_pairs = js_obj['select']
        
        for pair in select_pairs:
            # how do that pairs come
            if "value" in pair:
                col_name =  pair["value"]
                idx = getIndex(columns, col_name)
                if idx < 0:
                    raise Exception(f"{col_name} is not in the schema of table {table_name}")
                else:
                    fut_cols_selects += [idx]
        return {"table": table.get_data(), "select": fut_cols_selects}

    if "groupby" in js_obj.keys():
        # Instantiate variables that need to be filled
        fut_cols_selects = []
        typ_cols_selects = []
        g_col = None # This will probably be an array if i fix futhark implementation of groupby

        g_col_name = js_obj["groupby"][0]["value"]
        g_col = getIndex(columns, g_col_name)
        if g_col < 0:
            raise Exception(f"{g_col_name} is not in the schema of table {table_name}")

        select_pairs = js_obj['select']
        for dic in select_pairs:
            if dic["value"] == g_col_name:
                fut_cols_selects += [g_col]
                typ_cols_selects += [0]
            elif type(dic["value"]) == type(""):
                bad_col_name = dic["value"]
                raise Exception(f"{bad_col_name} is not an aggregation function or the columns thats grouped on"}
            else:
                # Assume we are dealing with dic["value"] -> {"agg" : "col_name"}
                if "max" in dic["value"]:
                    agg_col_name = dic["value"]["max"]
                    agg_col = getIndex(columns, agg_col_name)
                    if agg_col < 0:
                        raise Exception(f"{agg_col_name} is not in the schema of table {table_name}")
                    fut_cols_selects += [agg_col]
                    typ_cols_selects += [3]
                elif "min" in dic["value"]:
                    agg_col_name = dic["value"]["min"]
                    agg_col = getIndex(columns, agg_col_name)
                    if agg_col < 0:
                        raise Exception(f"{agg_col_name} is not in the schema of table {table_name}")
                    fut_cols_selects += [agg_col]
                    typ_cols_selects += [4]
                elif "sum" in dic["value"]:
                    agg_col_name = dic["value"]["sum"]
                    agg_col = getIndex(columns, agg_col_name)
                    if agg_col < 0:
                        raise Exception(f"{agg_col_name} is not in the schema of table {table_name}")
                    fut_cols_selects += [agg_col]
                    typ_cols_selects += [2]
                elif "prod" in dic["value"]:
                    agg_col_name = dic["value"]["prod"]
                    agg_col = getIndex(columns, agg_col_name)
                    if agg_col < 0:
                        raise Exception(f"{agg_col_name} is not in the schema of table {table_name}")
                    fut_cols_selects += [agg_col]
                    typ_cols_selects += [1]
                else:
                    raise Exception("Don't currently support this aggregations function")
        return {"select": fut_cols_selects, "groupbys": typ_cols_selects, "table": table.get_data()}, "g_col": g_col}
    return {}





