from PIL import Image, ImageDraw, ImageFont, ImageColor
import textwrap
import requests
import time

def create_image(
            width, height, text, image_path, background_color = "#000000", 
            font = "seguisb.ttf", font_size = 42, text_color = "white", 
            line_spacing = 10):
    
    lines = text.split("\n")
    paragraph = []
    for line in lines:
        if line == "":
            blank = ""
            for i in range(40):
                blank += " "
            paragraph += [blank]
        else:
            paragraph += (textwrap.wrap(line, width=40))
    
    # paragraph = textwrap.wrap(text, width=40)
    color = ImageColor.getrgb(background_color)
    img = Image.new('RGB', (width, height), color)
    draw = ImageDraw.Draw(img)
    font = ImageFont.truetype(font, font_size)

    # calculate text height
    text_height = 0
    for line in paragraph:
        w, h = draw.textsize(line, font=font)
        text_height += h + line_spacing


    current_h = (height - text_height)/2
    for line in paragraph:
        w, h = draw.textsize(line, font=font)
        draw.text(((width - w) / 2, current_h), line, font=font, fill=text_color)
        current_h += h + line_spacing

    img.save(image_path)


def get_quote():
    source = requests.get("https://api.quotable.io/random?tags=famous-quotes").json()
    author = source["author"]
    content = source["content"]
    return content, author

content, author = get_quote()
text = "\"" + content + "\"\n\n" + "- " + author

create_image(1000, 1000, text,'test.png', font="Karla-Regular.ttf")
time.sleep(1)