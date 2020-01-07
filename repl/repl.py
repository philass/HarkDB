#!/usr/bin/env python3

from parser import command_parser

user_input = input("harkdb> ")
while (user_input != ".exit"):
  command_parser(user_input)
  user_input = input("harkdb> ")
