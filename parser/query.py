""""
Class for Representing an SQL Query.
Reads following grammar

select col1, col2, ...., coln
from t1, t2, ....., tn
where cond1 ..... cond2
group by coli,.... colj
Having condk.... condm


"""

import sqlparse as sqp  # Library for tokenizing SQL statement


def query_parse(query):
    """
    Parse Query into SQL Dictionary with key value pairs of
    SubClause : identifier List
    """
    clause_dic = {"select": [], "from": [], "where": [],
                  "having": [], "group by": []}
    # Get Tokenized list of first SQL statement
    statement = sqp.parse(query)[0]
    def func(arg):
        return str(arg.ttype)[11:21] != 'Whitespace'

    s1_clean = list(filter(func, list(statement)))
    # Where is treated different by sqparse"
    keywords = clause_dic.keys() - "where"
    for i in range(len(list(s1_clean))):
        val = s1_clean[i].value
        if val in keywords:
            clause_dic[val] = identifier_to_list(s1_clean[i+1])
        elif val[:5] == "where":
            clause_dic["where"] = val[5:]
    print(clause_dic)
    return clause_dic


def identifier_to_list(sqp_token):
    """
    Convert IDENTIFIER List or Identifier into a list of columns
    """
    try:
        named_cols = filter(lambda x: x.ttype is None)
        hack = list(map(lambda x: x.value, named_cols))
        return [x.strip() for x in hack[0].split(',')]

    # Assumes failure means we are working with Identifier only
    finally:
        hack = list([sqp_token.value])
    return [x.strip() for x in hack[0].split(',')]


class Query:
    """
    Class represents a Query
    """

    def __init__(self, query):
        """
        Constructor taking a string Query for analysis
        """
        clause_dictionary = query_parse(query)
        self._select = clause_dictionary["select"]
        self._from = clause_dictionary["from"]
        self._where = clause_dictionary["where"]
        self._having = clause_dictionary["having"]
        self._group_by = clause_dictionary["group by"]

    def get_select(self):
        """
        Get select statement
        """
        return self._select

    def get_from(self):
        """
        Get from statement
        """
        return self._from

    def get_where(self):
        """
        Get where statement
        """
        return self._where

    def get_having(self):
        """
        Get having statement
        """
        return self._having

    def get_group_by(self):
        """
        Get group by
        """
        return self._group_by
