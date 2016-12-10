#!/usr/bin/env bash

## This command will find all image files, if you are using other
## extensions, you can add them: -o "*.foo"
find . -iname "*.jpg" -o -iname "*.jpeg" -o -iname "*.tif" -o \
 -iname "*.tiff" -o -iname "*.png" | 

## Go through the results, saving each as $img
while IFS= read -r img; do
    ## Find will return full paths, so an image in the current
    ## directory will be ./foo.jpg and the first dot screws up 
    ## bash's pattern matching. Use basename and dirname to extract
    ## the needed information.
    name=$(basename "$img")
    path=$(dirname "$img")
    ext="${name/#*./}"; 

    ## Check whether this file has exif data
    if exiv2 "$img" 2>&1 | grep timestamp >/dev/null 
    ## If it does, read it and add the water mark   
    then
    echo "Processing $img...";
    convert "$img" -gravity SouthEast  -pointsize 22 -fill white \
             -annotate +30+30  %[exif:DateTimeOriginal] \
             "$path"/"${name/%.*/.time.$ext}";
    ## If the image has no exif data, use the creation date of the
    ## file. CAREFUL: this is the date on which this particular file
    ## was created and it will often not be the same as the date the 
    ## photo was taken. This is probably not the desired behaviour so
    ## I have commented it out. To activate, just remove the # from
    ## the beginning of each line.

     else
       date=$(stat "$img" | grep Modify | cut -d ' ' -f 2,3 | cut -d ':' -f1,2)
       convert "$img" -gravity SouthEast  -pointsize 22 -fill white \
              -annotate +30+30  "$date" \
              "$path"/"${name/%.*/.time.$ext}";
    fi 
done


