"""
File contains code for parsing SQL Queries
and Meta Commands
"""

from load_table import load_table


# Tables maps table names -> (table, headers)
tables = {}

def command_parser(query):
  """Parse the text into a SQL Query"""
  keywords = query.split()
  print(keywords)
  if keywords[0][0] == '.':
    meta_command(keywords)
  else:
    print("non meta commands are not supported yet")
    


def meta_command(keywords):
  """
  Handles database meta commands
  which are command starting with "."
  """
  if keywords[0] == ".import":
    if len(keywords) == 3:
      [file_name, table_name] = keywords[1:]
      try:
        (table, headers) = load_table(file_name)
        tables[table_name] = (table, headers)
        print(table_name + " is loaded")
      except:
        print("Couldn't read/handle file")
    else:
      print("INVALID COMMAND")
      print(".import FILENAME TABLE : is format")
      print("Provided " + str(len(keywords)) + " instead of 3")
  elif keywords[0] == ".list":
    print(list(tables.keys()))
  else: 
    print("Didn't recognize command " + keywords[0])
        
        


    
