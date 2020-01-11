"""
functionality for checking that a Query matches Schemas of Tables
"""


def get_relevant_tables(tables, query):
  """
  Returns the relevant tables based on a query
  """
  table_names = query.get_From()
  tables_to_keep = [t in tables if tables.get_name() in query]
  return tables_to_keep



def from_check(tables, query):
  """
  Check the from clause of a query
  """
  from_ts = query.get_From():
  table_names = [t.get_name() for t in tables]
  for t in from_ts:
    t_present = False
    if t not in table_names:
      return (False, t + " Not in tables")
  return (True, "")


def query_check(tables, query):
  """
  Tables is a list of Table objects and a Query object
  returns (True, "") if the query is valid, or 
  (False, "errormessage") if query is invalid
  """
  (valid_from, from_error) = from_check(tables, query)  
  (valid_select, select_error) = check_select(tables, query)
  (valid_group, group_error) = check_group_by(tables, query)



def check_select(tables, query):
  """
  Check the select clause is legal with relation
  to table schemas
  """
  select_cols = query.get_Select()
  for c in select_cols:
    col_present = False
    for t in ts:
      if c in t.get_schema():
        col_present = True
    if not col_present:
      return (False, "column " + c + " wasn't present in tables")


    
def check_group_by(tables, query):
  """
  Check the group by clause is legal with relation
  to table schemas
  """
  select_cols = query.get_Group_by()
  for c in select_cols:
    col_present = False
    for t in ts:
      if c in t.get_schema():
        col_present = True
    if not col_present:
      return (False, "column " + c + " wasn't present in tables")


  
