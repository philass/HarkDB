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


--let reduce_rows (db: [n][m]u32): [][m]u32


let mk_flags [n] (row_ids: [n]i32): [n]i32 =
  let idxs = iota n
  let vals = map (\i ->   if i == 0
  			  then 1
			  else (if row_ids[i-1] != row_ids[i] 
			  	then 1
				else 0)) idxs

