-- File contains functions for handling SQL Queries


-- select columns of relevance
let select (cols : []i32) (row : []i64) : []i64 = 
  let f = (\i -> row[i])
  in map f cols

-- From table -> db
-- Select [cols] -> [cols]
-- Where exps -> ([cols] -> Bool)

let select_from_where (db : [][]i64) (cols : []i32) (f : ([]i64 -> bool)) : [][]i64 =
  let rows_to_keep = filter f db
  let result = map (select cols) rows_to_keep
  in result
 

