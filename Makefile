go: fut_compile shared_lib

fut_compile: dotprod.fut
	futhark c --library dotprod.fut

shared_lib: dotprod.c
	gcc dotprod.c -o libdotprod.so -fPIC -shared

clean: libdotprod.so dotprod.c dotprod.h
	rm libdotprod.so dotprod.c dotprod.h
