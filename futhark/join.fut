-- Contains JOIN CODE
--
-- ==
-- input @ dataset_join.data
open import "lib/github.com/diku-dk/segmented/segmented"


-- Radix sort helper function
let rsort_step [n] (xs: [n](u32, i32, i32), bitn: i32): [n](u32, i32, i32) =
  let (xs_val, _, _) = unzip3 xs
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
let rsort [n] (xs: [n](u32, i32, i32)): [n](u32, i32, i32) =
  loop (xs) for i < 32 do rsort_step(xs,i)


--Make flag array (index new rows)
let mk_flags_col [n] (row_ids: [n](u32, i32, i32)): [n]i32 =
  let idxs = iota n
  let vals = map (\i ->   if i == 0
  			  then 1
			  else (if (row_ids[i-1]).0 != (row_ids[i]).0
			  	then 1
				else 0)) idxs
  in vals

let mk_flags_table [n] (row_ids: [n](u32, i32, i32)): [n]i32 =
  let idxs = iota n
  let vals = map (\i ->   if i == 0
  			  then row_ids[0].1
			  else (if (row_ids[i-1]).1 != (row_ids[i]).1
			  	then row_ids[i].1
				else 0)) idxs
  in vals

let generate_pairs [n] (arr :[n](i32, i32)) : [](i32, i32) = 
        let (t_arr1, t_arr2) = partition (\x -> x.1 == 1) arr
	let (_, arr1) = unzip t_arr1
	let (_, arr2) = unzip t_arr2
	in expand (\x->length arr2) (\x i -> (x, arr2[i])) arr1

let dim_helper [n] (flags: [n]i32) : []i32 = segmented_reduce (+) 0 (map (== 1) flags) (replicate n 1)

let join [n][m][s][t][l][k] (db1: [n][m]u32) (db2: [s][t]u32) 
			    (col1: i32)  (col2: i32)
			    (cols1: [l]i32) (cols2: [k]i32)  = 
	let l1 = zip3 db1[:, col1] (replicate n 1) (iota n)
	let l2 = zip3 db2[:, col2] (replicate s 2) (iota s)
	let to_sort = concat l1 l2
	let sorted = rsort to_sort
	let flags = mk_flags_col sorted
	let f_lens = dim_helper flags
	let fc = copy f_lens
	let p_lens = scatter (rotate (-1) fc) [0] [0]
	let inds = zip p_lens f_lens
	let (_, k1, k2)  = unzip3 (copy sorted)
	let sorted_copy = zip k1 k2
	let pairs = loop acc = [] for (s, f) in inds do
		concat acc (generate_pairs sorted_copy[s:f])
	in pairs
			
				      

