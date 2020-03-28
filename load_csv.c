#include <stdio.h>
#include <ctype.h>
#include <inttypes.h>
#include <stdlib.h>

#define MAX_LINE 4096


/**
 * Loads the contents of the fileName into data
**/
void load_csv(uint32_t* data, char* fileName, char delimiter) {
	FILE *file;
	file = fopen(fileName, "r");
	if (file == NULL) {
		perror("Can't open file");
	}
	
	int current_data_point = 0;
	char current_line[MAX_LINE];
	while (fgets(current_line, MAX_LINE, file) != NULL) {
                int index = 0;
		char* current = &current_line;
		char* next;
		while (current_line[index] != '\n' && current_line[index] != EOF) {
			if (current_line[index] == delimiter) {
				uint32_t val = strtol(current, &next, 10);
				data[current_data_point] = val;
				current_data_point = current_data_point + 1;
				current = &(next[1]);
			}
			index = index + 1;
	        }
		uint32_t val = strtol(current, &next, 10);
		data[current_data_point] = val;
		current_data_point = current_data_point + 1;
		current = &(next[1]);
	}
}
				
