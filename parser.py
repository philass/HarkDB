"""
File contains code for parsing SQL Queries
and Meta Commands
"""


def sql_parser(query):
  """Parse the text into a SQL Query"""
  keywords = text.split()
  if keywords[0][0] == '.':
    meta_command(keywords)
  else:
    

