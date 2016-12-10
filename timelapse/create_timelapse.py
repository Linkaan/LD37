import os
import time
import calendar
import fnmatch
from datetime import datetime, date
from PIL import Image, ImageDraw, ImageFont

with_border = True
color = 'white'
dpi = 300
font_size = 7
font_file = "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf"
#font_file = "/usr/share/fonts/truetype/liberation/LiberationMono-Bold.ttf"

padding_left = 12
padding_bottom = padding_left

def format_date(secs):
    return time.strftime('%a %d %b %Y, %H:%M', time.localtime(secs))

def add_timestamp(filename):
    start = time.time()
    old_file = os.path.join("./images", filename)
    secs = os.path.getmtime(old_file)
    dt_formatted = format_date(secs)
    new_path = "./archive"
    if not os.path.exists(new_path):
        os.makedirs(new_path)
    new_file = os.path.join(new_path, filename)
    draw_timestamp(old_file, new_file, dt_formatted)
    elapsed = time.time() - start
    print("Processed file %s in %.2f seconds" % (old_file, elapsed))

def add_timestamps():
    for filename in os.listdir("./images"):
        if filename.endswith(".png"):
            add_timestamp(filename)

def draw_text_with_border(draw, x_pos, y_pos, text, color, font):
    for x in xrange(-3, 4):
        for y in xrange(-3, 4):
            draw.text((x_pos + x, y_pos + y), text, font=font, fill="black")
    draw.text((x_pos, y_pos), text, font=font, fill=color)
            
def draw_timestamp(old_file, new_file, datetime):
    image = Image.open(old_file)
    width, height = image.size
    
    image.info['dpi'] = (dpi, dpi)
    dfsize = int(dpi * font_size / 72.0)
    font = ImageFont.truetype(font_file, dfsize, encoding="unic")
    
    draw = ImageDraw.Draw(image)
    text = u"%s" % (datetime)
    text_width, text_height = font.getsize(text)
    x_pos, y_pos = width - padding_left - text_width, height - padding_bottom - text_height
    
    if with_border:
        draw_text_with_border(draw, x_pos, y_pos, text, color, font)
    else:
        draw.text((x_pos, y_pos), text, font=font, fill=color)
    
    image.save(new_file, "PNG")
    
def create_timelapse():
    timelapse_file = os.path.join("./archive", "timelapse.mp4")
    os.system("ffmpeg -r 24 -pattern_type glob -i './archive/*.png' -c:v libx264 -b:v 1500k %s" % (timelapse_file))
            
if __name__ == "__main__":
    add_timestamps()
    create_timelapse()