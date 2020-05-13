IF exist live rd /s /q live
call live_unpack.bat
call live_patch.bat
move original\live live
