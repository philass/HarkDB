"""
WILL HAVE TO BE REWRITTEN FOR MULTITABLE RULES
"""
def query_to_representation(tables, query):
    """
    Transforms query into representation
    """
    table_name = query.get_From()[0]
    table = None
    for tbl in tables:
        if tbl.get_name() == table_name:
            table = tbl
    header = table.get_schema()
    cols = query.get_Select()
    idxs = [header.index(c) for c in cols]
    return (table, idxs)

class Representation:
    """
    Representation of the Query
    """
    def __init__(self, tables, query):
        (table, idxs) = query_to_representation(tables, query)
        self._where = query.get_Where()
        self._table = table
        self._idxs = idxs

    def get_table(self):
        """
        Get table
        """
        return self._table

    def get_idxs(self):
        """
        Get Indices
        """
        return self._idxs

    def where_rewrite(self):
        """
        Rewrites where clause into Futhark acceptable Format
        """
        if self._where == []:
            return "let keep = filter (\\row -> true) db\n"

        where_eq_adjusted = self._where.replace("=", " == ")

        if where_eq_adjusted.find(" and ") != -1:
            where_and_adjusted = "(" + where_eq_adjusted.replace(" and ", ") && ")
        else:
            where_and_adjusted = where_eq_adjusted
        if where_and_adjusted.find(" AND ") != -1:
            where_and_adjusted = "(" +  where_and_adjusted.replace(" AND ", ") && ")
        if where_and_adjusted.find(" or ") != -1:
            where_or_adjusted = "(" + where_and_adjusted.replace(" or ", ") || ")
        else:
            where_or_adjusted = where_and_adjusted
        if where_or_adjusted.find(" OR ") != -1:
            where_or_adjusted = "(" + where_or_adjusted.replace(" OR ", ") || ")
        where_clean = where_or_adjusted
        updating_statement = where_clean
        for i, header in enumerate(self._table.get_schema()):
            index = updating_statement.find(header)
            while index != -1:
                updating_statement = updating_statement[:index] \
                    + updating_statement[index + len(header):]
                updating_statement = updating_statement[:index] + "row[" + str(i) + "]" \
                    + updating_statement[len(str(i)) + index:]
                index = updating_statement.find(header)
        return "let keep = filter (\\row -> unsafe " + updating_statement + " ) db\n"


    def generate_futhark(self):
        """
        Generates Futhark code for the SQL Query
        based on the internal Representation
        """
        select_header = "let select (cols : []i32) (row : []f32) : []f32 = \n"
        select_s1 = "  let f = (\\i -> unsafe row[i])\n"
        select_s2 = "  in map f cols\n"
        select_from_header = "entry select_from_where (db : [][]f32) (cols : []i32) : [][]f32 =\n"
        where_part = self.where_rewrite()
        result = "  in map (select cols) keep\n"
        fut_file = open("futhark/db_sel.fut", "w+")
        fut_file.write(select_header)
        fut_file.write(select_s1)
        fut_file.write(select_s2)
        fut_file.write("")
        fut_file.write(select_from_header)
        fut_file.write(where_part)
        fut_file.write(result)
