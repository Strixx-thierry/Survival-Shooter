"""Generates the AR plane tracker texture with the student's full name.

Usage: python Tools/make_plane_texture.py "Your Full Name"
"""
import sys
from PIL import Image, ImageDraw, ImageFont

name = (sys.argv[1] if len(sys.argv) > 1 else "YOUR FULL NAME").upper()
S = 1024
img = Image.new("RGBA", (S, S), (20, 22, 24, 90))
d = ImageDraw.Draw(img)
silver = (196, 198, 200, 255)

for i in range(0, S + 1, S // 4):  # grid, edges wrap seamlessly
    d.line([(i, 0), (i, S)], fill=silver[:3] + (150,), width=6)
    d.line([(0, i), (S, i)], fill=silver[:3] + (150,), width=6)

size = 200
font = ImageFont.truetype("Assets/UI/Fonts/Jaro.ttf", size)
while d.textlength(name, font=font) > S - 120 and size > 20:
    size -= 4
    font = ImageFont.truetype("Assets/UI/Fonts/Jaro.ttf", size)

w = d.textlength(name, font=font)
d.rectangle([(S - w) / 2 - 30, S / 2 - size * 0.75, (S + w) / 2 + 30, S / 2 + size * 0.75], fill=(10, 10, 10, 200))
d.text((S / 2, S / 2), name, font=font, fill=silver, anchor="mm")
img.save("Assets/Materials/PlaneTracker.png")
print("saved for:", name)
