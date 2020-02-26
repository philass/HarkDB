go: fut_compile shared_lib result

fut_compile: dotprod.fut
	futhark c --library dotprod.fut

shared_lib: dotprod.c dotprod.h
	gcc dotprod.c -o libdotprod.so -fPIC -shared

# Generate output directory
result: libdotprod.so luser.c
	gcc libdotprod.so luser.c -o out

clean: libdotprod.so dotprod.c dotprod.h out
	rm libdotprod.so dotprod.c dotprod.h out
