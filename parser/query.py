""""
Class for Representing an SQL Query.
Reads following grammar

select col1, col2, ...., coln
from t1, t2, ....., tn
where cond1 ..... cond2
group by coli,.... colj
Having condk.... condm


"""

import sqlparse as sqp #Library for tokenizing SQL statement


class Query:
  def __init__(self, query):

  def query_parse(query):
    """
    Parse Query into SQL Dictionary with key value pairs of 
    SubClause : identifier List
    """
    statement = sqp.parse(query)[0] #Get Tokenized list of first SQL statement
    s1_clean = filter(lambda x : str(x.ttype)[11:21] != 'Whitespace')


  def identifier_to_list(sqp_token):
    """
    Convert IDENTIFIER List or Identifier into a list of columns
    """
    try:
      tokens = list(sqp_token)
      named_cols = filter(lambda x: x.ttype == None)
      return list(map(lambda x: x.value, named_cols))

    #Assumes failure means we are working with Identifier only
    except:
      return list([sqp_token.value])




 
