cmake_minimum_required(VERSION 3.11)

project(HARKDB)

set (CMAKE_CXX_STANDARD 17)
include(FetchContent)

FetchContent_Declare(
  googletest
  GIT_REPOSITORY https://github.com/google/googletest.git
  GIT_TAG release-1.10.0
)

FetchContent_GetProperties(googletest)
if(NOT googletest_POPULATED)
  FetchContent_Populate(googletest)
  add_subdirectory(${googletest_SOURCE_DIR} ${googletest_BINARY_DIR})
endif()



add_custom_command(
     OUTPUT
         ${CMAKE_CURRENT_BINARY_DIR}/select.c
         ${CMAKE_CURRENT_BINARY_DIR}/select.h
     COMMAND futhark c 
         --library futhark/select.fut 
         -o ${CMAKE_CURRENT_BINARY_DIR}/select
     DEPENDS futhark/select.fut
     WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
     VERBATIM
)

add_library(select SHARED
   ${CMAKE_CURRENT_BINARY_DIR}/select.c
   ${CMAKE_CURRENT_BINARY_DIR}/select.h
)

add_executable(out CLI.cpp  db_gpu_load.cpp db_gpu_load.h metaCommand.cpp
                            metaCommand.h query_parser.cpp query_parser.h
                            table.cpp table.h)

target_link_libraries(out PRIVATE
    select
)
