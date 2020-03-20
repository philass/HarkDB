-- File contains functions for handling SQL Queries


-- select columns of relevance
let select (cols : []i32) (row : []u32) : []u64 =
  let f = (\i -> row[i])
  in map f cols

-- From table -> db
-- Select [cols] -> [cols]
-- Where exps -> ([cols] -> Bool)

let select_from_where (db : [][]u32) (cols : []i32) : [][]u64 =
  -- let rows_to_keep = filter f db
  let result = map (select cols) db
  in result


let main (db : [][]u32) (cols : []i32) : [][]u64 = select_from_where db cols
