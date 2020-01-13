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

def query_parse(query):
  """
  Parse Query into SQL Dictionary with key value pairs of 
  SubClause : identifier List
  """
  clause_dic = {"select" : [], "from" : [], "where" : [],
                "having" : [], "group by" : []}
  statement = sqp.parse(query)[0] #Get Tokenized list of first SQL statement
  f = lambda x : str(x.ttype)[11:21] != 'Whitespace'
  s1_clean = list(filter(f, list(statement)))
  keywords = clause_dic.keys() - "where" # Where is treated different by sqparse"
  for i in range(len(list(s1_clean))):
    val = s1_clean[i].value
    if val in keywords:
      clause_dic[val] = identifier_to_list(s1_clean[i+1])
    elif val[:5] == "where":
      clause_dic["where"] = val[5:]
  print(clause_dic)


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



class Query:
  def __init__(self, query):
    """
    Constructor taking a string Query for analysis
    """
    clause_dictionary = query_parse(query)
    self._Select = clause_dictionary["select"]
    self._From = clause_dictionary["from"]
    self._Where = clause_dictionary["where"]
    self_.Having = clause_dictionary["having"]
    self._Group_by = clause_dictionary["group by"]

  def get_Select(self):
    return self._Select

  def get_From(self):
    return self._From

  def get_Where(self):
    return self._Where

  def get_Having(self):
    return self._Having

  def get_Group_by(self):
    return self._Group_by

