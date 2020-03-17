open import "lib/github.com/diku-dk/segmented/segmented"
-- Contains GROUP BY CODE


--Groupby Functionality
--let groupby (db : [][]i64) (g_cols: []i32) (s_cols: []i32) : []i64 =

-- Radix sort helper function
let rsort_step [n][m] (xs: [n][m]u32, bitn: i32): [n][m]u32 =
  let xs_val = xs[:, 0]
  let bits1 = map (\x -> (i32.u32 (x >> u32.i32 bitn)) & 1) xs_val
  let bits0 = map (1-) bits1
  let idxs0 = map2 (*) bits0 (scan (+) 0 bits0)
  let idxs1 = scan (+) 0 bits1
  let offs  = reduce (+) 0 bits0
  let idxs1 = map2 (*) bits1 (map (+offs) idxs1)
  let idxs  = map2 (+) idxs0 idxs1
  let idxs  = map (\x->x-1) idxs
  in scatter (copy xs) idxs xs

-- Radix sort algorithm, ascending (for matrices)
let rsort [n][m] (xs: [n][m]u32): [n][m]u32 =
  loop (xs) for i < 32 do rsort_step(xs,i)


--Make flag array (index new rows)
let mk_flags [n] (row_ids: [n]u32): [n]u32 =
  let idxs = iota n
  let vals = map (\i ->   if i == 0
  			  then 1
			  else (if row_ids[i-1] != row_ids[i] 
			  	then 1
				else 0)) idxs
  in vals


let f  (a: []u32) (b: []u32) = map2 (+) a  b --map (\i -> a[i] + b[i]) s_cols


--let mapping [n] (idxs: [n]i32) (vals: [n]i32) = map2 (\x y -> ([x], [y])) idxs vals
--
--let reduce_f ((x1: []i32), (y1: []i32)) ((x2: []i32), (y2: []i32)) = 
--  if (x1,y1) == ([0], [0]) then (x2, y2)
--  else 
--  if (x2,y2) == ([0], [0]) then (x1, y1)
--  else
--  if last y1 == head y2
--  then ((concat x1 (map (+ (last x1)) x2)), concat y1 y2)
--  else ((concat x1 (map (+ 1 + (last x1)) x2)), concat y1 y2)
--  

--
let main (db : [][]u32)  (s_cols: []i32) : [][]u32 =
  let sorted_rows = trace (rsort db) -- ideally pass groupby col here
  let idxs = trace (mk_flags sorted_rows[:, 0])
  let idxs_b = map (== 1) idxs
  in segmented_reduce f (replicate (length db[0, :]) 0) idxs_b sorted_rows
  --let red_idxs = trace (scan (+) 0 idxs)
  --in reduce_by_index (copy sorted_rows) (f s_cols) (replicate (length db[0, :]) 0) (map i32.u32 (copy red_idxs)) (copy sorted_rows)
