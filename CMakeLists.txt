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


#add_executable(tableTest table.h table.cpp ${CMAKE_CURRENT_SOURCE_DIR}/tests/table.cpp)
add_executable(tableTest table.h table.cpp tests/tableTest.cpp)
add_executable(queryRepTest table.h table.cpp query_parser.h 
                        query_parser.cpp tests/queryRepTest.cpp)
add_executable(metaCommandTest table.h table.cpp  metaCommand.cpp
                            metaCommand.h tests/metaCommandTest.cpp)

target_link_libraries(out PRIVATE
    select
)

TARGET_LINK_LIBRARIES(tableTest LINK_PUBLIC gtest_main gtest) 
TARGET_LINK_LIBRARIES(queryRepTest LINK_PUBLIC gtest_main gtest) 
TARGET_LINK_LIBRARIES(metaCommandTest LINK_PUBLIC gtest_main gtest) 

add_custom_target(runtests
  COMMAND tableTest
  COMMAND metaCommandTest
  COMMAND queryRepTest
  WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
)

