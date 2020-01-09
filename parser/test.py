import query as q

q.query_parse("select col1, col2 from t1, t2 where col3 < col4 group by col1 having col1 > 3")
