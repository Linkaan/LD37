bl_info = {
    "name": "Backup in LD37 folder",
    "author": "Linus Styrén",
    "description": "Makes a backup copy everytime you save for LD37." ,
    "version": (0, 1),
    "blender": (2, 7, 8),
    "category": "System",
    }


import bpy
import os
import shutil

from bpy.app.handlers import persistent

@persistent
def backup_handler(dummy):
    source = bpy.data.filepath
    fname = bpy.path.display_name_from_filepath(source)
    dstfile = "/home/linkan/Programmering/LD37/LD37/Assets/" + fname + ".blend1"
    shutil.copy2(source, dstfile)
    print("*** backed up sucessfully! to", dstfile)

def register():
    bpy.app.handlers.save_post.append(backup_handler)

def unregister():
    bpy.app.handlers.save_post.remove(backup_handler)

if __name__ == "__main__":
    register()
