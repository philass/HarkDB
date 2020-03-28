go: fut_compile shared_lib result

fut_compile: futhark/select_where.fut
	mkdir build && futhark c --library futhark/select_where.fut -o build/select_where

shared_lib: build/select_where.c build/select_where.h
	gcc build/select_where.c -o build/lib_select_where.so -fPIC -shared

# Generate output directory
result: build/lib_select_where.so db_gpu_load.c
	g++ -g build/lib_select_where.so  db_gpu_load.cpp -o build/out

clean: 
	rm -rf build/
